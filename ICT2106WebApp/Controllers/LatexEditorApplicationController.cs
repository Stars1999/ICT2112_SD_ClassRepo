using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using MongoDB.Driver;

[Route("understem")]
public class LatexEditorApplicationController : Controller
{
    private readonly iGetGeneratedLatex _latexGenerator;
    private readonly ErrorCheckingFacade _errorCheckingFacade;
    private readonly PDFGenerator _pdfGenerator;
    private readonly MongoDbContext _dbContext;
    private readonly EditorDoc _editorDoc;
    private readonly BibTeXConverter _converter;


    public LatexEditorApplicationController(BibTeXConverter converter,
    iGetGeneratedLatex latexGenerator, ErrorCheckingFacade errorCheckingFacade, PDFGenerator pdfGenerator, MongoDbContext dbContext, EditorDoc editorDoc)
    {
        _latexGenerator = latexGenerator ?? throw new ArgumentNullException(nameof(latexGenerator));
        _errorCheckingFacade = errorCheckingFacade ?? throw new ArgumentNullException(nameof(errorCheckingFacade));
        _pdfGenerator = pdfGenerator ?? throw new ArgumentNullException(nameof(pdfGenerator));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _editorDoc = editorDoc ?? throw new ArgumentNullException(nameof(editorDoc));
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
    }

