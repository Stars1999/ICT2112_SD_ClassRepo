using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.Services;
using ICT2106WebApp.Models;
using System;
using System.Collections.Generic;

namespace ICT2106WebApp.Pages
{
    public class Logger : PageModel
    {
        private readonly LoggerService _loggerService;

        public Logger(LoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public List<LogModel> Logs { get; set; } = new List<LogModel>();
        public List<string> AvailableLocations { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public DateTime? FilterDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterLocation { get; set; }

        public void OnGet()
        {
            Logs = _loggerService.GetLogs(FilterDate, FilterLocation);
            AvailableLocations = _loggerService.GetAvailableLocations(); // Ensure this populates locations
        }

        public IActionResult OnPostAddLog()
        {
            _loggerService.AddLog(DateTime.Now, "Hardcoded log entry", "System");
            return RedirectToPage();
        }
    }
}