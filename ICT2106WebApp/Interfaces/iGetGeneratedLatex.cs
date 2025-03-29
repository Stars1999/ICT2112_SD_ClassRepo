public interface iGetGeneratedLatex
{
    /// <summary>
    /// Generates LaTeX content from the provided JSON data.
    /// </summary>
    void GenerateLatex();

    /// <summary>
    /// Retrieves the generated LaTeX content.
    /// </summary>
    string GetLatexContent();
}