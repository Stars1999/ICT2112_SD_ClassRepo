// FOR NOW BEFORE INTEGRATION WITH MOD 3
namespace ICT2106WebApp.mod1grp4
{
    public class TableAbstractNode
    {
        public string nodeID { get; set; }
        public List<Run> runs { get; set; }
    }

    public class Run
    {
        public List<Cell> runs { get; set; }
    }

    public class Cell
    {
        public string content { get; set; }
        public Dictionary<string, bool> styling { get; set; }
    }
}