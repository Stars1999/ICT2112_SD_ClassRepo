public interface iConversionStatus
{
    /// Checks if the conversion process has completed successfully.
    /// Returns true if a converted JSON exists, otherwise false.
    bool fetchConversionStatus();

    /// <summary>
    /// Allows setting the updated JSON to be shared with LatexCompiler.
    void SetUpdatedJson(string convertedJson);

    /// <summary>
    /// Retrieves the updated JSON.
    string GetUpdatedJson();
}
