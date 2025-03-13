public interface IMLA
{
    string FormatCitations(string latexContent);
    string FormatBibliographies(string latexContent);
    void ApplyMLAFormatting(); // Optional MLA-specific logic
}
