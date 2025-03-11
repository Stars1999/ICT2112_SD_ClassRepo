using System;
using System.Text.Json.Serialization;

public class BibliographyStyle
{
    public int BibliographyTypeID { get; set; }

    [JsonPropertyName("BibliographyType")]
    public required string BibliographyType { get; set; }

    [JsonPropertyName("Author")]
    public required string Author { get; set; }

    [JsonPropertyName("Title")]
    public required string Title { get; set; }

    [JsonPropertyName("Publisher")]
    public required string Publisher { get; set; }

    [JsonPropertyName("Date")]
    public string? DateString { get; set; } = "2000-01-01"; // ✅ Default to prevent null errors

    [JsonIgnore]
    public DateTime Date => DateTime.TryParse(DateString, out var parsedDate) ? parsedDate : default;
}