    [HttpGet("load-and-insert")]
    public async Task<IActionResult> LoadFromFileAndInsert([FromQuery] string file = "apa_test.json")
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file);
        if (!System.IO.File.Exists(path))
        {
            return NotFound("File not found.");
        }

        string json = await System.IO.File.ReadAllTextAsync(path);

        var reference = JsonSerializer.Deserialize<Reference>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
        {
            return BadRequest("Invalid reference data.");
        }

        reference.Id = null;
        reference.InsertedAt = DateTime.UtcNow;

        string detectedStyle = "apa";
        if (file.ToLower().Contains("mla"))
        {
            detectedStyle = "mla";
            reference.Source = "MLA";
        }
        else
        {
            reference.Source = "APA";
        }

        Response.Cookies.Append("selectedCitationStyle", detectedStyle, new CookieOptions
        {
            Expires = DateTime.Now.AddDays(30),
            Path = "/"
        });

        // Convert citations to correct style BEFORE inserting
        string convertedJson = _converter.ConvertCitationsAndBibliography(JsonSerializer.Serialize(reference), detectedStyle);
        var convertedReference = JsonSerializer.Deserialize<Reference>(convertedJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        foreach (var doc in convertedReference.Documents)
        {
            doc.OriginalLatexContent = doc.LatexContent;
        }

        await _dbContext.References.InsertOneAsync(convertedReference);

        // Generate LaTeX and save to EditorDoc
        _latexGenerator.GenerateLatex();
        string freshLatex = _latexGenerator.GetLatexContent();
        await _editorDoc.UpdateLatexContentAsync(freshLatex);

        return RedirectToAction("convert", new { style = detectedStyle });
    }

    [HttpGet("load-and-insert2")]
    public async Task<IActionResult> LoadFromFileAndInsert2([FromQuery] string file = "mla_test.json")
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file);
        if (!System.IO.File.Exists(path))
        {
            return NotFound("File not found.");
        }

        string json = await System.IO.File.ReadAllTextAsync(path);

        var reference = JsonSerializer.Deserialize<Reference>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
        {
            return BadRequest("Invalid reference data.");
        }

        reference.Id = null;
        reference.InsertedAt = DateTime.UtcNow;

        string detectedStyle = "apa";
        if (file.ToLower().Contains("mla"))
        {
            detectedStyle = "mla";
            reference.Source = "MLA";
        }
        else
        {
            reference.Source = "APA";
        }

        Response.Cookies.Append("selectedCitationStyle", detectedStyle, new CookieOptions
        {
            Expires = DateTime.Now.AddDays(30),
            Path = "/"
        });

        // Convert citations to correct style BEFORE inserting
        string convertedJson = _converter.ConvertCitationsAndBibliography(JsonSerializer.Serialize(reference), detectedStyle);
        var convertedReference = JsonSerializer.Deserialize<Reference>(convertedJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        foreach (var doc in convertedReference.Documents)
        {
            doc.OriginalLatexContent = doc.LatexContent;
        }

        await _dbContext.References.InsertOneAsync(convertedReference);

        // Generate LaTeX and save to EditorDoc
        _latexGenerator.GenerateLatex();
        string freshLatex = _latexGenerator.GetLatexContent();
        await _editorDoc.UpdateLatexContentAsync(freshLatex);

        return RedirectToAction("convert", new { style = detectedStyle });
    }


    [HttpGet("convert")]
    public async Task<IActionResult> Convert([FromQuery] string style = null)
    {
        try
        {
            Console.WriteLine($"[DEBUG] Convert() called with style: {style}");

            var reference = await _dbContext.References
                .Find(_ => true)
                .SortByDescending(r => r.InsertedAt)
                .FirstOrDefaultAsync();

            if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
            {
                ErrorPresenter.LogError("No bibliography documents found in MongoDB!");
                return Content("Error: No bibliography documents found in MongoDB!");
            }

            // If style is still null or empty, apply auto-detection logic
            if (string.IsNullOrWhiteSpace(style))
            {
                string detectedSource = reference.Source?.ToLower() ?? "";
                if (detectedSource.Contains("mla"))
                {
                    style = "mla";
                    Console.WriteLine("[AUTO-DETECT] Citation style set to MLA based on source.");
                }
                else
                {
                    style = "apa"; // default fallback
                    Console.WriteLine("[AUTO-DETECT] Citation style defaulted to APA.");
                }
            }

            // Proceed with conversion using the determined style
            // Existing conversion logic...

            return RedirectToAction("Editor");
        }
        catch (Exception ex)
        {
            ErrorPresenter.LogError(ex.Message);
            return Content($"Error: {ex.Message}");
        }
    }



    // New endpoint for APA style
    [HttpGet("load-apa-latex")]
    public async Task<IActionResult> LoadApaLatex()
    {
        try
        {
            Console.WriteLine("[DEBUG] load-apa-latex endpoint called");
            
            // Get the latest reference from the database
            var reference = await _dbContext.References
                .Find(_ => true)
                .SortByDescending(r => r.InsertedAt)
                .FirstOrDefaultAsync();
            
            if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
            {
                ErrorPresenter.LogError("No bibliography documents found in MongoDB!");
                return Content("Error: No bibliography documents found in MongoDB!");
            }
            
            // Convert to APA style
            string jsonData = JsonSerializer.Serialize(reference, new JsonSerializerOptions { WriteIndented = true });
            string updatedJson = _converter.ConvertCitationsAndBibliography(jsonData, "apa");
            
            if (string.IsNullOrEmpty(updatedJson))
            {
                ErrorPresenter.LogError("APA style conversion failed.");
                return Content("Error: APA style conversion failed.");
            }
            
            // Generate LaTeX with APA style
            _latexGenerator.GenerateLatex();
            
            string generatedLatex = _latexGenerator.GetLatexContent();
            if (string.IsNullOrEmpty(generatedLatex))
            {
                ErrorPresenter.LogError("LatexGenerator did not generate any content after APA style change.");
                return Content("Error: LaTeX generation failed after APA style change.");
            }
            
            // Update the editor content with the new style
            await _editorDoc.UpdateLatexContentAsync();
            
            // Save the style preference in a cookie
            Response.Cookies.Append("selectedCitationStyle", "apa", new CookieOptions { 
                Expires = DateTime.Now.AddDays(30),
                Path = "/" 
            });
            
            Console.WriteLine("[INFO] Citation style successfully changed to APA.");
            
            // Redirect back to the editor
            return RedirectToAction("Editor");
        }
        catch (Exception ex)
        {
            ErrorPresenter.LogError(ex.Message);
            return Content($"Error: {ex.Message}");
        }
    }

    // New endpoint for MLA style
    [HttpGet("load-mla-latex")]
    public async Task<IActionResult> LoadMlaLatex()
    {
        try
        {
            Console.WriteLine("[DEBUG] load-mla-latex endpoint called");
            
            // Get the latest reference from the database
            var reference = await _dbContext.References
                .Find(_ => true)
                .SortByDescending(r => r.InsertedAt)
                .FirstOrDefaultAsync();
            
            if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
            {
                ErrorPresenter.LogError("No bibliography documents found in MongoDB!");
                return Content("Error: No bibliography documents found in MongoDB!");
            }
            
            // Convert to MLA style
            string jsonData = JsonSerializer.Serialize(reference, new JsonSerializerOptions { WriteIndented = true });
            string updatedJson = _converter.ConvertCitationsAndBibliography(jsonData, "mla");
            
            if (string.IsNullOrEmpty(updatedJson))
            {
                ErrorPresenter.LogError("MLA style conversion failed.");
                return Content("Error: MLA style conversion failed.");
            }
            
            // Generate LaTeX with MLA style
            _latexGenerator.GenerateLatex();
            
            string generatedLatex = _latexGenerator.GetLatexContent();
            if (string.IsNullOrEmpty(generatedLatex))
            {
                ErrorPresenter.LogError("LatexGenerator did not generate any content after MLA style change.");
                return Content("Error: LaTeX generation failed after MLA style change.");
            }
            
            // Update the editor content with the new style
            await _editorDoc.UpdateLatexContentAsync();
            
            // Save the style preference in a cookie
            Response.Cookies.Append("selectedCitationStyle", "mla", new CookieOptions { 
                Expires = DateTime.Now.AddDays(30),
                Path = "/" 
            });
            
            Console.WriteLine("[INFO] Citation style successfully changed to MLA.");
            
            // Redirect back to the editor
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

            string latexContent = _latexGenerator.GetLatexContent();

            if (string.IsNullOrEmpty(latexContent))
            {
                return BadRequest("No LaTeX content provided.");
            }

            // Restored LaTeX Error Checking
            var errors = _errorCheckingFacade.ProcessError(latexContent);
            if (errors.Count > 0)
            {
                return Json(new { success = false, error = "Fix LaTeX errors before compilation!", errors });
            }

            bool success = await _pdfGenerator.GeneratePDF();
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
            {
                return BadRequest("No LaTeX content provided.");
            }

            await _editorDoc.UpdateLatexContentAsync(request.LatexContent);

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