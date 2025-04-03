using ICT2106WebApp.Models;

namespace ICT2106WebApp.Interfaces
{
    public interface IPDFQualityChecker
    {
        QualityReport CheckPDFQuality(byte[] pdfContent);
    }
}
