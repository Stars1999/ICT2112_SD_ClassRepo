using MongoDB.Bson.Serialization.Attributes;

namespace ICT2106WebApp.mod1grp4 {    
    // CellStyling (Joel - COMPLETED)
    public class CellStyling
    {
        [BsonElement("bold")]
        public bool bold { get; set; }

        [BsonElement("italic")]
        public bool italic { get; set; }

        [BsonElement("underline")]
        public bool underline { get; set; }

        [BsonElement("highlight")]
        public string highlight { get; set; }

        [BsonElement("textcolor")]
        public string textcolor { get; set; }


        [BsonElement("fontsize")]
        public int fontsize { get; set; }

        [BsonElement("horizontalalignment")]
        public string horizontalalignment { get; set; }

        [BsonElement("border")]
        public Dictionary<string, string> border { get; set; }

        [BsonElement("size")]
        public Dictionary<string, string> size { get; set; }

        [BsonElement("backgroundcolor")]
        public string backgroundcolor { get; set; }

        public CellStyling()
        {
            border = new Dictionary<string, string>();
            size = new Dictionary<string, string>();
        }
    }
}