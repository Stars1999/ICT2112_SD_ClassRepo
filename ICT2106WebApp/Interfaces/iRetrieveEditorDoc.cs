using System.Threading.Tasks;

public interface iRetrieveEditorDoc
{
    Task<EditorDocument> GetLatestAsync();
}
