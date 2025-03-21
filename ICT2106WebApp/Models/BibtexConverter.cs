using System;
using System.Collections.Generic;
using System.Text.Json;

public class BibTeXConverter : iConversionStatus // ✅ Implements the interface
{
    private readonly IScannerFactory _citationFactory;
    private readonly IScannerFactory _bibliographyFactory;
    private string _preferredStyle; // Default APA, can be changed
    private readonly iConversionStatus _latexCompiler; // ✅ Reference to LatexCompiler

    /// <summary>
    /// Constructor for BibTeXConverter
    /// </summary>
    public BibTeXConverter(IScannerFactory citationFactory, IScannerFactory bibliographyFactory, iConversionStatus latexCompiler, string preferredStyle = "apa")
    {
        _citationFactory = citationFactory;
        _bibliographyFactory = bibliographyFactory;
        _preferredStyle = preferredStyle.ToLower(); // Ensure lowercase for consistency
        _latexCompiler = latexCompiler; // ✅ Initialize LatexCompiler

        if (_preferredStyle != "apa" && _preferredStyle != "mla")
        {
            Console.WriteLine($"[WARNING] Invalid preferred style '{_preferredStyle}', defaulting to APA.");
            _preferredStyle = "apa";
        }
    }

    /// <summary>
    /// Converts citations and bibliography based on the preferred style.
    /// </summary>
    public string ConvertCitationsAndBibliography(string jsonData)
    {
        Console.WriteLine($"[DEBUG] Received JSON: {jsonData}");

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            Console.WriteLine("[ERROR] No input JSON received in BibTeXConverter.");
            return null;
        }

        try
        {
            // ✅ Deserialize directly into a list of BibliographyDocument
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
                    string latexContent = doc.LatexContent?.Trim() ?? "";

                    // ✅ Apply citation formatting
                    if (_preferredStyle == "apa")
                    {
                        IAPA apaScanner = _citationFactory.CreateAPA();
                        latexContent = apaScanner.FormatCitations(latexContent);

                        IAPA apaBibliographyScanner = _bibliographyFactory.CreateAPA();
                        latexContent = apaBibliographyScanner.FormatBibliographies(latexContent);

                        apaScanner.ApplyAPAFormatting();
                    }
                    else if (_preferredStyle == "mla")
                    {
                        IMLA mlaScanner = _citationFactory.CreateMLA();
                        latexContent = mlaScanner.FormatCitations(latexContent);

                        IMLA mlaBibliographyScanner = _bibliographyFactory.CreateMLA();
                        latexContent = mlaBibliographyScanner.FormatBibliographies(latexContent);

                        mlaScanner.ApplyMLAFormatting();
                    }

                    // ✅ Clean LaTeX content formatting
                    latexContent = latexContent
                        .Replace(@"\documentclass{article}", "\\documentclass{article}") 
                        .Replace(@"\title{", "\n\\title{")  
                        .Replace(@"\author{", "\n\\author{") 
                        .Replace(@"\date{", "\n\\date{") 
                        .Replace(@"\begin{document}", "\n\\begin{document}")
                        .Replace("\n\n", "\n");

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

            // ✅ Store JSON via interface
            _latexCompiler.SetUpdatedJson(updatedJson);
            return updatedJson;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] JSON deserialization error: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// ✅ Fetches the conversion status.
    /// Returns true if the converted JSON has been updated in memory.
    /// </summary>
    public bool fetchConversionStatus()
    {
        return _latexCompiler.fetchConversionStatus();
    }
    
    public void SetUpdatedJson(string convertedJson)
    {
        _latexCompiler.SetUpdatedJson(convertedJson);
    }

    public string GetUpdatedJson()
    {
        return _latexCompiler.GetUpdatedJson();
    }
}
