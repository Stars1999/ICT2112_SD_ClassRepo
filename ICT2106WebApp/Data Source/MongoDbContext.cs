using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        // var client = new MongoClient("mongodb+srv://2301915:eD8eNLFZtqLAZRdM@inf2112.e9qpy.mongodb.net/"); // Replace with your MongoDB connection string
        // _database = client.GetDatabase("inf2112"); // Replace with your database name
        var client = new MongoClient("mongodb://localhost:27017/"); // aaron version
        _database = client.GetDatabase("ICT2112");
    }

    public IMongoCollection<Reference> References => _database.GetCollection<Reference>("BibTexEntry");
    public IMongoDatabase Database => _database;
    //public IMongoCollection<Reference> References => _database.GetCollection<Reference>("References");
    public IMongoCollection<ErrorStyle> ErrorStyles => _database.GetCollection<ErrorStyle>("ErrorStyles");
}


public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}

