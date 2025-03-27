using MongoDB.Driver;
using ICT2106WebApp.mod2grp6.Template; // adjust if your namespace is different

namespace ICT2106WebApp.mod2grp6
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var client = new MongoClient("mongodb+srv://2301915:eD8eNLFZtqLAZRdM@inf2112.e9qpy.mongodb.net/");
            _database = client.GetDatabase("inf2112");
        }

        // Your template collection
        public IMongoCollection<TemplateDocument> Templates => _database.GetCollection<TemplateDocument>("Templates");

        public IMongoDatabase Database => _database;
    }
}
