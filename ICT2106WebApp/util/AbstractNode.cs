using MongoDB.Bson; // Add this namespace
using MongoDB.Bson.Serialization.Attributes; // And this one
using MongoDB.Driver;
using System.Collections.Generic;
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
