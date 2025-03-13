using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

[Route("home")]
public class HomeController : Controller
{
    [HttpGet("convert")]
    public IActionResult Convert()
    {
        try
        {
            Console.WriteLine("[DEBUG] Convert() called.");

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");

            if (!System.IO.File.Exists(jsonFilePath))
            {
                Console.WriteLine("[ERROR] JSON file not found!");
                return Content("Error: JSON file not found!");
            }

            // Step 1: Convert citations & bibliography
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);
            var citationFactory = new CitationScannerFactory();
            var bibliographyFactory = new BibliographyScannerFactory();
            var converter = new BibTeXConverter(citationFactory, bibliographyFactory);
            string updatedJson = converter.ConvertCitationsAndBibliography(jsonData);

            // Step 2: Store updated JSON in-memory
            var latexCompiler = new LatexCompiler();
            latexCompiler.UpdateJson(updatedJson);

            // Step 3: Generate LaTeX from updated JSON
            var latexGenerator = new LatexGenerator();
            latexGenerator.GenerateLatex(latexCompiler.GetUpdatedJson());

            string generatedLatex = latexGenerator.GetLatexContent();
            if (string.IsNullOrEmpty(generatedLatex))
            {
                Console.WriteLine("[ERROR] LatexGenerator did not generate any content.");
            }

            // Step 4: Store LaTeX for editor
            var editorDoc = new EditorDoc();
            editorDoc.SetLatexContent(generatedLatex);

            Console.WriteLine("[INFO] LaTeX content successfully stored in EditorDoc.");
            
            return RedirectToAction("Editor");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            return Content($"Error: {ex.Message}");
        }
    }


    [HttpGet("load-latex")]
    public IActionResult LoadLatex()
    {
        Console.WriteLine("[DEBUG] /home/load-latex was accessed.");

        var editorDoc = new EditorDoc();
        string latexContent = editorDoc.GetLatexContent();

        if (string.IsNullOrEmpty(latexContent))
        {
            Console.WriteLine("[ERROR] No LaTeX content found in EditorDoc.");
            return Content("Error: No LaTeX content found.");
        }

        Console.WriteLine("[INFO] Returning LaTeX content to the editor.");
        return Content(latexContent);
    }

    [HttpPost("compile-latex")]
    public async Task<IActionResult> CompileLaTeX([FromBody] LaTeXRequest request)
    {
        try
        {
            Console.WriteLine("[DEBUG] Received LaTeX content for compilation.");

            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string latexFilePath = Path.Combine(outputDirectory, "document.tex");
            string pdfFilePath = Path.Combine(outputDirectory, "document.pdf");

            // Step 1: Save LaTeX content to a .tex file
            await System.IO.File.WriteAllTextAsync(latexFilePath, request.LatexContent);
            Console.WriteLine("[INFO] LaTeX file saved.");

            // Step 2: Compile LaTeX to PDF using pdflatex
            bool success = CompileWithPdfLaTeX(latexFilePath, outputDirectory);

            if (!success)
            {
                Console.WriteLine("[ERROR] LaTeX compilation failed.");
                return Json(new { success = false, error = "LaTeX compilation failed." });
            }

            Console.WriteLine("[INFO] PDF compiled successfully.");

            return Json(new { success = true, pdfUrl = "/pdfs/document.pdf" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            return Json(new { success = false, error = ex.Message });
        }
    }

    private bool CompileWithPdfLaTeX(string latexFilePath, string outputDirectory)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "pdflatex",
                Arguments = $"-interaction=nonstopmode -output-directory {outputDirectory} {latexFilePath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = Process.Start(psi);
            process.WaitForExit();

            return process.ExitCode == 0; // 0 means success
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] LaTeX compilation error: {ex.Message}");
            return false;
        }
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
