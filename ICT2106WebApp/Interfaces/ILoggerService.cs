using ICT2106WebApp.Models;
using System;
using System.Collections.Generic;

namespace ICT2106WebApp.Services
{
    public interface ILoggerService
    {
        void AddLog(DateTime logTimestamp, string logDescription, string logLocation);       
    }
}