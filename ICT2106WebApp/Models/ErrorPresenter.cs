using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class for presenting LaTeX errors
/// </summary>
public class ErrorPresenter : iErrorPresenter
{
    private readonly iErrorAnalyser _errorAnalyser;
    private static List<string> _errorMessages = new List<string>();
    
    /// <summary>
    /// Constructor for ErrorPresenter
    /// </summary>
    public ErrorPresenter(iErrorAnalyser errorAnalyser)
    {
        _errorAnalyser = errorAnalyser ?? throw new ArgumentNullException(nameof(errorAnalyser));
    }
    
    /// <summary>
    /// Logs an error message
    /// </summary>
    public static void LogError(string errorMessage)
    {
        Console.WriteLine($"[ERROR] {errorMessage}");
        _errorMessages.Add(errorMessage);
    }
    
    /// <summary>
    /// Retrieves all logged errors and clears the list
    /// </summary>
    public static List<string> GetErrors()
    {
        List<string> errorsToReturn = new List<string>(_errorMessages);
        _errorMessages.Clear();
        return errorsToReturn;
    }
    
    /// <summary>
    /// Shows PDF generation error messages
    /// </summary>
    public string ShowPDFGenerationErrorMessage(string errorMessage)
    {
        string formattedMessage = $"PDF Generation Error: {errorMessage}";
        LogError(formattedMessage);
        return formattedMessage;
    }
    
    /// <summary>
    /// Shows LaTeX compiler error messages
    /// </summary>
    public string ShowLaTeXCompilerErrorMessage(string errorMessage)
    {
        string formattedMessage = $"LaTeX Compiler Error: {errorMessage}";
        LogError(formattedMessage);
        return formattedMessage;
    }
    
    /// <summary>
    /// Shows BibTeX converter error messages
    /// </summary>
    public string ShowBiBTeXConvertorErrorMessage(string errorMessage)
    {
        string formattedMessage = $"BibTeX Converter Error: {errorMessage}";
        LogError(formattedMessage);
        return formattedMessage;
    }
    
    /// <summary>
    /// Gets details for a specific error type
    /// </summary>
    public ErrorStyle GetErrorTypeDetails(int errorTypeID)
    {
        // In a real implementation, this would fetch error details from a database
        // For now, return a placeholder error
        return new ErrorStyle
        {
            LineNumber = 0,
            ErrorType = "Unknown Error",
            Message = "Unknown error type",
            Suggestion = "Please check your LaTeX syntax",
            Severity = "medium",
            CanFix = false
        };
    }
    
    /// <summary>
    /// Fetches all errors for a document
    /// </summary>
    public List<ErrorStyle> FetchAllError(string latexContent)
    {
        // Use the ErrorAnalyser to detect and suggest solutions for errors
        return _errorAnalyser.SuggestErrorSolution(latexContent);
    }
    
    /// <summary>
    /// Updates an error in the system
    /// </summary>
    public void UpdateError(ErrorStyle error)
    {
        // In a real implementation, this would update error status in a database
        Console.WriteLine($"[INFO] Updated error: {error.ErrorType} on line {error.LineNumber}");
    }
    
    /// <summary>
    /// Formats errors for display in the UI
    /// </summary>
    public List<object> FormatErrorsForUI(List<ErrorStyle> errors)
    {
        return errors.Select(e => new 
        {
            line = e.LineNumber,
            type = e.ErrorType,
            message = e.Message,
            suggestion = e.Suggestion,
            severity = e.Severity,
            canFix = e.CanFix,
            fixed_ = e.Fixed,
            column = e.Column,
            length = e.Length
        }).ToList<object>();
    }
}