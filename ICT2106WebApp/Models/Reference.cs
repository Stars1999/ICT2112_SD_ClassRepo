using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

public class Reference
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("documents")]
    public List<BibliographyDocument> Documents { get; set; } = new List<BibliographyDocument>();
}

public class BibliographyDocument
{
    [BsonElement("Title")]  // ✅ Match MongoDB field names exactly
    public string Title { get; set; }

    [BsonElement("Author")]
    public string Author { get; set; }

    [BsonElement("Date")]
    public string Date { get; set; }

    [BsonElement("LatexContent")]
    public string LatexContent { get; set; }
}
