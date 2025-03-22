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

                // Simulate conversion progress 
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

        // GET: /dashboard/runtest
        [HttpGet("runtest")]
        public async Task<IActionResult> RunCitationTest()
        {
            // Construct file paths relative to your project; adjust as needed.
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");
            string latexFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document.tex");

            // Use LoggerGateway_TDG so that logs are persisted and appear in your logging dashboard.
            CustomLogger logger = new LoggerGateway_TDG();

            // Create and run your citation validator test.
            var validator = new CitationValidator(logger);
            bool isValid = await validator.ValidateCitationConversionAsync(jsonFilePath, latexFilePath);

            string resultMessage = isValid 
                ? "Test Passed: All citations were correctly converted." 
                : "Test Failed: Some citations were not converted correctly.";

            return Ok(new { message = resultMessage });
        }
    }
}
