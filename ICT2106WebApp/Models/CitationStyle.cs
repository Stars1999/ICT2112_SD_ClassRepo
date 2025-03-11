public class CitationStyle
{
    public int CitationTypeID { get; set; }
    public required string CitationType { get; set; } = "APA";
    public string Author { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public List<string> CitationData { get; set; } = new();
}
