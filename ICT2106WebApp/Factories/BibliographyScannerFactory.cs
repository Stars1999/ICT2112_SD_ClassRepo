public class BibliographyScannerFactory
{
    // returns an instance of APA or MLA 
    public IBibliographyScanner CreateBibliographyScanner(string style)
    {
        return style.ToLower() switch
        {
            "apa" => new APABibliographyScanner(),
            "mla" => new MLABibliographyScanner(),
            _ => throw new ArgumentException($"Unsupported bibliography style: {style}")
        };
    }

    // use the correct scanner to process bib and return formatted output
    public List<string> FetchAllBibliography(List<BibliographyStyle> bibliographies, string style)
    {
        var scanner = CreateBibliographyScanner(style);
        return scanner.ScanBibliographies(bibliographies);
    }
}
