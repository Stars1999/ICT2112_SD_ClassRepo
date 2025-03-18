using System.Collections.Generic;

/// <summary>
/// Interface for the ErrorAnalyser class
/// Responsible for analyzing LaTeX errors and suggesting solutions.
/// </summary>
public interface iErrorAnalyser
{
    /// <summary>
    /// Suggests solutions for detected errors in LaTeX content.
    /// </summary>
    List<ErrorStyle> SuggestErrorSolution(string latexContent);

    /// <summary>
    /// Applies an automatic fix for a given error.
    /// </summary>
    string ChangeError(string latexContent, ErrorStyle error);
}
