using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
/// Implementation of the ErrorAnalyser class that analyzes LaTeX errors and suggests solutions
/// </summary>
public class ErrorAnalyser : iErrorAnalyser
{
    private readonly Dictionary<string, string> _commonErrorPatterns;
    private readonly Dictionary<string, Func<string, int, ErrorStyle>> _errorDetectors;

    public ErrorAnalyser()
    {
        // Initialize common error patterns and their solutions
        _commonErrorPatterns = new Dictionary<string, string>
        {
            { @"\\alpha", "Symbol \\alpha requires math mode. Enclose it with $\\alpha$" },
            { @"\\beta", "Symbol \\beta requires math mode. Enclose it with $\\beta$" },
            { @"\\gamma", "Symbol \\gamma requires math mode. Enclose it with $\\gamma$" },
            { @"\\delta", "Symbol \\delta requires math mode. Enclose it with $\\delta$" },
            { @"\\cite{([^}]*)}", "Citation '$1' undefined. Check that this citation key exists in your bibliography." },
            { @"\\begin{([^}]*)}(?!.*\\end{\1})", "Missing \\end{$1} tag. Add \\end{$1} to close the environment." },
            { @"\\end{document", "Missing } in \\end{document} command. Add } to close the command." },
            { @"\\usepackage{([^}]*)}(?!.*\\begin{document})", "Package '$1' loaded after \\begin{document}. Move it before \\begin{document}." },
            { @"\\section{}", "Empty section title. Add a title inside the braces." },
            { @"\\section{\s*}", "Empty section title. Add a title inside the braces." },
            { @"\\bibliography{([^}]*)}(?!.*\\bibliographystyle)", "Missing \\bibliographystyle command. Add \\bibliographystyle{style} before using \\bibliography." },
            { @"\\begin{figure}(?!.*\\caption)", "Figure environment missing \\caption. Add \\caption{...} to your figure." },
            { @"\\ref{([^}]*)}", "Reference '$1' undefined. Check that this label exists in your document." }
        };

        // Initialize error detectors
        _errorDetectors = new Dictionary<string, Func<string, int, ErrorStyle>>
        {
            { "Undefined control sequence", DetectUndefinedControlSequence },
            { "Missing $ inserted", DetectMissingMathMode },
            { "Citation undefined", DetectUndefinedCitation },
            { "Environment mismatch", DetectEnvironmentMismatch },
            { "Missing brace", DetectMissingBrace },
            { "Undefined reference", DetectUndefinedReference }
        };
    }

    /// <summary>
    /// Suggests solutions for LaTeX errors
    /// </summary>
    /// <param name="latexContent">The LaTeX document content</param>
    /// <returns>List of errors with suggested solutions</returns>
    public List<ErrorStyle> SuggestErrorSolution(string latexContent)
    {
        if (string.IsNullOrEmpty(latexContent))
        {
            return new List<ErrorStyle>();
        }

        var errors = new List<ErrorStyle>();
        var lines = latexContent.Split('\n');

        // Check for common errors using regex patterns
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            int lineNumber = i + 1;

            // Check for unclosed braces (basic level)
            CheckUnclosedBraces(line, lineNumber, errors);

            // Check for math mode errors
            CheckMathModeErrors(line, lineNumber, errors);

            // Check for unclosed environments
            CheckUnclosedEnvironments(latexContent, line, lineNumber, errors);

            // Check for undefined citations
            CheckUndefinedCitations(line, lineNumber, errors);

            // Check for undefined references
            CheckUndefinedReferences(line, lineNumber, errors);

            // Check for missing closing bracket in \\end{document}
            CheckEndDocumentSyntax(line, lineNumber, errors);
        }

