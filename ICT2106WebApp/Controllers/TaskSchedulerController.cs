using Microsoft.AspNetCore.Mvc;
using ICT2106WebApp.Interfaces;

namespace ICT2106WebApp.Controllers
{
    [Route("api/[controller]")]
    public class TaskSchedulerController : Controller, ITaskScheduling

    {
        private readonly IDocument _parser;
        private readonly IDocumentTestCase _testCaseControl;

        public TaskSchedulerController(IDocument parser, IDocumentTestCase testCaseControl)
        {
            _parser = parser;
            _testCaseControl = testCaseControl;
        }

        public async Task<bool> ScheduleMod1Conversion(string fileName)
        {
            try
            {
                // Simulate scheduler processing Mod1
                _parser.UpdateConversionStatus(fileName, "TaskScheduler: Queuing Mod1");
                //await Task.Delay(500);

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
                // Simulate scheduler processing Mod3
                _parser.UpdateConversionStatus(fileName, "TaskScheduler: Queuing Mod3");
                await Task.Delay(500);

                // Update progress through stages
                _parser.UpdateConversionStatus(fileName, "Mod3: Processing Started");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod3: 25% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod3: 50% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod3: 75% complete");
                await Task.Delay(1000);
                _parser.UpdateConversionStatus(fileName, "Mod3: Conversion Complete");

                return true;
            }
            catch (Exception)
            {
                _parser.UpdateConversionStatus(fileName, "Mod2: Conversion Failed");
                return false;
            }
        }

        public async Task<bool> ScheduleMod1TestCase()
        {
            try
            {
                // Run the test cases asynchronously
                bool testResult = await Task.Run(() => _testCaseControl.runModTestCases(1));

                // Get the test status
                string testStatus = await Task.Run(() => _testCaseControl.getTestStatus(1));

                // Update the parser with the test status
                _parser.UpdateConversionStatus("", $"Mod1 Test: {testStatus}");

                return testResult;
            }
            catch (Exception)
            {
                _parser.UpdateConversionStatus("", "Mod1 Test: Failed (Error occurred)");
                return false;
            }
        }

        public async Task<bool> ScheduleMod2TestCase()
        {
            try
            {
                // Run the test cases asynchronously
                bool testResult = await Task.Run(() => _testCaseControl.runModTestCases(2));

                // Get the test status
                string testStatus = await Task.Run(() => _testCaseControl.getTestStatus(2));

                // Update the parser with the test status
                _parser.UpdateConversionStatus("", $"Mod2 Test: {testStatus}");

                return testResult;
            }
            catch (Exception)
            {
                _parser.UpdateConversionStatus("", "Mod2 Test: Failed (Error occurred)");
                return false;
            }
        }

        public async Task<bool> ScheduleMod3TestCase()
        {
            try
            {
                // Run the test cases asynchronously
                bool testResult = await Task.Run(() => _testCaseControl.runModTestCases(3));

                // Get the test status
                string testStatus = await Task.Run(() => _testCaseControl.getTestStatus(3));

                // Update the parser with the test status
                _parser.UpdateConversionStatus("", $"Mod3 Test: {testStatus}");

                return testResult;
            }
            catch (Exception)
            {
                _parser.UpdateConversionStatus("", "Mod3 Test: Failed (Error occurred)");
                return false;
            }
        }
    }
}