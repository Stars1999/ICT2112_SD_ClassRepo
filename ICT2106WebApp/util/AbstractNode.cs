namespace Utilities
{
	public abstract class AbstractNode
	{
		protected int nodeId;
		protected int nodeLevel;
		protected string nodeType = string.Empty;
		protected string content = string.Empty;
		protected List<Dictionary<string, object>> styling = new List<Dictionary<string, object>>();
		protected bool converted;

		protected AbstractNode(int id, int nl, string nt, string c, List<Dictionary<string, object>> s)
		{
			SetNodeId(id);
			SetNodeLevel(nl);
			SetNodeType(nt);
			SetContent(c);
			SetStyling(s);
			SetConverted(false);
		}

		// Abstract Methods
		public abstract int GetNodeId();
		public abstract int GetNodeLevel();
		public abstract string GetNodeType();
		public abstract string GetContent();
		public abstract List<Dictionary<string, object>> GetStyling();
		public abstract bool IsConverted();

		// Abstract Setters
		internal abstract void SetNodeId(int id);
		internal abstract void SetNodeLevel(int nl);
		internal abstract void SetNodeType(string nodeType);
		public abstract void SetContent(string content);
		public abstract void SetStyling(List<Dictionary<string, object>> styling);
		public abstract void SetConverted(bool converted);
	}
}
