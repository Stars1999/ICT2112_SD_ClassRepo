using System;

namespace ICT2106WebApp.Interfaces
{
    public interface ILogger
    {
        void InsertLog(DateTime logTimestamp, string logDescription, string logLocation);
    }
}
