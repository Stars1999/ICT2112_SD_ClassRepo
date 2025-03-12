using ICT2106WebApp.Interfaces;

namespace ICT2106WebApp.Domain
{
    public class PDFQualityChecker : IPDFQualityChecker
    {
        private readonly List<QualityCheck> _qualityChecks;

        public PDFQualityChecker(List<QualityCheck> qualityChecks)
        {
            _qualityChecks = qualityChecks;
        }

        public QualityReport CheckPDFQuality(byte[] pdfContent)
        {
            QualityReport report = new QualityReport();

            foreach (var check in _qualityChecks)
            {
                check.Execute(pdfContent, report);
            }

            // Calculate overall quality score.
            // Deduct 5% for each issue, with a minimum score of 0.
            int penaltyPerIssue = 5;
            int issuesCount = report.Issues.Count;
            report.QualityScore = Math.Max(0, 100 - issuesCount * penaltyPerIssue);
            report.Metrics["QualityScore"] = report.QualityScore.ToString();

            return report;
        }

        public List<string> GetQualityMetrics()
        {
            return _qualityChecks.Select(q => q.MetricName).ToList();
        }
    }
}
