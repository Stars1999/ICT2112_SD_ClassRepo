using System.Collections.Generic;
using MongoDB.Bson; // Add this namespace
using MongoDB.Bson.Serialization.Attributes; // And this one
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Utilities
{
	[BsonDiscriminator("AbstractNode")]
	[BsonKnownTypes(typeof(CompositeNode))]
	[BsonKnownTypes(typeof(SimpleNode))]
	public abstract class AbstractNode
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string MongoId { get; set; }

		// [JsonProperty("nodeId")]
		[BsonElement("nodeId")]
		protected int nodeId;

		// [JsonProperty("nodeLevel")]
		[BsonElement("nodeLevel")]
		protected int nodeLevel;

		// [JsonProperty("nodeType")]
		[BsonElement("nodeType")]
		protected string nodeType = string.Empty;

		// [JsonProperty("content")]
		[BsonElement("content")]
		protected string content = string.Empty;

		// [JsonProperty("styling")]
		[BsonElement("styling")]
		protected List<Dictionary<string, object>> styling = new List<Dictionary<string, object>>();

		// [JsonProperty("converted")]
		[BsonElement("converted")]
		protected bool converted;

		// Abstract Constructor
		protected AbstractNode(
			int id,
			int nl,
			string nt,
			string c,
			List<Dictionary<string, object>> s
		)
		{
			SetNodeId(id);
			SetNodeLevel(nl);
			SetNodeType(nt);
			SetContent(c);
			SetStyling(s);
			SetConverted(false);
		}

		// Abstract Getters
		protected abstract int GetNodeId();
		protected abstract int GetNodeLevel();
		protected abstract string GetNodeType();
		protected abstract string GetContent();
		protected abstract List<Dictionary<string, object>> GetStyling();
		protected abstract bool IsConverted();

		// Abstract Setters
		protected abstract void SetNodeId(int id);
		protected abstract void SetNodeLevel(int nl);
		protected abstract void SetNodeType(string nodeType);
		protected abstract void SetContent(string content);
		protected abstract void SetStyling(List<Dictionary<string, object>> styling);
		protected abstract void SetConverted(bool converted);

		// Abstract methods
		public abstract Dictionary<string, object> GetNodeData(string purpose);
		public abstract void SetNodeData(
			string content,
			bool converted
		);
	}
}
