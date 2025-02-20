using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICT2106WebApp.Pages
{
    public class LoggerModel : PageModel
    {
        // Store log data
        public List<LogEntry> Logs { get; set; } = new List<LogEntry>();

        // Filter properties
        [BindProperty(SupportsGet = true)]
        public DateTime? FilterDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterLocation { get; set; }

        public List<string> AvailableLocations { get; set; } = new List<string>();

        public void OnGet()
        {
            // Dummy log data for UI testing
            var allLogs = new List<LogEntry>
            {
                new LogEntry { LogID = 1, LogTimestamp = DateTime.Now, LogDescription = "System started", LogLocation = "Server A" },
                new LogEntry { LogID = 2, LogTimestamp = DateTime.Now.AddDays(-1), LogDescription = "User login successful", LogLocation = "Dashboard" },
                new LogEntry { LogID = 3, LogTimestamp = DateTime.Now.AddDays(-2), LogDescription = "File uploaded", LogLocation = "File Manager" },
                new LogEntry { LogID = 4, LogTimestamp = DateTime.Now.AddDays(-3), LogDescription = "System update", LogLocation = "Server A" }
            };

            // Get unique log locations for dropdown
            AvailableLocations = allLogs.Select(l => l.LogLocation).Distinct().ToList();

            // Apply filters only if at least one filter is selected
            Logs = allLogs;

            if (FilterDate.HasValue)
            {
                Logs = Logs.Where(log => log.LogTimestamp.Date == FilterDate.Value.Date).ToList();
            }

            if (!string.IsNullOrEmpty(FilterLocation))
            {
                Logs = Logs.Where(log => log.LogLocation == FilterLocation).ToList();
            }
        }
    }

    // Log data model
    public class LogEntry
    {
        public int LogID { get; set; }
        public DateTime LogTimestamp { get; set; }
        public string LogDescription { get; set; }
        public string LogLocation { get; set; }
    }
}
