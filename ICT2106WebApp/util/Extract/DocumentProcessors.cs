using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ICT2106WebApp.mod1Grp3;
using Microsoft.Extensions.Options;
using MongoDB.Bson; // Bson - Binary JSON
// MongoDB packages
using MongoDB.Driver;
using Utilities;

namespace Utilities
{
	public class DocumentProcessors
	{
		private static readonly string jsonOutputPath = "output.json";
		private static readonly string filePath = "Datarepository_zx_v4.docx";

		// Method: Return full path
		private static string ReturnFullFilePath(string fileName)
		{
			string currentDir = Directory.GetCurrentDirectory();
			return Path.Combine(currentDir, fileName);
		}

		// Method: Save content as JSON
		// Return A string containing the serialized JSON for checking purposes later
		public static string SaveDocumentDataToJsonFile(object documentData)
		{
			var jsonOutput = JsonSerializer.Serialize(
				documentData,
				new JsonSerializerOptions
				{
					WriteIndented = true,
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				}
			);

			File.WriteAllText(jsonOutputPath, jsonOutput);
			Console.WriteLine($"✅ JSON saved to {jsonOutputPath}");

			return jsonOutput;
		}

		//Method: Parse the document
		public async Task<List<object>> ParseDocument(IMongoDatabase database, string fileName)
		{
			var documentControl = new DocumentControl(); // Must be declared inside the method
			var fullFilePath = ReturnFullFilePath(fileName);

			using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, false))
			{
				var documentContents = ExtractContent.ExtractDocumentContents(wordDoc);
				var rootElement = ExtractContent.elementRoot();
				documentContents.Insert(0, ExtractContent.ExtractLayout(wordDoc));
				documentContents.Insert(0, rootElement);

				// for the other contents
				var documentData = new
				{
					// metadata = DocumentMetadataExtractor.GetMetadata(wordDoc),
					metadata = ExtractContent.GetDocumentMetadata(
						wordDoc,
						ReturnFullFilePath(fileName)
					),
					headers = DocumentHeadersFooters.ExtractHeaders(wordDoc),
					footers = DocumentHeadersFooters.ExtractFooters(wordDoc),
					document = documentContents,
				};

				// save extracted content to json and return json string
				string jsonString = SaveDocumentDataToJsonFile(documentData);

				// uncomment to see consolelogs for checking purposes
				// ExtractContent.checkJson((jsonString);

				return documentContents;
			}
		}
	}
}
