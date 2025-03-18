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
    private readonly iErrorCheckingFacade _errorCheckingFacade;
    private readonly iErrorPresenter _errorPresenter;
    
    /// <summary>
    /// Constructor for ErrorController
    /// </summary>
    public ErrorController(iErrorCheckingFacade errorCheckingFacade, iErrorPresenter errorPresenter)
    {
        _errorCheckingFacade = errorCheckingFacade ?? throw new ArgumentNullException(nameof(errorCheckingFacade));
        _errorPresenter = errorPresenter ?? throw new ArgumentNullException(nameof(errorPresenter));
    }
    
    /// <summary>
    /// Detects errors in LaTeX content
    /// </summary>
    [HttpPost("detect")]
    public IActionResult DetectErrors([FromBody] LaTeXRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.LatexContent))
            {
                return BadRequest("LaTeX content is required");
            }
            
            // Process the errors using the facade
            var errors = _errorCheckingFacade.ProcessError("current-document", request.LatexContent);
            
            // Format errors for UI display
            var formattedErrors = ((ErrorPresenter)_errorPresenter).FormatErrorsForUI(errors);
            
            return Json(new { success = true, errors = formattedErrors });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Error detecting LaTeX errors: {ex.Message}");
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
            if (request == null || string.IsNullOrEmpty(request.LatexContent))
            {
                return BadRequest("LaTeX content is required");
            }
            
            if (request.Error == null)
            {
                // Create an error object manually if one wasn't provided correctly
                Console.WriteLine($"[DEBUG] Creating error object from line: {request.LineNumber}");
                var error = new ErrorStyle
                {
                    LineNumber = request.LineNumber,
                    ErrorType = request.ErrorType ?? "Unknown Error",
                    Message = request.ErrorMessage ?? "Error to fix",
                    Severity = "high",
                    CanFix = true
                };
                
                // Fix the error using the facade
                string fixedContent = _errorCheckingFacade.FixError(request.LatexContent, error);
                
                return Json(new { success = true, fixedContent = fixedContent });
            }
            else
            {
                // Normal flow
                string fixedContent = _errorCheckingFacade.FixError(request.LatexContent, request.Error);
                return Json(new { success = true, fixedContent = fixedContent });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Error fixing LaTeX error: {ex.Message}");
            return Json(new { success = false, error = ex.Message });
        }
    }

    // Updated request model
    public class FixErrorRequest
    {
        public string LatexContent { get; set; }
        public ErrorStyle Error { get; set; }
        public int LineNumber { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    /// <summary>
    /// Gets all errors for the current document
    /// </summary>
    [HttpGet("all")]
    public IActionResult GetAllErrors()
    {
        return Json(new { errors = ErrorPresenter.GetErrors() });
    }
}

/// <summary>
/// Request model for fixing an error
/// </summary>
public class FixErrorRequest
{
    public string LatexContent { get; set; }
    public ErrorStyle Error { get; set; }
}