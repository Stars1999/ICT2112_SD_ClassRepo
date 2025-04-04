// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Text.Encodings.Web;
// using System.Text.Json;
// using System.Text.RegularExpressions;
// using DocumentFormat.OpenXml;
// using DocumentFormat.OpenXml.Packaging;
// using DocumentFormat.OpenXml.Wordprocessing;
// using ICT2106WebApp.mod1Grp3;
// // using ICT2106WebApp.mod1grp4;
// using Microsoft.Extensions.Options;
// using MongoDB.Bson; // Bson - Binary JSON
// // MongoDB packages
// using MongoDB.Driver;
// using Newtonsoft.Json; // For JsonConvert
// using Newtonsoft.Json.Linq; // Bson - Binary JSON
// using Utilities;

// namespace Utilities
// {
// 	// Class
// 	public static partial class ExtractContent
// 	{
// 		// the following is ok
// 		// check if there in content and return accordingly


// 		public static Dictionary<string, object> GetDocumentLayout(WordprocessingDocument doc)
// 		{
// 			var layout = new Dictionary<string, object>();
// 			var mainDocumentPart = doc.MainDocumentPart;

// 			if (mainDocumentPart == null || mainDocumentPart.Document.Body == null)
// 				return layout;

// 			// Get all section properties in the document
// 			var allSectionProps = mainDocumentPart
// 				.Document.Body.Descendants<SectionProperties>()
// 				.ToList();

// 			if (allSectionProps.Count == 0)
// 				return layout;

// 			// Get the first section only
// 			var sectionProps = allSectionProps.FirstOrDefault();

// 			// Page size
// 			var pageSize = sectionProps?.Elements<PageSize>().FirstOrDefault();
// 			if (pageSize != null)
// 			{
// 				bool isLandscape =
// 					pageSize.Orient != null
// 					&& pageSize.Orient.Value == PageOrientationValues.Landscape;

// 				layout["orientation"] = isLandscape ? "Landscape" : "Portrait";
// 				// Console.WriteLine($"Orientation: {(isLandscape ? "Landscape" : "Portrait")}");

// 				if (pageSize.Width != null)
// 				{
// 					layout["pageWidth"] = ConvertTwipsToCentimeters((int)pageSize.Width.Value);
// 					// Console.WriteLine(
// 					//     $"Page Width: {layout["pageWidth"]} cm (Original: {pageSize.Width.Value} twips)"
// 					// );
// 				}

// 				if (pageSize.Height != null)
// 				{
// 					layout["pageHeight"] = ConvertTwipsToCentimeters((int)pageSize.Height.Value);
// 					// Console.WriteLine(
// 					//     $"Page Height: {layout["pageHeight"]} cm (Original: {pageSize.Height.Value} twips)"
// 					// );
// 				}
// 			}
// 			else
// 				Console.WriteLine("No page size found in section properties.");

// 			// Columns
// 			var columns = sectionProps?.Elements<Columns>().FirstOrDefault();
// 			if (columns != null)
// 			{
// 				int columnCount = 1;
// 				double columnSpacing = 1.27;

// 				if (columns.ColumnCount != null)
// 					columnCount = columns.ColumnCount.Value;

// 				if (columns.Space != null)
// 					columnSpacing = ConvertTwipsToCentimeters(int.Parse(columns.Space.Value ?? ""));

// 				layout["columnNum"] = columnCount;
// 				layout["columnSpacing"] = columnSpacing;
// 			}
// 			else
// 			{
// 				layout["columnNum"] = 1;
// 				layout["columnSpacing"] = 1.27;
// 			}

// 			// Page margins
// 			var pageMargins = sectionProps?.Elements<PageMargin>().FirstOrDefault();
// 			if (pageMargins != null)
// 			{
// 				var margins = new Dictionary<string, double>();
// 				// Console.WriteLine("Margins found:");

// 				if (pageMargins.Top != null)
// 					margins["top"] = ConvertTwipsToCentimeters(pageMargins.Top.Value);

// 				if (pageMargins.Bottom != null)
// 					margins["bottom"] = ConvertTwipsToCentimeters(pageMargins.Bottom.Value);

// 				if (pageMargins.Left != null)
// 					margins["left"] = ConvertTwipsToCentimeters((int)pageMargins.Left.Value);

// 				if (pageMargins.Right != null)
// 					margins["right"] = ConvertTwipsToCentimeters((int)pageMargins.Right.Value);

// 				if (pageMargins.Header != null)
// 					margins["header"] = ConvertTwipsToCentimeters((int)pageMargins.Header.Value);

// 				if (pageMargins.Footer != null)
// 					margins["footer"] = ConvertTwipsToCentimeters((int)pageMargins.Footer.Value);

// 				layout["margins"] = margins;
// 			}
// 			else
// 				Console.WriteLine("No page margins found in section properties.");

// 			return layout;
// 		}

// 		// extract document content
// 		public static List<object> ExtractDocumentContents(WordprocessingDocument doc)
// 		{
// 			var elements = new List<object>();
// 			var body = doc.MainDocumentPart?.Document?.Body;

// 			if (body == null)
// 			{
// 				Console.WriteLine("❌ Error: Document body is null.");
// 				return elements;
// 			}

// 			bool haveBibliography = false;

