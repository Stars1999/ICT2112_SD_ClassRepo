using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using MongoDB.Driver;

[Route("home")]
public class LatexEditorApplicationController : Controller
{
    private readonly iConversionStatus _conversionStatus;
    private readonly ErrorCheckingFacade _errorCheckingFacade;
    private readonly PDFGenerator _pdfGenerator;
    private readonly MongoDbContext _dbContext;
    private readonly EditorDoc _editorDoc;

    public LatexEditorApplicationController(iConversionStatus conversionStatus, ErrorCheckingFacade errorCheckingFacade, PDFGenerator pdfGenerator, MongoDbContext dbContext, EditorDoc editorDoc)
    {
        _conversionStatus = conversionStatus ?? throw new ArgumentNullException(nameof(conversionStatus));
        _errorCheckingFacade = errorCheckingFacade ?? throw new ArgumentNullException(nameof(errorCheckingFacade));
        _pdfGenerator = pdfGenerator ?? throw new ArgumentNullException(nameof(pdfGenerator));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _editorDoc = editorDoc ?? throw new ArgumentNullException(nameof(editorDoc));
    }

    [HttpGet("convert")]
    public async Task<IActionResult> Convert([FromQuery] string style = "apa")
    {
        try
        {
            Console.WriteLine($"[DEBUG] Convert() called with style: {style}");

            var reference = await _dbContext.References.Find(_ => true).FirstOrDefaultAsync();

            if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
            {
                ErrorPresenter.LogError("No bibliography documents found in MongoDB!");
                return Content("Error: No bibliography documents found in MongoDB!");
            }

            // ✅ Wrap documents in an object so it matches the Reference class
            string jsonData = JsonSerializer.Serialize(new { Documents = reference.Documents });
            Console.WriteLine($"[DEBUG] Successfully serialized JSON: {jsonData}");

            // ✅ Create citation factories
            var citationFactory = new CitationScannerFactory();
            var bibliographyFactory = new BibliographyScannerFactory();

            // ✅ Initialize BibTeXConverter
            var converter = new BibTeXConverter(citationFactory, bibliographyFactory, _conversionStatus, style);

            // ✅ Convert citations and bibliography
            string updatedJson = converter.ConvertCitationsAndBibliography(jsonData);
            var latexCompiler = new LatexCompiler();
            latexCompiler.SetUpdatedJson(updatedJson);
            // ✅ Pass converted JSON to LatexGenerator
            var latexGenerator = new LatexGenerator();
            latexGenerator.GenerateLatex(updatedJson);

            string generatedLatex = latexGenerator.GetLatexContent();
            if (string.IsNullOrEmpty(generatedLatex))
            {
                ErrorPresenter.LogError("LatexGenerator did not generate any content.");
                return Content("Error: LaTeX generation failed.");
            }
            Console.WriteLine("[DEBUG] Generated LaTeX:");
            Console.WriteLine(generatedLatex);
            // var editorDoc = new EditorDoc();
            // editorDoc.SetLatexContent(generatedLatex);

            // ✅ Store LaTeX in Editor
            await _editorDoc.UpdateLatexContentAsync(generatedLatex);

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
    public async Task<IActionResult> LoadLatex([FromQuery] string style = "apa")
    {
        Console.WriteLine($"[DEBUG] /home/load-latex accessed with style: {style}");

        var latexContent = await _editorDoc.GetLatexContentAsync();

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

            if (string.IsNullOrEmpty(request.LatexContent))
            {
                return BadRequest("No LaTeX content provided.");
            }

            // ✅ Restored LaTeX Error Checking
            var errors = _errorCheckingFacade.ProcessError(request.LatexContent);
            if (errors.Count > 0)
            {
                return Json(new { success = false, error = "Fix LaTeX errors before compilation!", errors });
            }

            bool success = await _pdfGenerator.GeneratePDF(request.LatexContent);
            if (!success)
            {
                ErrorPresenter.LogError("LaTeX compilation failed.");
                return Json(new { success = false, error = "LaTeX compilation failed. Check logs for details." });
            }

            return Json(new { success = true, pdfUrl = _pdfGenerator.GetGeneratedPDFUrl() });
        }
        catch (Exception ex)
        {
            ErrorPresenter.LogError(ex.Message);
            return Json(new { success = false, error = ex.Message });
        }
    }
    [HttpPost("save-latex")]
    public async Task<IActionResult> SaveLatex([FromBody] LaTeXRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.LatexContent))
                return BadRequest("No LaTeX content provided.");

            var editorDocMapper = new EditorDocumentMapper(_dbContext);
            var editorDoc = new EditorDoc(editorDocMapper);

            await editorDoc.UpdateLatexContentAsync(request.LatexContent);

            return Ok(new { success = true, message = "Saved successfully." });
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