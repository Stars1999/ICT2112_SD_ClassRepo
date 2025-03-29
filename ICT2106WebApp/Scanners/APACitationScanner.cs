using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class APACitationScanner : IAPA
{
    public string FormatCitations(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting APA citations...");

        // ✅ Match inline citations like (Smith, 45) and replace with correct APA format (Smith, 2019)
        // Convert MLA-style: (Author PageNumber) ➡️ (Author, Year)
        latexContent = Regex.Replace(latexContent, @"\(([A-Z][a-z]+) (\d+)\)", match =>
        {
            string author = match.Groups[1].Value;
            string year = GetPublicationYear(author, latexContent);
            return $"({author}, {year})";
        });

        // Optional: reprocess (Author, wrongYear) to fix APA if needed
        latexContent = Regex.Replace(latexContent, @"\(([A-Z][a-z]+),\s*(\d{4})\)", match =>
        {
            string author = match.Groups[1].Value;
            string year = GetPublicationYear(author, latexContent);
            return $"({author}, {year})";
        });

        return latexContent;
    }

    public string FormatBibliographies(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting APA bibliography...");
        return latexContent.Replace(@"\bibliographystyle{plain}", @"\bibliographystyle{apalike}");
    }

    public void ApplyAPAFormatting()
    {
        Console.WriteLine("[DEBUG] Applying additional APA formatting...");
    }

    private string GetPublicationYear(string author, string latexContent)
    {
        var match = Regex.Match(latexContent, $@"{author}.*?(\d{{4}})");

        if (match.Success)
        {
            Console.WriteLine($"[INFO] Extracted year {match.Groups[1].Value} for author {author}");
            return match.Groups[1].Value; // Return extracted year
        }

        Console.WriteLine($"[WARNING] No year found for author {author}, defaulting to n.d.");
        return "n.d."; // Default to "no date" if no year is found
    }
}
