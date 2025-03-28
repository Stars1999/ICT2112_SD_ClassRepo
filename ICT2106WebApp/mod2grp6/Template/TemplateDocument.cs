using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;
using MongoDB.Bson.Serialization;

namespace ICT2106WebApp.mod2grp6.Template
{
    [BsonIgnoreExtraElements]
    public class TemplateDocument
    {
        static TemplateDocument()
        {
            // Register class maps for deserialization
            if (!BsonClassMap.IsClassMapRegistered(typeof(SimpleNode)))
            {
                BsonClassMap.RegisterClassMap<SimpleNode>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(CompositeNode)))
            {
                BsonClassMap.RegisterClassMap<CompositeNode>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string MongoId { get; set; }
        
        [BsonElement("Id")]
        public string Id { get; set; }
        
        [BsonElement("TemplateName")]
        public string TemplateName { get; set; }
        
        [BsonElement("Content")]
        [BsonIgnoreIfNull]
        public List<SimpleNode> Content { get; set; } = new List<SimpleNode>();
        
        // Convert MongoDB document to Template object
        public Template ToTemplate()
        {
            // Cast SimpleNode list to AbstractNode list since SimpleNode inherits from AbstractNode
            List<AbstractNode> abstractNodes = Content?.Cast<AbstractNode>().ToList() ?? new List<AbstractNode>();
            return new Template(Id, TemplateName, abstractNodes);
        }
        
        // Create document from Template
        public static TemplateDocument FromTemplate(Template template)
        {
            // Cast AbstractNode list to SimpleNode list (this assumes all nodes are SimpleNodes)
            List<SimpleNode> simpleNodes = template.GetContent()?
                .Where(n => n is SimpleNode)
                .Cast<SimpleNode>()
                .ToList() ?? new List<SimpleNode>();
                
            return new TemplateDocument
            {
                Id = template.GetId(),
                TemplateName = template.GetTemplateName(),
                Content = simpleNodes
            };
        }
    }
}