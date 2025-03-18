using System;

/// <summary>
/// Class representing a LaTeX error with style information
/// </summary>
public class ErrorStyle
{
    /// <summary>
    /// The line number where the error occurred
    /// </summary>
    public int LineNumber { get; set; }
    
    /// <summary>
    /// The type of error
    /// </summary>
    public string ErrorType { get; set; }
    
    /// <summary>
    /// The error message
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Suggestion for fixing the error
    /// </summary>
    public string Suggestion { get; set; }
    
    /// <summary>
    /// The severity of the error (high, medium, low)
    /// </summary>
    public string Severity { get; set; }
    
    /// <summary>
    /// Whether the error can be automatically fixed
    /// </summary>
    public bool CanFix { get; set; }
    
    /// <summary>
    /// Whether the error has been fixed
    /// </summary>
    public bool Fixed { get; set; }
    
    /// <summary>
    /// The column position of the error
    /// </summary>
    public int Column { get; set; }
    
    /// <summary>
    /// The length of the error text
    /// </summary>
    public int Length { get; set; }
    
    /// <summary>
    /// Automatic fix for the error (if available)
    /// </summary>
    public string AutoFix { get; set; }
    
    /// <summary>
    /// Create a new ErrorStyle instance
    /// </summary>
    public ErrorStyle()
    {
        // Default values
        LineNumber = 0;
        ErrorType = string.Empty;
        Message = string.Empty;
        Suggestion = string.Empty;
        Severity = "medium";
        CanFix = false;
        Fixed = false;
        Column = 0;
        Length = 0;
        AutoFix = string.Empty;
    }
}