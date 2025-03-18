using System;
using System.Collections.Generic;

/// <summary>
/// Facade for error checking in LaTeX documents.
/// Simplifies the usage of iErrorAnalyser and iErrorPresenter.
/// </summary>
public class ErrorCheckingFacade
{
    private readonly iErrorAnalyser _errorAnalyser;
    private readonly iErrorPresenter _errorPresenter;
    
    /// <summary>
    /// Constructor for ErrorCheckingFacade.
    /// </summary>
    public ErrorCheckingFacade(iErrorAnalyser errorAnalyser, iErrorPresenter errorPresenter)
    {
        _errorAnalyser = errorAnalyser ?? throw new ArgumentNullException(nameof(errorAnalyser));
        _errorPresenter = errorPresenter ?? throw new ArgumentNullException(nameof(errorPresenter));
    }
    
    /// <summary>
    /// Processes errors in a LaTeX document.
    /// </summary>
    /// <param name="latexContent">The LaTeX content to analyze.</param>
    /// <returns>List of detected errors.</returns>
    public List<ErrorStyle> ProcessError(string latexContent)
    {
        try
        {
            if (string.IsNullOrEmpty(latexContent))
            {
                _errorPresenter.ShowLaTeXCompilerErrorMessage("No LaTeX content provided.");
                return new List<ErrorStyle>();
            }

            // Log the process start
            Console.WriteLine("[INFO] Starting LaTeX error analysis...");
            
            // Use the ErrorAnalyser to detect errors
            var errors = _errorAnalyser.SuggestErrorSolution(latexContent);
            
            // Log the number of errors found
            Console.WriteLine($"[INFO] Found {errors.Count} errors.");

            // If errors are found, log them
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    string errorMessage = $"{error.ErrorType} at line {error.LineNumber}: {error.Message}";
                    _errorPresenter.ShowLaTeXCompilerErrorMessage(errorMessage);
                }
            }

            return errors;
        }
        catch (Exception ex)
        {
            // Log and handle any exceptions
            _errorPresenter.ShowLaTeXCompilerErrorMessage($"Error processing LaTeX content: {ex.Message}");
            return new List<ErrorStyle>();
        }
    }
    
    /// <summary>
    /// Fixes a detected error in a LaTeX document.
    /// </summary>
    /// <param name="latexContent">The LaTeX content.</param>
    /// <param name="error">The error to fix.</param>
    /// <returns>The fixed LaTeX content.</returns>
    public string FixError(string latexContent, ErrorStyle error)
    {
        try
        {
            if (error == null)
            {
                _errorPresenter.ShowLaTeXCompilerErrorMessage("No error provided for fixing.");
                return latexContent;
            }

            // Log the fix attempt
            Console.WriteLine($"[INFO] Attempting to fix error: {error.ErrorType} on line {error.LineNumber}");
            
            // Use the ErrorAnalyser to apply the fix
            string fixedContent = _errorAnalyser.ChangeError(latexContent, error);
            
            // Update the error status
            error.Fixed = true;
            _errorPresenter.UpdateError(error);
            
            // Log the successful fix
            Console.WriteLine($"[INFO] Successfully fixed error: {error.ErrorType} on line {error.LineNumber}");
            
            return fixedContent;
        }
        catch (Exception ex)
        {
            // Log and handle any exceptions
            _errorPresenter.ShowLaTeXCompilerErrorMessage($"Error fixing document: {ex.Message}");
            return latexContent;
        }
    }
}
