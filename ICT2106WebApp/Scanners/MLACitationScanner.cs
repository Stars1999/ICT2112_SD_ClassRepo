using System.Text.RegularExpressions;

public class MLACitationScanner : IMLA
{
public string FormatCitations(string latexContent)
    {
    Console.WriteLine("[DEBUG] Formatting MLA citations...");

    return Regex.Replace(latexContent, @"\\cite{(.*?)}", match =>
    {
        string citation = match.Groups[1].Value;
        string[] parts = citation.Split('_');

        string author = parts[0]; // Always extract the author
        string formattedCitation;

        if (parts.Length == 2) // If there are exactly two parts (Author_X)
        {
            string secondPart = parts[1];

            // If the second part is a 4-digit number (potential year), assume it's a year
            if (Regex.IsMatch(secondPart, @"^\d{4}$"))
            {
                formattedCitation = $"({author})"; // MLA format without a page number
            }
            else
            {
                formattedCitation = $"({author}, {secondPart})"; // MLA format with page number
            }
        }
        else if (parts.Length > 2 && int.TryParse(parts[^1], out int pageNumber))
        {
            formattedCitation = $"({author}, {pageNumber})"; // MLA format with page number
        }
        else
        {
            formattedCitation = $"({author})"; // Fallback case
        }

        return formattedCitation;
    });
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
