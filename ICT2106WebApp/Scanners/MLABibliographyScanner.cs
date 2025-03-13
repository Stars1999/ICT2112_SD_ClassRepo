public class MLABibliographyScanner : IMLA
{
    public string FormatCitations(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting MLA citations...");
        return latexContent.Replace(@"\cite{", "(").Replace("}", ")");

    }

    public string FormatBibliographies(string latexContent)
    {
        Console.WriteLine("[DEBUG] Formatting MLA bibliography...");
        return latexContent.Replace(@"\bibliographystyle{plain}", @"\bibliographystyle{mla}");
    }

    public void ApplyMLAFormatting()
    {
        Console.WriteLine("[DEBUG] Applying additional MLA bibliography formatting...");
    }
}
