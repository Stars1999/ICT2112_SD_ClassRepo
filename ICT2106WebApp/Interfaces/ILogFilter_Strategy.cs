using ICT2106WebApp.Models;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;

namespace ICT2106WebApp.Interfaces
{
    public interface ILogFilter_Strategy
    {
        List<Logger_SDM> FilterLogs(List<Logger_SDM> logs);
    }
}
