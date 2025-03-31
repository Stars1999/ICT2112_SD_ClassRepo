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


        [BsonElement("topstyle")]
        public string topstyle { get; set; }

        [BsonElement("bottomstyle")]
        public string bottomstyle { get; set; }

        [BsonElement("leftstyle")]
        public string leftstyle { get; set; }

        [BsonElement("rightstyle")]
        public string rightstyle { get; set; }

        [BsonElement("topwidth")]
        public string topwidth { get; set; }

        [BsonElement("bottomwidth")]
        public string bottomwidth { get; set; }

        [BsonElement("leftwidth")]
        public string leftwidth { get; set; }

        [BsonElement("rightwidth")]
        public string rightwidth { get; set; }

        [BsonElement("topcolor")]
        public string topcolor { get; set; }

        [BsonElement("bottomcolor")]
        public string bottomcolor { get; set; }

        [BsonElement("leftcolor")]
        public string leftcolor { get; set; }

        [BsonElement("rightcolor")]
        public string rightcolor { get; set; }

        [BsonElement("bordertopstyle")]
        public string bordertopstyle { get; set; }

        [BsonElement("borderbottomstyle")]
        public string borderbottomstyle { get; set; }

        [BsonElement("borderleftstyle")]
        public string borderleftstyle { get; set; }

        [BsonElement("borderrightstyle")]
        public string borderrightstyle { get; set; }

        [BsonElement("bordertopwidth")]
        public string bordertopwidth { get; set; }

        [BsonElement("borderbottomwidth")]
        public string borderbottomwidth { get; set; }



        [BsonElement("cellWidth")]
        public string cellWidth { get; set; }

        [BsonElement("rowHeight")]
        public string rowHeight { get; set; }

        [BsonElement("backgroundcolor")]
        public string backgroundcolor { get; set; }

        public CellStyling()
        {
            // border = new Dictionary<string, string>();
            // size = new Dictionary<string, string>();
        }

    }
}