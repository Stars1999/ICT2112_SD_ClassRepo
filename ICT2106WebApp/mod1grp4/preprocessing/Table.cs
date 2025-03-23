using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ICT2106WebApp.mod1grp4
{
    public class Table
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId MongoId { get; set; }

        [BsonElement("tableId")]
        public int tableId { get; set; }

        [BsonElement("cells")]
        public List<TableCell> cells { get; set; }

        [BsonElement("tableCompletionState")]
        public bool tableCompletionState { get; set; }

        [BsonElement("latexOutput")]
        public string latexOutput { get; set; }

        public Table(int tableId, List<TableCell> cells, bool tableCompletionState, string latexOutput)
        {
            MongoId = ObjectId.GenerateNewId();
            this.tableId = tableId;
            this.cells = cells ?? new List<TableCell>();
            this.tableCompletionState = tableCompletionState;
            this.latexOutput = latexOutput;
        }
    }
}