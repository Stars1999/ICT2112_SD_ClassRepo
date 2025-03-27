using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using MongoDB.Driver;
using ICT2106WebApp.Controllers;
using CustomLogger = ICT2106WebApp.Interfaces.ILogger;

[Route("understem")]
public class LatexEditorApplicationController : Controller
{
    private readonly iConversionStatus _conversionStatus;
    private readonly iGetGeneratedLatex _latexGenerator;
    private readonly ErrorCheckingFacade _errorCheckingFacade;
    private readonly PDFGenerator _pdfGenerator;
    private readonly MongoDbContext _dbContext;
    private readonly EditorDoc _editorDoc;
    private readonly CustomLogger _logger;

    public LatexEditorApplicationController(
        iConversionStatus conversionStatus,
        iGetGeneratedLatex latexGenerator,
        ErrorCheckingFacade errorCheckingFacade,
        PDFGenerator pdfGenerator,
        MongoDbContext dbContext,
        EditorDoc editorDoc,
        CustomLogger logger)
    {
        _conversionStatus = conversionStatus ?? throw new ArgumentNullException(nameof(conversionStatus));
        _latexGenerator = latexGenerator ?? throw new ArgumentNullException(nameof(latexGenerator));
        _errorCheckingFacade = errorCheckingFacade ?? throw new ArgumentNullException(nameof(errorCheckingFacade));
        _pdfGenerator = pdfGenerator ?? throw new ArgumentNullException(nameof(pdfGenerator));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _editorDoc = editorDoc ?? throw new ArgumentNullException(nameof(editorDoc));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("load-and-insert")]
    public async Task<IActionResult> LoadFromFileAndInsert()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bibliography_test.json");

        if (!System.IO.File.Exists(path))
        {
            _logger.InsertLog(DateTime.Now, "File not found during load-and-insert.", nameof(LoadFromFileAndInsert));
            return NotFound("File not found.");
        }

        string json = await System.IO.File.ReadAllTextAsync(path);
        _logger.InsertLog(DateTime.Now, "Read JSON file for bibliography.", nameof(LoadFromFileAndInsert));

        var reference = JsonSerializer.Deserialize<Reference>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
        {
            _logger.InsertLog(DateTime.Now, "Invalid reference data in JSON.", nameof(LoadFromFileAndInsert));
            return BadRequest("Invalid reference data.");
        }

        reference.InsertedAt = DateTime.UtcNow;
        reference.Source = "File Load";

        await _dbContext.References.InsertOneAsync(reference);
        _logger.InsertLog(DateTime.Now, "Inserted reference into MongoDB.", nameof(LoadFromFileAndInsert));

