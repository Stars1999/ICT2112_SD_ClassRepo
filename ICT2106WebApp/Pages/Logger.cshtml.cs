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

            Logs = _loggerControl.FilterLogs(FilterDate, FilterLocation);
        }

        public IActionResult OnPostAddLog()
        {
            try
            {
                // Insert a test log (replace with actual data if needed)
                _loggerControl.InsertLog(0, DateTime.Now, "Hardcoded log entry", "System");

                // Notify observers (if implemented in the control class)
                _loggerControl.NotifyLogsUpdate("Log entry added at " + DateTime.Now);

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
            var csvData = _loggerControl.DownloadLogs(FilterDate, FilterLocation);
            return File(csvData, "text/csv", "Logs.csv");
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
