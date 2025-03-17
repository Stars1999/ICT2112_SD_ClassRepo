using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ICT2106WebApp.Controllers
{
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        private readonly string _uploadPath = "wwwroot/uploads"; // Path to store files

        public DashboardController()
        {
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        // Upload document endpoint
        [HttpPost("upload")]
public async Task<IActionResult> UploadDocument(IFormFile UploadedFile)
{
    if (UploadedFile == null || UploadedFile.Length == 0)
    {
        return BadRequest(new { message = "No file uploaded.", success = false });
    }

    var allowedExtensions = new[] { ".docx" };
    var fileExtension = Path.GetExtension(UploadedFile.FileName).ToLower();

    if (!allowedExtensions.Contains(fileExtension))
    {
        return BadRequest(new { message = "Invalid file type. Only .docx files are allowed.", success = false });
    }

    string filePath = Path.Combine(_uploadPath, UploadedFile.FileName);

    try
    {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await UploadedFile.CopyToAsync(stream);
        }

        return Ok(new { message = "File uploaded successfully!", fileName = UploadedFile.FileName, success = true });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = $"Error uploading file: {ex.Message}", success = false });
    }
}



        // Retrieve uploaded document
        [HttpGet("retrieve/{fileName}")]
        public IActionResult RetrieveDocument(string fileName)
        {
            string filePath = Path.Combine(_uploadPath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            return PhysicalFile(filePath, "application/octet-stream", fileName);
        }

        // Track conversion progress
        [HttpGet("status/{fileName}")]
        public IActionResult GetConversionStatus(string fileName)
        {
            // Placeholder: Fetch conversion status from TaskScheduler
            return Ok(new { fileName, status = "Processing" }); 
        }
    }
}
