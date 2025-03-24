using MongoDB.Bson.Serialization.Attributes;

namespace ICT2106WebApp.mod1grp4
{
    public class TableCell
    {
        [BsonElement("type")]
        public string type { get; set; }
        
        [BsonElement("content")]
        public string content { get; set; }

        [BsonElement("styling")]
        public CellStyling styling { get; set; }

        public TableCell(string content, CellStyling styling)
        {
            this.content = content;
            this.styling = styling ?? new CellStyling();
        }
    }
}