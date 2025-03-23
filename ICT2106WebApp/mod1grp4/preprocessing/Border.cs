using MongoDB.Bson.Serialization.Attributes;

namespace ICT2106WebApp.mod1grp4 {
    public class Border
    {
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
    }
}