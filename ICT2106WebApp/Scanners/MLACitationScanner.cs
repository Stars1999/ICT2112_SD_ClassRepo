using System;
using System.Text.RegularExpressions;

public class MLACitationScanner : IMLA
{
    public string FormatCitations(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting MLA citations...");

        // Match inline citations like (Smith, 2019) or (Brown, 45) 
        latexContent = Regex.Replace(latexContent, @"\((\w+),\s*(\d{4}|\d+)\)", match =>
        {
            string author = match.Groups[1].Value;  // Extract author name
            string number = match.Groups[2].Value;  // Extract number (could be a year or page)

            // Ensure it is a **page number**, not a year
            if (int.TryParse(number, out int num) && num >= 1000)  
            {
                Console.WriteLine($"[WARNING] Detected a **year** ({number}) instead of a page number for {author}.");
                return $"({author})"; // MLA should not include years
            }

            return $"({author} {number})";  // Correct MLA format: (Author Page)
        });

        return latexContent;
    }

    public string FormatBibliographies(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting MLA bibliography...");
        return latexContent.Replace(@"\bibliographystyle{plain}", @"\bibliographystyle{mla}");
    }

    public void ApplyMLAFormatting()
    {
        Console.WriteLine("[DEBUG] Applying additional MLA formatting...");
    }
}
