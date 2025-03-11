public class MLACitationScanner : ICitationScanner
{
    public List<string> ScanCitations(List<CitationStyle> citations)
    {
        return citations.Select(c => $"{c.Author}. \"{string.Join(", ", c.CitationData)}\". {c.Date:yyyy}.").ToList();
    }

    public void FormatCitation() { /* Additional MLA citation formatting */ }
}
