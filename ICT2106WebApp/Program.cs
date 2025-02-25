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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

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
        string filePath = "Datarepository.docx"; // Change this to your actual file path
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