using ICT2106WebApp.Abstract;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICT2106WebApp.Data
{
    public class LoggerGateway_TDG : Interfaces.ILogger, Interfaces.IRetrieveLog
    {
        private readonly IMongoCollection<BsonDocument> _logsCollection;  // Ensure it's BsonDocument for flexible handling
        private NotifyLogUpdate _notifier = new NotifyLogUpdate();

        public LoggerGateway_TDG()
        {
            var client = new MongoClient("mongodb+srv://inf2003:inf2003@sd-test-db.2pnic.mongodb.net/");
            var database = client.GetDatabase("LoggerTest");
            _logsCollection = database.GetCollection<BsonDocument>("LoggerTest"); // Corrected to BsonDocument
        }

        // Implements IRetrieveLog
        public List<Logger_SDM> RetrieveAllLog()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var bsonLogs = _logsCollection.Find(filter).ToList();
            var projectedLogs = new List<Logger_SDM>();

            foreach (var bson in bsonLogs)
            {
                // Check for both "LogTimestamp" and "logTimestamp"
                DateTime logTimestamp = DateTime.MinValue;
                if (bson.Contains("LogTimestamp"))
                {
                    logTimestamp = bson["LogTimestamp"].ToUniversalTime();
                }
                else if (bson.Contains("logTimestamp"))
                {
                    logTimestamp = bson["logTimestamp"].ToUniversalTime();
                }

                string logDescription = bson.Contains("LogDescription") ? bson["LogDescription"].AsString :
                                        bson.Contains("logDescription") ? bson["logDescription"].AsString :
                                        "No description";

                string logLocation = bson.Contains("LogLocation") ? bson["LogLocation"].AsString :
                                     bson.Contains("logLocation") ? bson["logLocation"].AsString :
                                     "Unknown location";

                if (logTimestamp > DateTime.MinValue) // Ensuring that a valid timestamp is found
                {
                    var log = new Logger_SDM(logTimestamp, logDescription, logLocation);
                    projectedLogs.Add(log);
                }
            }

            return projectedLogs;
        }
        // Implements ILogger Interface for inserting logs
        public void InsertLog(DateTime logTimestamp, string logDescription, string logLocation)
        {
            var newLog = new Logger_SDM(logTimestamp.AddHours(8), logDescription, logLocation);
            var bsonDocument = new BsonDocument
            {
                { "LogTimestamp", newLog.GetLogDetails().Item2 },
                { "LogDescription", newLog.GetLogDetails().Item3 },
                { "LogLocation", newLog.GetLogDetails().Item4 }
            };
            _logsCollection.InsertOne(bsonDocument);
        }
        public List<string> GetAvailableLogLocations()
        {
            var logs = _logsCollection.Find(new BsonDocument())
                                      .Project(Builders<BsonDocument>.Projection.Include("LogLocation").Exclude("_id"))
                                      .ToList();

            var locations = new HashSet<string>();

            foreach (var bson in logs)
            {
                if (bson.Contains("LogLocation"))
                    locations.Add(bson["LogLocation"].AsString);
                else if (bson.Contains("logLocation"))
                    locations.Add(bson["logLocation"].AsString);
            }

            return locations.ToList();
        }
        public void RegisterObserver(ILogObserver observer)
        {
            _notifier.RegisterObserver(observer);
        }
        public void DeregisterObserver(ILogObserver observer)
        {
            _notifier.DeregisterObserver(observer);
        }
        public void NotifyObservers(string message)
        {
            _notifier.NotifyObservers(message);
        }
    }
}
