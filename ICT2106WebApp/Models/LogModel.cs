using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ICT2106WebApp.Models
{
    public class LogModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Ensures MongoDB handles _id properly
        public string Id { get; set; }
        public DateTime LogTimestamp { get; set; }
        public string LogDescription { get; set; }
        public string LogLocation { get; set; }
    }
}
