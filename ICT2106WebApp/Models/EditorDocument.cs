using MongoDB.Bson.Serialization.Attributes;
using System;

public class EditorDocument
{
    [BsonId]
    public int DocumentID { get; set; }

    public string LatexContent { get; set; }

    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}
