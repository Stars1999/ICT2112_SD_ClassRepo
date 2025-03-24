using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ICT2106WebApp.mod1grp4
{
    // TableRow (Joel - COMPLETED)
    public class TableRow
    {
        [BsonElement("type")]
        public string type { get; set; } // Represents type in json for ROW level

        [BsonElement("content")]
        public string content { get; set; } // Represents content in json (table level)

        [BsonElement("cells")]
        public List<TableCell> cells { get; set; } // Represents SECOND runs giving cells in json

        public TableRow(string content, List<TableCell> cells)
        {
            this.content = content;
            this.cells = cells ?? new List<TableCell>();
        }
    }
}