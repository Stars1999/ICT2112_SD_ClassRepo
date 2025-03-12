namespace ICT2106WebApp.Domain
{
    public class QualityReport
    {
        public bool IsSuccessful { get; set; }
        public List<string> Issues { get; set; } = new List<string>();
        public Dictionary<string, string> Metrics { get; set; } = new Dictionary<string, string>();

        // New property for overall quality score (0 to 100)
        public int QualityScore { get; set; }
    }
}
