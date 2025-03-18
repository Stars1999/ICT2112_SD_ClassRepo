using System;
using System.Collections.Generic;

/// <summary>
/// Facade for error checking in LaTeX documents
/// </summary>
public class ErrorCheckingFacade : iErrorCheckingFacade
{
    private readonly iErrorAnalyser _errorAnalyser;
    private readonly iErrorPresenter _errorPresenter;
    
    /// <summary>
    /// Constructor for ErrorCheckingFacade
    /// </summary>
    public ErrorCheckingFacade(iErrorAnalyser errorAnalyser, iErrorPresenter errorPresenter)
    {
        _errorAnalyser = errorAnalyser ?? throw new ArgumentNullException(nameof(errorAnalyser));
        _errorPresenter = errorPresenter ?? throw new ArgumentNullException(nameof(errorPresenter));
    }
    
    /// <summary>
    /// Processes errors in a document
    /// </summary>
    public List<ErrorStyle> ProcessError(string documentID, string latexContent)
    {
        try
        {
            // Log the process start
            Console.WriteLine($"[INFO] Processing errors for document ID: {documentID}");
            
            // Use the ErrorAnalyser to detect errors
            var errors = _errorAnalyser.SuggestErrorSolution(latexContent);
            
            // Log the number of errors found
            Console.WriteLine($"[INFO] Found {errors.Count} errors in document ID: {documentID}");
            
            return errors;
        }
        catch (Exception ex)
        {
            // Log and handle any exceptions
            _errorPresenter.ShowLaTeXCompilerErrorMessage($"Error processing document: {ex.Message}");
            return new List<ErrorStyle>();
        }
    }
    
    /// <summary>
    /// Fixes an error in a document
    /// </summary>
    public string FixError(string latexContent, ErrorStyle error)
    {
        try
        {
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