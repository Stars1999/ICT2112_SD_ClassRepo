using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

[Route("home")]
public class HomeController : Controller
{
    [HttpGet("convert")]
    public IActionResult Convert([FromQuery] string style = "apa") // Default to APA
    {
        try
        {
            Console.WriteLine($"[DEBUG] Convert() called with style: {style}");

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");

            if (!System.IO.File.Exists(jsonFilePath))
            {
                ErrorPresenter.LogError("JSON file not found!");
                return Content("Error: JSON file not found!");
            }

            // Read the JSON data
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);

            // Create citation factories
            var citationFactory = new CitationScannerFactory();
            var bibliographyFactory = new BibliographyScannerFactory();

            // Initialize BibTeXConverter with the selected style
            var converter = new BibTeXConverter(citationFactory, bibliographyFactory, style);

            // Convert citations and bibliography
            string updatedJson = converter.ConvertCitationsAndBibliography(jsonData);

            // Store updated JSON in-memory
            var latexCompiler = new LatexCompiler();
            latexCompiler.UpdateJson(updatedJson);

            // Generate LaTeX
            var latexGenerator = new LatexGenerator();
            latexGenerator.GenerateLatex(latexCompiler.GetUpdatedJson());

            string generatedLatex = latexGenerator.GetLatexContent();
            if (string.IsNullOrEmpty(generatedLatex))
            {
                ErrorPresenter.LogError("LatexGenerator did not generate any content.");
            }

            // Store LaTeX for editor
            var editorDoc = new EditorDoc();
            editorDoc.SetLatexContent(generatedLatex);

            Console.WriteLine("[INFO] LaTeX content successfully stored in EditorDoc.");

            return RedirectToAction("Editor");
        }
        catch (Exception ex)
        {
            ErrorPresenter.LogError(ex.Message);
            return Content($"Error: {ex.Message}");
        }
    }

    [HttpGet("load-latex")]
    public IActionResult LoadLatex([FromQuery] string style = "apa")
    {
        Console.WriteLine($"[DEBUG] /home/load-latex accessed with style: {style}");

        var editorDoc = new EditorDoc();
        string latexContent = editorDoc.GetLatexContent();

        if (string.IsNullOrEmpty(latexContent))
        {
            ErrorPresenter.LogError("No LaTeX content found in EditorDoc.");
            return Content("Error: No LaTeX content found.");
        }

        return Content(latexContent);
    }

    [HttpPost("compile-latex")]
    public async Task<IActionResult> CompileLaTeX([FromBody] LaTeXRequest request)
    {
        try
        {
            Console.WriteLine("[DEBUG] Received LaTeX content for compilation.");

            PDFGenerator pdfGenerator = new PDFGenerator();
            bool success = await pdfGenerator.GeneratePDF(request.LatexContent);

            if (!success)
            {
                ErrorPresenter.LogError("LaTeX compilation failed.");
                return Json(new { success = false, error = "LaTeX compilation failed. Check logs for details." });
            }

            return Json(new { success = true, pdfUrl = pdfGenerator.GetGeneratedPDFUrl() });
        }
        catch (Exception ex)
        {
            ErrorPresenter.LogError(ex.Message);
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpGet("errors")]
    public IActionResult GetErrors()
    {
        return Json(new { errors = ErrorPresenter.GetErrors() });
    }

    [HttpGet("editor")]
    public IActionResult Editor()
    {
        return RedirectToPage("/Editor");
    }
}

// Request model to handle LaTeX content sent from JavaScript
public class LaTeXRequest
{
    public string LatexContent { get; set; }
}
