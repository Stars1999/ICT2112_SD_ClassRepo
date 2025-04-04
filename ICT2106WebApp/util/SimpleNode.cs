namespace Utilities
{
	public class SimpleNode : AbstractNode
	{
		public SimpleNode(int id, int nl, string nt, string c, List<Dictionary<string, object>> s)
			: base(id, nl, nt, c, s) { }

		protected override int GetNodeId()
		{
			return nodeId;
		}

		protected override int GetNodeLevel()
		{
			return nodeLevel;
		}

		protected override string GetNodeType()
		{
			return nodeType;
		}

		protected override string GetContent()
		{
			return content;
		}

		protected override List<Dictionary<string, object>> GetStyling()
		{
			return styling;
		}

		protected override bool IsConverted()
		{
			return converted;
		}

		protected override void SetNodeId(int id)
		{
			nodeId = id;
		}

		protected override void SetNodeLevel(int nl)
		{
			nodeLevel = nl;
		}

		protected override void SetNodeType(string nt)
		{
			nodeType = nt;
		}

		protected override void SetContent(string c)
		{
			content = c;
		}

		protected override void SetStyling(List<Dictionary<string, object>> s)
		{
			styling = s;
		}

		protected override void SetConverted(bool c)
		{
			converted = c;
		}

		public override Dictionary<string, object> GetNodeData(string purpose)
		{
			switch (purpose.ToLower())
			{
				case "treecreation":
					return new Dictionary<string, object>
					{
						{ "nodeLevel", GetNodeLevel() },
						{ "nodeType", GetNodeType() }
					};
				case "contentvalidation":
					return new Dictionary<string, object>
					{
						{ "nodeType", GetNodeType() },
						{ "content", GetContent() }
					};
				case "treestructurevalidation":
					return new Dictionary<string, object>
					{
						{ "nodeLevel", GetNodeLevel() },
						{ "nodeType", GetNodeType() }
					};
				case "treeprint":
					return new Dictionary<string, object>
					{
						{ "nodeId", GetNodeId() },
						{ "nodeType", GetNodeType() },
						{ "content", GetContent() },
						{ "styling", GetStyling() }
					};
				case "nodetraversal":
					return new Dictionary<string, object>
					{
						{ "nodeType", GetNodeType() }
					};
				case "nodeinfo":
					return new Dictionary<string, object>
					{
						{ "nodeId", GetNodeId() },
						{ "nodeLevel", GetNodeLevel() },
						{ "nodeType", GetNodeType() },
						{ "content", GetContent() },
						{ "styling", GetStyling() },
						{ "converted", IsConverted() }
					};
				default:
					throw new ArgumentException("Invalid purpose specified.");
			}
		}

		public override void SetNodeData(string content, List<Dictionary<string, object>> styling, bool converted)
		{
			SetContent(content);
			SetStyling(styling);
			SetConverted(converted);
		}
	}
}
