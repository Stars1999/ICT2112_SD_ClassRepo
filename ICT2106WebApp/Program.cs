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
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq; // Bson - Binary JSON

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddLogging(); // Add logging for testing MongoDB

// // Start of MongoDB setup
// builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB")); // inside appsettings.json

// // Register MongoDB client as a singleton
// builder.Services.AddSingleton<IMongoClient>(sp =>
// {
// 	var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
// 	return new MongoClient(settings.ConnectionString);
// });

// // Register IMongoDatabase as a singleton, using the DatabaseName from MongoDbSettings
// builder.Services.AddSingleton<IMongoDatabase>(sp =>
// {
// 	var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
// 	var client = sp.GetRequiredService<IMongoClient>();
// 	return client.GetDatabase(settings.DatabaseName);
// });

// // END OF MONGODB SETUP

// // Register services for IDocumentRetrieveNotify and IDocumentUpdateNotify
// builder.Services.AddScoped<IDocumentRetrieveNotify, DocumentFailSafe>(); // DocumentFailSafe Implements IDocumentRetrieveNotify
// builder.Services.AddScoped<IDocumentUpdateNotify, DocumentParsing>(); // DocumentParsing implemenst IDocumentUpdateNotify

// // Register DocxRDG for both IDocumentRetrieve and IDocumentUpdate
// builder.Services.AddScoped<IDocumentRetrieve, DocxRDG>();
// builder.Services.AddScoped<IDocumentUpdate, DocxRDG>();

var app = builder.Build();
Console.WriteLine("✅ App built successfully");

// // Get logger instance
var logger = app.Services.GetRequiredService<ILogger<Program>>();


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

