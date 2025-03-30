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
        _logger.InsertLog(DateTime.Now, "Extracted original citation keys.", "Mod3TestCases");
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

        _logger.InsertLog(DateTime.Now, "Extracted converted citation keys.", "Mod3TestCases");
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

        _logger.InsertLog(DateTime.Now, $"Original Citations: {string.Join(", ", originalCitations)}", "Mod3TestCases");
        _logger.InsertLog(DateTime.Now, $"Converted Citations: {string.Join(", ", convertedCitations)}", "Mod3TestCases");

        foreach (var expected in originalCitations)
        {
            if (!convertedCitations.Contains(expected))
            {
                _logger.InsertLog(DateTime.Now, $"Citation key '{expected}' not found in the converted output.", "Mod3TestCases");
                return false;
            }
        }
        _logger.InsertLog(DateTime.Now, "All expected citations have been converted correctly.", "Mod3TestCases");
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
        private readonly string _apaJsonFilePath = "wwwroot/apa_test.json";
        private readonly string _mlaJsonFilePath = "wwwroot/mla_test.json";
        private readonly string _latexFilePathPass = "wwwroot/document.tex";

        public Mod3TestCases(CustomLogger logger)
        {
            _logger = logger;
            _validator = new CitationValidator(logger);
        }

        // Runs the pass test for APA or MLA and returns true if citations are correctly converted.
        public async Task<bool> RunPassTests(string citationFormat)
        {
            string jsonFilePath = GetJsonFilePath(citationFormat);

            if (string.IsNullOrEmpty(jsonFilePath))
            {
                return false;
            }

            _logger.InsertLog(DateTime.Now, $"Running tests for {citationFormat} format.", "Mod3TestCases");

            bool isValidPass = await _validator.ValidateCitationConversionAsync(jsonFilePath, _latexFilePathPass);

            _logger.InsertLog(DateTime.Now, isValidPass
                ? $"{citationFormat} citation conversion validation passed."
                : $"{citationFormat} citation conversion validation failed.", "Mod3TestCases");

            return isValidPass;
        }

        // Runs the fail test for APA or MLA and returns true if the conversion fails as expected.
        public async Task<bool> RunFailTests(string citationFormat)
        {
            string jsonFilePath = GetJsonFilePath(citationFormat);

            if (string.IsNullOrEmpty(jsonFilePath))
            {
                return false;
            }

            _logger.InsertLog(DateTime.Now, $"Running tests for {citationFormat} format.", "Mod3TestCases");

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

            bool isValidFail = await _validator.ValidateCitationConversionAsync(jsonFilePath, latexContentFail, isFileContent: true);

            _logger.InsertLog(DateTime.Now, isValidFail
                ? $"{citationFormat} citation conversion validation passed."
                : $"{citationFormat} citation conversion validation failed.", "Mod3TestCases");

            return !isValidFail;
        }

        public async Task<bool> RunAllTests()
        {
            bool apaPassResult = await RunPassTests("APA");
            bool apaFailResult = await RunFailTests("APA");

            bool mlaPassResult = await RunPassTests("MLA");
            bool mlaFailResult = await RunFailTests("MLA");

            return apaPassResult && apaFailResult && mlaPassResult && mlaFailResult;
        }

        // Helper method to get the correct JSON file path based on the citation format.
        private string GetJsonFilePath(string citationFormat)
        {
            return citationFormat.ToUpper() switch
            {
                "APA" => _apaJsonFilePath,
                "MLA" => _mlaJsonFilePath,
                _ => null
            };
        }
    }
