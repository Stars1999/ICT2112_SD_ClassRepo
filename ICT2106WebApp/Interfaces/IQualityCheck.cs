using ICT2106WebApp.Models;

namespace ICT2106WebApp.Interfaces
{
    public interface IQualityCheck
    {
        string MetricName { get; }
        void Execute(byte[] pdfContent, QualityReport report);
    }
}