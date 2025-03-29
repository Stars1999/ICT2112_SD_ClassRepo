using System;
using System.Text.Json.Serialization;

public class BibliographyDocument
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Date { get; set; }
    public string LatexContent { get; set; }
    public string OriginalLatexContent { get; set; }
}

