using Microsoft.AspNetCore.Mvc;
using ICT2106WebApp.Interfaces;

namespace ICT2106WebApp.Controllers
{
    [Route("api/[controller]")]
    public class TaskSchedulerController : Controller, ITaskScheduling

    {
        private readonly IDocument _parser;

        public TaskSchedulerController(IDocument parser)
        {
            _parser = parser;
        }

        public async Task<bool> ScheduleMod1Conversion(string fileName)
        {
            try
            {
                // Simulate scheduler processing Mod1
                _parser.UpdateConversionStatus(fileName, "TaskScheduler: Queuing Mod1");
                await Task.Delay(500);

                // Update progress through stages
                _parser.UpdateConversionStatus(fileName, "Mod1: Processing Started");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod1: 25% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod1: 50% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod1: 75% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod1: Conversion Complete");

                return true;
            }
            catch (Exception)
            {
                _parser.UpdateConversionStatus(fileName, "Mod1: Conversion Failed");
                return false;
            }
        }

        public async Task<bool> ScheduleMod2Conversion(string fileName)
        {
            try
            {
                // Simulate scheduler processing Mod2
                _parser.UpdateConversionStatus(fileName, "TaskScheduler: Queuing Mod2");
                await Task.Delay(500);

                // Update progress through stages
                _parser.UpdateConversionStatus(fileName, "Mod2: Processing Started");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod2: 25% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod2: 50% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod2: 75% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod2: Conversion Complete");

                return true;
            }
            catch (Exception)
            {
                _parser.UpdateConversionStatus(fileName, "Mod2: Conversion Failed");
                return false;
            }
        }

               public async Task<bool> ScheduleMod3Conversion(string fileName)
        {
            try
            {
                // Simulate scheduler processing Mod2
                _parser.UpdateConversionStatus(fileName, "TaskScheduler: Queuing Mod2");
                await Task.Delay(500);

                // Update progress through stages
                _parser.UpdateConversionStatus(fileName, "Mod2: Processing Started");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod2: 25% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod2: 50% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod2: 75% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod2: Conversion Complete");

                return true;
            }
            catch (Exception)
            {
                _parser.UpdateConversionStatus(fileName, "Mod2: Conversion Failed");
                return false;
            }
        }
    }
}