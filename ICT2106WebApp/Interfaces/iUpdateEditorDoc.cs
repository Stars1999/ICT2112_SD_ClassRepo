using System.Threading.Tasks;

public interface iUpdateEditorDoc
{
    Task UpdateLatexContentAsync(string latexContent);
    Task UpdateLatexContentAsync(); // For pipeline
}
