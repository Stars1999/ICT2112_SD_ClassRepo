using ICT2106WebApp.Abstract;
using ICT2106WebApp.Class;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;
using System.Text;

namespace ICT2106WebApp.Control
{
    public class LoggerControl
    {
        private readonly IRetrieveLog _logRetriever;
        private readonly Interfaces.ILogger _logger;  // Renamed for clarity and best practices
        private List<ILogFilter_Strategy> _logFilters; // Renamed for consistency
        private readonly NotifyLogUpdate _notifyLogUpdate; // Renamed for clarity
        public LoggerControl(IRetrieveLog logRetriever, Interfaces.ILogger logger)
        {
            _logRetriever = logRetriever ?? throw new ArgumentNullException(nameof(logRetriever), "Log retriever cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _logFilters = new List<ILogFilter_Strategy>();
            _notifyLogUpdate = new NotifyLogUpdate();
        }
        public void Observer(ILogObserver observer)
        {
            _notifyLogUpdate.RegisterObserver(observer);
        }
        public void NotifyLogsUpdate(string updateMessage)
        {
            _notifyLogUpdate.NotifyObservers(updateMessage);
        }

        public void AddLogFilter(ILogFilter_Strategy filter)
        {
            _logFilters.Add(filter);
        }
        public List<Logger_SDM> RetrieveAllLogs()
        {
            if (_logRetriever == null)
            {
                throw new InvalidOperationException("Log retriever is not initialized.");
            }

            return _logRetriever.RetrieveAllLog();
        }
        public List<string> GetAvailableLocations()
        {
            return _logRetriever.GetAvailableLogLocations();
        }
        public void InsertLog(int logID, DateTime errorTimeStamp, string errorDescription, string errorLocation)
        {
            try
            {
                _logger.InsertLog(errorTimeStamp, errorDescription, errorLocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting log: {ex.Message}");
            }
        }
        public List<Logger_SDM> FilterLogs(DateTime? timestamp, string errorLocation)
        {
            var logs = RetrieveAllLogs();

            if (timestamp.HasValue)
            {
                var timestampFilter = new TimestampLogFilter(timestamp.Value);
                logs = timestampFilter.FilterLogs(logs);
            }

            if (!string.IsNullOrEmpty(errorLocation))
            {
                var locationFilter = new LocationLogFilter(errorLocation);
                logs = locationFilter.FilterLogs(logs);
            }

            return logs;
        }
        public byte[] DownloadLogs(DateTime? filterDate, string filterLocation)
        {
            var logs = filterDate.HasValue || !string.IsNullOrEmpty(filterLocation)
                ? FilterLogs(filterDate, filterLocation)
                : RetrieveAllLogs();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("LogTimestamp,LogDescription,LogLocation");

            foreach (var log in logs)
            {
                var details = log.GetLogDetails();
                string logTimestamp = details.Item2.ToString("yyyy-MM-dd HH:mm:ss");
                string logDescription = details.Item3;
                string logLocation = details.Item4;

                csvBuilder.AppendLine($"{logTimestamp},{logDescription},{logLocation}");
            }

            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        public void ClearLogs()
        {
            // Implement logic to clear logs
            Console.WriteLine("All logs have been cleared.");
        }
    }
}
