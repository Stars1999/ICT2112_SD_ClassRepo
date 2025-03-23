using System.Collections.Generic;

namespace ICT2106WebApp.mod1grp4
{
    public class TableAbstractNode
    {
        public string nodeID { get; set; }
        public List<RowNode> runs { get; set; }
    }

    public class RowNode
    {
        public List<CellNode> runs { get; set; }
    }

    public class CellNode
    {
        public string content { get; set; }
        public Dictionary<string, object> styling { get; set; }
    }
}