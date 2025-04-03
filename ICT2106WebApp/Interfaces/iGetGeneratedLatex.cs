public interface iGetGeneratedLatex
{
    /// Generates LaTeX content from the provided JSON data.
    void GenerateLatex();

    /// Retrieves the generated LaTeX content.
    string GetLatexContent();
}