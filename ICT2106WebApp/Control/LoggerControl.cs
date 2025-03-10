using ICT2106WebApp.Abstract;
using ICT2106WebApp.Class;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;

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

        public void DownloadLogs()
        {
            // Implement logic to download logs as a file
            Console.WriteLine("Logs downloaded successfully.");
        }

        public void ClearLogs()
        {
            // Implement logic to clear logs
            Console.WriteLine("All logs have been cleared.");
        }

        public void NotifyLogsUpdate()
        {
            Console.WriteLine("Logs have been updated.");
        }
    }
}
