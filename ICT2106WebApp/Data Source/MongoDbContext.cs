using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient("mongodb+srv://2301915:eD8eNLFZtqLAZRdM@inf2112.e9qpy.mongodb.net/"); // Replace with your MongoDB connection string
        _database = client.GetDatabase("inf2112"); // Replace with your database name
    }

    public IMongoCollection<Reference> References => _database.GetCollection<Reference>("BibTexEntry");
}


public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}

