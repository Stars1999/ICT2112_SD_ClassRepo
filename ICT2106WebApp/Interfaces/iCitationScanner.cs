public interface ICitationScanner
{
    // converts raw citation data into the format
    List<string> ScanCitations(List<CitationStyle> citations);

    // applies additional formatting logic for APA/MLA (optional)
    void FormatCitation();
}
