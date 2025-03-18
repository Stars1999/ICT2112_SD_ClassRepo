public interface iConversionStatus
{
    /// <summary>
    /// ✅ Checks if the conversion process has completed successfully.
    /// Returns true if a converted JSON exists, otherwise false.
    /// </summary>
    bool fetchConversionStatus();

    /// <summary>
    /// ✅ Allows setting the updated JSON to be shared with LatexCompiler.
    /// </summary>
    void SetUpdatedJson(string convertedJson);

    /// <summary>
    /// ✅ Retrieves the updated JSON.
    /// </summary>
    string GetUpdatedJson();
}
