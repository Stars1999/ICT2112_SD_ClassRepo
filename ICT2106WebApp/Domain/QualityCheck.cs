using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;

namespace ICT2106WebApp.Domain
{
    public abstract class QualityCheck : IQualityCheck
    {
        public abstract string MetricName { get; }
        public abstract void Execute(byte[] pdfContent, QualityReport report);
    }
}
   