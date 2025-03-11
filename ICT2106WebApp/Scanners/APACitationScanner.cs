public class APACitationScanner : ICitationScanner
{
    public List<string> ScanCitations(List<CitationStyle> citations)
    {
        Console.WriteLine("[DEBUG] Processing APA citations...");
        foreach (var c in citations)
        {
            Console.WriteLine($"[DEBUG] Citation Found: {c.Author} - {c.Date}");
        }

        return citations.Select(c => $"({c.Author}, {c.Date:yyyy})").ToList();
    }

    public void FormatCitation() { /* Additional APA citation formatting */ }
}