// 			foreach (var element in body.Elements<OpenXmlElement>())
// 			{
// 				// ✅ Check for a Drawing element inside the run (Extract Images)
// 				var drawing = element
// 					.Descendants<DocumentFormat.OpenXml.Wordprocessing.Drawing>()
// 					.FirstOrDefault();
// 				if (drawing != null)
// 				{
// 					var imageObjects = ExtractContent.ExtractImagesFromDrawing(doc, drawing);
// 					elements.AddRange(imageObjects);
// 				}
// 				else if (element is DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph)
// 				{
// 					// ✅ Extract Paragraphs
// 					elements.Add(
// 						ExtractContent.ExtractParagraph(paragraph, doc, ref haveBibliography)
// 					);
// 				}
// 				else if (element is DocumentFormat.OpenXml.Wordprocessing.Table table)
// 				{
// 					// Console.WriteLine("📝 Extracting Table by another modue");
// 					elements.Add(ExtractContent.ExtractTable(table)); // ✅ Extract Tables
// 				}
// 			}
// 			return elements;
// 		}

// 		// get my meta data
// 		public static Dictionary<string, string> GetDocumentMetadata(
// 			WordprocessingDocument doc,
// 			string filepath
// 		)
// 		{
// 			var metadata = new Dictionary<string, string>();
// 			if (doc.PackageProperties.Title != null)
// 				metadata["Title"] = doc.PackageProperties.Title;
// 			if (doc.PackageProperties.Creator != null)
// 				metadata["Author"] = doc.PackageProperties.Creator;

// 			// Created & Modified (from the DOCX metadata, not the OS timestamps)
// 			if (doc.PackageProperties.Created != null)
// 				metadata["CreatedDate_Internal"] = doc.PackageProperties.Created.Value.ToString(
// 					"u"
// 				);
// 			if (doc.PackageProperties.Modified != null)
// 				metadata["LastModified_Internal"] = doc.PackageProperties.Modified.Value.ToString(
// 					"u"
// 				);

// 			FileInfo fileInfo = new FileInfo(filepath);

// 			string fileName = fileInfo.Name; // "Example.docx"
// 			long fileSize = fileInfo.Length; // size in bytes

// 			metadata["filename"] = fileName;
// 			metadata["size"] = fileSize.ToString();

// 			Console.WriteLine(metadata);
// 			return metadata;
// 		}

// 		// json related stuff
// 		public static Dictionary<string, object> ConvertJsonElements(
// 			Dictionary<string, object> input
// 		)
// 		{
// 			var result = new Dictionary<string, object>();
// 			foreach (var kvp in input)
// 			{
// 				if (kvp.Value is JsonElement jsonElement)
// 				{
// 					switch (jsonElement.ValueKind)
// 					{
// 						case JsonValueKind.String:
// 							result[kvp.Key] = jsonElement.GetString();
// 							break;
// 						case JsonValueKind.Number:
// 							result[kvp.Key] = jsonElement.GetDouble(); // or GetInt32() depending
// 							break;
// 						case JsonValueKind.True:
// 						case JsonValueKind.False:
// 							result[kvp.Key] = jsonElement.GetBoolean();
// 							break;
// 						case JsonValueKind.Object:
// 						case JsonValueKind.Array:
// 							result[kvp.Key] = jsonElement.ToString(); // fallback as string
// 							break;
// 						default:
// 							result[kvp.Key] = null;
// 							break;
// 					}
// 				}
// 				else
// 				{
// 					result[kvp.Key] = kvp.Value;
// 				}
// 			}
// 			return result;
// 		}

// 		public static async Task ToSaveJson(
// 			DocumentControl documentControl,
// 			string filePath,
// 			string jsonOutputPath
// 		)
// 		{
// 			using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
// 			{
// 				// Get layout information
// 				var layoutInfo = GetDocumentLayout(wordDoc);

// 				// Extract document contents
// 				var documentContents = ExtractDocumentContents(wordDoc);

// 				// Create layout element
// 				var layoutElement = new Dictionary<string, object>
// 				{
// 					{ "type", "layout" },
// 					{ "content", "" },
// 					{
// 						"styling",
// 						new List<object> { layoutInfo }
// 					},
// 				};

// 				// Insert layout as the first element in document contents
// 				documentContents.Insert(0, layoutElement);

// 				// Create root node
// 				var layoutElementRoot = new Dictionary<string, object>
// 				{
// 					{ "id", 0 },
// 					{ "type", "root" },
// 					{ "content", "" },
// 				};
// 				documentContents.Insert(0, layoutElementRoot);

// 				var documentData = new
// 				{
// 					metadata = GetDocumentMetadata(wordDoc, filePath), // Fixed `filePath_full`
// 					document = documentContents,
// 				};

// 				// Convert to JSON format with UTF-8 encoding fix
// 				string jsonOutput = System.Text.Json.JsonSerializer.Serialize(
// 					documentData,
// 					new JsonSerializerOptions
// 					{
// 						WriteIndented = true,
// 						Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
// 					}
// 				);

// 				// Write JSON to file
// 				File.WriteAllText(jsonOutputPath, jsonOutput);
// 				Console.WriteLine($"✅ New data saved to {jsonOutputPath}");

// 				// Save JSON to database (assuming `saveJsonToDatabase` is an async method)
// 				await documentControl.saveJsonToDatabase(jsonOutputPath);
// 			}
// 		}

// 		// Convert twips (1/1440 of an inch) to centimeters
// 		private static double ConvertTwipsToCentimeters(int twips)
// 		{
// 			// 1 inch = 2.54 cm, and 1 inch = 1440 twips
// 			return Math.Round((double)twips / 1440 * 2.54, 2);
// 		}
// 	}
// }
