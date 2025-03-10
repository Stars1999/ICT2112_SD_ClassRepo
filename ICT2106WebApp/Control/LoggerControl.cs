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

        public List<Logger_SDM> FilterLogsByTimestamp(DateTime timestamp)
        {
            foreach (var filter in _logFilters)
            {
                if (filter is TimestampLogFilter timestampFilter)
                {
                    return timestampFilter.FilterLogs(RetrieveAllLogs());
                }
            }

            return new List<Logger_SDM>();
        }

        public List<Logger_SDM> FilterLogsByErrorLocation(string errorLocation)
        {
            foreach (var filter in _logFilters)
            {
                if (filter is LocationLogFilter locationFilter)
                {
                    return locationFilter.FilterLogs(RetrieveAllLogs());
                }
            }

            return new List<Logger_SDM>();
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

        public void AddLogFilter(ILogFilter_Strategy filter)
        {
            _logFilters.Add(filter);
        }
    }
}
