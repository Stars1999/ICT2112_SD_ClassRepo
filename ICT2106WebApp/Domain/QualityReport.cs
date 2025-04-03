namespace ICT2106WebApp.Domain
{
    public class QualityReport
    {
        private bool isSuccessful;
        private List<string> issues = new List<string>();
        private Dictionary<string, string> metrics = new Dictionary<string, string>();
        private int qualityScore;

        // Constructor
        public QualityReport()
        {
            // Default constructor
        }


        // Private Getters
        private bool GetIsSuccessful() => isSuccessful;
        private List<string> GetIssues() => issues;
        private Dictionary<string, string> GetMetrics() => metrics;
        private int GetQualityScore() => qualityScore;

        // Private Setters
        private void SetIsSuccessful(bool successful) => isSuccessful = successful;
        private void SetIssues(List<string> issueList) => issues = issueList;
        private void SetMetrics(Dictionary<string, string> metricDict) => metrics = metricDict;
        private void SetQualityScore(int score) => qualityScore = score;

        // Public Method to Update Quality Report Data
        public void UpdateQualityReport(bool isSuccessful, List<string> issues, Dictionary<string, string> metrics, int qualityScore)
        {
            SetIsSuccessful(isSuccessful);
            SetIssues(issues);
            SetMetrics(metrics);
            SetQualityScore(qualityScore);
        }

        // Public Method to Retrieve Quality Report Information 
        public (bool, List<string>, Dictionary<string, string>, int) GetQualityReportDetails()
        {
            return (GetIsSuccessful(), GetIssues(), GetMetrics(), GetQualityScore());
        }
    }
}
