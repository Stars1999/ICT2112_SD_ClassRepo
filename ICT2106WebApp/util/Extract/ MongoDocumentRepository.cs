using System;
using System.Threading.Tasks;
using ICT2106WebApp.mod1Grp3;

public class MongoDocumentRepository : IDocumentRepository
{
	private readonly IDocumentUpdate _gateway;

	public MongoDocumentRepository(IDocumentUpdate gateway)
	{
		_gateway = gateway;
	}

	public async Task SaveDocument(Docx docx)
	{
		try
		{
			await _gateway.saveDocument(docx);
			Console.WriteLine($"✅ Document saved: {docx.GetDocxAttributeValue("title")}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ Error saving document: {ex.Message}");
		}
	}

	public async Task SaveJson(string jsonFilePath)
	{
		try
		{
			await _gateway.saveJsonFile(jsonFilePath);
			Console.WriteLine("✅ JSON saved to database.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ Error saving JSON: {ex.Message}");
		}
	}
}
