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
        _latexCompiler = latexCompiler;// ✅ Initialize LatexCompiler

        if (_preferredStyle != "apa" && _preferredStyle != "mla")
        {
            Console.WriteLine($"[WARNING] Invalid preferred style '{_preferredStyle}', defaulting to APA.");
            _preferredStyle = "apa";
        }
    }

    /// <summary>
    /// Changes the preferred citation style.
    /// </summary>
    public void SetPreferredStyle(string newStyle)
    {
        if (newStyle.ToLower() == "apa" || newStyle.ToLower() == "mla")
        {
            _preferredStyle = newStyle.ToLower();
            Console.WriteLine($"[INFO] Preferred citation style changed to {_preferredStyle.ToUpper()}.");
        }
        else
        {
            Console.WriteLine($"[ERROR] Unsupported style '{newStyle}'. Keeping current style ({_preferredStyle.ToUpper()}).");
        }
    }

    /// <summary>
    /// Converts citations and bibliography based on the preferred style.
    /// </summary>
    public string ConvertCitationsAndBibliography(string jsonData)
    {
        using var document = JsonDocument.Parse(jsonData);
        var root = document.RootElement;

        if (!root.TryGetProperty("documents", out JsonElement documents) || documents.GetArrayLength() == 0)
        {
            Console.WriteLine("[ERROR] No documents found in JSON.");
            return jsonData; // Return original JSON if nothing to process
        }

        var updatedDocuments = new List<object>();

        foreach (var doc in documents.EnumerateArray())
        {
            string title = doc.TryGetProperty("Title", out var titleProp) ? titleProp.GetString() ?? "Unknown Title" : "Unknown Title";
            string author = doc.TryGetProperty("Author", out var authorProp) ? authorProp.GetString() ?? "Unknown Author" : "Unknown Author";
            string latexContent = doc.TryGetProperty("LatexContent", out var latexProp) ? latexProp.GetString() ?? "" : "";

            try
            {
                latexContent = latexContent.Trim(); // ✅ Remove extra spaces

                // Apply the preferred citation style
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

                // ✅ Format LaTeX content properly
                latexContent = latexContent
                    .Replace(@"\documentclass{article}", "\\documentclass{article}") 
                    .Replace(@"\title{", "\n\\title{")  
                    .Replace(@"\author{", "\n\\author{") 
                    .Replace(@"\date{", "\n\\date{") 
                    .Replace(@"\begin{document}", "\n\\begin{document}")
                    .Replace("\n\n", "\n"); // ✅ Ensure no double blank lines

                updatedDocuments.Add(new
                {
                    Title = title,
                    Author = author,
                    Date = doc.TryGetProperty("Date", out var dateProp) ? dateProp.GetString() ?? "" : "",
                    LatexContent = latexContent.Trim() // ✅ Ensure no extra spaces
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to process '{title}'. Exception: {ex.Message}\n{ex.StackTrace}");
            }
        }

        string updatedJson = JsonSerializer.Serialize(new { documents = updatedDocuments }, new JsonSerializerOptions { WriteIndented = true });

        // ✅ Pass JSON via interface
        _latexCompiler.SetUpdatedJson(updatedJson);
        return updatedJson;
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