// MongoDB + DocumentControl , DocumentFailSafe, DocumentGateway_RDG testing
// Check if test document flag is provided
if (args.Length > 0 && args[0] == "--test-document")
{
	// Get the paths from arguments or use defaults
	string localDocxPath = args.Length > 1 ? args[1] : "Datarepository_zx_v2.docx";
	string outputPath = args.Length > 2 ? args[2] : "retrieved-document11.docx";

	Console.WriteLine($"Testing document functionality with {localDocxPath}");

	try
	{
		// Create instances of required classes
		var documentControl = new DocumentControl();
		var documentGateway = new DocumentGateway_RDG();
		var documentFailSafe = new DocumentFailSafe();
		// Step 1: Save document to database
		Console.WriteLine("Attempting to save document to database...");
		await documentControl.saveDocumentToDatabase(localDocxPath);
		Console.WriteLine("Document saved successfully!");

		// Step 2: Retrieve all documents
		Console.WriteLine("Retrieving all documents...");
		var allDocuments = await documentGateway.GetAllAsync();
		Console.WriteLine($"Retrieved {allDocuments.Count} documents");

		// Step 3: Get the last (most recently added) document
		if (allDocuments.Any())
		{
			var latestDocument = allDocuments.Last();
			Console.WriteLine($"Latest document ID: {latestDocument.Id}");
			Console.WriteLine($"Latest document Title: {latestDocument.Title}");

			// Optional: Demonstrate retrieval of a specific document
			var retrievedDocument = await documentGateway.getDocument(latestDocument.Id);
			if (retrievedDocument != null)
			{
				Console.WriteLine("Document retrieved successfully!");
				Console.WriteLine($"Retrieved Document Title: {retrievedDocument.Title}");
			}
			await documentFailSafe.retrieveSavedDocument(latestDocument.Id, outputPath);
		}
		else
		{
			Console.WriteLine("No documents found in the database.");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error during document testing: {ex.Message}");
		Console.WriteLine($"Stack trace: {ex.StackTrace}");
	}
} // END of MongoDB + DocumentControl , DocumentFailSafe, DocumentGateway_RDG testing

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

			// // to create rootnode
			var layoutElementRoot = new Dictionary<string, object>
			{
				{ "id", 0 },
				{ "type", "root" },
				{ "content", "" },
			};
			documentContents.Insert(0, layoutElementRoot);

			var documentData = new
			{
				// metadata = DocumentMetadataExtractor.GetMetadata(wordDoc),
				metadata = GetDocumentMetadata(wordDoc, filePath_full),
				// headers = DocumentHeadersFooters.ExtractHeaders(wordDoc),
				// !!footer still exists issues. Commented for now
				// footers = DocumentHeadersFooters.ExtractFooters(wordDoc),

				// documentContents is what i need
				document = documentContents,
			};

			Console.WriteLine("\ndocumentContents");
			Console.WriteLine(documentContents);

			NodeManager nodeManager = new NodeManager(); // Create instance of NodeMa
			TreeProcessor treeProcessor = new TreeProcessor(); // Create instance of NodeMa

			int id = 1;
			List<AbstractNode> nodesList = new List<AbstractNode>();
			string jsonOutput = string.Empty;


			var numberofRunNode = 0;
			var numberofMainNode = 0;

			// Going through each object in the document list
			List<AbstractNode> runListNodes = new List<AbstractNode>();
			List<AbstractNode> runRunListNodes = new List<AbstractNode>();

			foreach (var item in documentContents)
			{
				// Going through each item's key-value pair of the object
				if (item is Dictionary<string, object> dictionary)
				{
					Console.WriteLine("Dictionary contents:");
					string nodeType = "";
					string content = "";
					List<Dictionary<string, object>> styling = new List<Dictionary<string, object>>();

					// Loop through the dictionary and print the key-value pairs
					foreach (var kvp in dictionary)
					{

						if (kvp.Key == "type")
						{
							nodeType = (string)kvp.Value;
							Console.WriteLine($"type: {nodeType}");
						}
						if (kvp.Key == "content")
						{
							content = (string)kvp.Value;
							Console.WriteLine($"content {content}");
						}
						if (kvp.Key == "styling")
						{
							Console.WriteLine("Styling L1:");

							// Check if kvp.Value is a List<object>
							if (kvp.Value is List<object> objectList)
							{
								List<Dictionary<string, object>> stylingList = new List<Dictionary<string, object>>();

								// Now iterate through the List<object> and check each item
								foreach (var itemhere in objectList)
								{
									// Check if each item is a Dictionary<string, object>
									if (itemhere is Dictionary<string, object> stylingDictionary)
									{
										// Add the dictionary to the stylingList
										stylingList.Add(stylingDictionary);

										// Process the dictionary (just printing the contents for now)
										Console.WriteLine("Found a dictionary in styling:");
										foreach (var styleKvp in stylingDictionary)
										{
											string styleKey = styleKvp.Key;
											object styleValue = styleKvp.Value;
											Console.WriteLine($"Key: {styleKey}, Value: {styleValue}");
										}
									}
									else
									{
										Console.WriteLine("Item in styling list is not a dictionary.");
									}
								}
								Console.WriteLine("Finished processing styling list.");
							}
							else
							{
								Console.WriteLine("The 'styling' value is not a List<object>.");
							}
						}

						// run nodes below and i need to create it
						// check for run
						// Check for 'runs' key
						if (kvp.Key == "runs")
						{
							var runsList = (List<Dictionary<string, object>>)kvp.Value;
							// Loop through each text_run in runs
							foreach (var run in runsList)
							{
								string runType = "";
								string runContent = "";
								Dictionary<string, object> runStyling = new Dictionary<string, object>();

								Console.WriteLine("JSONBUILDINGrun");
								Console.WriteLine(run);
								// Process the 'type' and 'content' for each run
								foreach (var runKvp in run)
								{
									if (runKvp.Key == "type")
									{
										runType = (string)runKvp.Value;
										Console.WriteLine($"runType: {runKvp.Value}");
										Console.WriteLine($"runType: {runType}");
									}
									if (runKvp.Key == "content")
									{
										runContent = (string)runKvp.Value;
										Console.WriteLine($"runContent: {runKvp.Value}");
										Console.WriteLine($"runContent: {runContent}");
									}
									if (runKvp.Key == "styling")
									{
										if (kvp.Value is List<object> objectList)
										{
											List<Dictionary<string, object>> stylingList = new List<Dictionary<string, object>>();

											// Now iterate through the List<object> and check each item
											foreach (var itemhere in objectList)
											{
												// Check if each item is a Dictionary<string, object>
												if (itemhere is Dictionary<string, object> stylingDictionary)
												{
													// Add the dictionary to the stylingList
													stylingList.Add(stylingDictionary);

													// Process the dictionary (just printing the contents for now)
													Console.WriteLine("Found a dictionary in styling:");
													foreach (var styleKvp in stylingDictionary)
													{
														string styleKey = styleKvp.Key;
														object styleValue = styleKvp.Value;
														Console.WriteLine($"Key: {styleKey}, Value: {styleValue}");
													}
												}
												else
												{
													Console.WriteLine("Item in styling list is not a dictionary.");
												}
											}
											Console.WriteLine("Finished processing styling list.");
										}
										else
										{
											Console.WriteLine("The 'styling' value is not a List<object>.");
										}
									}
									
									if(runKvp.Key == "runs") {
										var runRunsList = (List<Dictionary<string, object>>)runKvp.Value;
										// Loop through each text_run in runs
										foreach (var runRun in runRunsList)
										{
											string runRunType = "";
											string runRunContent = "";
											Dictionary<string, object> runRunStyling = new Dictionary<string, object>();

											Console.WriteLine("JSONBUILDINGrunrun");
											Console.WriteLine(runRun);
											// Process the 'type' and 'content' for each run
											foreach (var runRunKvp in runRun)
											{
												if (runRunKvp.Key == "type")
												{
													runRunType = (string)runRunKvp.Value;
													Console.WriteLine($"runType: {runRunKvp.Value}");
													Console.WriteLine($"runType: {runRunType}");
												}
												if (runRunKvp.Key == "content")
												{
													runRunContent = (string)runRunKvp.Value;
													Console.WriteLine($"runContent: {runRunKvp.Value}");
													Console.WriteLine($"runContent: {runRunContent}");
												}
												if (runRunKvp.Key == "styling")
												{
													if (kvp.Value is List<object> objectList)
													{
														List<Dictionary<string, object>> stylingList = new List<Dictionary<string, object>>();

														// Now iterate through the List<object> and check each item
														foreach (var itemhere in objectList)
														{
															// Check if each item is a Dictionary<string, object>
															if (itemhere is Dictionary<string, object> stylingDictionary)
															{
																// Add the dictionary to the stylingList
																stylingList.Add(stylingDictionary);

																// Process the dictionary (just printing the contents for now)
																Console.WriteLine("Found a dictionary in styling:");
																foreach (var styleKvp in stylingDictionary)
																{
																	string styleKey = styleKvp.Key;
																	object styleValue = styleKvp.Value;
																	Console.WriteLine($"Key: {styleKey}, Value: {styleValue}");
																}
															}
															else
															{
																Console.WriteLine("Item in styling list is not a dictionary.");
															}
														}
														Console.WriteLine("Finished processing styling list.");
													}
													else
													{
														Console.WriteLine("The 'styling' value is not a List<object>.");
													}
												}
											}

											// Create a node for each run (assuming it's a "text_run")
											if (runRunType != "")
											{
												var runRunNode = nodeManager.CreateNode(id++, runRunType, runRunContent, new List<Dictionary<string, object>> { runRunStyling });
												numberofRunNode = numberofRunNode + 1;
												Console.WriteLine($"run myid:{id} {runRunType}: {runRunContent}\n");
												// nodesList.Add(runNode);
												runRunListNodes.Add(runRunNode);
											}
											// nodesList.Add(runNode);
										}
									}
								}

								// Create a node for each run (assuming it's a "text_run")
								if (runType != "")
								{
									var runNode = nodeManager.CreateNode(id++, runType, runContent, new List<Dictionary<string, object>> { runStyling });
									numberofRunNode = numberofRunNode + 1;
									Console.WriteLine($"run myid:{id} {runType}: {runContent}\n");
									// nodesList.Add(runNode);
									runListNodes.Add(runNode);
									foreach (var runrunnodeitem in runRunListNodes)
									{
										runListNodes.Add(runrunnodeitem);
									}
									runRunListNodes.Clear();
								}
								// nodesList.Add(runNode);
							}
							// end of run nodes
						}

						// end of an object / Parent node
					}

					if (nodeType != "" || content != "")
					{
						var node = nodeManager.CreateNode(id++, nodeType, content, styling);
						numberofMainNode = numberofMainNode + 1;
						nodesList.Add(node);
						nodeType = "";
						content = "";
						styling = null;
					}
					else
					{
						Console.WriteLine($"Weird its null\n type: {nodeType}\ncontent: {content}\n");
					}


					foreach (var runnodeitem in runListNodes)
					{
						nodesList.Add(runnodeitem);
					}
					runListNodes.Clear();

				}
				// end of checking dictionary

				// Convert to JSON format with UTF-8 encoding fix (preserves emojis, math, and Chinese)
				jsonOutput = JsonSerializer.Serialize(
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
				//end
			}

			Console.WriteLine($"number of Main node: {numberofMainNode}\n");
			Console.WriteLine($"number of Run node: {numberofRunNode}\n");

			/* Checking my json & nodes*/

			//Check nodelist and count the number of nodes
			Console.WriteLine("\n\n\n\n\n\n\n\n\n nodesList.Count");
			Console.WriteLine(nodesList.Count);
			int nodeNum = 0;
			foreach (var nodeInList in nodesList)
			{
				nodeNum = nodeNum + 1;
				Console.Write("nodeNum:");
				Console.Write(nodeNum);
				Console.Write("\n");

				var thetypehere = nodeInList.GetNodeType();
				Console.WriteLine($"type:{thetypehere}");

				var thelevelhere = nodeInList.GetNodeLevel();
				Console.WriteLine($"level:{thelevelhere}");

				var thecontenthere = nodeInList.GetContent();
				Console.WriteLine($"content:{thecontenthere}");
				Console.Write("\n");

			}

			// Parse the JSON string
			JObject jsonObject = JObject.Parse(jsonOutput);
			// Count the number of items in the "document" array
			JArray documentArray = (JArray)jsonObject["document"];
			int documentCount = documentArray.Count;
			Console.WriteLine($"\n\n Number of items in the JSON document array: {documentCount}");

			// Check if there are "runs" in any of the document items
			var totalCounts = 0;
			var i = 0;
			foreach (var itemhere in documentArray)
			{
				var runs = itemhere["runs"];

				Console.WriteLine(i);

				if (runs != null)
				{
					totalCounts = totalCounts + runs.Count();
					// Console.WriteLine($"This item has {runs.Count()} runs.");
				}
				else
				{
					// Console.WriteLine("This item has no runs.");
				}

				i = i + 1;
			}

			Console.WriteLine("total run count");
			Console.WriteLine(totalCounts);
			Console.WriteLine("\n");

			totalCounts = totalCounts + i;
			Console.WriteLine("total = ");
			Console.WriteLine(totalCounts);

			// CREATE AND PRINT TREE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			CompositeNode rootnodehere = treeProcessor.CreateTree(nodesList);
			treeProcessor.PrintTree(rootnodehere, 0);

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
