public class CitationScannerFactory : IScannerFactory
{
    public IAPA CreateAPA() => new APACitationScanner();
    
    public IMLA CreateMLA() => new MLACitationScanner();
}
