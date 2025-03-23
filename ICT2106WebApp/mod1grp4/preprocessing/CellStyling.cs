using MongoDB.Bson.Serialization.Attributes;

namespace ICT2106WebApp.mod1grp4 {    
    public class CellStyling
    {
        [BsonElement("bold")]
        public bool bold { get; set; }

        [BsonElement("italic")]
        public bool italic { get; set; }

        [BsonElement("border")]
        public Border border { get; set; }

        [BsonElement("size")]
        public Size size { get; set; }

        [BsonElement("backgroundcolor")]
        public string backgroundcolor { get; set; }

        public CellStyling()
        {
            border = new Border();
            size = new Size();
        }
    }
}