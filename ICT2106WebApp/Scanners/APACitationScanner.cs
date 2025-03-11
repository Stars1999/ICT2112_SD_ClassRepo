public class APACitationScanner : ICitationScanner
{
    private List<string> citationData = new();

    // formats citations in APA style
    public List<string> ScanCitations(List<CitationStyle> citations)
    {
        foreach (var citation in citations)
        {
            citationData.Add($"({citation.CitationData[0]}, {citation.CitationData[1]})");
        }
        return citationData;
    }

    public void FormatCitation()
    {
        // Format APA citations if necessary
    }

    public void SetCitationData(List<string> data) => citationData = data;
    
    public List<string> GetCitationData() => citationData;

    public void UpdateCitation()
    {
        // Logic to update citation data
    }
}
