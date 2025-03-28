using MongoDB.Driver;
using System;
using MongoDB.Bson.Serialization;
using ICT2106WebApp.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<TemplateDocument> Templates { get; private set; }

        public MongoDbContext()
        {
            try
            {
                // Register discriminators for AbstractNode hierarchy
                RegisterClassMaps();

                // Use a connection string that points to a local MongoDB instance
                var client = new MongoClient("mongodb+srv://2301915:eD8eNLFZtqLAZRdM@inf2112.e9qpy.mongodb.net/");

                // Create or get the database
                _database = client.GetDatabase("inf2112");

                // Get or create the templates collection
                Templates = _database.GetCollection<TemplateDocument>("Templates");

                Console.WriteLine("MongoDB connection established successfully");

                try
                {
                    // Try to get all templates without deserializing the Content
                    var filter = Builders<BsonDocument>.Filter.Empty;
                    var documents = _database.GetCollection<BsonDocument>("Templates").Find(filter).ToList();
                    Console.WriteLine($"Found {documents.Count} raw documents in the Templates collection");

                    foreach (var doc in documents)
                    {
                        Console.WriteLine($"Document ID: {doc["Id"]}, Name: {doc["TemplateName"]}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error listing raw documents: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MongoDB connection error: {ex.Message}");
                throw;
            }
        }

        private void RegisterClassMaps()
        {
            try
            {
                // First, register the TemplateDocument class
                if (!BsonClassMap.IsClassMapRegistered(typeof(TemplateDocument)))
                {
                    Console.WriteLine("Registering TemplateDocument class map...");
                    BsonClassMap.RegisterClassMap<TemplateDocument>(cm =>
                    {
                        cm.AutoMap();
                        cm.SetIgnoreExtraElements(true);
                        // Map the properties explicitly to ensure they match the MongoDB document structure
                        cm.MapIdProperty(c => c.MongoId);
                        cm.MapProperty(c => c.Id).SetElementName("Id");
                        cm.MapProperty(c => c.TemplateName).SetElementName("TemplateName");
                        // For the Content property, we'll set a special serializer later
                        cm.MapProperty(c => c.Content).SetElementName("Content");
                    });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering class maps: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}