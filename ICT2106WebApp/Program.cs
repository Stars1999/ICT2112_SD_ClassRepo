using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MongoDB.Bson; // Bson - Binary JSON
// MongoDB packages
using MongoDB.Driver;
using Utilities;

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
        var document = new BsonDocument
        {
            { "name", "Test User" },
            { "email", "test@example.com" },
        };
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
    static Dictionary<string, string> GetDocumentMetadata(
        WordprocessingDocument doc,
        string filepath
    )
    {
        var metadata = new Dictionary<string, string>();
        if (doc.PackageProperties.Title != null)
            metadata["Title"] = doc.PackageProperties.Title;
        if (doc.PackageProperties.Creator != null)
            metadata["Author"] = doc.PackageProperties.Creator;

        // Created & Modified (from the DOCX metadata, not the OS timestamps)
        if (doc.PackageProperties.Created != null)
            metadata["CreatedDate_Internal"] = doc.PackageProperties.Created.Value.ToString("u");
        if (doc.PackageProperties.Modified != null)
            metadata["LastModified_Internal"] = doc.PackageProperties.Modified.Value.ToString("u");

        FileInfo fileInfo = new FileInfo(filepath);

        string fileName = fileInfo.Name; // "Example.docx"
        long fileSize = fileInfo.Length; // size in bytes

        metadata["filename"] = fileName;
        metadata["size"] = fileSize.ToString();

        Console.WriteLine(metadata);
        return metadata;
    }

    public static void RunMyProgram()
    {
        string filePath = "Datarepository_zx_v2.docx"; // Change this to your actual file path
        string jsonOutputPath = "output.json"; // File where JSON will be saved

        string currentDir = Directory.GetCurrentDirectory();
        string filePath_full = Path.Combine(currentDir, filePath);

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
        {
            // Get layout information
            var layoutInfo = GetDocumentLayout(wordDoc);

            // Extract document contents
            var documentContents = ExtractDocumentContents(wordDoc);

            // Create layout element
            var layoutElement = new Dictionary<string, object>
            {
                { "type", "layout" },
                { "content", "" },
                {
                    "styling",
                    new List<object> { layoutInfo }
                },
            };

            // Insert layout as the first element in document contents
            documentContents.Insert(0, layoutElement);

            var documentData = new
            {
                // metadata = DocumentMetadataExtractor.GetMetadata(wordDoc),
                metadata = GetDocumentMetadata(wordDoc, filePath_full),
                // headers = DocumentHeadersFooters.ExtractHeaders(wordDoc),
                // !!footer still exists issues. Commented for now
                // footers = DocumentHeadersFooters.ExtractFooters(wordDoc),

                document = documentContents,
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

    public static Dictionary<string, object> GetDocumentLayout(WordprocessingDocument doc)
    {
        var layout = new Dictionary<string, object>();
        var mainDocumentPart = doc.MainDocumentPart;

        if (mainDocumentPart == null || mainDocumentPart.Document.Body == null)
        {
            // Console.WriteLine("Error: Document body is null.");
            return layout;
        }

        // Get all section properties in the document
        var allSectionProps = mainDocumentPart
            .Document.Body.Descendants<SectionProperties>()
            .ToList();

        // Console.WriteLine($"Found {allSectionProps.Count} section(s) in the document");

        if (allSectionProps.Count == 0)
        {
            // Console.WriteLine("No section properties found in document.");
            return layout;
        }

        // Get the first section only
        var sectionProps = allSectionProps.FirstOrDefault();

        // Page size
        var pageSize = sectionProps?.Elements<PageSize>().FirstOrDefault();
        if (pageSize != null)
        {
            bool isLandscape =
                pageSize.Orient != null && pageSize.Orient.Value == PageOrientationValues.Landscape;

            layout["orientation"] = isLandscape ? "Landscape" : "Portrait";
            // Console.WriteLine($"Orientation: {(isLandscape ? "Landscape" : "Portrait")}");

            if (pageSize.Width != null)
            {
                layout["pageWidth"] = ConvertTwipsToCentimeters((int)pageSize.Width.Value);
                // Console.WriteLine(
                //     $"Page Width: {layout["pageWidth"]} cm (Original: {pageSize.Width.Value} twips)"
                // );
            }

            if (pageSize.Height != null)
            {
                layout["pageHeight"] = ConvertTwipsToCentimeters((int)pageSize.Height.Value);
                // Console.WriteLine(
                //     $"Page Height: {layout["pageHeight"]} cm (Original: {pageSize.Height.Value} twips)"
                // );
            }
        }
        else
        {
            Console.WriteLine("No page size found in section properties.");
        }

        // Columns
        var columns = sectionProps?.Elements<Columns>().FirstOrDefault();
        if (columns != null)
        {
            int columnCount = 1;
            double columnSpacing = 1.27;

            if (columns.ColumnCount != null)
            {
                columnCount = columns.ColumnCount.Value;
            }
            // Console.WriteLine($"Column Count: {columnCount}");

            if (columns.Space != null)
            {
                columnSpacing = ConvertTwipsToCentimeters(int.Parse(columns.Space.Value ?? ""));
                // Console.WriteLine(
                //     $"Column Spacing: {columnSpacing} cm (Original: {columns.Space.Value} twips)"
                // );
            }

            layout["columnNum"] = columnCount;
            layout["columnSpacing"] = columnSpacing;
        }
        else
        {
            // Console.WriteLine("No explicit column settings found, using defaults (1 column).");
            layout["columnNum"] = 1;
            layout["columnSpacing"] = 1.27;
        }

        // Page margins
        var pageMargins = sectionProps?.Elements<PageMargin>().FirstOrDefault();
        if (pageMargins != null)
        {
            var margins = new Dictionary<string, double>();
            // Console.WriteLine("Margins found:");

            if (pageMargins.Top != null)
            {
                margins["top"] = ConvertTwipsToCentimeters(pageMargins.Top.Value);
                // Console.WriteLine(
                //     $"   - Top: {margins["top"]} cm (Original: {pageMargins.Top.Value} twips)"
                // );
            }

            if (pageMargins.Bottom != null)
            {
                margins["bottom"] = ConvertTwipsToCentimeters(pageMargins.Bottom.Value);
                // Console.WriteLine(
                //     $"   - Bottom: {margins["bottom"]} cm (Original: {pageMargins.Bottom.Value} twips)"
                // );
            }

            if (pageMargins.Left != null)
            {
                margins["left"] = ConvertTwipsToCentimeters((int)pageMargins.Left.Value);
                // Console.WriteLine(
                //     $"   - Left: {margins["left"]} cm (Original: {pageMargins.Left.Value} twips)"
                // );
            }

            if (pageMargins.Right != null)
            {
                margins["right"] = ConvertTwipsToCentimeters((int)pageMargins.Right.Value);
                // Console.WriteLine(
                //     $"   - Right: {margins["right"]} cm (Original: {pageMargins.Right.Value} twips)"
                // );
            }

            if (pageMargins.Header != null)
            {
                margins["header"] = ConvertTwipsToCentimeters((int)pageMargins.Header.Value);
                // Console.WriteLine(
                //     $"   - Header: {margins["header"]} cm (Original: {pageMargins.Header.Value} twips)"
                // );
            }

            if (pageMargins.Footer != null)
            {
                margins["footer"] = ConvertTwipsToCentimeters((int)pageMargins.Footer.Value);
                // Console.WriteLine(
                //     $"   - Footer: {margins["footer"]} cm (Original: {pageMargins.Footer.Value} twips)"
                // );
            }

            layout["margins"] = margins;
        }
        else
        {
            Console.WriteLine("No page margins found in section properties.");
        }

        return layout;
    }

    // Convert twips (1/1440 of an inch) to centimeters
    private static double ConvertTwipsToCentimeters(int twips)
    {
        // 1 inch = 2.54 cm, and 1 inch = 1440 twips
        return Math.Round((double)twips / 1440 * 2.54, 2);
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

		bool haveBibliography = false;

        foreach (var element in body.Elements<OpenXmlElement>())
        {
            // ✅ Check for a Drawing element inside the run (Extract Images)
            var drawing = element
                .Descendants<DocumentFormat.OpenXml.Wordprocessing.Drawing>()
                .FirstOrDefault();
            if (drawing != null)
            {
                var imageObjects = ExtractContent.ExtractImagesFromDrawing(doc, drawing);
                elements.AddRange(imageObjects);
            }
            else if (element is DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph)
            {
                // ✅ Extract Paragraphs
                elements.Add(ExtractContent.ExtractParagraph(paragraph, doc, ref haveBibliography));
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
