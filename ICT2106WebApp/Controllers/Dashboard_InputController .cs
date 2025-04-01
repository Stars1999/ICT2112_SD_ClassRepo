using Microsoft.AspNetCore.Mvc;
using ICT2106WebApp.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using CustomLogger = ICT2106WebApp.Interfaces.ILogger;
using ICT2106WebApp.Data;

namespace ICT2106WebApp.Controllers
{
    [Route("dashboard")]
    public class Dashboard_InputController : Controller
    {
        private readonly IDocument _parser;
        private readonly ITaskScheduling _taskScheduler;
        private readonly IDocumentTestCase _testCaseControl;

        public Dashboard_InputController(IDocument parser, ITaskScheduling taskScheduler)
        {
            _parser = parser;
            _taskScheduler = taskScheduler;
            CustomLogger _logger = new LoggerGateway_TDG(); // Initialize the logger
            _testCaseControl = new TestCaseControl(_logger); // Pass the logger to TestCaseControl
        }



        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile uploadedFile)
        {
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded.", success = false });
            }

            try
            {
                // Store the uploaded document temporarily
                _parser.StoreDocument(uploadedFile);
                _parser.UpdateConversionStatus(uploadedFile.FileName, "File uploaded, starting conversions");

                // Run all conversions and tests
                if (!await RunAllConversionsAndTests(uploadedFile.FileName))
                {
                    return StatusCode(500, new { message = "Conversion or test failed", success = false });
                }

                // Get the test status to determine format
                string testStatus = _testCaseControl.getTestStatus(3);

                // Update frontend about completion
                _parser.UpdateConversionStatus(uploadedFile.FileName, "All conversions and tests completed successfully");

                // Create the redirect result
                var redirectResult = uploadedFile.FileName.ToLower().Contains("mla")
                    ? RedirectToAction("LoadFromFileAndInsert2", "LatexEditorApplication", new { file = "mla_test.json" })
                    : RedirectToAction("LoadFromFileAndInsert", "LatexEditorApplication", new { file = "apa_test.json" });

                // Set no-cache headers to ensure redirect works
                Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                Response.Headers.Add("Pragma", "no-cache");
                Response.Headers.Add("Expires", "0");

                return redirectResult;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in process: {ex.Message}");
                return StatusCode(500, new { message = $"Error processing file: {ex.Message}", success = false });
            }
        }

        private async Task<bool> RunAllConversionsAndTests(string fileName)
        {
            try
            {
                // Mod1
                _parser.UpdateConversionStatus(fileName, "Starting Mod1 conversion");
                bool mod1Result = await _taskScheduler.ScheduleMod1Conversion(fileName);
                if (!mod1Result)
                {
                    _parser.UpdateConversionStatus(fileName, "Mod1 conversion failed");
                    return false;
                }

                _parser.UpdateConversionStatus(fileName, "Starting Mod1 test case");
                bool mod1TestResult = await _taskScheduler.ScheduleMod1TestCase();
                if (!mod1TestResult)
                {
                    _parser.UpdateConversionStatus(fileName, "Mod1 test case failed");
                    return false;
                }

                // Mod2
                _parser.UpdateConversionStatus(fileName, "Starting Mod2 conversion");
                bool mod2Result = await _taskScheduler.ScheduleMod2Conversion(fileName);
                if (!mod2Result)
                {
                    _parser.UpdateConversionStatus(fileName, "Mod2 conversion failed");
                    return false;
                }

                _parser.UpdateConversionStatus(fileName, "Starting Mod2 test case");
                bool mod2TestResult = await _taskScheduler.ScheduleMod2TestCase();
                if (!mod2TestResult)
                {
                    _parser.UpdateConversionStatus(fileName, "Mod2 test case failed");
                    return false;
                }

                // Mod3
                _parser.UpdateConversionStatus(fileName, "Starting Mod3 conversion");
                bool mod3Result = await _taskScheduler.ScheduleMod3Conversion(fileName);
                if (!mod3Result)
                {
                    _parser.UpdateConversionStatus(fileName, "Mod3 conversion failed");
                    return false;
                }

                _parser.UpdateConversionStatus(fileName, "Starting Mod3 test case");
                bool mod3TestResult = await _taskScheduler.ScheduleMod3TestCase();
                if (!mod3TestResult)
                {
                    _parser.UpdateConversionStatus(fileName, "Mod3 test case failed");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _parser.UpdateConversionStatus(fileName, $"Error in conversions: {ex.Message}");
                return false;
            }
        }



        // GET: /dashboard/status/{fileName}
        [HttpGet("status/{fileName}")]
        public IActionResult GetConversionStatus(string fileName)
        {
            var status = _parser.GetConversionStatus(fileName);
            return Ok(new { fileName, status });
        }

        // GET: /dashboard/retrieve/{fileName}
        [HttpGet("retrieve/{fileName}")]
        public IActionResult RetrieveDocument(string fileName)
        {
            var document = _parser.RetrieveDocument(fileName);

            if (document == null)
            {
                return NotFound("Document not found.");
            }

            return PhysicalFile(document.FilePath, "application/octet-stream", fileName);
        }

        [HttpGet("runtestmod{modNumber}pass")]
        public IActionResult RunTestPass(int modNumber)
        {
            // Call the interface method to run the pass test
            bool result = _testCaseControl.runModTestCases(modNumber);

            // Return the result as a response
            string message = result ? $"Module {modNumber}: Test Passed" : $"Module {modNumber}: Test Failed";
            return Ok(new { message });
        }

        // GET: /dashboard/runtestmod1
        [HttpGet("runtestmod1fail")]
        public async Task<IActionResult> runCitationTest1Fail()
        {
            CustomLogger logger = new LoggerGateway_TDG();

            var mod1Test = new Mod1TestCases(logger);

            // Call the RunPassTests() or RunFailTests()
            bool passResults = mod1Test.RunFailTests(); // RunPassTests() or RunFailTests()

            // return the test results as success/failure message
            var resultMessage = $"{(passResults ? "Test Passed:" : "Some tests failed")}";

            return Ok(new { message = resultMessage });
        }

        // GET: /dashboard/runtestmod2
        [HttpGet("runtestmod2fail")]
        public async Task<IActionResult> runCitationTest2Fail()
        {
            CustomLogger logger = new LoggerGateway_TDG();
            var mod2Test = new mod2testcases(logger);

            // Call the RunPassTests() or RunFailTests()
            bool passResults = mod2Test.RunFailTests(); // RunPassTests() or RunFailTests()

            // return the test results as success/failure message
            var resultMessage = $"{(passResults ? "Test Passed:" : "Some tests failed")}";

            return Ok(new { message = resultMessage });
        }

        // GET: /dashboard/runtestruntestmod3fail
        [HttpGet("runtestmod3fail")]
        public async Task<IActionResult> RunCitationTest3Fail()
        {
            CustomLogger logger = new LoggerGateway_TDG();
            var mod3 = new Mod3TestCases(logger);

            // Run the fail test with APA format
            bool result = await mod3.RunFailTests("APA");

            string resultMessage = result
                ? "Test Failed: Some citations were not converted correctly."
                : "Test Passed: All citations were correctly converted.";

            return Ok(new
            {
                message = resultMessage,
                isValid = !result, // Invert the result since this is a fail test
                scenarioType = "Failure"
            });
        }
    }
}