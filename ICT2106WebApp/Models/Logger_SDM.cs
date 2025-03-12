using System;

namespace ICT2106WebApp.Models
{
    public class Logger_SDM
    {
        private int logID;
        private DateTime logTimestamp;
        private string logDescription;
        private string logLocation;

        // Constructor
        public Logger_SDM(DateTime logTimestamp, string logDescription, string logLocation)
        {
            this.logTimestamp = logTimestamp;
            this.logDescription = logDescription;
            this.logLocation = logLocation;
        }

        // Private Getters
        private int GetLogID() => logID;
        private DateTime GetLogTimestamp() => logTimestamp;
        private string GetLogDescription() => logDescription;
        private string GetLogLocation() => logLocation;

        // Private Setters
        private void SetLogID(int id) => logID = id;
        private void SetLogTimestamp(DateTime timestamp) => logTimestamp = timestamp;
        private void SetLogDescription(string description) => logDescription = description;
        private void SetLogLocation(string location) => logLocation = location;

        // Public Method to Update Log Data (Encapsulated)
        public void UpdateLog(int logID, DateTime logTimestamp, string logDescription, string logLocation)
        {
            SetLogID(logID);
            SetLogTimestamp(logTimestamp);
            SetLogDescription(logDescription);
            SetLogLocation(logLocation);
        }

        // Public Method to Retrieve Log Information (Encapsulated)
        public (int, DateTime, string, string) GetLogDetails()
        {
            return (GetLogID(), GetLogTimestamp(), GetLogDescription(), GetLogLocation());
        }
    }
}