        return errors;
    }

    /// <summary>
    /// Applies an automatic fix for a given error
    /// </summary>
    /// <param name="latexContent">The original LaTeX content</param>
    /// <param name="error">The error to fix</param>
    /// <returns>The fixed LaTeX content</returns>
    public string ChangeError(string latexContent, ErrorStyle error)
    {
        if (error == null || string.IsNullOrEmpty(latexContent))
        {
            return latexContent;
        }

        var lines = latexContent.Split('\n');
        
        // Apply the fix based on error type
        if (error.LineNumber > 0 && error.LineNumber <= lines.Length)
        {
            int lineIndex = error.LineNumber - 1;
            string line = lines[lineIndex];

            // Handle math mode fixes
            if (error.ErrorType.Contains("math mode"))
            {
                foreach (var pattern in _commonErrorPatterns.Keys.Where(k => k.Contains("alpha") || k.Contains("beta") || k.Contains("gamma") || k.Contains("delta")))
                {
                    var match = Regex.Match(line, pattern);
                    if (match.Success)
                    {
                        string mathSymbol = match.Value;
                        lines[lineIndex] = line.Replace(mathSymbol, "$" + mathSymbol + "$");
                        break;
                    }
                }
            }
            // Handle missing closing brace in \\end{document}
            else if (error.ErrorType.Contains("Missing } inserted") && line.Contains("\\end{document"))
            {
                lines[lineIndex] = "\\end{document}";
            }
            // Handle unclosed environments
            else if (error.ErrorType.Contains("environment") && error.Suggestion.Contains("\\end{"))
            {
                var match = Regex.Match(error.Suggestion, @"\\end{([^}]*)}");
                if (match.Success)
                {
                    string environment = match.Groups[1].Value;
                    // Add the missing \\end tag at the end of the document
                    lines[lines.Length - 1] += "\n\\end{" + environment + "}";
                }
            }
            // Handle other errors with specific fixes
            else if (!string.IsNullOrEmpty(error.AutoFix))
            {
                lines[lineIndex] = error.AutoFix;
            }
        }

        return string.Join("\n", lines);
    }

    /// <summary>
    /// Checks for unclosed braces in a LaTeX line
    /// </summary>
    private void CheckUnclosedBraces(string line, int lineNumber, List<ErrorStyle> errors)
    {
        int openBraces = line.Count(c => c == '{');
        int closeBraces = line.Count(c => c == '}');

        if (openBraces > closeBraces)
        {
            errors.Add(new ErrorStyle
            {
                LineNumber = lineNumber,
                ErrorType = "Missing } inserted",
                Message = "There is an unclosed curly brace.",
                Suggestion = "Add } to close the open brace.",
                Severity = "high",
                CanFix = true,
                AutoFix = line + new string('}', openBraces - closeBraces)
            });
        }
        else if (closeBraces > openBraces)
        {
            errors.Add(new ErrorStyle
            {
                LineNumber = lineNumber,
                ErrorType = "Extra } inserted",
                Message = "There is an extra closing brace.",
                Suggestion = "Remove the extra } character.",
                Severity = "high",
                CanFix = true,
                AutoFix = line.Substring(0, line.Length - (closeBraces - openBraces))
            });
        }
    }

    /// <summary>
    /// Checks for math mode errors in a LaTeX line
    /// </summary>
    private void CheckMathModeErrors(string line, int lineNumber, List<ErrorStyle> errors)
    {
        foreach (var pattern in _commonErrorPatterns.Where(p => p.Key.Contains("alpha") || 
                                                               p.Key.Contains("beta") || 
                                                               p.Key.Contains("gamma") || 
                                                               p.Key.Contains("delta")))
        {
            var matches = Regex.Matches(line, pattern.Key);
            foreach (Match match in matches)
            {
                if (!IsMathMode(line, match.Index))
                {
                    errors.Add(new ErrorStyle
                    {
                        LineNumber = lineNumber,
                        ErrorType = "Missing $ inserted",
                        Message = "Symbol requires math mode.",
                        Suggestion = pattern.Value,
                        Severity = "high",
                        CanFix = true,
                        Column = match.Index + 1,
                        Length = match.Length
                    });
                }
            }
        }
    }

    /// <summary>
    /// Checks if a position in a line is already in math mode
    /// </summary>
    private bool IsMathMode(string line, int position)
    {
        int dollarCount = 0;
        for (int i = 0; i < position; i++)
        {
            if (line[i] == '$')
            {
                dollarCount++;
            }
        }
        return dollarCount % 2 == 1; // Odd number of $ means we're in math mode
    }

    /// <summary>
    /// Checks for unclosed environments in a LaTeX document
    /// </summary>
    private void CheckUnclosedEnvironments(string fullContent, string line, int lineNumber, List<ErrorStyle> errors)
    {
        var beginMatch = Regex.Match(line, @"\\begin{([^}]*)}");
        if (beginMatch.Success)
        {
            string environment = beginMatch.Groups[1].Value;
            string endPattern = @"\\end{" + environment + "}";
            if (!Regex.IsMatch(fullContent, endPattern))
            {
                errors.Add(new ErrorStyle
                {
                    LineNumber = lineNumber,
                    ErrorType = "Environment mismatch",
                    Message = $"Environment '{environment}' is not closed.",
                    Suggestion = $"Add \\end{{{environment}}} to close the environment.",
                    Severity = "high",
                    CanFix = true,
                    Column = beginMatch.Index + 1,
                    Length = beginMatch.Length
                });
            }
        }
    }

    /// <summary>
    /// Checks for undefined citations in a LaTeX line
    /// </summary>
    private void CheckUndefinedCitations(string line, int lineNumber, List<ErrorStyle> errors)
    {
        var citeMatch = Regex.Matches(line, @"\\cite{([^}]*)}");
        foreach (Match match in citeMatch)
        {
            string citation = match.Groups[1].Value;
            // In a real implementation, you would check against a bibliography database
            // For now, just flagging all citations as potential errors
            errors.Add(new ErrorStyle
            {
                LineNumber = lineNumber,
                ErrorType = "Citation undefined",
                Message = $"Citation '{citation}' undefined.",
                Suggestion = "Check that this citation key exists in your bibliography.",
                Severity = "medium",
                CanFix = false,
                Column = match.Index + 1,
                Length = match.Length
            });
        }
    }

    /// <summary>
    /// Checks for undefined references in a LaTeX line
    /// </summary>
    private void CheckUndefinedReferences(string line, int lineNumber, List<ErrorStyle> errors)
    {
        var refMatch = Regex.Matches(line, @"\\ref{([^}]*)}");
        foreach (Match match in refMatch)
        {
            string reference = match.Groups[1].Value;
            // In a real implementation, you would check against a list of labels
            // For now, just flagging all references as potential errors
            errors.Add(new ErrorStyle
            {
                LineNumber = lineNumber,
                ErrorType = "Reference undefined",
                Message = $"Reference '{reference}' undefined.",
                Suggestion = "Check that this label exists in your document.",
                Severity = "medium",
                CanFix = false,
                Column = match.Index + 1,
                Length = match.Length
            });
        }
    }

    /// <summary>
    /// Checks for proper syntax in the \\end{document} tag
    /// </summary>
    private void CheckEndDocumentSyntax(string line, int lineNumber, List<ErrorStyle> errors)
    {
        if (line.Contains("\\end{document") && !line.Contains("\\end{document}"))
        {
            errors.Add(new ErrorStyle
            {
                LineNumber = lineNumber,
                ErrorType = "Missing } inserted",
                Message = "There is an unclosed curly brace in \\end{document.",
                Suggestion = "Add } to close the \\end{document command.",
                Severity = "high",
                CanFix = true,
                Column = line.IndexOf("\\end{document") + 1,
                Length = "\\end{document".Length,
                AutoFix = "\\end{document}"
            });
        }
    }

    // Error detector methods for specific error types
    private ErrorStyle DetectUndefinedControlSequence(string line, int lineNumber)
    {
        // Implementation would check for undefined LaTeX commands
        return null;
    }

    private ErrorStyle DetectMissingMathMode(string line, int lineNumber)
    {
        // Implementation would look for math symbols outside math mode
        return null;
    }

    private ErrorStyle DetectUndefinedCitation(string line, int lineNumber)
    {
        // Implementation would check citations against bibliography
        return null;
    }

    private ErrorStyle DetectEnvironmentMismatch(string line, int lineNumber)
    {
        // Implementation would check for mismatched environments
        return null;
    }

    private ErrorStyle DetectMissingBrace(string line, int lineNumber)
    {
        // Implementation would check for missing braces
        return null;
    }

    private ErrorStyle DetectUndefinedReference(string line, int lineNumber)
    {
        // Implementation would check references against labels
        return null;
    }
}