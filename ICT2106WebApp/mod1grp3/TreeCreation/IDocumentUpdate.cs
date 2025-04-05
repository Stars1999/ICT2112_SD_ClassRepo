namespace ICT2106WebApp.mod1Grp3
{
	public interface IDocumentUpdate
	{
		IDocumentUpdateNotify docxUpdate { get; set; }

		Task<List<Docx>> GetAllAsync();

		Task saveDocument(Docx docx);
		Task saveJsonFile(string filepath);
	}
}
