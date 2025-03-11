using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class CitationStyle
{
    public int CitationTypeID { get; set; }

    [JsonPropertyName("CitationType")]
    public required string CitationType { get; set; }

    [JsonPropertyName("Author")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("Date")]
    public string? DateString { get; set; } = "2000-01-01"; // ✅ Default value to prevent null issues

    [JsonIgnore]
    public DateTime Date => DateTime.TryParse(DateString, out var parsedDate) ? parsedDate : default;

    [JsonPropertyName("CitationData")]
    public List<string> CitationData { get; set; } = new();
}
