using System;
using System.Collections.Generic;
using CustomLogger = ICT2106WebApp.Interfaces.ILogger;

/// <summary>
/// Facade for error checking in LaTeX documents.
/// Simplifies the usage of iErrorAnalyser and iErrorPresenter.
/// </summary>
public class ErrorCheckingFacade
{
    private readonly iErrorAnalyser _errorAnalyser;
    private readonly iErrorPresenter _errorPresenter;
    private readonly CustomLogger _logger;
    
    /// <summary>
    /// Constructor for ErrorCheckingFacade.
    /// </summary>
    public ErrorCheckingFacade(iErrorAnalyser errorAnalyser, iErrorPresenter errorPresenter, CustomLogger logger)
    {
        _errorAnalyser = errorAnalyser ?? throw new ArgumentNullException(nameof(errorAnalyser));
        _errorPresenter = errorPresenter ?? throw new ArgumentNullException(nameof(errorPresenter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            // Log the process start using the injected logger
            _logger.InsertLog(DateTime.Now, "Starting LaTeX error analysis...", nameof(ProcessError));
            
            // Use the ErrorAnalyser to detect errors
            var errors = _errorAnalyser.SuggestErrorSolution(latexContent);
            
            // Log the number of errors found
            _logger.InsertLog(DateTime.Now, $"Found {errors.Count} errors.", nameof(ProcessError));

            // If errors are found, log them and display error messages
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    string errorMessage = $"{error.ErrorType} at line {error.LineNumber}: {error.Message}";
                    _errorPresenter.ShowLaTeXCompilerErrorMessage(errorMessage);
                    _logger.InsertLog(DateTime.Now, errorMessage, nameof(ProcessError));
                }
            }

            return errors;
        }
        catch (Exception ex)
        {
            // Log and handle any exceptions using the logger
            string exceptionMessage = $"Error processing LaTeX content: {ex.Message}";
            _errorPresenter.ShowLaTeXCompilerErrorMessage(exceptionMessage);
            _logger.InsertLog(DateTime.Now, exceptionMessage, nameof(ProcessError));
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
                string noErrorMessage = "No error provided for fixing.";
                _errorPresenter.ShowLaTeXCompilerErrorMessage(noErrorMessage);
                _logger.InsertLog(DateTime.Now, noErrorMessage, nameof(FixError));
                return latexContent;
            }

            // Log the fix attempt using the logger
            _logger.InsertLog(DateTime.Now, $"Attempting to fix error: {error.ErrorType} on line {error.LineNumber}", nameof(FixError));
            
            // Use the ErrorAnalyser to apply the fix
            string fixedContent = _errorAnalyser.ChangeError(latexContent, error);
            
            // Update the error status
            error.Fixed = true;
            _errorPresenter.UpdateError(error);
            
            // Log the successful fix
            _logger.InsertLog(DateTime.Now, $"Successfully fixed error: {error.ErrorType} on line {error.LineNumber}", nameof(FixError));
            
            return fixedContent;
        }
        catch (Exception ex)
        {
            // Log and handle any exceptions
            string exceptionMessage = $"Error fixing document: {ex.Message}";
            _errorPresenter.ShowLaTeXCompilerErrorMessage(exceptionMessage);
            _logger.InsertLog(DateTime.Now, exceptionMessage, nameof(FixError));
            return latexContent;
        }
    }
}
