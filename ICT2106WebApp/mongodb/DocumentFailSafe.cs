// public class DocumentFailSafe : IDocumentRetrieveNotify{
    
//     private readonly Lazy<IDocumentRetrieve> _docxRetrieve;
//     // private readonly IDocumentRetrieve _docxRetrieve;

//     public DocumentFailSafe(Lazy<IDocumentRetrieve> docxRetrieve)
//     {
//         _docxRetrieve = docxRetrieve;
//     }
    
//     //Retrieve Saved Document
//     public async Task retrieveSavedDocument(string id, string outputPath)
//     {
//         var docx = await _docxRetrieve.Value.getDocument(id);
//         if (docx == null)
//         {
//             throw new FileNotFoundException($"Document with ID {id} not found in database.");
//         }
        
//         // Write the bytes to a file
//         await File.WriteAllBytesAsync(outputPath, docx.FileData);
//         Console.WriteLine($"DocumentFailSafe -> Document retrieved and saved to: {outputPath}");
//     }

//     //IDocumentRetrieveNotify
//     public async Task notifyRetrievedDocument(Docx docx)
//     {
//         Console.WriteLine($"DocumentFailSafe -> Document retrieved: {docx.Title}");
//         // If needed, additional async operations (like logging or further processing) can be added here.
//         await Task.CompletedTask; // Keeps method async-compatible
//     }
// }



// public class DocumentFailSafe : IDocumentRetrieveNotify
// {
//     private readonly IDocumentRetrieve _docxRetrieve;

//     public DocumentFailSafe(IDocumentRetrieve docxRetrieve)
//     {
//         _docxRetrieve = docxRetrieve;
//     }

//     // Retrieve Saved Document
//     public async Task retrieveSavedDocument(string id, string outputPath)
//     {
//         var docx = await _docxRetrieve.getDocument(id);
//         if (docx == null)
//         {
//             throw new FileNotFoundException($"Document with ID {id} not found in database.");
//         }

//         // Write the bytes to a file
//         await File.WriteAllBytesAsync(outputPath, docx.FileData);
//         Console.WriteLine($"DocumentFailSafe -> Document retrieved and saved to: {outputPath}");
//     }

//     // IDocumentRetrieveNotify
//     public async Task notifyRetrievedDocument(Docx docx)
//     {
//         Console.WriteLine($"DocumentFailSafe -> Document retrieved: {docx.Title}");
//         await Task.CompletedTask;
//     }
// }

// public class DocumentFailSafe : IDocumentRetrieveNotify
// {
//     // private readonly IDocumentRetrieve _docxRetrieve;

//     private readonly DocxRDG _docxRDG;
//     public DocumentFailSafe(DocxRDG docxRetrieve)
//     {
//         _docxRDG = docxRetrieve;
//     }

//     // Retrieve Saved Document
//     public async Task retrieveSavedDocument(string id, string outputPath)
//     {
//         var docx = await _docxRDG.getDocument(id);
//         if (docx == null)
//         {
//             throw new FileNotFoundException($"Document with ID {id} not found in database.");
//         }

//         // Write the bytes to a file
//         await File.WriteAllBytesAsync(outputPath, docx.FileData);
//         Console.WriteLine($"DocumentFailSafe -> Document retrieved and saved to: {outputPath}");
//     }

//     // IDocumentRetrieveNotify
//     public async Task notifyRetrievedDocument(Docx docx)
//     {
//         Console.WriteLine($"DocumentFailSafe -> Document retrieved: {docx.Title}");
//         await Task.CompletedTask;
//     }
// }

public class DocumentFailSafe : IDocumentRetrieveNotify
{
    private readonly Lazy<IDocumentRetrieve> _docxRetrieve;

    // private readonly DocxRDG _docxRDG;
    public DocumentFailSafe(IServiceProvider serviceProvider)
    {
        _docxRetrieve = new Lazy<IDocumentRetrieve>(() => 
            serviceProvider.GetRequiredService<IDocumentRetrieve>());
    }

    // Retrieve Saved Document
    public async Task retrieveSavedDocument(string id, string outputPath)
    {
        var docx = await _docxRetrieve.Value.getDocument(id);
        if (docx == null)
        {
            throw new FileNotFoundException($"Document with ID {id} not found in database.");
        }

        // Write the bytes to a file
        await File.WriteAllBytesAsync(outputPath, docx.FileData);
        Console.WriteLine($"DocumentFailSafe -> Document retrieved and saved to: {outputPath}");
    }

    // IDocumentRetrieveNotify
    public async Task notifyRetrievedDocument(Docx docx)
    {
        Console.WriteLine($"DocumentFailSafe -> Document retrieved: {docx.Title}");
        await Task.CompletedTask;
    }
}
