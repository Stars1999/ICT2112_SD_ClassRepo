public class CitationScannerFactory
{

    // returns an instance of APA or MLA citation scanner
    public ICitationScanner CreateCitationScanner(string style)
    {
        return style.ToLower() switch
        {
            "apa" => new APACitationScanner(),
            "mla" => new MLACitationScanner(),
            _ => throw new ArgumentException($"Unsupported citation style: {style}")
        };
    }

    // use the correct scanner to process citations and return formatted output
    public List<string> FetchAllCitation(List<CitationStyle> citations, string style)
    {
        var scanner = CreateCitationScanner(style);
        return scanner.ScanCitations(citations);
    }
}



