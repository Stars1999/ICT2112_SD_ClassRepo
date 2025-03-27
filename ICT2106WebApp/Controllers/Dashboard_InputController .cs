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

    public Dashboard_InputController(IDocument parser)
    {
        _parser = parser;
    }

    // POST: /dashboard/upload
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

            // Set the conversion status as "Processing"
            _parser.UpdateConversionStatus(uploadedFile.FileName, "Processing");

            // Simulate conversion progress (can be modified to real logic)
            await Task.Delay(1000); // Simulate processing delay
            _parser.UpdateConversionStatus(uploadedFile.FileName, "25% complete");

            await Task.Delay(1000);
            _parser.UpdateConversionStatus(uploadedFile.FileName, "50% complete");

            await Task.Delay(1000);
            _parser.UpdateConversionStatus(uploadedFile.FileName, "75% complete");

            await Task.Delay(1000);
            _parser.UpdateConversionStatus(uploadedFile.FileName, "Completed");

            return Ok(new { message = "File uploaded and ready for conversion.", success = true });
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            Console.Error.WriteLine($"Error uploading file: {ex.Message}");
            return StatusCode(500, new { message = $"Error uploading file: {ex.Message}", success = false });
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

    [HttpGet("runtestmod3fail")]
    public async Task<IActionResult> RunCitationTest3Fail()
    {
        string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");

        // Fail scenario LaTeX content
        string latexContentFail = @"\documentclass{article}
    \title{The Role of AI in Modern Healthcare}
    \author{Dr. Emily Johnson}
    \date{2024-03-15}
    \begin{document}
    \maketitle
    AI is transforming medical diagnostics. Predictive analytics does not mention the source.
    \section{References}
    Smith, John. ""Artificial Intelligence in Medical Diagnostics."" AI \& Healthcare Journal, 2019.
    \end{document}";

        CustomLogger logger = new LoggerGateway_TDG();

        var validator = new CitationValidator(logger);

        // Track progress of citation validation
        int totalSteps = 5;
        for (int i = 1; i <= totalSteps; i++)
        {
            int progress = (i * 100) / totalSteps;
            _parser.UpdateConversionStatus("Citation Test", $"{progress}% complete");
        }

        // Pass isFileContent: true to use the string content directly
        bool isValid = await validator.ValidateCitationConversionAsync(jsonFilePath, latexContentFail, isFileContent: true);

        string resultMessage = isValid 
            ? "Test Passed: All citations were correctly converted." 
            : "Test Failed: Some citations were not converted correctly.";

        return Ok(new { 
            message = resultMessage, 
            isValid = isValid,
            scenarioType = "Failure"
        });
    }



// GET: /dashboard/runtest
    [HttpGet("runtestmod3pass")]
    public async Task<IActionResult> runCitationTest3Pass()
    {
        string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");
        string latexFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document.tex");

        CustomLogger logger = new LoggerGateway_TDG();

        var validator = new CitationValidator(logger);

        // Track progress of citation validation
        int totalSteps = 5;
        for (int i = 1; i <= totalSteps; i++)
        {
            //await Task.Delay(1000); // Simulate each step with delay
            int progress = (i * 100) / totalSteps;
            _parser.UpdateConversionStatus("Citation Test", $"{progress}% complete");
            //_logger.LogInformation($"Step {i}/{totalSteps}: {progress}% complete");
        }

        bool isValid = await validator.ValidateCitationConversionAsync(jsonFilePath, latexFilePath);

        string resultMessage = isValid 
            ? "Test Passed: All citations were correctly converted." 
            : "Test Failed: Some citations were not converted correctly.";

        return Ok(new { message = resultMessage });
    }



    // GET: /dashboard/runtestmod2
    [HttpGet("runtestmod2pass")]
    public async Task<IActionResult> runCitationTest2Pass()
    {
        var mod2Test = new mod2testcases();

        // Call the RunPassTests() or RunFailTests()
        var passResults = mod2Test.RunPassTests(); // RunPassTests() or RunFailTests()

        // return the test results as success/failure message
        var resultMessage = passResults.All(r => r) ? "Test Passed (MOD2)" : "Test Failed (MOD2)";
        
        return Ok(new { message = resultMessage });
    }

    // GET: /dashboard/runtestmod2
    [HttpGet("runtestmod2fail")]
    public async Task<IActionResult> runCitationTest2Fail()
    {
        var mod2Test = new mod2testcases();

        // Call the RunPassTests() or RunFailTests()
        var passResults = mod2Test.RunFailTests(); // RunPassTests() or RunFailTests()

        // return the test results as success/failure message
        var resultMessage = passResults.All(r => r) ? "Test Passed (MOD2)" : "Test Failed (MOD2)";
        
        return Ok(new { message = resultMessage });
    }

}

}
