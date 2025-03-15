using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class APACitationScanner : IAPA
{
    public string FormatCitations(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting APA citations...");

        // ✅ Match inline citations like (Smith, 45) and replace with correct APA format (Smith, 2019)
        latexContent = Regex.Replace(latexContent, @"\((\w+),\s*\d+\)", match =>
        {
            string author = match.Groups[1].Value; // Extract author name
            string year = GetPublicationYear(author, latexContent); // Get the correct year from bibliography
            return $"({author}, {year})"; // Correct APA format
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
        // ✅ Improved regex to match full bibliography entries like:
        // ✅ Smith, John. "Title of Paper." Journal Name, 2019.
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
