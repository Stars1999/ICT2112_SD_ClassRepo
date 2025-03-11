public class MLABibliographyScanner : IBibliographyScanner
{
    public List<string> ScanBibliographies(List<BibliographyStyle> bibliographies)
    {
        return bibliographies.Select(b => $"{b.Author}. \"{b.Title}.\" {b.Publisher}, {b.Date:yyyy}.").ToList();
    }

    public void FormatBibliography() { /* Additional MLA formatting if needed */ }
}
