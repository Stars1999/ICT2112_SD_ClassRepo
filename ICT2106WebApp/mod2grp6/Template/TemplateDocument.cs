using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using ICT2106WebApp.Utilities; // Add this namespace for AbstractNode

namespace ICT2106WebApp.mod2grp6.Template
{
    [BsonIgnoreExtraElements]
    public class TemplateDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string MongoId { get; set; }
        
        [BsonElement("Id")]
        public string Id { get; set; }
        
        [BsonElement("TemplateName")]
        public string TemplateName { get; set; }
        
        [BsonElement("Content")]
        public List<AbstractNode> Content { get; set; }
    }
}