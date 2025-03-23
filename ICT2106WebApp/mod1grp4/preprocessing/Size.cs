using MongoDB.Bson.Serialization.Attributes;    

namespace ICT2106WebApp.mod1grp4 {
    public class Size
    {
        [BsonElement("cellwidth")]
        public string cellwidth { get; set; }

        [BsonElement("cellheight")]
        public string cellheight { get; set; }
    }
}