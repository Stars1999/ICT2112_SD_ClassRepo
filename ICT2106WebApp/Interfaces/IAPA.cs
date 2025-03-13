public interface IAPA
{
    string FormatCitations(string latexContent);
    string FormatBibliographies(string latexContent);
    void ApplyAPAFormatting(); // Optional APA-specific logic
}
