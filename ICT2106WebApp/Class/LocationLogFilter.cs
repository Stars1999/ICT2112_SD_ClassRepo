using System;
using System.Collections.Generic;
using System.Linq;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;

public class LocationLogFilter : ILogFilter_Strategy
{
    private string targetLocation;

    public LocationLogFilter(string targetLocation)
    {
        this.targetLocation = targetLocation;
    }

    public List<Logger_SDM> FilterLogs(List<Logger_SDM> logs)
    {
        return logs.Where(log => log.GetLogDetails().Item4 == targetLocation).ToList();
    }
}
