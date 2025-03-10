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
            var newLog = new Logger_SDM(logTimestamp, logDescription, logLocation);
            var bsonDocument = new BsonDocument
            {
                { "logTimestamp", newLog.GetLogDetails().Item2 },
                { "logDescription", newLog.GetLogDetails().Item3 },
                { "logLocation", newLog.GetLogDetails().Item4 }
            };
            _logsCollection.InsertOne(bsonDocument);
        }

        // Retrieves logs with optional filtering by date and location
        public List<Logger_SDM> RetrieveLogs(DateTime? filterDate, string filterLocation)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filters = new List<FilterDefinition<BsonDocument>>();

            if (filterDate.HasValue)
            {
                var dateFilter = builder.Or(
                    builder.Eq("LogTimestamp", filterDate.Value.ToUniversalTime()),
                    builder.Eq("logTimestamp", filterDate.Value.ToUniversalTime())
                );
                filters.Add(dateFilter);
            }

            if (!string.IsNullOrEmpty(filterLocation))
            {
                var locationFilter = builder.Or(
                    builder.Eq("LogLocation", filterLocation),
                    builder.Eq("logLocation", filterLocation)
                );
                filters.Add(locationFilter);
            }

            var combinedFilter = filters.Count > 0 ? builder.And(filters) : builder.Empty;

            var bsonLogs = _logsCollection.Find(combinedFilter).ToList();
            var projectedLogs = new List<Logger_SDM>();

            foreach (var bson in bsonLogs)
            {
                // Handle logTimestamp
                DateTime logTimestamp = DateTime.MinValue;
                if (bson.Contains("LogTimestamp"))
                    logTimestamp = bson["LogTimestamp"].ToUniversalTime();
                else if (bson.Contains("logTimestamp"))
                    logTimestamp = bson["logTimestamp"].ToUniversalTime();

                // Handle logDescription
                string logDescription = "No description";
                if (bson.Contains("LogDescription"))
                    logDescription = bson["LogDescription"].AsString;
                else if (bson.Contains("logDescription"))
                    logDescription = bson["logDescription"].AsString;

                // Handle logLocation
                string logLocation = "Unknown location";
                if (bson.Contains("LogLocation"))
                    logLocation = bson["LogLocation"].AsString;
                else if (bson.Contains("logLocation"))
                    logLocation = bson["logLocation"].AsString;

                if (logTimestamp != DateTime.MinValue)
                {
                    var log = new Logger_SDM(logTimestamp, logDescription, logLocation);
                    projectedLogs.Add(log);
                }
            }

            return projectedLogs;
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


        // Generates a CSV byte array for log download
        public byte[] DownloadLog()
        {
            var logs = _logsCollection.Find(Builders<BsonDocument>.Filter.Empty).ToList();
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("LogTimestamp,LogDescription,LogLocation");

            foreach (var bson in logs)
            {
                // Check for both "LogTimestamp" and "logTimestamp"
                DateTime logTimestamp = bson.Contains("LogTimestamp") ? bson["LogTimestamp"].ToUniversalTime() :
                                        bson.Contains("logTimestamp") ? bson["logTimestamp"].ToUniversalTime() :
                                        DateTime.MinValue;

                // Check for both "LogDescription" and "logDescription"
                string logDescription = bson.Contains("LogDescription") ? bson["LogDescription"].AsString :
                                        bson.Contains("logDescription") ? bson["logDescription"].AsString :
                                        "No description";

                // Check for both "LogLocation" and "logLocation"
                string logLocation = bson.Contains("LogLocation") ? bson["LogLocation"].AsString :
                                     bson.Contains("logLocation") ? bson["logLocation"].AsString :
                                     "Unknown location";

                // Append line to CSV if timestamp is valid
                if (logTimestamp != DateTime.MinValue)
                {
                    csvBuilder.AppendLine($"{logTimestamp},{logDescription},{logLocation}");
                }
            }

            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

    }
}
