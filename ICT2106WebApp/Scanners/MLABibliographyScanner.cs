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

        // ✅ Change "\section{References}" to "\section{Works Cited}"
        latexContent = latexContent.Replace(@"\section{References}", @"\section{Works Cited}");

        // ✅ Ensure the correct bibliography style is applied
        latexContent = latexContent.Replace(@"\bibliographystyle{plain}", @"\bibliographystyle{mla}");
        
        latexContent = latexContent.Replace(" & ", " \\& ");

        return latexContent;
    }

    public void ApplyMLAFormatting()
    {
        Console.WriteLine("[DEBUG] Applying additional MLA bibliography formatting...");
    }
}
