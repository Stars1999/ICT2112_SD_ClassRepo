namespace Utilities
{    
    public class SimpleNode : AbstractNode {
        public SimpleNode(int id, string nt, string c, List<Dictionary<string, object>> s) : base(id, nt, c, s) {
        }
    
        public override int GetNodeId()
        {
            return nodeId;
        }

        public override string GetNodeType()
        {
            return nodeType;
        }

        public override string GetContent()
        {
            return content;
        }

        public override List<Dictionary<string, object>> GetStyling()
        {
            return styling;
        }

        public override bool IsConverted()
        {
            return converted;
        }

        internal override void SetNodeId(int id)
        {
            nodeId = id;
        }

        internal override void SetNodeType(string nt)
        {
            nodeType = nt;
        }
        public override void SetContent(string c)
        {
            content = c;
        }

        public override void SetStyling(List<Dictionary<string, object>> s)
        {
            styling = s;
        }

        public override void SetConverted(bool c)
        {
            converted = c;
        }
    }
}