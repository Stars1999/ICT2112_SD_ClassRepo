public class BibliographyStyle
{
    public int BibliographyTypeID { get; set; }

    public required string BibliographyType { get; set; } = "APA";
    public required string Author { get; set; }
    public required string Title { get; set; }
    public required string Publisher { get; set; }

    public DateTime Date { get; set; }
}

