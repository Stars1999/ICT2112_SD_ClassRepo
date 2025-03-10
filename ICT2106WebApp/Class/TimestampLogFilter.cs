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
        private DateTime targetTimestamp;

        public TimestampLogFilter(DateTime targetTimestamp)
        {
            this.targetTimestamp = targetTimestamp;
        }

        public List<Logger_SDM> FilterLogs(List<Logger_SDM> logs)
        {
            return logs.Where(log => log.GetLogDetails().Item2.Date == targetTimestamp.Date).ToList();
        }
    }
}
