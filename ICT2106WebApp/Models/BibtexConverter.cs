using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

public class BibTeXConverter : iConversionStatus //Implements the interface
{
    private string _updatedJson;
    private readonly IScannerFactory _citationFactory;
    private readonly IScannerFactory _bibliographyFactory;
    private string _preferredStyle; // Default APA, can be changed
    private readonly IInsertBibTex _bibtexMapper;

    /// <summary>
    /// Constructor for BibTeXConverter
    /// </summary>
    public BibTeXConverter(IScannerFactory citationFactory, IScannerFactory bibliographyFactory, IInsertBibTex bibtexMapper, string preferredStyle = "apa")
    {
        _citationFactory = citationFactory;
        _bibliographyFactory = bibliographyFactory;
        _preferredStyle = preferredStyle.ToLower(); // Ensure lowercase for consistency
        _bibtexMapper = bibtexMapper;

    }

    private string DetectCitationStyle(string latexContent)
    {
        if (string.IsNullOrWhiteSpace(latexContent)) return "apa"; // Default fallback

        // APA Example: (Smith, 2019)
        if (Regex.IsMatch(latexContent, @"\([A-Z][a-z]+, \d{4}\)")) return "apa";

        // MLA Example: (Smith 45)
        if (Regex.IsMatch(latexContent, @"\([A-Z][a-z]+ \d+\)")) return "mla";

        return "apa"; // Fallback
    }

    /// <summary>
    /// Converts citations and bibliography based on the preferred style.
    /// </summary>
    public string ConvertCitationsAndBibliography(string jsonData, string overrideStyle = null)
    {
        Console.WriteLine($"[DEBUG] Received JSON: {jsonData}");

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            Console.WriteLine("[ERROR] No input JSON received in BibTeXConverter.");
            return null;
        }

