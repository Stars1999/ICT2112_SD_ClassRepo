public class MLACitationScanner : ICitationScanner
{
    private List<string> citationData = new();

    public List<string> ScanCitations(List<CitationStyle> citations)
    {
        foreach (var citation in citations)
        {
            citationData.Add($"{citation.CitationData[0]} {citation.CitationData[1]}.");
        }
        return citationData;
    }

    public void FormatCitation()
    {
        // Format MLA citations if necessary
    }
 
    public void SetCitationData(List<string> data) => citationData = data;

    public List<string> GetCitationData() => citationData;

    public void UpdateCitation()
    {
        // Logic to update citation data
    }
}
