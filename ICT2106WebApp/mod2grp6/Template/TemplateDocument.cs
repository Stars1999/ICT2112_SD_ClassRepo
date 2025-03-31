using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using ICT2106WebApp.Utilities;

//used to map mongodb json to template entity class due to abstractnode being weird
namespace ICT2106WebApp.mod2grp6.Template
{
    [BsonIgnoreExtraElements]
    public class TemplateDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }

        [BsonElement("Id")]
        public string Id { get; set; }

        [BsonElement("TemplateName")]
        public string TemplateName { get; set; }
        
        // Use NodeDocument for MongoDB serialization/deserialization
        [BsonElement("Content")]
        public List<NodeDocument> Content { get; set; }
        
        // Convert NodeDocument list to AbstractNode list for application use
        [BsonIgnore]
        public List<AbstractNode> AbstractContent 
        { 
            get
            {
                if (Content == null) return new List<AbstractNode>();
                return Content.Select(n => new SimpleNode(
                    n.NodeId,
                    n.NodeType,
                    n.Content,
                    n.Styling) as AbstractNode).ToList();
            }
        }

        public static TemplateDocument FromTemplate(Template template)
        {
            if (template == null) return null;

            // Create a list of NodeDocuments
            var nodeDocuments = template.getContent()
                .Select(node => new NodeDocument
                {
                    NodeId = node.GetNodeId(),
                    NodeType = node.GetNodeType(),
                    Content = node.GetContent(),
                    Styling = node.GetStyling()
                })
                .ToList();

            return new TemplateDocument
            {
                Id = template.getId(),
                TemplateName = template.getTemplateName(),
                Content = nodeDocuments
            };
        }
    }

    // This class directly maps to the MongoDB document structure
    [BsonIgnoreExtraElements]
    public class NodeDocument
    {
        [BsonElement("NodeId")]
        public int NodeId { get; set; }

        [BsonElement("NodeType")]
        public string NodeType { get; set; }

        [BsonElement("Content")]
        public string Content { get; set; }

        [BsonElement("Styling")]
        public List<Dictionary<string, object>> Styling { get; set; }
    }
}