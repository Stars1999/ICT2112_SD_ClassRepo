using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Controller for handling LaTeX error detection and correction
/// </summary>
[Route("error")]
public class ErrorController : Controller
{
    private readonly ErrorCheckingFacade _errorCheckingFacade;
    
    /// <summary>
    /// Constructor for ErrorController
    /// </summary>
    public ErrorController(ErrorCheckingFacade errorCheckingFacade)
    {
        _errorCheckingFacade = errorCheckingFacade ?? throw new ArgumentNullException(nameof(errorCheckingFacade));
    }
    
    /// <summary>
    /// Detects errors in LaTeX content.
    /// </summary>
    [HttpPost("detect")]
    public IActionResult DetectErrors([FromBody] LaTeXRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.LatexContent))
            {
                return BadRequest("LaTeX content is required.");
            }
            
            // Use the Facade to process errors
            var errors = _errorCheckingFacade.ProcessError(request.LatexContent);
            
            return Json(new { success = true, errors });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            return Json(new { success = false, error = ex.Message });
        }
    }
    
    /// <summary>
    /// Fixes a specific error in LaTeX content
    /// </summary>
    [HttpPost("fix")]
    public IActionResult FixError([FromBody] FixErrorRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.LatexContent) || request.Error == null)
            {
                return BadRequest("Invalid request data.");
            }
            
            // ✅ Use the Facade to fix the error
            string fixedContent = _errorCheckingFacade.FixError(request.LatexContent, request.Error);
            
            return Json(new { success = true, fixedContent });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            return Json(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Processes errors without fixing them.
    /// </summary>
    [HttpPost("process")]
    public IActionResult ProcessError([FromBody] LaTeXRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.LatexContent))
            {
                return BadRequest("LaTeX content is required.");
            }

            // ✅ Use the Facade to process errors
            var errors = _errorCheckingFacade.ProcessError(request.LatexContent);
            
            return Json(new { success = true, errors });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            return Json(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves and processes errors for the current document.
    /// </summary>
    [HttpGet("all")]
    public IActionResult GetAllErrors([FromQuery] string latexContent)
    {
        if (string.IsNullOrEmpty(latexContent))
        {
            return Json(new { errors = ErrorPresenter.GetErrors() });
        }

        // ✅ Process errors before retrieving them
        var errors = _errorCheckingFacade.ProcessError(latexContent);

        return Json(new { success = true, errors });
    }
}

/// <summary>
/// Request model for fixing an error
/// </summary>
public class FixErrorRequest
{
    public string LatexContent { get; set; }
    public ErrorStyle Error { get; set; }
    public int LineNumber { get; set; }
    public string ErrorType { get; set; }
    public string ErrorMessage { get; set; }
}
