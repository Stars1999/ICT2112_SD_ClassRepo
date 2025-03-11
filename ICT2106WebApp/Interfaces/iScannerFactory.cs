public interface IScannerFactory
{
    //creates the scanner (APA/MLA)
    ICitationScanner CreateCitationScanner(string style);
    IBibliographyScanner CreateBibliographyScanner(string style);
}
