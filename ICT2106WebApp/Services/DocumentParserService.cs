using ICT2106WebApp.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace ICT2106WebApp.Services
{
    public class DocumentParserService : IParser
    {
        private readonly string _uploadPath = "wwwroot/uploads"; // Temporary path to store files
        private readonly Dictionary<string, string> _conversionStatus; // To track conversion status

        // Constructor to ensure the upload directory exists
        public DocumentParserService()
        {
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            _conversionStatus = new Dictionary<string, string>();
        }

        // Method to store the document temporarily
        public void StoreDocument(IFormFile file)
        {
            var filePath = Path.Combine(_uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Set initial conversion status 
            _conversionStatus[file.FileName] = "Processing";
        }

        // Method to retrieve the document by file name
        public IParser.Document RetrieveDocument(string documentIdentifier)
        {
            var filePath = Path.Combine(_uploadPath, documentIdentifier);
            if (File.Exists(filePath))
            {
                return new IParser.Document(documentIdentifier, documentIdentifier, filePath);  // Return IParser.Document
            }

            return null;  // Return null if the document is not found
        }

        // Method to get the current conversion status of the document
        public string GetConversionStatus(string documentIdentifier)
        {
            if (_conversionStatus.ContainsKey(documentIdentifier))
            {
                return _conversionStatus[documentIdentifier];
            }
            return "Unknown"; // Default status if no status found
        }

        // Method to update the conversion status 
        public void UpdateConversionStatus(string documentIdentifier, string status)
        {
            if (_conversionStatus.ContainsKey(documentIdentifier))
            {
                _conversionStatus[documentIdentifier] = status;
            }
        }
    }
}
