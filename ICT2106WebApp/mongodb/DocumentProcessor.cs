// using System;
// using System.IO;
// using System.Threading.Tasks;

// public class DocumentParsing : IDocumentUpdateNotify
// {
//     private readonly IDocumentUpdate _docxUpdate;  // Directly inject the dependency instead of using Lazy<T>

// private readonly DocxRDG
//     // Constructor injection instead of Lazy<T>
//     public DocumentParsing(IDocumentUpdate docxUpdate)
//     {
//         _docxUpdate = docxUpdate ?? throw new ArgumentNullException(nameof(docxUpdate));  // Ensure valid injection
//     }

//     // ✅ Create new document and store in DB
//     public async Task CreateDocxAsync(Docx docx)
//     {
//         if (docx == null)
//         {
//             throw new ArgumentNullException(nameof(docx), "Document cannot be null.");
//         }

//         await _docxUpdate.saveDocument(docx); // Save document using injected dependency
//     }

//     // IDocumentUpdateNotify
//     public async Task notifyUpdatedDocument(Docx docx)
//     {
//         Console.WriteLine($"DocumentProcessor -> Document updated: {docx.Title}");
//         // Additional async operations if necessary
//         await Task.CompletedTask; // Keeps method async-compatible
//     }

//     // To save local document
//     public async Task saveDocumentToDatabase(string filePath)
//     {
//         // Validate file exists
//         if (!File.Exists(filePath))
//         {
//             throw new FileNotFoundException($"File not found: {filePath}");
//         }

//         // Validate it's a .docx file
//         if (!filePath.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
//         {
//             throw new ArgumentException("File is not a .docx format");
//         }

//         try
//         {
//             // Read file into byte array
//             byte[] fileData = await File.ReadAllBytesAsync(filePath);

//             // Create Docx object
//             var docx = new Docx
//             {
//                 Title = Path.GetFileNameWithoutExtension(filePath),
//                 FileName = Path.GetFileName(filePath),
//                 ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
//                 UploadDate = DateTime.UtcNow,
//                 FileData = fileData
//             };

//             // Check if _docxUpdate is null or not initialized
//             if (_docxUpdate == null)
//             {
//                 Console.WriteLine("Error: _docxUpdate is not initialized.");
//                 return;
//             }

//             // Use RDG method to save document
//             await _docxUpdate.saveDocument(docx);

//             Console.WriteLine($"DocumentProcessor -> Document saved: {docx.Title}");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"DocumentProcessor -> Error saving document: {ex.Message}");
//             Console.WriteLine(ex.StackTrace);  // Log the stack trace to help identify where the issue occurred
//             throw;
//         }
//     }
// }


public class DocumentParsing : IDocumentUpdateNotify
{
	private readonly Lazy<IDocumentUpdate> _docxUpdate; // Directly inject the dependency instead of using Lazy<T>

	// private readonly DocxRDG _docxRDG;

	public DocumentParsing(IServiceProvider serviceProvider)
	{
		_docxUpdate = new Lazy<IDocumentUpdate>(
			() => serviceProvider.GetRequiredService<IDocumentUpdate>()
		);
		// _docxUpdate = docxUpdate ?? throw new ArgumentNullException(nameof(docxUpdate));  // Ensure valid injection
	}

	// ✅ Create new document and store in DB
	public async Task CreateDocxAsync(Docx docx)
	{
		if (docx == null)
		{
			throw new ArgumentNullException(nameof(docx), "Document cannot be null.");
		}

		await _docxUpdate.Value.saveDocument(docx); // Save document using injected dependency
	}

	// IDocumentUpdateNotify
	public async Task notifyUpdatedDocument(Docx docx)
	{
		Console.WriteLine($"DocumentProcessor -> Notify Document updated: {docx.Title}");
		// Additional async operations if necessary
		await Task.CompletedTask; // Keeps method async-compatible
	}

	// To save local document
	public async Task saveDocumentToDatabase(string filePath)
	{
		// Validate file exists
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException($"File not found: {filePath}");
		}

		// Validate it's a .docx file
		if (!filePath.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException("File is not a .docx format");
		}

		try
		{
			// Read file into byte array
			byte[] fileData = await File.ReadAllBytesAsync(filePath);

			// Create Docx object
			var docx = new Docx
			{
				Title = Path.GetFileNameWithoutExtension(filePath),
				FileName = Path.GetFileName(filePath),
				ContentType =
					"application/vnd.openxmlformats-officedocument.wordprocessingml.document",
				UploadDate = DateTime.UtcNow,
				FileData = fileData,
			};

			// Check if _docxUpdate is null or not initialized
			if (_docxUpdate == null)
			{
				Console.WriteLine("Error: _docxUpdate is not initialized.");
				return;
			}

			// Use RDG method to save document
			await _docxUpdate.Value.saveDocument(docx);

			Console.WriteLine($"DocumentProcessor -> Document saved: {docx.Title}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"DocumentProcessor -> Error saving document: {ex.Message}");
			Console.WriteLine(ex.StackTrace); // Log the stack trace to help identify where the issue occurred
			throw;
		}
	}
}
