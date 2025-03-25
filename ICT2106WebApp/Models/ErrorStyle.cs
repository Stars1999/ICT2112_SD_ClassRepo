using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Class representing a LaTeX error with style information
/// </summary>
public class ErrorStyle
{
     [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ErrorId { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The line number where the error occurred
    /// </summary>
    [BsonElement("lineNumber")]
    public int LineNumber { get; set; }
    
    /// <summary>
    /// The type of error
    /// </summary>
    [BsonElement("errorType")]
    public string ErrorType { get; set; }
    
    /// <summary>
    /// The error message
    /// </summary>
    [BsonElement("message")]
    public string Message { get; set; }
    
    /// <summary>
    /// Suggestion for fixing the error
    /// </summary>
    [BsonElement("suggestion")]
    public string Suggestion { get; set; }
    
    /// <summary>
    /// The severity of the error (high, medium, low)
    /// </summary>
    [BsonElement("severity")]
    public string Severity { get; set; }
    
    /// <summary>
    /// Whether the error can be automatically fixed
    /// </summary>
    [BsonElement("canFix")]
    public bool CanFix { get; set; }
    
    /// <summary>
    /// Whether the error has been fixed
    /// </summary>
    [BsonElement("fixed")]
    public bool Fixed { get; set; }
    
    /// <summary>
    /// The column position of the error
    /// </summary>
    [BsonElement("column")]
    public int Column { get; set; }
    
    /// <summary>
    /// The length of the error text
    /// </summary>
    [BsonElement("length")]
    public int Length { get; set; }
    
    /// <summary>
    /// Automatic fix for the error (if available)
    /// </summary>
    [BsonElement("autoFix")]
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