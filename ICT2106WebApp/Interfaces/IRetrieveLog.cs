using System.Collections.Generic;
using ICT2106WebApp.Models; // Ensure Logger_SDM is imported

namespace ICT2106WebApp.Interfaces
{
    public interface IRetrieveLog
    {
        List<Logger_SDM> RetrieveAllLog();
        List<string> GetAvailableLogLocations();
    }
}
