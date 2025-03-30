using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Utilities
{
	public class CompositeNode : AbstractNode
	{

        // [JsonProperty("children")]  // Add JsonProperty to ensure it's serialized
		[BsonElement("children")]
		private List<AbstractNode> children;

		public CompositeNode(int id, int nl, string nt, string c, List<Dictionary<string, object>> s)
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

		public override int GetNodeId()
		{
			return nodeId;
		}

		public override int GetNodeLevel()
		{
			return nodeLevel;
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

		internal override void SetNodeLevel(int nl)
		{
			nodeLevel = nl;
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
