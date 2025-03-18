using System.Collections.Generic;

/// <summary>
/// Interface for the ErrorAnalyser class
/// </summary>
public interface iErrorAnalyser
{
    /// <summary>
    /// Suggests solutions for detected errors in LaTeX content
    /// </summary>
    List<ErrorStyle> SuggestErrorSolution(string latexContent);
    
    /// <summary>
    /// Applies an automatic fix for a given error
    /// </summary>
    string ChangeError(string latexContent, ErrorStyle error);
}

/// <summary>
/// Interface for the ErrorPresenter class
/// </summary>
public interface iErrorPresenter
{
    /// <summary>
    /// Shows PDF generation error messages
    /// </summary>
    string ShowPDFGenerationErrorMessage(string errorMessage);
    
    /// <summary>
    /// Shows LaTeX compiler error messages
    /// </summary>
    string ShowLaTeXCompilerErrorMessage(string errorMessage);
    
    /// <summary>
    /// Shows BibTeX converter error messages
    /// </summary>
    string ShowBiBTeXConvertorErrorMessage(string errorMessage);
    
    /// <summary>
    /// Gets details for a specific error type
    /// </summary>
    ErrorStyle GetErrorTypeDetails(int errorTypeID);
    
    /// <summary>
    /// Fetches all errors for a document
    /// </summary>
    List<ErrorStyle> FetchAllError(string latexContent);
    
    /// <summary>
    /// Updates an error in the system
    /// </summary>
    void UpdateError(ErrorStyle error);
}

/// <summary>
/// Interface for error checking facade
/// </summary>
public interface iErrorCheckingFacade
{
    /// <summary>
    /// Processes errors in a document
    /// </summary>
    List<ErrorStyle> ProcessError(string documentID, string latexContent);
    
    /// <summary>
    /// Fixes an error in a document
    /// </summary>
    string FixError(string latexContent, ErrorStyle error);
}

/// <summary>
/// Interface for retrieving errors
/// </summary>
public interface iRetrieveError
{
    /// <summary>
    /// Fetches all errors
    /// </summary>
    List<ErrorStyle> FetchAllError();
}

/// <summary>
/// Interface for updating errors
/// </summary>
public interface iUpdateError
{
    /// <summary>
    /// Updates all errors
    /// </summary>
    void UpdateAllError(List<ErrorStyle> errors);
}