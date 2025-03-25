using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

public class Reference
{
    [BsonId]  // tells Mongo this maps to _id
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }  // Handles "_id" field

    public List<BibliographyDocument> Documents { get; set; }
    public DateTime InsertedAt { get; set; }
    public string Source { get; set; }
}
