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

                // Retrieve the file path of the stored document
                string tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", uploadedFile.FileName);

                // Determine the citation format (APA or MLA) based on the file name or content
                string citationFormat = DetermineCitationFormat(tempFilePath);

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

        private string DetermineCitationFormat(string filePath)
        {
            string fileName = Path.GetFileName(filePath).ToLower();

            // Determine format based on file name
            if (fileName.Contains("apa"))
            {
                return "APA";
            }
            else if (fileName.Contains("mla"))
            {
                return "MLA";
            }

            return null;
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

        [HttpGet("runtestmod{modNumber}fail")]
        public IActionResult RunTestFail(int modNumber)
        {
            // Call the interface method to run the fail test
            bool result = !_testCaseControl.runModTestCases(modNumber); // Assuming fail test is the inverse

            // Return the result as a response
            string message = result ? $"Module {modNumber}: Test Failed as Expected" : $"Module {modNumber}: Unexpected Pass";
            return Ok(new { message });
        }

        // // GET: /dashboard/runtestmod1
        // [HttpGet("runtestmod1pass")]
        // public async Task<IActionResult> runCitationTest1Pass()
        // {
        //     CustomLogger logger = new LoggerGateway_TDG();
            
        //     var mod1Test = new Mod1TestCases(logger);

        //     // Call the RunPassTests() or RunFailTests()
        //     var passResults = mod1Test.RunPassTests(); // RunPassTests() or RunFailTests()

        //     // return the test results as success/failure message
        //     var resultMessage = $"{(passResults ? "Test Passed:" : "Some tests failed")}";

        //     return Ok(new { message = resultMessage });
        // }

        // // GET: /dashboard/runtestmod1
        // [HttpGet("runtestmod1fail")]
        // public async Task<IActionResult> runCitationTest1Fail()
        // {
        //     CustomLogger logger = new LoggerGateway_TDG();
            
        //     var mod1Test = new Mod1TestCases(logger);

        //     // Call the RunPassTests() or RunFailTests()
        //     bool passResults = mod1Test.RunFailTests(); // RunPassTests() or RunFailTests()

        //     // return the test results as success/failure message
        //     var resultMessage = $"{(passResults ? "Test Passed:" : "Some tests failed")}";

        //     return Ok(new { message = resultMessage });
        // }

        // // GET: /dashboard/runtestmod2
        // [HttpGet("runtestmod2pass")]
        // public async Task<IActionResult> runCitationTest2Pass()
        // {
        //     var mod2Test = new mod2testcases();

        //     // Call the RunPassTests() or RunFailTests()
        //     var passResults = mod2Test.RunPassTests(); // RunPassTests() or RunFailTests()

        //     // return the test results as success/failure message
        //     var resultMessage = $"{(passResults ? "Test Passed:" : "Some tests failed")}";

        //     return Ok(new { message = resultMessage });
        // }

        // // GET: /dashboard/runtestmod2
        // [HttpGet("runtestmod2fail")]
        // public async Task<IActionResult> runCitationTest2Fail()
        // {
        //     var mod2Test = new mod2testcases();

        //     // Call the RunPassTests() or RunFailTests()
        //     bool passResults = mod2Test.RunFailTests(); // RunPassTests() or RunFailTests()

        //     // return the test results as success/failure message
        //     var resultMessage = $"{(passResults ? "Test Passed:" : "Some tests failed")}";

        //     return Ok(new { message = resultMessage });
        // }

        // // GET: /dashboard/runtestruntestmod3fail
        // [HttpGet("runtestmod3fail")]
        // public async Task<IActionResult> RunCitationTest3Fail()
        // {
        //     string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");

        //     // Fail scenario LaTeX content
        //     string latexContentFail = @"\documentclass{article}
        // \title{The Role of AI in Modern Healthcare}
        // \author{Dr. Emily Johnson}
        // \date{2024-03-15}
        // \begin{document}
        // \maketitle
        // AI is transforming medical diagnostics. Predictive analytics does not mention the source.
        // \section{References}
        // Smith, John. ""Artificial Intelligence in Medical Diagnostics."" AI \& Healthcare Journal, 2019.
        // \end{document}";

        //     CustomLogger logger = new LoggerGateway_TDG();

        //     var validator = new CitationValidator(logger);

        //     // Pass isFileContent: true to use the string content directly
        //     bool isValid = await validator.ValidateCitationConversionAsync(jsonFilePath, latexContentFail, isFileContent: true);

        //     string resultMessage = isValid 
        //         ? "Test Passed: All citations were correctly converted." 
        //         : "Test Failed: Some citations were not converted correctly.";

        //     return Ok(new { 
        //         message = resultMessage, 
        //         isValid = isValid,
        //         scenarioType = "Failure"
        //     });
        // }

        // // GET: /dashboard/runtestruntestmod3pass
        // [HttpGet("runtestmod3pass")]
        // public async Task<IActionResult> runCitationTest3Pass()
        // {
        //     string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");
        //     string latexFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document.tex");

        //     CustomLogger logger = new LoggerGateway_TDG();

        //     var validator = new CitationValidator(logger);

        //     bool isValid = await validator.ValidateCitationConversionAsync(jsonFilePath, latexFilePath);

        //     string resultMessage = isValid 
        //         ? "Test Passed: All citations were correctly converted." 
        //         : "Test Failed: Some citations were not converted correctly.";

        //     return Ok(new { message = resultMessage });
        // }
    }
}