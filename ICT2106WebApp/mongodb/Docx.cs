using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Docx // might need to change class diagram's Document class
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Title { get; set; }
	public string FileName { get; set; }
	public string ContentType { get; set; }
	public DateTime UploadDate { get; set; }
	public byte[] FileData { get; set; }
}
