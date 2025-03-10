using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.Models;
using ICT2106WebApp.Control;
using System;
using System.Collections.Generic;

namespace ICT2106WebApp.Pages
{
    public class Logger : PageModel
    {
        private readonly LoggerControl _loggerControl;

        // Injecting LoggerControl instead of LoggerGateway_TDG
        public Logger(LoggerControl loggerControl)
        {
            _loggerControl = loggerControl;
        }

        public List<Logger_SDM> Logs { get; set; } = new List<Logger_SDM>();
        public List<string> AvailableLocations { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public DateTime? FilterDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterLocation { get; set; }

        public void OnGet()
        {
            // Retrieve all logs first
            Logs = _loggerControl.RetrieveAllLogs();

            AvailableLocations = _loggerControl.GetAvailableLocations();

            // Apply filters if provided
            if (FilterDate.HasValue)
            {
                Logs = _loggerControl.FilterLogsByTimestamp(FilterDate.Value);
            }

            if (!string.IsNullOrEmpty(FilterLocation)) // Filtering by location
            {
                Logs = _loggerControl.FilterLogsByErrorLocation(FilterLocation);
            }
        }

        public IActionResult OnPostAddLog()
        {
            try
            {
                // Insert a test log (replace with actual data if needed)
                _loggerControl.InsertLog(0, DateTime.Now, "Hardcoded log entry", "System");

                // Notify observers (if implemented in the control class)
                _loggerControl.NotifyLogsUpdate();

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding log: {ex.Message}");
                return Content("An error occurred while adding the log.");
            }
        }

        public IActionResult OnGetDownloadLog()
        {
            try
            {
                // Implement download logic within LoggerControl
                _loggerControl.DownloadLogs();

                return Content("Logs downloaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating log file: {ex.Message}");
                return Content("An error occurred while generating the log file.");
            }
        }

        public IActionResult OnPostClearLogs()
        {
            try
            {
                _loggerControl.ClearLogs();
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing logs: {ex.Message}");
                return Content("An error occurred while clearing logs.");
            }
        }
    }
}
