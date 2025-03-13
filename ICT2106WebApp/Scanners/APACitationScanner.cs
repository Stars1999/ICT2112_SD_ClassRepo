using System.Text.RegularExpressions;

public class APACitationScanner : IAPA
{
public string FormatCitations(string latexContent)
{
    Console.WriteLine("[DEBUG] Formatting APA citations...");

    return Regex.Replace(latexContent, @"\\cite{(.*?)}", match =>
    {
        string[] citations = match.Groups[1].Value.Split(',');
        for (int i = 0; i < citations.Length; i++)
        {
            // Trim spaces and ensure correct APA formatting (Assumes citation keys use underscores)
            citations[i] = citations[i].Trim().Replace("_", " "); // Convert underscores to spaces
        }
        return "(" + string.Join("; ", citations) + ")";
    });
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
}
