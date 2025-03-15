public interface IDocumentUpdate
{
	Task<List<Docx>> GetAllAsync();

	Task saveDocument(Docx docx);
}
