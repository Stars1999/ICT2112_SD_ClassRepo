public class BibliographyScannerFactory : IScannerFactory
{
    public IAPA CreateAPA() => new APABibliographyScanner();
    
    public IMLA CreateMLA() => new MLABibliographyScanner();
}
