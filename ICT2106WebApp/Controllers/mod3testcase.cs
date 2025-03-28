using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ICT2106WebApp.Interfaces;
using CustomLogger = ICT2106WebApp.Interfaces.ILogger;

public class CitationValidator
{
    private readonly CustomLogger _logger;

    // Inject the logger via constructor
    public CitationValidator(CustomLogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Extracts citation references from the original JSON file.
    /// Looks for patterns like (Smith, 2019) in the LatexContent field.
    /// </summary>
    public async Task<List<string>> ExtractOriginalCitationsAsync(string jsonFilePath)
    {
        string jsonContent = await File.ReadAllTextAsync(jsonFilePath);
        JObject root = JObject.Parse(jsonContent);
        var citations = new List<string>();

        // Assuming the JSON structure contains an array "documents"
        JArray documents = (JArray)root["documents"];
        if (documents != null && documents.Count > 0)
        {
            string latexContent = (string)documents[0]["LatexContent"] ?? "";
            // Regex pattern to match citations in the format: (Smith, 2019)
            var matches = Regex.Matches(latexContent, @"\((?<author>[A-Z][a-zA-Z\.\- ]+(?:\s+et\s+al\.)?),\s*(?<year>\d{4})\)");
            foreach (Match match in matches)
            {
                if (match.Success && match.Groups["author"].Success && match.Groups["year"].Success)
                {
                    string author = match.Groups["author"].Value.Trim().Replace(" ", "");
                    string year = match.Groups["year"].Value;
                    // Form a citation key like "Smith2019" or "Smithetal2019"
                    string citationKey = $"{author}{year}";
                    citations.Add(citationKey);
                }
            }
        }
        _logger.InsertLog(DateTime.Now, "Extracted original citation keys.", "CitationValidator");
        return citations;
    }

    /// <summary>
    /// Extracts citation commands from the final LaTeX output.
    /// Looks for patterns like \cite{Smith2019}.
    /// </summary>
    public List<string> ExtractConvertedCitations(string latexContent)
    {
        var citations = new List<string>();

        // Extract citations in the format (Smith, 2019)
        var matches = Regex.Matches(latexContent, @"\(([A-Z][a-zA-Z\.\- ]+),\s*(\d{4})\)");
        foreach (Match match in matches)
        {
            if (match.Success && match.Groups.Count > 2)
            {
                string author = match.Groups[1].Value.Trim().Replace(" ", "");
                string year = match.Groups[2].Value;
                citations.Add($"{author}{year}");
            }
        }

        _logger.InsertLog(DateTime.Now, "Extracted converted citation keys.", "CitationValidator");
        return citations;
    }

    /// <summary>
    /// Validates that every citation extracted from the original JSON has been
    /// converted properly in the final LaTeX output.
    /// </summary>
    public async Task<bool> ValidateCitationConversionAsync(string jsonFilePath, string latexContent, bool isFileContent = false)
    {
        List<string> originalCitations = await ExtractOriginalCitationsAsync(jsonFilePath);

        string finalLatex = isFileContent 
            ? latexContent 
            : await File.ReadAllTextAsync(latexContent);

        List<string> convertedCitations = ExtractConvertedCitations(finalLatex);

        _logger.InsertLog(DateTime.Now, $"Original Citations: {string.Join(", ", originalCitations)}", "CitationValidator");
        _logger.InsertLog(DateTime.Now, $"Converted Citations: {string.Join(", ", convertedCitations)}", "CitationValidator");

        foreach (var expected in originalCitations)
        {
            if (!convertedCitations.Contains(expected))
            {
                _logger.InsertLog(DateTime.Now, $"Citation key '{expected}' not found in the converted output.", "CitationValidator");
                return false;
            }
        }
        _logger.InsertLog(DateTime.Now, "All expected citations have been converted correctly.", "CitationValidator");
        return true;
    }
}

public class ConsoleLogger : CustomLogger
{
    public void InsertLog(DateTime logTimestamp, string logDescription, string logLocation)
    {
        // Simple console-based logger implementation
        Console.WriteLine($"[{logTimestamp}] {logLocation}: {logDescription}");
    }
}

public class Mod3TestCases
    {
        private readonly CitationValidator _validator;
        private readonly CustomLogger _logger;
        private readonly string _jsonFilePath = "wwwroot/bibliography_test.json";
        private readonly string _latexFilePathPass = "wwwroot/document.tex";

        public Mod3TestCases(CustomLogger logger)
        {
            _logger = logger;
            _validator = new CitationValidator(logger);
        }

        // Runs the pass test and returns true if citations are correctly converted.
        public async Task<bool> RunPassTests()
        {
            bool isValidPass = await _validator.ValidateCitationConversionAsync(_jsonFilePath, _latexFilePathPass);
            Console.WriteLine(isValidPass ? "Citation Conversion Validation Passed" : "Citation Conversion Validation Failed");
            return isValidPass;
        }

        // Runs the fail test and returns true if the conversion fails
        public async Task<bool> RunFailTests()
        {
            string latexContentFail = @"\documentclass{article}
\title{The Role of AI in Modern Healthcare}
\author{Dr. Emily Johnson}
\date{2024-03-15}
\begin{document}
\maketitle
AI is transforming medical diagnostics. Predictive analytics does not mention the source.
\section{References}
Smith, John. ""Artificial Intelligence in Medical Diagnostics."" AI \& Healthcare Journal, 2019.
\end{document}";

            bool isValidFail = await _validator.ValidateCitationConversionAsync(_jsonFilePath, latexContentFail, isFileContent: true);
            Console.WriteLine(isValidFail ? "Citation Conversion Validation Passed" : "Citation Conversion Validation Failed");
            return !isValidFail;
        }

        // Runs all tests for Mod3 and returns true if all tests pass.
        public async Task<bool> RunAllTests()
        {
            bool passResult = await RunPassTests();
            bool failResult = await RunFailTests();
            return passResult && failResult;
        }
    }
