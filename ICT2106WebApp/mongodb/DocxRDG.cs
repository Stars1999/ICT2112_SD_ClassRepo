using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Utilities;

// public class DocxRDG : IDocumentRetrieve, IDocumentUpdate
// {
//     private readonly IMongoCollection<Docx> _docxCollection;
//     private readonly IDocumentRetrieveNotify _docxRetrieveNotify;

//     private readonly IDocumentUpdateNotify _docxUpdateNotify;

//     public DocxRDG(IMongoClient mongoClient, string databaseName,
//                    IDocumentUpdateNotify docxUpdateNotify, IDocumentRetrieveNotify docxRetrieveNotify)
//     {
//         var database = mongoClient.GetDatabase(databaseName);
//         _docxCollection = database.GetCollection<Docx>("wordoc");
//         _docxUpdateNotify = docxUpdateNotify;
//         _docxRetrieveNotify = docxRetrieveNotify;
//     }

//     // IDocumentRetrieve
//     public async Task<Docx> getDocument(string id)
//     {
//         Console.WriteLine("DocxRDG -> getDocument");
//         var docx = await _docxCollection.Find(d => d.Id == id).FirstOrDefaultAsync();
//         if (docx != null)
//         {
//             await _docxRetrieveNotify.notifyRetrievedDocument(docx);  // Directly notify retrieval
//         }
//         return docx;
//     }

//     // IDocumentUpdate
//     public async Task saveDocument(Docx docx)
//     {
//         Console.WriteLine("DocxRDG -> saveDocument");
//         await _docxCollection.InsertOneAsync(docx);
//         await _docxUpdateNotify.notifyUpdatedDocument(docx); // Notify about the update
//     }

//     public async Task<List<Docx>> GetAllAsync()
//     {
//         return await _docxCollection.Find(d => true).ToListAsync();
//     }

//     public async Task UpdateAsync(Docx docx)
//     {
//         await _docxCollection.ReplaceOneAsync(d => d.Id == docx.Id, docx);
//     }

//     public async Task DeleteAsync(string id)
//     {
//         await _docxCollection.DeleteOneAsync(d => d.Id == id);
//     }
// }

public class DocxRDG : IDocumentRetrieve, IDocumentUpdate
{
	private readonly IMongoCollection<Docx> _docxCollection;
	private readonly Lazy<IDocumentRetrieveNotify> _docxRetrieveNotify; // need to use Lazy<> because DocxRDG uses IDocumentRetrieveNotify, DocumentFailSafe implements IDocumentRetrieveNotify, creates circular dependency, Lazy<> prevents it by initialising the interface that it uses only when required.
	private readonly Lazy<IDocumentUpdateNotify> _docxUpdateNotify; // same as _docxRetrieveNotfiy

	public DocxRDG(IMongoDatabase database, IServiceProvider serviceProvider)
	{
		_docxCollection = database.GetCollection<Docx>("wordoc");
		_docxUpdateNotify = new Lazy<IDocumentUpdateNotify>(
			() => serviceProvider.GetRequiredService<IDocumentUpdateNotify>()
		);
		_docxRetrieveNotify = new Lazy<IDocumentRetrieveNotify>(
			() => serviceProvider.GetRequiredService<IDocumentRetrieveNotify>()
		);
	}

	// IDocumentRetrieve
	public async Task<Docx> getDocument(string id)
	{
		Console.WriteLine("DocxRDG -> getDocument");
		var docx = await _docxCollection.Find(d => d.Id == id).FirstOrDefaultAsync();
		if (docx != null)
		{
			await _docxRetrieveNotify.Value.notifyRetrievedDocument(docx); // Directly notify retrieval
		}
		return docx;
	}

	// IDocumentUpdate
	public async Task saveDocument(Docx docx)
	{
		Console.WriteLine("DocxRDG -> saveDocument");
		await _docxCollection.InsertOneAsync(docx);
		await _docxUpdateNotify.Value.notifyUpdatedDocument(docx); // Notify about the update
	}

	public async Task<List<Docx>> GetAllAsync()
	{
		return await _docxCollection.Find(d => true).ToListAsync();
	}

	public async Task UpdateAsync(Docx docx)
	{
		await _docxCollection.ReplaceOneAsync(d => d.Id == docx.Id, docx);
	}

	public async Task DeleteAsync(string id)
	{
		await _docxCollection.DeleteOneAsync(d => d.Id == id);
	}
}