        return Ok("Loaded and inserted JSON from file.");
    }

    [HttpGet("convert")]
    public async Task<IActionResult> Convert([FromQuery] string style = "apa")
    {
        try
        {
            Console.WriteLine($"[DEBUG] Convert() called with style: {style}");
            _logger.InsertLog(DateTime.Now, $"Convert() called with style: {style}", nameof(Convert));

            var reference = await _dbContext.References.Find(_ => true).FirstOrDefaultAsync();

            if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
            {
                ErrorPresenter.LogError("No bibliography documents found in MongoDB!");
                _logger.InsertLog(DateTime.Now, "No bibliography documents found in MongoDB.", nameof(Convert));
                return Content("Error: No bibliography documents found in MongoDB!");
            }

            string jsonData = JsonSerializer.Serialize(reference, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"[DEBUG] Successfully serialized JSON: {jsonData}");
            _logger.InsertLog(DateTime.Now, "Successfully serialized JSON data.", nameof(Convert));

            var citationFactory = new CitationScannerFactory();
            var bibliographyFactory = new BibliographyScannerFactory();

            var bibtexMapper = new BibTexMapper(_dbContext);
            var converter = new BibTeXConverter(citationFactory, bibliographyFactory, _conversionStatus, bibtexMapper, style);

            string updatedJson = converter.ConvertCitationsAndBibliography(jsonData);
            _logger.InsertLog(DateTime.Now, "Converted citations and bibliography.", nameof(Convert));

            var latexCompiler = new LatexCompiler();
            latexCompiler.SetUpdatedJson(updatedJson);

            _latexGenerator.GenerateLatex(updatedJson);

            string generatedLatex = _latexGenerator.GetLatexContent();
            if (string.IsNullOrEmpty(generatedLatex))
            {
                ErrorPresenter.LogError("LatexGenerator did not generate any content.");
                _logger.InsertLog(DateTime.Now, "LaTeX generation failed (empty content).", nameof(Convert));
                return Content("Error: LaTeX generation failed.");
            }

            Console.WriteLine("[DEBUG] Generated LaTeX:");
            Console.WriteLine(generatedLatex);
            _logger.InsertLog(DateTime.Now, "Generated LaTeX content.", nameof(Convert));

            await _editorDoc.UpdateLatexContentAsync(generatedLatex);
            Console.WriteLine("[INFO] LaTeX content successfully stored in EditorDoc.");
            _logger.InsertLog(DateTime.Now, "LaTeX content successfully stored in EditorDoc.", nameof(Convert));

            return RedirectToAction("Editor");
        }
        catch (Exception ex)
        {
            ErrorPresenter.LogError(ex.Message);
            _logger.InsertLog(DateTime.Now, $"Exception in Convert: {ex.Message}", nameof(Convert));
            return Content($"Error: {ex.Message}");
        }
    }

    [HttpGet("load-latex")]
    public async Task<IActionResult> LoadLatex([FromQuery] string style = "apa")
    {
        Console.WriteLine($"[DEBUG] /home/load-latex accessed with style: {style}");
        _logger.InsertLog(DateTime.Now, $"/home/load-latex accessed with style: {style}", nameof(LoadLatex));

        var latexContent = await _editorDoc.GetLatexContentAsync();

        if (string.IsNullOrEmpty(latexContent))
        {
            ErrorPresenter.LogError("No LaTeX content found in EditorDoc.");
            _logger.InsertLog(DateTime.Now, "No LaTeX content found in EditorDoc.", nameof(LoadLatex));
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
            _logger.InsertLog(DateTime.Now, "Received LaTeX content for compilation.", nameof(CompileLaTeX));

            if (string.IsNullOrEmpty(request.LatexContent))
            {
                _logger.InsertLog(DateTime.Now, "Empty LaTeX content received.", nameof(CompileLaTeX));
                return BadRequest("No LaTeX content provided.");
            }

            var errors = _errorCheckingFacade.ProcessError(request.LatexContent);
            if (errors.Count > 0)
            {
                _logger.InsertLog(DateTime.Now, $"Compilation blocked due to {errors.Count} errors.", nameof(CompileLaTeX));
                return Json(new { success = false, error = "Fix LaTeX errors before compilation!", errors });
            }

            bool success = await _pdfGenerator.GeneratePDF(request.LatexContent);
            if (!success)
            {
                ErrorPresenter.LogError("LaTeX compilation failed.");
                _logger.InsertLog(DateTime.Now, "LaTeX compilation failed.", nameof(CompileLaTeX));
                return Json(new { success = false, error = "LaTeX compilation failed. Check logs for details." });
            }

            _logger.InsertLog(DateTime.Now, "PDF successfully generated.", nameof(CompileLaTeX));
            return Json(new { success = true, pdfUrl = _pdfGenerator.GetGeneratedPDFUrl() });
        }
        catch (Exception ex)
        {
            ErrorPresenter.LogError(ex.Message);
            _logger.InsertLog(DateTime.Now, $"Exception in CompileLaTeX: {ex.Message}", nameof(CompileLaTeX));
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpPost("save-latex")]
    public async Task<IActionResult> SaveLatex([FromBody] LaTeXRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.LatexContent))
            {
                _logger.InsertLog(DateTime.Now, "Empty LaTeX content in SaveLatex.", nameof(SaveLatex));
                return BadRequest("No LaTeX content provided.");
            }

            var editorDocMapper = new EditorDocumentMapper(_dbContext);
            var editorDoc = new EditorDoc(editorDocMapper);

            await editorDoc.UpdateLatexContentAsync(request.LatexContent);
            _logger.InsertLog(DateTime.Now, "LaTeX content saved via SaveLatex.", nameof(SaveLatex));

            return Ok(new { success = true, message = "Saved successfully." });
        }
        catch (Exception ex)
        {
            ErrorPresenter.LogError(ex.Message);
            _logger.InsertLog(DateTime.Now, $"Exception in SaveLatex: {ex.Message}", nameof(SaveLatex));
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
