namespace ICT2106WebApp.Utilities
{
    public abstract class AbstractNode
    {
        protected int nodeId;
        protected string nodeType = string.Empty;
        protected string content = string.Empty;
        protected List<Dictionary<string, object>> styling = new List<Dictionary<string, object>>();
        protected bool converted;

        protected AbstractNode(int id, string nt, string c, List<Dictionary<string, object>> s)
        {
            SetNodeId(id);
            SetNodeType(nt);
            SetContent(c);
            SetStyling(s);
            SetConverted(false);
        }

        // Abstract Methods
        public abstract int GetNodeId();
        public abstract string GetNodeType();
        public abstract string GetContent();
        public abstract List<Dictionary<string, object>> GetStyling();
        public abstract bool IsConverted();

        // Abstract Setters
        internal abstract void SetNodeId(int id);
        internal abstract void SetNodeType(string nodeType);
        public abstract void SetContent(string content);
        public abstract void SetStyling(List<Dictionary<string, object>> styling);
        public abstract void SetConverted(bool converted);
    }
}