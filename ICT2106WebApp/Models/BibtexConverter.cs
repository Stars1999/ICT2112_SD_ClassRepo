using System;
using System.Collections.Generic;
using System.Text.Json;

public class BibTeXConverter
{
    private readonly IScannerFactory _citationFactory;
    private readonly IScannerFactory _bibliographyFactory;
    private string _preferredStyle; // Default APA, can be changed

    public BibTeXConverter(IScannerFactory citationFactory, IScannerFactory bibliographyFactory, string preferredStyle = "apa")
    {
        _citationFactory = citationFactory;
        _bibliographyFactory = bibliographyFactory;
        _preferredStyle = preferredStyle.ToLower(); // Ensure lowercase for consistency

        if (_preferredStyle != "apa" && _preferredStyle != "mla")
        {
            Console.WriteLine($"[WARNING] Invalid preferred style '{_preferredStyle}', defaulting to APA.");
            _preferredStyle = "apa";
        }
    }

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

            // Use the preferred style (APA by default)
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

            // ✅ Fix blank lines
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

    return JsonSerializer.Serialize(new { documents = updatedDocuments }, new JsonSerializerOptions { WriteIndented = true });
}

}
