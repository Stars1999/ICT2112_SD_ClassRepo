using System.Collections.Generic;

/// <summary>
/// Interface for the ErrorPresenter class
/// Responsible for displaying/logging errors in LaTeX processing.
/// </summary>
public interface iErrorPresenter
{
    /// <summary>
    /// Shows PDF generation error messages.
    string ShowPDFGenerationErrorMessage(string errorMessage);

    /// <summary>
    /// Shows LaTeX compiler error messages.
    string ShowLaTeXCompilerErrorMessage(string errorMessage);

    /// <summary>
    /// Shows BibTeX converter error messages.
    string ShowBiBTeXConvertorErrorMessage(string errorMessage);

    /// <summary>
    /// Gets details for a specific error type.
    ErrorStyle GetErrorTypeDetails(int errorTypeID);

    /// <summary>
    /// Fetches all errors for a document.
    List<ErrorStyle> FetchAllError(string latexContent);

    /// <summary>
    /// Updates an error in the system.
    void UpdateError(ErrorStyle error);
}
