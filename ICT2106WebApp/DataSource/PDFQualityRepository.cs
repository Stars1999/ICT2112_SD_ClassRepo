using ICT2106WebApp.Domain;

namespace ICT2106WebApp.DataSource
{
    public class PDFQualityRepository
    {
        private readonly List<QualityReport> _reports = new List<QualityReport>();

        public void SaveReport(QualityReport report)
        {
            _reports.Add(report);
        }

        public List<QualityReport> GetReports()
        {
            return _reports;
        }
    }
}
