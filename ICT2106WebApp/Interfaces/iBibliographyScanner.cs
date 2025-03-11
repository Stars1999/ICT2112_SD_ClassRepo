public interface IBibliographyScanner
{
    // scans and formats a list of bibliography entries
    List<string> ScanBibliographies(List<BibliographyStyle> bibliographies);

    // applies additional formatting logic for APA/MLA (optional)
    void FormatBibliography();
}
