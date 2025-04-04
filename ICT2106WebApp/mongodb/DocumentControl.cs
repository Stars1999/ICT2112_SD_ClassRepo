// public class DocumentControl : IDocumentUpdateNotify
// {
// 	// private readonly IDocumentUpdate _docxUpdate;

// 	private readonly IDocumentUpdate _dbGateway;

// 	private readonly Docx docxEntity;
// 	public DocumentControl()
// 	{
// 		_dbGateway = (IDocumentUpdate)new DocumentGateway_RDG();
// 		_dbGateway.docxUpdate = this;
// 	}

// 	// IDocumentUpdateNotify
// 	public async Task notifyUpdatedDocument(Docx docx)
// 	{
// 		// Console.WriteLine($"DocumentControl -> Notify Document updated: {docx.Title}");
// 		Console.WriteLine($"DocumentControl -> Notify Document updated: {docx.GetDocxAttributeValue("title")}");
// 		// Additional async operations if necessary
// 		await Task.CompletedTask; // Keeps method async-compatible
// 	}

// 	// To save local document
// 	public async Task saveDocumentToDatabase(string filePath)
// 	{
// 		// Validate file exists
// 		if (!File.Exists(filePath))
// 		{
// 			throw new FileNotFoundException($"File not found: {filePath}");
// 		}

// 		// Validate it's a .docx file
// 		if (!filePath.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
// 		{
// 			throw new ArgumentException("File is not a .docx format");
// 		}

// 		try
// 		{
// 			// Read file into byte array
// 			byte[] fileData = await File.ReadAllBytesAsync(filePath);

// 			// Create Docx object
// 			// var docx = new Docx
// 			// {
// 			// 	Title = Path.GetFileNameWithoutExtension(filePath),
// 			// 	FileName = Path.GetFileName(filePath),
// 			// 	ContentType =
// 			// 		"application/vnd.openxmlformats-officedocument.wordprocessingml.document",
// 			// 	UploadDate = DateTime.UtcNow,
// 			// 	FileData = fileData,
// 			// };
// 			var docx = docxEntity.CreateDocx(Path.GetFileNameWithoutExtension(filePath),Path.GetFileName(filePath), fileData);
// 			// Check if _docxUpdate is null or not initialized
// 			// if (_docxUpdate == null)
// 			// {
// 			// 	Console.WriteLine("Error: _docxUpdate is not initialized.");
// 			// 	return;
// 			// }

// 			// Use RDG method to save document
// 			await _dbGateway.saveDocument(docx);

// 			// Console.WriteLine($"DocumentControl -> Document saved: {docx.Title}");
// 			Console.WriteLine($"DocumentControl -> Document saved: {docx.GetDocxAttributeValue("title")}");

// 		}
// 		catch (Exception ex)
// 		{
// 			Console.WriteLine($"DocumentControl -> Error saving document: {ex.Message}");
// 			Console.WriteLine(ex.StackTrace); // Log the stack trace to help identify where the issue occurred
// 			throw;
// 		}
// 	}

// 	public async Task saveJsonToDatabase(string filePath)
// 	{
// 		Console.WriteLine("DocumentControl -> saving json");
// 		await _dbGateway.saveJsonFile(filePath);
// 	}
// }
