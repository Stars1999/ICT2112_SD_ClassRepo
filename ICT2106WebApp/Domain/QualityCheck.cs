namespace ICT2106WebApp.Domain
{
    public abstract class QualityCheck
    {
        public abstract string MetricName { get; }
        public abstract void Execute(byte[] pdfContent, QualityReport report);
    }
}
