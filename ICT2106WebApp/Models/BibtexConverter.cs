using System;
using System.Text.Json;

public class BibTeXConverter
{
    private readonly IScannerFactory _citationFactory;
    private readonly IScannerFactory _bibliographyFactory;

    public BibTeXConverter(IScannerFactory citationFactory, IScannerFactory bibliographyFactory)
    {
        _citationFactory = citationFactory;
        _bibliographyFactory = bibliographyFactory;
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
            string title = doc.GetProperty("Title").GetString() ?? "Unknown Title";
            string author = doc.GetProperty("Author").GetString() ?? "Unknown Author";
            string citationStyle = doc.GetProperty("CitationStyle").GetString()?.ToLower() ?? "apa";
            string latexContent = doc.GetProperty("LatexContent").GetString() ?? "";

            try
            {
                IAPA? apaScanner = citationStyle == "apa" ? _citationFactory.CreateAPA() : null;
                IMLA? mlaScanner = citationStyle == "mla" ? _citationFactory.CreateMLA() : null;

                if (apaScanner != null)
                {
                    latexContent = apaScanner.FormatCitations(latexContent);
                    latexContent = apaScanner.FormatBibliographies(latexContent);
                    apaScanner.ApplyAPAFormatting();
                }
                else if (mlaScanner != null)
                {
                    latexContent = mlaScanner.FormatCitations(latexContent);
                    latexContent = mlaScanner.FormatBibliographies(latexContent);
                    mlaScanner.ApplyMLAFormatting();
                }

                updatedDocuments.Add(new
                {
                    Title = title,
                    Author = author,
                    Date = doc.GetProperty("Date").GetString() ?? "",
                    CitationStyle = citationStyle,
                    LatexContent = latexContent
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
            }
        }

        // Convert updated documents to JSON string and return as variable
        return JsonSerializer.Serialize(new { documents = updatedDocuments }, new JsonSerializerOptions { WriteIndented = true });
    }
}
