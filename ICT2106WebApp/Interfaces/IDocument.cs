namespace ICT2106WebApp.Interfaces
{
    public interface IDocument
    {
        // Method to store the document temporarily
        void StoreDocument(IFormFile file);

        // Method to retrieve the document by file name
        Document RetrieveDocument(string documentIdentifier);

        // Method to get the conversion status of the document
        string GetConversionStatus(string documentIdentifier);

        // Method to update the conversion status
        void UpdateConversionStatus(string documentIdentifier, string status);

        // Document class is nested within the IParser interface
        public class Document
        {
            public string DocumentId { get; set; }  // Unique identifier for the document
            public string FileName { get; set; } // The name of the document file
            public string FilePath { get; set; } // The path to the document

            // Constructor to initialize properties
            public Document(string documentId, string fileName, string filePath)
            {
                DocumentId = documentId;
                FileName = fileName;
                FilePath = filePath;
            }
        }
    }
}
