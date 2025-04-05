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

		// CompositeNode Constructor
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

		// Getters
		protected override int GetNodeId()
		{
			return nodeId;
		}

		public int GetNodeIdWrapper()
		{
			return GetNodeId();
		}

		protected override int GetNodeLevel()
		{
			return nodeLevel;
		}

		protected override string GetNodeType()
		{
			return nodeType;
		}

		public string GetNodeTypeWrapper()
		{
			return GetNodeType();
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

		// Setters
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

		// Methods for managing children
		internal void AddChild(AbstractNode child)
		{
			children.Add(child);
		}

		internal List<AbstractNode> GetChildren()
		{
			return children;
		}

		// Create iterator for children
		public INodeIterator CreateIterator()
		{
			return new NodeIterator(children);
		}

		// Method that returns node data based on purpose
		public override Dictionary<string, object> GetNodeData(string purpose)
		{
			switch (purpose.ToLower())
			{
				case "treecreation":
					return new Dictionary<string, object>
					{
						{ "nodeLevel", GetNodeLevel() },
						{ "nodeType", GetNodeType() },
					};
				case "contentvalidation":
					return new Dictionary<string, object>
					{
						{ "nodeType", GetNodeType() },
						{ "content", GetContent() },
					};
				case "treestructurevalidation":
					return new Dictionary<string, object>
					{
						{ "nodeLevel", GetNodeLevel() },
						{ "nodeType", GetNodeType() },
					};
				case "treeprint":
					return new Dictionary<string, object>
					{
						{ "nodeId", GetNodeId() },
						{ "nodeType", GetNodeType() },
						{ "content", GetContent() },
						{ "styling", GetStyling() },
					};
				case "nodetraversal":
					return new Dictionary<string, object> { { "nodeType", GetNodeType() } };
				case "nodeinfo":
					return new Dictionary<string, object>
					{
						{ "nodeId", GetNodeId() },
						{ "nodeType", GetNodeType() },
						{ "content", GetContent() },
						{ "styling", GetStyling() },
						{ "converted", IsConverted() },
					};
				case "peek":
					return new Dictionary<string, object> { { "nodeLevel", GetNodeLevel() } };
				default:
					throw new ArgumentException("Invalid purpose specified.");
			}
		}

		// Method to set or change node data
		// public override void SetNodeData(
		// 	string content,
		// 	List<Dictionary<string, object>> styling,
		// 	bool? converted = null
		// )
		// {
		// 	// SetContent(content);
		// 	// SetStyling(styling);
		// 	// SetConverted(converted);
		// 	if (content != null)
		// 		SetContent(content);
		// 	if (styling != null)
		// 		SetStyling(styling);
		// 	if (converted.HasValue)
		// 		SetConverted(converted.Value);
		// }

		public override void SetNodeData(
			string content = null,
			// List<Dictionary<string,object>> styling,
			bool converted = false
		)
		{
			if (content != null)
			{
			SetContent(content);
			}
			if (converted != false)
			{
			SetConverted(converted);
			}
			// SetStyling(styling);
		}
	}
}
