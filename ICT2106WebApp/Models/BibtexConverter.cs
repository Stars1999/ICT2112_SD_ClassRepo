using System;
using System.Collections.Generic;
using System.IO;
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

    public void ConvertAndPrint(string jsonFilePath)
    {
        Console.WriteLine($"[DEBUG] Reading JSON file: {jsonFilePath}");

        var jsonData = File.ReadAllText(jsonFilePath);
        using var document = JsonDocument.Parse(jsonData);
        var root = document.RootElement;

        if (!root.TryGetProperty("documents", out JsonElement documents) || documents.GetArrayLength() == 0)
        {
            Console.WriteLine("[ERROR] JSON data is invalid or contains no documents.");
            return;
        }

        Console.WriteLine($"[DEBUG] Found {documents.GetArrayLength()} LaTeX documents.");

        foreach (var doc in documents.EnumerateArray())
        {
            string title = doc.GetProperty("Title").GetString() ?? "Unknown Title";
            string author = doc.GetProperty("Author").GetString() ?? "Unknown Author";
            string citationStyle = doc.GetProperty("CitationStyle").GetString()?.ToLower() ?? "apa"; // Default to APA
            string latexContent = doc.GetProperty("LatexContent").GetString() ?? "";

            Console.WriteLine($"\nProcessing Document: {title} by {author} (Style: {citationStyle})");

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

                Console.WriteLine("\nConverted LaTeX Content:\n");
                Console.WriteLine(latexContent);
                Console.WriteLine("\n----------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
            }
        }
    }
}
