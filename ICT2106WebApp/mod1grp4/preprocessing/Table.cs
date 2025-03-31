using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ICT2106WebApp.mod1grp4
{
    // Table (Joel - COMPLETED)
    public class Table
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        private ObjectId MongoId { get; set; } // MongoDB ID

        [BsonElement("tableId")]
        public int tableId { get; set; } // Represents nodeid in json

        [BsonElement("type")]
        public string type { get; set; } // Represents type in json

        [BsonElement("latexOutput")]
        public string latexOutput { get; set; } // Represents content in json (table level)

        [BsonElement("rows")]
        public List<TableRow> rows { get; set; } // Represents FIRST runs giving rows in json

        [BsonElement("tableCompletionState")]
        public bool tableCompletionState { get; set; } // NEW for us to track

        public Table(int tableId, List<TableRow> rows, bool tableCompletionState, string latexOutput)
        {
            MongoId = ObjectId.GenerateNewId();
            this.tableId = tableId;
            this.rows = rows ?? new List<TableRow>();
            this.tableCompletionState = tableCompletionState;
            this.latexOutput = latexOutput;
        }
    }
}