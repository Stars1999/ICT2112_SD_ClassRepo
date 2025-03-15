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
            Console.WriteLine("[ERROR] JSON file not found!");
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
            Console.WriteLine("[ERROR] LatexGenerator did not generate any content.");
        }

        // Store LaTeX for editor
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
    public IActionResult LoadLatex([FromQuery] string style = "apa")
    {
        Console.WriteLine($"[DEBUG] /home/load-latex accessed with style: {style}");

        var editorDoc = new EditorDoc();
        string latexContent = editorDoc.GetLatexContent();

        if (string.IsNullOrEmpty(latexContent))
        {
            Console.WriteLine("[ERROR] No LaTeX content found in EditorDoc.");
            return Content("Error: No LaTeX content found.");
        }

        Console.WriteLine($"[INFO] Returning LaTeX content in {style.ToUpper()} format.");
        return Content(latexContent);
    }

    [HttpPost("compile-latex")]
public async Task<IActionResult> CompileLaTeX([FromBody] LaTeXRequest request)
{
    try
    {
        Console.WriteLine("[DEBUG] Received LaTeX content for compilation.");
        Console.WriteLine("[DEBUG] LaTeX Content:\n" + request.LatexContent); // ✅ Log LaTeX content

        string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        string latexFilePath = Path.Combine(outputDirectory, "document.tex");

        // ✅ Validate LaTeX content
        if (string.IsNullOrWhiteSpace(request.LatexContent) || !request.LatexContent.Contains("\\begin{document}"))
        {
            Console.WriteLine("[ERROR] Invalid LaTeX content.");
            return Json(new { success = false, error = "Invalid LaTeX content." });
        }

        // Save LaTeX to a file
        await System.IO.File.WriteAllTextAsync(latexFilePath, request.LatexContent);
        Console.WriteLine("[INFO] LaTeX file saved at: " + latexFilePath);

        // Compile LaTeX
        bool success = CompileWithPdfLaTeX(latexFilePath, outputDirectory);
        if (!success)
        {
            Console.WriteLine("[ERROR] LaTeX compilation failed.");
            return Json(new { success = false, error = "LaTeX compilation failed. Check logs for details." });
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
            Arguments = $"-interaction=nonstopmode -output-directory \"{outputDirectory}\" \"{latexFilePath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = Process.Start(psi);
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Console.WriteLine("[INFO] LaTeX Output:\n" + output);
        Console.WriteLine("[ERROR] LaTeX Errors:\n" + error);

        if (process.ExitCode != 0)
        {
            Console.WriteLine($"[ERROR] LaTeX compilation failed. Exit code: {process.ExitCode}");
            return false;
        }

        Console.WriteLine("[INFO] LaTeX compilation successful.");
        return true;
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
