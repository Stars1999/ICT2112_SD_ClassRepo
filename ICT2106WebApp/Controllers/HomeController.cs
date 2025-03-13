using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;

[Route("home")]
public class HomeController : Controller
{
    [HttpGet("convert")]
    public IActionResult Convert()
    {
        try
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");

            if (!System.IO.File.Exists(jsonFilePath))
            {
                return Content("Error: JSON file not found! Ensure bibliography_test.json is inside wwwroot.");
            }

            // Instantiate the required factories
            var citationFactory = new CitationScannerFactory();
            var bibliographyFactory = new BibliographyScannerFactory();

            // Now pass them to BibTeXConverter
            var converter = new BibTeXConverter(citationFactory, bibliographyFactory);

            // Capture formatted LaTeX content
            using StringWriter outputWriter = new();
            Console.SetOut(outputWriter); // Redirect console output

            converter.ConvertAndPrint(jsonFilePath);

            string formattedText = outputWriter.ToString();
            return Content($"<pre>{formattedText}</pre>", "text/html");
        }
        catch (Exception ex)
        {
            return Content($"Error: {ex.Message}");
        }
    }
}
