using ICT2106WebApp.mod1Grp3;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Utilities
{
	public class CompositeNode : AbstractNode, INodeCollection
	{
		// [JsonProperty("children")]  // Add JsonProperty to ensure it's serialized
		[BsonElement("children")]
		private List<AbstractNode> children;

		public CompositeNode(
			int id,
			int nl,
			string nt,
			string c,
			List<Dictionary<string, object>> s
		)
			: base(id, nl, nt, c, s)
		{
			children = new List<AbstractNode>();
		}

		internal void AddChild(AbstractNode child)
		{
			children.Add(child);
		}

		internal List<AbstractNode> GetChildren()
		{
			return children;
		}

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

		public INodeIterator CreateIterator()
		{
			return new NodeIterator(children);
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
						{ "nodeType", GetNodeType() },
						{ "content", GetContent() },
						{ "styling", GetStyling() },
						{ "converted", IsConverted() }
					};
				case "peek":
					return new Dictionary<string, object>
					{
						{ "nodeLevel", GetNodeLevel() }
					};
				default:
					throw new ArgumentException("Invalid purpose specified.");
			}
		}
	}
}
