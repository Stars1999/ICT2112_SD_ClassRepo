using System;
using System.Threading.Tasks;

public class EditorDoc: iUpdateEditorDoc
{
    private readonly EditorDocumentMapper _mapper;
    private readonly iGetGeneratedLatex _latexProvider;

    public EditorDoc(EditorDocumentMapper mapper, iGetGeneratedLatex latexProvider)
    {
        _mapper = mapper;
        _latexProvider = latexProvider;
    }


    // Used for autosave / live editing
    public async Task UpdateLatexContentAsync(string latexContent)
    {
        if (string.IsNullOrEmpty(latexContent)) return;

        var doc = new EditorDocument
        {
            DocumentID = 1,
            LatexContent = latexContent,
            LastModified = DateTime.UtcNow
        };

        await _mapper.UpsertAsync(doc);
    }

    // Pipeline-based update using internal latex provider
    public async Task UpdateLatexContentAsync()
    {
        string latex = _latexProvider.GetLatexContent();
        await UpdateLatexContentAsync(latex); // Reuse logic
    }


    public async Task<string> GetLatexContentAsync()
    {
        var doc = await _mapper.GetLatestAsync();
        if (doc == null || string.IsNullOrEmpty(doc.LatexContent))
        {
            Console.WriteLine("[WARNING] No LaTeX content found in EditorDocuments collection.");
            return string.Empty;
        }

        return doc.LatexContent;
    }
}
