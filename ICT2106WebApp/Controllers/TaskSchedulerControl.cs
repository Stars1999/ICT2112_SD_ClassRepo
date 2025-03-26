using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

// namespace ICT2106WebApp.Controllers
// {
//     [Route("taskscheduler")]
//     public class TaskSchedulerControl : Controller
//     {
//         private readonly BibTeXConverter _bibTeXConverter;
//         private readonly CitationValidator _citationValidator;

//         public TaskSchedulerControl(BibTeXConverter bibTeXConverter, CitationValidator citationValidator)
//         {
//             _bibTeXConverter = bibTeXConverter;
//             _citationValidator = citationValidator;
//         }

//         // POST: /taskscheduler/process
//         [HttpPost("process")]
//         public async Task<IActionResult> ProcessDocument(IFormFile uploadedFile)
//         {
//             if (uploadedFile == null || uploadedFile.Length == 0)
//             {
//                 return BadRequest(new { message = "No file uploaded.", success = false });
//             }

//             try
//             {
//                 // Read file content
//                 using var stream = new MemoryStream();
//                 await uploadedFile.CopyToAsync(stream);
//                 string jsonData = System.Text.Encoding.UTF8.GetString(stream.ToArray());

//                 // Convert document to LaTeX format
//                 string convertedData = _bibTeXConverter.ConvertCitationsAndBibliography(jsonData);
//                 if (string.IsNullOrEmpty(convertedData))
//                 {
//                     return StatusCode(500, new { message = "Conversion failed.", success = false });
//                 }

//                 // Save converted LaTeX content to a temporary file
//                 string latexFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "converted_document.tex");
//                 await System.IO.File.WriteAllTextAsync(latexFilePath, convertedData);

//                 // Validate citations in converted document
//                 bool isValid = await _citationValidator.ValidateCitationConversionAsync(uploadedFile.FileName, latexFilePath);

//                 return Ok(new
//                 {
//                     message = isValid ? "Document processed successfully and citations validated." : "Document processed, but citations validation failed.",
//                     success = isValid
//                 });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new { message = $"Error processing document: {ex.Message}", success = false });
//             }
//         }
//     }
// }
