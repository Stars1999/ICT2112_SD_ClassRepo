using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

public class BibTeXConverter
{
    public void ConvertAndPrint(string jsonFilePath)
    {
        Console.WriteLine($"[DEBUG] Reading JSON file: {jsonFilePath}");

        // Read raw JSON
        var jsonData = File.ReadAllText(jsonFilePath);
        
        // Parse JSON dynamically
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
            string date = doc.GetProperty("Date").GetString() ?? "Unknown Date";
            string latexContent = doc.GetProperty("LatexContent").GetString() ?? "";

            Console.WriteLine($"\nProcessing Document: {title} by {author}");

            // Convert citations and references
            latexContent = ConvertCitations(latexContent);
            latexContent = ConvertReferences(latexContent);

            Console.WriteLine("\nConverted LaTeX Content:\n");
            Console.WriteLine(latexContent);
            Console.WriteLine("\n----------------------------------------");
        }
    }

    private string ConvertCitations(string latexContent)
    {
        Console.WriteLine("[DEBUG] Converting citations...");

        // Convert numeric citations like [1], [2] → \cite{ref1}, \cite{ref2}
        var citationPattern = new Regex(@"\\cite\{(\d+)\}");
        latexContent = citationPattern.Replace(latexContent, match =>
            {
                return match.Groups[1].Value switch
                    {
                        "1" => @"\cite{Brown_2021}",
                        "2" => @"\cite{Taylor_2018}",
                        _ => match.Value // If no match, keep the original
                    };
            });


        // Convert author-year citations like "Doe et al. (2021)" → \cite{Doe_2021}
        var authorYearPattern = new Regex(@"(\w+ et al\. \(\d{4}\))");
        latexContent = authorYearPattern.Replace(latexContent, match => $@"\cite{{{match.Groups[1].Value.Replace(" ", "_")}}}");

        return latexContent;
    }

    private string ConvertReferences(string latexContent)
    {
        Console.WriteLine("[DEBUG] Converting references...");

        // Detect reference sections and replace them with BibTeX
        var referencePattern = new Regex(@"\\section\{References\}([\s\S]+?)\\end\{document\}", RegexOptions.IgnoreCase);

        latexContent = referencePattern.Replace(latexContent, match =>
        {
            string referencesText = match.Groups[1].Value.Trim();
            string bibtexEntries = GenerateBibTeX(referencesText);
            return $"\n\\bibliographystyle{{plain}}\n\\bibliography{{references}}\n\\end{{document}}";
        });

        return latexContent;
    }

    private string GenerateBibTeX(string referencesText)
    {
        Console.WriteLine("[DEBUG] Generating BibTeX entries...");

        string[] references = referencesText.Split("\n");
        List<string> bibtexEntries = new List<string>();

        foreach (var reference in references)
        {
            if (string.IsNullOrWhiteSpace(reference)) continue;

            string refKey = "ref" + Guid.NewGuid().ToString("N").Substring(0, 6); // Unique reference key
            bibtexEntries.Add($"@article{{{refKey},\n  author = {{Unknown}},\n  title = {{{reference}}},\n  journal = {{Unknown Journal}},\n  year = {{2024}}\n}}");
        }

        return string.Join("\n\n", bibtexEntries);
    }
}
