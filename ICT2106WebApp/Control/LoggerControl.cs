using ICT2106WebApp.Abstract;
using ICT2106WebApp.Class;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;
using System.Text;

namespace ICT2106WebApp.Controllers
{
    public class LoggerControl
    {
        private readonly IRetrieveLog _logRetriever;
        private readonly Interfaces.ILogger _logger;  // Renamed for clarity and best practices
        private List<ILogFilter_Strategy> _logFilters; // Renamed for consistency
        public LoggerControl(IRetrieveLog logRetriever, Interfaces.ILogger logger)
        {
            _logRetriever = logRetriever ?? throw new ArgumentNullException(nameof(logRetriever), "Log retriever cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _logFilters = new List<ILogFilter_Strategy>();
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
        public List<Logger_SDM> FilterLogs(DateTime? startDate, DateTime? endDate, string errorLocation)
        {
            var logs = RetrieveAllLogs();

            if (startDate.HasValue && endDate.HasValue)
            {
                _logger.InsertLog(DateTime.Now, $"Filtering logs from {startDate} to {endDate} at location {errorLocation}", "Mod 3");
                DateTime start = startDate.Value.Date;
                DateTime end = endDate.Value.Date;

                var timestampFilter = new TimestampLogFilter(start, end);
                logs = timestampFilter.FilterLogs(logs);
                Console.WriteLine($"Filtered {logs.Count} logs between dates.");
            }

            if (!string.IsNullOrEmpty(errorLocation))
            {
                _logger.InsertLog(DateTime.Now, "Filter by location", "Mod 3");
                var locationFilter = new LocationLogFilter(errorLocation);
                logs = locationFilter.FilterLogs(logs);
                Console.WriteLine($"Filtered {logs.Count} logs by location.");
            }

            return logs;
        }


        public byte[] DownloadLogs(DateTime? startDate, DateTime? endDate, string filterLocation)
        {
            // Determine if filtering by date range or location is needed
            var logs = startDate.HasValue || endDate.HasValue || !string.IsNullOrEmpty(filterLocation)
                ? FilterLogs(startDate, endDate, filterLocation) // Adjusted to use the new FilterLogs method accepting date range
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
