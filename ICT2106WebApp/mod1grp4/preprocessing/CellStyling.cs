using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ICT2106WebApp.mod1grp4 {    
    public class CellStyling
    {
        [BsonElement("bold")]
        public bool bold { get; set; }

        [BsonElement("italic")]
        public bool italic { get; set; }

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