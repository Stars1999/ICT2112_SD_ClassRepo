public interface iGetGeneratedLatex
{
    /// <summary>
    /// Generates LaTeX content from the provided JSON data.
    /// </summary>
    void GenerateLatex(string jsonData);

    /// <summary>
    /// Retrieves the generated LaTeX content.
    /// </summary>
    string GetLatexContent();
}