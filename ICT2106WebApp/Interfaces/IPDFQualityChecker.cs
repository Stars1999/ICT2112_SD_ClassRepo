namespace ICT2106WebApp.Interfaces
{
    public interface IPDFQualityChecker
    {
        Domain.QualityReport CheckPDFQuality(byte[] pdfContent);
    }
}
