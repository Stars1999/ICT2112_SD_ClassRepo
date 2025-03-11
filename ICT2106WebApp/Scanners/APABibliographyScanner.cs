public class APABibliographyScanner : IBibliographyScanner
{
    public List<string> ScanBibliographies(List<BibliographyStyle> bibliographies)
    {
        Console.WriteLine("[DEBUG] Processing APA bibliographies...");
        foreach (var b in bibliographies)
        {
            Console.WriteLine($"[DEBUG] Bibliography Found: {b.Author} - {b.Title}");
        }

        return bibliographies.Select(b => $"{b.Author} ({b.Date:yyyy}). {b.Title}. {b.Publisher}.").ToList();
    }

    public void FormatBibliography() { /* Additional APA formatting if needed */ }
}
