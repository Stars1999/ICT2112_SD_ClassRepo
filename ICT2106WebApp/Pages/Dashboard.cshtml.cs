using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ICT2106WebApp.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ILogger<DashboardModel> _logger;

        // Constructor to inject the logger service
        public DashboardModel(ILogger<DashboardModel> logger)
        {
            _logger = logger;
        }

        // Logs an error message
        public void LogError(string message)
        {
            _logger.LogError(message); // Log to the server logs
        }

        // Handles GET request for the dashboard (if needed)
        public void OnGet()
        {
            // Initialize page data if needed
        }
    }
}
