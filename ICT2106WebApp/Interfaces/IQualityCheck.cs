namespace ICT2106WebApp.Interfaces
{
    public interface IQualityCheck
    {
        string MetricName { get; }
        void Execute(byte[] pdfContent, Domain.QualityReport report);
    }
}