
namespace ICT2106WebApp.Interfaces
{
    public interface ITaskScheduling
    {
      Task<bool> ScheduleMod1Conversion(string fileName);
      Task<bool> ScheduleMod2Conversion(string fileName);
      Task<bool> ScheduleMod3Conversion(string fileName);
    }
}