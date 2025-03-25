using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient("mongodb://localhost:27017/"); // Replace with your MongoDB connection string
        _database = client.GetDatabase("ICT2112"); // Replace with your database name
        // var client = new MongoClient("mongodb://localhost:27017/"); // aaron version
        // _database = client.GetDatabase("ICT2112");
    }

    public IMongoCollection<Reference> References => _database.GetCollection<Reference>("BibTexEntry");
    public IMongoDatabase Database => _database;
    //public IMongoCollection<Reference> References => _database.GetCollection<Reference>("References");
}


public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}

