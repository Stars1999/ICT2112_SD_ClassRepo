using ICT2106WebApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace ICT2106WebApp.Services
{
    public class LoggerService
    {
        private readonly IMongoCollection<LogModel> _logsCollection;

        public LoggerService()
        {
            var client = new MongoClient("mongodb+srv://inf2003:inf2003@sd-test-db.2pnic.mongodb.net/");
            var database = client.GetDatabase("LoggerTest");
            _logsCollection = database.GetCollection<LogModel>("LoggerTest");
        }

        public void AddLog(DateTime logTimestamp, string logDescription, string logLocation)
        {
            var newLog = new LogModel
            {
                LogTimestamp = logTimestamp,
                LogDescription = logDescription,
                LogLocation = logLocation
            };

            _logsCollection.InsertOne(newLog);
        }

        public List<LogModel> GetLogs(DateTime? filterDate, string filterLocation)
        {
            var filter = Builders<LogModel>.Filter.Empty;

            if (filterDate.HasValue)
            {
                filter &= Builders<LogModel>.Filter.Gte(log => log.LogTimestamp, filterDate.Value.Date) &
                          Builders<LogModel>.Filter.Lt(log => log.LogTimestamp, filterDate.Value.Date.AddDays(1));
            }

            if (!string.IsNullOrEmpty(filterLocation))
            {
                filter &= Builders<LogModel>.Filter.Eq(log => log.LogLocation, filterLocation);
            }

            return _logsCollection.Find(filter).ToList();
        }

        public List<string> GetAvailableLocations()
        {
            return _logsCollection.AsQueryable()
                                  .Select(log => log.LogLocation)
                                  .Distinct()
                                  .ToList();
        }

        public byte[] DownloadLog()
        {
            var logs = _logsCollection.Find(_ => true).ToList();
            StringBuilder csvBuilder = new StringBuilder();

            // Add CSV Header
            csvBuilder.AppendLine("LogTimestamp,LogDescription,LogLocation");

            // Add Log Entries
            foreach (var log in logs)
            {
                csvBuilder.AppendLine($"{log.LogTimestamp},{log.LogDescription},{log.LogLocation}");
            }

            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

    }
}