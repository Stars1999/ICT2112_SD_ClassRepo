using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICT2106WebApp.Class
{
    public class TimestampLogFilter : ILogFilter_Strategy
    {
        private DateTime startDate;
        private DateTime endDate;

        public TimestampLogFilter(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public List<Logger_SDM> FilterLogs(List<Logger_SDM> logs)
        {
            // If the start date and end date are the same, filter logs for that specific day.
            if (startDate.Date == endDate.Date)
            {
                return logs.Where(log => log.GetLogDetails().Item2.Date == startDate.Date).ToList();
            }
            else // If the start date and end date are different, filter logs between those dates inclusive.
            {
                return logs.Where(log => log.GetLogDetails().Item2.Date >= startDate.Date && log.GetLogDetails().Item2.Date <= endDate.Date).ToList();
            }
        }
    }
}
