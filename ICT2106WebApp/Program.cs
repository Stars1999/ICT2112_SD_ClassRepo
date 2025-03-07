using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Utilities;

// MongoDB packages
using MongoDB.Driver;
using MongoDB.Bson; // Bson - Binary JSON

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddLogging(); // Add logging for testing MongoDB

// Register MongoDB client as a singleton
builder.Services.AddSingleton<IMongoClient>(sp =>
{
	var connectionString = builder.Configuration.GetConnectionString("MongoDbConnection"); // "MongoDbConnection" credentials stored in appsettings.json
	return new MongoClient(connectionString);
});

// Register the database as a singleton , database transactions will use same instance
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
	var client = sp.GetRequiredService<IMongoClient>();
	return client.GetDatabase("inf2112");
});


var app = builder.Build();

// Get logger instance
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// MongoDB testing, FYI test pass, able to insert into collection
app.Lifetime.ApplicationStarted.Register(async () =>
{
	using var scope = app.Services.CreateScope();
	var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
	var collection = database.GetCollection<BsonDocument>("testCollection");

	try
	{
		// Insert Data
		var document = new BsonDocument { { "name", "Test User" }, { "email", "test@example.com" } };
		await collection.InsertOneAsync(document);
		logger.LogInformation("✅ Test document inserted into MongoDB.");

		// Read Data
		var firstDocument = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();
		if (firstDocument != null)
		{
			logger.LogInformation("📌 Retrieved Document: {Document}", firstDocument.ToJson());
		}
		else
		{
			logger.LogWarning("⚠️ No data found in MongoDB.");
		}
	}
	catch (Exception ex)
	{
		logger.LogError("❌ MongoDB test failed: {ErrorMessage}", ex.Message);
	}
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

DocumentProcessor.RunMyProgram();

app.Run();


// ✅ Extracts content from Word document
public static class DocumentProcessor
{
	public static void RunMyProgram()
	{
		string filePath = "Datarepository_v2.docx"; // Change this to your actual file path
		string jsonOutputPath = "output.json"; // File where JSON will be saved

		using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
		{
			var documentData = new
			{
				// metadata = DocumentMetadataExtractor.GetMetadata(wordDoc),
				// headers = DocumentHeadersFooters.ExtractHeaders(wordDoc),
				// !!footer still exists issues. Commented for now
				// footers = DocumentHeadersFooters.ExtractFooters(wordDoc),

				document = ExtractDocumentContents(wordDoc), // ✅ Calls ExtractDocumentContents()
			};

			// Convert to JSON format with UTF-8 encoding fix (preserves emojis, math, and Chinese)
			string jsonOutput = JsonSerializer.Serialize(
				documentData,
				new JsonSerializerOptions
				{
					WriteIndented = true,
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				}
			);

			// ✅ Write JSON to file
			File.WriteAllText(jsonOutputPath, jsonOutput);
			Console.WriteLine($"✅ JSON output saved to {jsonOutputPath}");
		}
	}

	public static List<object> ExtractDocumentContents(WordprocessingDocument doc)
	{
		var elements = new List<object>();
		var body = doc.MainDocumentPart?.Document?.Body;

		if (body == null)
		{
			Console.WriteLine("❌ Error: Document body is null.");
			return elements;
		}

		foreach (var element in body.Elements<OpenXmlElement>())
		{
			// ✅ Check for a Drawing element inside the run (Extract Images)
			var drawing = element.Descendants<DocumentFormat.OpenXml.Wordprocessing.Drawing>().FirstOrDefault();
			if (drawing != null)
			{
				var imageObjects = ExtractContent.ExtractImagesFromDrawing(doc, drawing);
				elements.AddRange(imageObjects);
			}
			else if (element is DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph)
			{
				// ✅ Extract Paragraphs
				elements.Add(ExtractContent.ExtractParagraph(paragraph, doc));
			}
			else if (element is Table table)
			{
				Console.WriteLine("📝 Extracting Table");
				elements.Add(ExtractContent.ExtractTable(table)); // ✅ Extract Tables
			}
		}
		return elements;
	}
}