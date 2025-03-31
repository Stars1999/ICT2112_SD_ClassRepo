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

                // Set initial status
               _parser.UpdateConversionStatus(uploadedFile.FileName, "File uploaded, starting conversions");

                // Schedule Mod1 Conversion through TaskScheduler
                bool mod1Result = await _taskScheduler.ScheduleMod1Conversion(uploadedFile.FileName);
                if (!mod1Result)
                {
                    return StatusCode(500, new { message = "Mod1 conversion failed", success = false });
                }

                // Schedule Mod2 Conversion through TaskScheduler
                bool mod2Result = await _taskScheduler.ScheduleMod2Conversion(uploadedFile.FileName);
                if (!mod2Result)
                {
                    return StatusCode(500, new { message = "Mod2 conversion failed", success = false });
                }

                // Schedule Mod3 Conversion through TaskScheduler
                bool mod3Result = await _taskScheduler.ScheduleMod3Conversion(uploadedFile.FileName);
                if (!mod3Result)
                {
                    return StatusCode(500, new { message = "Mod3 conversion failed", success = false });
                }

                return Ok(new { 
                    message = "File uploaded and all conversions completed successfully", 
                    success = true 
                });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.Error.WriteLine($"Error in process: {ex.Message}");
                return StatusCode(500, new { 
                    message = $"Error processing file: {ex.Message}", 
                    success = false 
                });
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
            var mod2Test = new mod2testcases();

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

            return Ok(new { 
                message = resultMessage, 
                isValid = !result, // Invert the result since this is a fail test
                scenarioType = "Failure"
            });
        }
    }
}