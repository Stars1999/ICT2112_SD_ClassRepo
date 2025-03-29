using System.Text.RegularExpressions;
public class APABibliographyScanner : IAPA
{
    public string FormatCitations(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting APA citations...");
        return Regex.Replace(latexContent, @"\cite\{(.*?)\}", "($1)");

    }

    public string FormatBibliographies(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting APA bibliography...");
        latexContent = latexContent.Replace(" & ", " \\& ");
        
        return latexContent.Replace(@"\bibliographystyle{plain}", @"\bibliographystyle{apalike}");
    }

    public void ApplyAPAFormatting()
    {
        Console.WriteLine("[DEBUG] Applying additional APA bibliography formatting...");
    }
}