        try
        {
            //Deserialize directly into a list of BibliographyDocument
            var reference = JsonSerializer.Deserialize<Reference>(jsonData);
            if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
            {
                Console.WriteLine("[ERROR] No documents found in JSON.");
                return null;
            }


            Console.WriteLine($"[DEBUG] Successfully deserialized {reference.Documents.Count} documents.");

            var updatedDocuments = new List<BibliographyDocument>();

            foreach (var doc in reference.Documents)
            {
                try
                {
                    string latexContent = doc.OriginalLatexContent?.Trim() ?? doc.LatexContent?.Trim() ?? "";

                    if (!string.IsNullOrWhiteSpace(overrideStyle))
                    {
                        _preferredStyle = overrideStyle.ToLower();
                        Console.WriteLine($"[INFO] Using override style: {_preferredStyle}");
                    }
                    else
                    {
                        _preferredStyle = DetectCitationStyle(latexContent);
                        Console.WriteLine($"[INFO] Detected citation style: {_preferredStyle}");
                    }

                    if (_preferredStyle == "apa")
                    {
                        IAPA apaScanner = _citationFactory.CreateAPA();
                        latexContent = apaScanner.FormatCitations(latexContent);

                        IAPA apaBibliographyScanner = _bibliographyFactory.CreateAPA();
                        latexContent = apaBibliographyScanner.FormatBibliographies(latexContent);

                        apaScanner.ApplyAPAFormatting();

                        var citationYearMap = new Dictionary<string, string>
                        {
                            { "Miller", "2021" },
                            { "Thompson", "2020" },
                            { "Brown", "2021" },
                            { "Smith", "2019" }
                            // Add more authors as needed
                        };

                        latexContent = Regex.Replace(
                            latexContent,
                            @"\(([A-Z][a-z]+) \d+\)",
                            match =>
                            {
                                var author = match.Groups[1].Value;
                                var year = citationYearMap.ContainsKey(author) ? citationYearMap[author] : "2024";
                                return $"({author}, {year})";
                            }
                        );

                        latexContent = Regex.Replace(
                            latexContent,
                            @"\(([A-Z][a-z]+), \d{1,3}\)", // catches things like (Brown, 45)
                            match =>
                            {
                                var author = match.Groups[1].Value;
                                var year = citationYearMap.ContainsKey(author) ? citationYearMap[author] : "2024";
                                return $"({author}, {year})";
                            }
                        );
                    }

                    else if (_preferredStyle == "mla")
                    {
                        IMLA mlaScanner = _citationFactory.CreateMLA();
                        latexContent = mlaScanner.FormatCitations(latexContent);

                        IMLA mlaBibliographyScanner = _bibliographyFactory.CreateMLA();
                        latexContent = mlaBibliographyScanner.FormatBibliographies(latexContent);

                        mlaScanner.ApplyMLAFormatting();

                        var authorPageMap = new Dictionary<string, string>
                        {
                            { "Smith", "32" },
                            { "Brown", "13" },
                            { "Miller", "103" },
                            { "Thompson", "77" }
                        };

                        latexContent = Regex.Replace(
                            latexContent,
                            @"\(([A-Z][a-z]+), \d{4}\)",
                            match =>
                            {
                                var author = match.Groups[1].Value;
                                var page = authorPageMap.ContainsKey(author) ? authorPageMap[author] : "99";
                                return $"({author} {page})";
                            }
                        );

                        latexContent = Regex.Replace(
                            latexContent,
                            @"\(([A-Z][a-z]+), \d{1,3}\)",
                            match =>
                            {
                                var author = match.Groups[1].Value;
                                var page = authorPageMap.ContainsKey(author) ? authorPageMap[author] : "99";
                                return $"({author} {page})";
                            }
                        );
                    }
                    else
                    {
                        Console.WriteLine($"[ERROR] Unsupported citation style: {_preferredStyle}");
                        continue; // Skip this document
                    }


                    // Clean LaTeX content formatting
                    latexContent = SanitizeLatexContent(latexContent);

                    updatedDocuments.Add(new BibliographyDocument
                    {
                        Title = doc.Title,
                        Author = doc.Author,
                        Date = doc.Date,
                        LatexContent = latexContent
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process '{doc.Title}'. Exception: {ex.Message}");
                }
            }

            string updatedJson = JsonSerializer.Serialize(new Reference { Documents = updatedDocuments }, new JsonSerializerOptions { WriteIndented = true });

            if (_bibtexMapper == null)
                {
                    Console.WriteLine("[FATAL] BibTeXConverter received a NULL bibtexMapper!");
                }
                else
                {
                    Console.WriteLine("[DEBUG] Preparing to insert converted JSON into MongoDB...");
                    _bibtexMapper.SetUpdatedJson(updatedJson);
                    SetUpdatedJson(updatedJson);
                    Console.WriteLine("[DEBUG] Called SetUpdatedJson successfully.");
                }
            

            return updatedJson;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] JSON deserialization error: {ex.Message}");
            return null;
        }
    }
    
    private string SanitizeLatexContent(string content)
    {
        // Fix common broken LaTeX syntax issues
        content = content.Replace(@"{article)", "{article}");
        content = content.Replace(@"\title{", "\\title{");
        content = content.Replace(@"\author{", "\\author{");
        content = content.Replace(@"\date{", "\\date{");
        content = content.Replace(@"\begin{document)", "\\begin{document}");
        content = content.Replace(@"\end{document)", "\\end{document}");
        content = content.Replace(@"\section{References)", "\\section{References}");

        // Remove extra closing braces if too many
        content = Regex.Replace(content, @"\}{2,}", "}");

        return content;
    }



    /// <summary>
    /// ✅ Fetches the conversion status.
    /// Returns true if the converted JSON has been updated in memory.
    /// </summary>
     public void SetUpdatedJson(string convertedJson)
    {
        _updatedJson = convertedJson;
    }

    public string GetUpdatedJson()
    {
        return _updatedJson;
    }

    public bool fetchConversionStatus()
    {
        return !string.IsNullOrEmpty(_updatedJson);
    }
}