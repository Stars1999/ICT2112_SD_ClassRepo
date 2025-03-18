using System.Collections.Generic;

/// <summary>
/// Interface for the ErrorPresenter class
/// Responsible for displaying/logging errors in LaTeX processing.
/// </summary>
public interface iErrorPresenter
{
    /// <summary>
    /// Shows PDF generation error messages.
    /// </summary>
    string ShowPDFGenerationErrorMessage(string errorMessage);

    /// <summary>
    /// Shows LaTeX compiler error messages.
    /// </summary>
    string ShowLaTeXCompilerErrorMessage(string errorMessage);

    /// <summary>
    /// Shows BibTeX converter error messages.
    /// </summary>
    string ShowBiBTeXConvertorErrorMessage(string errorMessage);

    /// <summary>
    /// Gets details for a specific error type.
    /// </summary>
    ErrorStyle GetErrorTypeDetails(int errorTypeID);

    /// <summary>
    /// Fetches all errors for a document.
    /// </summary>
    List<ErrorStyle> FetchAllError(string latexContent);

    /// <summary>
    /// Updates an error in the system.
    /// </summary>
    void UpdateError(ErrorStyle error);
}
