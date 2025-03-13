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



    [HttpGet("editor")]
    public IActionResult Editor()
    {
        return RedirectToPage("/Editor");
    }
}
