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
using ICT2106WebApp.mod1grp4;
using Microsoft.Extensions.Options;
using MongoDB.Bson; // Bson - Binary JSON
// MongoDB packages
using MongoDB.Driver;
using Newtonsoft.Json; // For JsonConvert
using Newtonsoft.Json.Linq; // Bson - Binary JSON
using Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddLogging(); // Add logging for testing MongoDB
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
	var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
	var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
	return mongoClient;
});
builder.Services.AddSingleton(serviceProvider =>
{
	var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
	var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
	return mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
});

var serviceProvider = builder.Services.BuildServiceProvider();
var database = serviceProvider.GetRequiredService<IMongoDatabase>();

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

DocumentProcessor.RunMyProgram(database);

// GRP3 JOHNATHAN CRASH RECOVERY TESTING
// await ExtractContent.RunCrashRecovery(database);


app.Run();

// ✅ Extracts content from Word document
public static class DocumentProcessor
{
	// Running whole program
	public async static void RunMyProgram(IMongoDatabase database)
	{
		var documentControl = new DocumentControl();
		string filePath = "Datarepository_zx_v4.docx"; // Change this to your actual file path
		string jsonOutputPath = "output.json"; // File where JSON will be saved

		string currentDir = Directory.GetCurrentDirectory();
		string filePath_full = Path.Combine(currentDir, filePath);
		await documentControl.saveDocumentToDatabase(filePath);

		using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
		{
			// Get layout information
			var layoutInfo = ExtractContent.GetDocumentLayout(wordDoc);

			// Extract document contents
			var documentContents = ExtractContent.ExtractDocumentContents(wordDoc);

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

			// important!!
			var documentData = new
			{
				// metadata = DocumentMetadataExtractor.GetMetadata(wordDoc),
				metadata = ExtractContent.GetDocumentMetadata(wordDoc, filePath_full),
				headers = DocumentHeadersFooters.ExtractHeaders(wordDoc),
				// !!footer still exists issues. Commented for now
				footers = DocumentHeadersFooters.ExtractFooters(wordDoc),
				// documentContents is what i need
				document = documentContents,
			};

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

			// this part creates the json
			foreach (var item in documentContents) //Go through doc content
			{
				// Going through each item's key-value pair of the object
				if (item is Dictionary<string, object> dictionary)
				{
					Console.WriteLine("Dictionary contents:");
					string nodeType = "";
					string content = "";
					List<Dictionary<string, object>> styling =
						new List<Dictionary<string, object>>();

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

							if (kvp.Value is List<object> objectList)
							{
								List<Dictionary<string, object>> stylingList =
									new List<Dictionary<string, object>>();

								foreach (var itemhere in objectList)
								{
									if (itemhere is Dictionary<string, object> stylingDictionary)
									{
										stylingList.Add(
											ExtractContent.ConvertJsonElements(stylingDictionary)
										);
									}
								}

								// Assign the processed styling list to the styling variable
								styling = stylingList;
							}
							else
							{
								Console.WriteLine("The 'styling' value is not a List<object>.");
							}
						}
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
								Dictionary<string, object> runStyling =
									new Dictionary<string, object>();

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
										if (runKvp.Value is List<object> objectList)
										{
											List<Dictionary<string, object>> stylingList =
												new List<Dictionary<string, object>>();

											foreach (var itemhere in objectList)
											{
												if (
													itemhere
													is Dictionary<string, object> stylingDictionary
												)
												{
													stylingList.Add(
														ExtractContent.ConvertJsonElements(
															stylingDictionary
														)
													);
												}
											}

											// Assign the processed styling list to the runStyling variable
											runStyling = stylingList.FirstOrDefault(); // Assuming only one styling dictionary per run
										}
										else
										{
											Console.WriteLine(
												"The 'styling' value is not a List<object>."
											);
										}
									}

									if (runKvp.Key == "runs")
									{ //If Table Go To Cell Level
										var runRunsList =
											(List<Dictionary<string, object>>)runKvp.Value;
										// Loop through each text_run in runs
										foreach (var runRun in runRunsList)
										{
											string runRunType = "";
											string runRunContent = "";
											Dictionary<string, object> runRunStyling =
												new Dictionary<string, object>();

											Console.WriteLine("JSONBUILDINGrunrun");
											Console.WriteLine(runRun);
											// Process the 'type' and 'content' for each run
											foreach (var runRunKvp in runRun)
											{
												if (runRunKvp.Key == "type")
												{
													runRunType = (string)runRunKvp.Value;
													Console.WriteLine(
														$"runType: {runRunKvp.Value}"
													);
													Console.WriteLine($"runType: {runRunType}");
												}
												if (runRunKvp.Key == "content")
												{
													runRunContent = (string)runRunKvp.Value;
													Console.WriteLine(
														$"runContent: {runRunKvp.Value}"
													);
													Console.WriteLine(
														$"runContent: {runRunContent}"
													);
												}
												if (runRunKvp.Key == "styling") //This is where we get Cell Style
												{
													if (
														runRunKvp.Value
														is Dictionary<
															string,
															object
														> stylingDictionary
													)
													{
														List<
															Dictionary<string, object>
														> stylingList =
															new List<Dictionary<string, object>>();
														stylingList.Add(stylingDictionary);
														runRunStyling =
															stylingList.FirstOrDefault();
													}
													else
													{
														Console.WriteLine(
															"The 'styling' value is not a Dictionary<string, object>."
														);
													}
												}
											}

											// Create a node for each run (assuming it's a "text_run")
											if (runRunType != "")
											{
												var runRunNode = nodeManager.CreateNode(
													id++,
													runRunType,
													runRunContent,
													new List<Dictionary<string, object>>
													{
														runRunStyling,
													}
												);
												numberofRunNode = numberofRunNode + 1;
												Console.WriteLine(
													$"run myid:{id} {runRunType}: {runRunContent}\n"
												);
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
									var runNode = nodeManager.CreateNode(
										id++,
										runType,
										runContent,
										new List<Dictionary<string, object>> { runStyling }
									);
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
						nodesList.Add(node);
					}
					else
					{
						Console.WriteLine(
							$"Weird its null\n type: {nodeType}\ncontent: {content}\n"
						);
					}

					foreach (var runnodeitem in runListNodes)
					{
						nodesList.Add(runnodeitem);
					}
					runListNodes.Clear();
				}
				// end of checking dictionary

				// Convert to JSON format with UTF-8 encoding fix (preserves emojis, math, and Chinese)
				jsonOutput = System.Text.Json.JsonSerializer.Serialize(
					documentData,
					new JsonSerializerOptions
					{
						WriteIndented = true,
						Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
					}
				);

				// ✅ Write JSON to file
				File.WriteAllText(jsonOutputPath, jsonOutput);
				await documentControl.saveJsonToDatabase(jsonOutputPath);
				Console.WriteLine($"✅ JSON output saved to {jsonOutputPath}");
				//end
			}

			// This calculates and accounts for the number of parent node and child nodes
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

				var thestylinghere = nodeInList.GetStyling();
				string consolidatedStyling = "";
				foreach (var dict in thestylinghere)
				{
					foreach (var kvp in dict)
					{
						consolidatedStyling += $"{kvp.Key}: {kvp.Value}";
					}
				}
				Console.WriteLine($"styling:{consolidatedStyling}");
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
			// treeProcessor.PrintTree(rootnodehere, 0);

			// SAVE TREE TO MONGODB
			await treeProcessor.SaveTreeToDatabase(rootnodehere);
			// // RETRIEVE TEE FROM MONGODB
			AbstractNode mongoRootNode = await treeProcessor.retrieveTree();
			CompositeNode mongoCompNode = null; // declare outside so it can be used outside of the if statement

			if (mongoRootNode is CompositeNode compnode) // Use pattern matching
			{
				Console.WriteLine("mongoRootNode is a CompositeNode!");
				mongoCompNode = compnode; // Assign to compNode
				Console.WriteLine("Typecasted mongoRootNode from AbstractNode to CompositeNode!");
			}
			else
				Console.WriteLine("mongoRootNode is not a CompositeNode!");
			if (mongoCompNode != null)
				treeProcessor.PrintTree(mongoCompNode, 0);
			// END TREE


			//TREE VALIDAITON

			// Flatten the tree
			List<AbstractNode> flattenedTree = treeProcessor.FlattenTree(mongoCompNode);

			// Call validation (pass the document array instead of the entire jsonObject)
			bool isContentValid = treeProcessor.ValidateContent(flattenedTree, documentArray);

			// Output validation result
			if (isContentValid)
				Console.WriteLine("Content is valid!");
			else
				Console.WriteLine("Content mismatch detected!");

			bool isValidStructure = treeProcessor.ValidateNodeStructure(rootnodehere, -1); // Root starts at level 0

			// Output validation result
			if (isValidStructure)
				Console.WriteLine("Tree structure is valid!");
			else
				Console.WriteLine("Invalid tree structure detected.");

			// 			// DO NOT REMOVE FOR TESTING PURPOSES
			// 			// INodeTraverser traverser = new NodeTraverser(rootnodehere);
			// 			// List<AbstractNode> traverseList = traverser.TraverseNode("image");

			//=========================FOR PRINTING ALL TRAVERSE NODES (NOT PART OF FEATURES)============================//
			// NodeTraverser traverser = new NodeTraverser(rootnodehere);
			// List<AbstractNode> traverseList = traverser.TraverseAllNodeTypes();
			// Console.WriteLine("Traversal complete. Check traverseNodes.cs for results.");

			// 			// GROUP 4 STUFF
			// Step 1: Get abstract nodes of table from group 3
			INodeTraverser traverser = new NodeTraverser(rootnodehere);
			List<AbstractNode> tableAbstractNodes = traverser.TraverseNode("tables");

			// Step 2: Convert abstract node to custom table entity
			var tableOrganiser = new TableOrganiserManager();
			List<ICT2106WebApp.mod1grp4.Table> tablesFromNode = tableOrganiser.organiseTables(
				tableAbstractNodes
			);

			// Step 3: Preprocess tables (setup observer, recover backup tables if exist, fix table integrity)
			var rowTabularGateway_RDG = new RowTabularGateway_RDG(database);
			var tablePreprocessingManager = new TablePreprocessingManager();
			tablePreprocessingManager.attach(rowTabularGateway_RDG);
			var tables = await tablePreprocessingManager.recoverBackupTablesIfExist(tablesFromNode);
			List<ICT2106WebApp.mod1grp4.Table> cleanedTables =
				await tablePreprocessingManager.fixTableIntegrity(tables);

			// Step 4: Convert tables to LaTeX
			var latexConversionManager = new TableLatexConversionManager();
			latexConversionManager.attach(rowTabularGateway_RDG);

			// NORMAL FLOW (this will prove for Andrea where she inserts the content to overleaf and jonathan for styling of table)
			List<ICT2106WebApp.mod1grp4.Table> processedTables =
				await latexConversionManager.convertToLatexAsync(cleanedTables);

			// JOEL CRASH RECOVERY FLOW (we will convert 2 tables then stop the program, this will prove for Joel run crash flow first then normal again)
			// List<ICT2106WebApp.mod1grp4.Table> processedTables = await latexConversionManager.convertToLatexWithLimitAsync(cleanedTables, 2);
			// Environment.Exit(0);

			// HIEW TENG VALIDATION CHECK FLOW (we will omit out some stuff in the latex conversion, will prove for hiew teng where validation is wrong)

			// Step 5: Post-processing (validation of latex, logging of validation status, convert processed tables to nodes to send over)
			var tableValidationManager = new TableValidationManager();
			var validationStatus = tableValidationManager.validateTableLatexOutput(
				tableAbstractNodes,
				processedTables
			);

			var processedTableManager = new ProcessedTableManager();
			processedTableManager.attach(rowTabularGateway_RDG);
			processedTableManager.logProcessingStatus(validationStatus);
			await processedTableManager.slotProcessedTableToTree(cleanedTables, tableAbstractNodes);

			// Will prove for Siti as we traverse the nodes again after updating
			List<AbstractNode> endingTableAbstractNodes = traverser.TraverseNode("tables");

			foreach (var tableNode in tableAbstractNodes)
			{
				if (tableNode.GetNodeType() == "table")
				{
					Console.WriteLine($"Table Node Content: {tableNode.GetContent()}");
				}
			}

			// Print tablesFromNode
			foreach (var table in tablesFromNode)
			{
				Console.WriteLine($"{table.tableId}");
				Console.WriteLine($"{table.latexOutput}");
				foreach (var row in table.rows)
				{
					foreach (var cell in row.cells)
					{
						Console.WriteLine($"Cell content: {cell.content}");
						Console.WriteLine($"Cell Styling: {cell.styling}");
					}
				}
			}
		}
	}

	/* // take json -> build list of nodes -> build Tree */
	// public static async Task toSaveTree(string filePath, string jsonOutputPath)
	// {
	// 	using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
	// 	{
	// 		// Get layout information
	// 		var layoutInfo = ExtractContent.GetDocumentLayout(wordDoc);

	// 		// Extract document contents
	// 		var documentContents = ExtractContent.ExtractDocumentContents(wordDoc);

	// 		// Create layout element
	// 		var layoutElement = new Dictionary<string, object>
	// 		{
	// 			{ "type", "layout" },
	// 			{ "content", "" },
	// 			{
	// 				"styling",
	// 				new List<object> { layoutInfo }
	// 			},
	// 		};

	// 		// Insert layout as the first element in document contents
	// 		documentContents.Insert(0, layoutElement);

	// 		// // to create rootnode
	// 		var layoutElementRoot = new Dictionary<string, object>
	// 		{
	// 			{ "id", 0 },
	// 			{ "type", "root" },
	// 			{ "content", "" },
	// 		};
	// 		documentContents.Insert(0, layoutElementRoot);
	// 		string currentDir = Directory.GetCurrentDirectory();
	// 		string filePath_full = Path.Combine(currentDir, filePath);
	// 		var documentData = new
	// 		{
	// 			// metadata = DocumentMetadataExtractor.GetMetadata(wordDoc),
	// 			metadata = ExtractContent.GetDocumentMetadata(wordDoc, filePath_full),
	// 			// headers = DocumentHeadersFooters.ExtractHeaders(wordDoc),
	// 			// !!footer still exists issues. Commented for now
	// 			// footers = DocumentHeadersFooters.ExtractFooters(wordDoc),

	// 			// documentContents is what i need
	// 			document = documentContents,
	// 		};

	// 		Console.WriteLine("\ndocumentContents");
	// 		Console.WriteLine(documentContents);

	// 		NodeManager nodeManager = new NodeManager(); // Create instance of NodeMa
	// 		TreeProcessor treeProcessor = new TreeProcessor(); // Create instance of NodeMa

	// 		int id = 1;
	// 		List<AbstractNode> nodesList = new List<AbstractNode>();
	// 		string jsonOutput = string.Empty;

	// 		var numberofRunNode = 0;
	// 		var numberofMainNode = 0;

	// 		// Going through each object in the document list
	// 		List<AbstractNode> runListNodes = new List<AbstractNode>();
	// 		List<AbstractNode> runRunListNodes = new List<AbstractNode>();

	// 		foreach (var item in documentContents) //Go through doc content
	// 		{
	// 			// Going through each item's key-value pair of the object
	// 			if (item is Dictionary<string, object> dictionary)
	// 			{
	// 				Console.WriteLine("Dictionary contents:");
	// 				string nodeType = "";
	// 				string content = "";
	// 				List<Dictionary<string, object>> styling =
	// 					new List<Dictionary<string, object>>();

	// 				// Loop through the dictionary and print the key-value pairs
	// 				foreach (var kvp in dictionary)
	// 				{
	// 					if (kvp.Key == "type")
	// 					{
	// 						nodeType = (string)kvp.Value;
	// 						Console.WriteLine($"type: {nodeType}");
	// 					}
	// 					if (kvp.Key == "content")
	// 					{
	// 						content = (string)kvp.Value;
	// 						Console.WriteLine($"content {content}");
	// 					}
	// 					if (kvp.Key == "styling")
	// 					{
	// 						Console.WriteLine("Styling L1:");

	// 						if (kvp.Value is List<object> objectList)
	// 						{
	// 							List<Dictionary<string, object>> stylingList =
	// 								new List<Dictionary<string, object>>();

	// 							foreach (var itemhere in objectList)
	// 							{
	// 								if (itemhere is Dictionary<string, object> stylingDictionary)
	// 								{
	// 									stylingList.Add(
	// 										ExtractContent.ConvertJsonElements(stylingDictionary)
	// 									);
	// 								}
	// 							}

	// 							// Assign the processed styling list to the styling variable
	// 							styling = stylingList;
	// 						}
	// 						else
	// 						{
	// 							Console.WriteLine("The 'styling' value is not a List<object>.");
	// 						}
	// 					}

	// 					// run nodes below and i need to create it
	// 					// check for run
	// 					// Check for 'runs' key
	// 					if (kvp.Key == "runs")
	// 					{
	// 						var runsList = (List<Dictionary<string, object>>)kvp.Value;
	// 						// Loop through each text_run in runs
	// 						foreach (var run in runsList)
	// 						{
	// 							string runType = "";
	// 							string runContent = "";
	// 							Dictionary<string, object> runStyling =
	// 								new Dictionary<string, object>();

	// 							Console.WriteLine("JSONBUILDINGrun");
	// 							Console.WriteLine(run);
	// 							// Process the 'type' and 'content' for each run
	// 							foreach (var runKvp in run)
	// 							{
	// 								if (runKvp.Key == "type")
	// 								{
	// 									runType = (string)runKvp.Value;
	// 									Console.WriteLine($"runType: {runKvp.Value}");
	// 									Console.WriteLine($"runType: {runType}");
	// 								}
	// 								if (runKvp.Key == "content")
	// 								{
	// 									runContent = (string)runKvp.Value;
	// 									Console.WriteLine($"runContent: {runKvp.Value}");
	// 									Console.WriteLine($"runContent: {runContent}");
	// 								}
	// 								if (runKvp.Key == "styling")
	// 								{
	// 									if (runKvp.Value is List<object> objectList)
	// 									{
	// 										List<Dictionary<string, object>> stylingList =
	// 											new List<Dictionary<string, object>>();

	// 										foreach (var itemhere in objectList)
	// 										{
	// 											if (
	// 												itemhere
	// 												is Dictionary<string, object> stylingDictionary
	// 											)
	// 											{
	// 												stylingList.Add(
	// 													ExtractContent.ConvertJsonElements(
	// 														stylingDictionary
	// 													)
	// 												);
	// 											}
	// 										}

	// 										// Assign the processed styling list to the runStyling variable
	// 										runStyling = stylingList.FirstOrDefault(); // Assuming only one styling dictionary per run
	// 									}
	// 									else
	// 									{
	// 										Console.WriteLine(
	// 											"The 'styling' value is not a List<object>."
	// 										);
	// 									}
	// 								}

	// 								if (runKvp.Key == "runs")
	// 								{ //If Table Go To Cell Level
	// 									var runRunsList =
	// 										(List<Dictionary<string, object>>)runKvp.Value;
	// 									// Loop through each text_run in runs
	// 									foreach (var runRun in runRunsList)
	// 									{
	// 										string runRunType = "";
	// 										string runRunContent = "";
	// 										Dictionary<string, object> runRunStyling =
	// 											new Dictionary<string, object>();

	// 										Console.WriteLine("JSONBUILDINGrunrun");
	// 										Console.WriteLine(runRun);
	// 										// Process the 'type' and 'content' for each run
	// 										foreach (var runRunKvp in runRun)
	// 										{
	// 											if (runRunKvp.Key == "type")
	// 											{
	// 												runRunType = (string)runRunKvp.Value;
	// 												Console.WriteLine(
	// 													$"runType: {runRunKvp.Value}"
	// 												);
	// 												Console.WriteLine($"runType: {runRunType}");
	// 											}
	// 											if (runRunKvp.Key == "content")
	// 											{
	// 												runRunContent = (string)runRunKvp.Value;
	// 												Console.WriteLine(
	// 													$"runContent: {runRunKvp.Value}"
	// 												);
	// 												Console.WriteLine(
	// 													$"runContent: {runRunContent}"
	// 												);
	// 											}
	// 											if (runRunKvp.Key == "styling") //This is where we get Cell Style
	// 											{
	// 												if (
	// 													runRunKvp.Value
	// 													is Dictionary<
	// 														string,
	// 														object
	// 													> stylingDictionary
	// 												)
	// 												{
	// 													List<
	// 														Dictionary<string, object>
	// 													> stylingList =
	// 														new List<Dictionary<string, object>>();
	// 													stylingList.Add(stylingDictionary);
	// 													runRunStyling =
	// 														stylingList.FirstOrDefault();
	// 												}
	// 												else
	// 												{
	// 													Console.WriteLine(
	// 														"The 'styling' value is not a Dictionary<string, object>."
	// 													);
	// 												}
	// 											}
	// 										}

	// 										// Create a node for each run (assuming it's a "text_run")
	// 										if (runRunType != "")
	// 										{
	// 											var runRunNode = nodeManager.CreateNode(
	// 												id++,
	// 												runRunType,
	// 												runRunContent,
	// 												new List<Dictionary<string, object>>
	// 												{
	// 													runRunStyling,
	// 												}
	// 											);
	// 											numberofRunNode = numberofRunNode + 1;
	// 											Console.WriteLine(
	// 												$"run myid:{id} {runRunType}: {runRunContent}\n"
	// 											);
	// 											// nodesList.Add(runNode);
	// 											runRunListNodes.Add(runRunNode);
	// 										}
	// 										// nodesList.Add(runNode);
	// 									}
	// 								}
	// 							}

	// 							// Create a node for each run (assuming it's a "text_run")
	// 							if (runType != "")
	// 							{
	// 								var runNode = nodeManager.CreateNode(
	// 									id++,
	// 									runType,
	// 									runContent,
	// 									new List<Dictionary<string, object>> { runStyling }
	// 								);
	// 								numberofRunNode = numberofRunNode + 1;
	// 								Console.WriteLine($"run myid:{id} {runType}: {runContent}\n");
	// 								// nodesList.Add(runNode);
	// 								runListNodes.Add(runNode);
	// 								foreach (var runrunnodeitem in runRunListNodes)
	// 								{
	// 									runListNodes.Add(runrunnodeitem);
	// 								}
	// 								runRunListNodes.Clear();
	// 							}
	// 							// nodesList.Add(runNode);
	// 						}
	// 						// end of run nodes
	// 					}

	// 					// end of an object / Parent node
	// 				}

	// 				if (nodeType != "" || content != "")
	// 				{
	// 					var node = nodeManager.CreateNode(id++, nodeType, content, styling);
	// 					nodesList.Add(node);
	// 				}
	// 				else
	// 				{
	// 					Console.WriteLine(
	// 						$"Weird its null\n type: {nodeType}\ncontent: {content}\n"
	// 					);
	// 				}

	// 				foreach (var runnodeitem in runListNodes)
	// 				{
	// 					nodesList.Add(runnodeitem);
	// 				}
	// 				runListNodes.Clear();
	// 			}
	// 			// end of checking dictionary
	// 		}

	// 		Console.WriteLine($"number of Main node: {numberofMainNode}\n");
	// 		Console.WriteLine($"number of Run node: {numberofRunNode}\n");

	// 		/* Checking my json & nodes*/

	// 		//Check nodelist and count the number of nodes
	// 		Console.WriteLine("\n\n\n\n\n\n\n\n\n nodesList.Count");
	// 		Console.WriteLine(nodesList.Count);
	// 		int nodeNum = 0;
	// 		foreach (var nodeInList in nodesList)
	// 		{
	// 			nodeNum = nodeNum + 1;
	// 			Console.Write("nodeNum:");
	// 			Console.Write(nodeNum);
	// 			Console.Write("\n");

	// 			var thetypehere = nodeInList.GetNodeType();
	// 			Console.WriteLine($"type:{thetypehere}");

	// 			var thelevelhere = nodeInList.GetNodeLevel();
	// 			Console.WriteLine($"level:{thelevelhere}");

	// 			var thecontenthere = nodeInList.GetContent();
	// 			Console.WriteLine($"content:{thecontenthere}");

	// 			var thestylinghere = nodeInList.GetStyling();
	// 			string consolidatedStyling = "";
	// 			foreach (var dict in thestylinghere)
	// 			{
	// 				foreach (var kvp in dict)
	// 				{
	// 					consolidatedStyling += $"{kvp.Key}: {kvp.Value}";
	// 				}
	// 			}
	// 			Console.WriteLine($"styling:{consolidatedStyling}");
	// 			Console.Write("\n");
	// 		}
	// 		jsonOutput = File.ReadAllText(jsonOutputPath);
	// 		// Parse the JSON string
	// 		JObject jsonObject = JObject.Parse(jsonOutput);
	// 		// Count the number of items in the "document" array
	// 		JArray documentArray = (JArray)jsonObject["document"];
	// 		int documentCount = documentArray.Count;
	// 		Console.WriteLine($"\n\n Number of items in the JSON document array: {documentCount}");

	// 		// Check if there are "runs" in any of the document items
	// 		var totalCounts = 0;
	// 		var i = 0;
	// 		foreach (var itemhere in documentArray)
	// 		{
	// 			var runs = itemhere["runs"];

	// 			Console.WriteLine(i);

	// 			if (runs != null)
	// 			{
	// 				totalCounts = totalCounts + runs.Count();
	// 				// Console.WriteLine($"This item has {runs.Count()} runs.");
	// 			}
	// 			else
	// 			{
	// 				// Console.WriteLine("This item has no runs.");
	// 			}

	// 			i = i + 1;
	// 		}

	// 		Console.WriteLine("total run count");
	// 		Console.WriteLine(totalCounts);
	// 		Console.WriteLine("\n");

	// 		totalCounts = totalCounts + i;
	// 		Console.WriteLine("total = ");
	// 		Console.WriteLine(totalCounts);

	// 		// CREATE AND PRINT TREE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	// 		CompositeNode rootnodehere = treeProcessor.CreateTree(nodesList);
	// 		await treeProcessor.SaveTreeToDatabase(rootnodehere);
	// 	}
	// }




	// public static Dictionary<string, object> GetDocumentLayout(WordprocessingDocument doc)
	// {
	// 	var layout = new Dictionary<string, object>();
	// 	var mainDocumentPart = doc.MainDocumentPart;

	// 	if (mainDocumentPart == null || mainDocumentPart.Document.Body == null)
	// 		return layout;

	// 	// Get all section properties in the document
	// 	var allSectionProps = mainDocumentPart
	// 		.Document.Body.Descendants<SectionProperties>()
	// 		.ToList();


	// 	if (allSectionProps.Count == 0)
	// 		return layout;

	// 	// Get the first section only
	// 	var sectionProps = allSectionProps.FirstOrDefault();

	// 	// Page size
	// 	var pageSize = sectionProps?.Elements<PageSize>().FirstOrDefault();
	// 	if (pageSize != null)
	// 	{
	// 		bool isLandscape =
	// 			pageSize.Orient != null && pageSize.Orient.Value == PageOrientationValues.Landscape;

	// 		layout["orientation"] = isLandscape ? "Landscape" : "Portrait";
	// 		// Console.WriteLine($"Orientation: {(isLandscape ? "Landscape" : "Portrait")}");

	// 		if (pageSize.Width != null)
	// 		{
	// 			layout["pageWidth"] = ConvertTwipsToCentimeters((int)pageSize.Width.Value);
	// 			// Console.WriteLine(
	// 			//     $"Page Width: {layout["pageWidth"]} cm (Original: {pageSize.Width.Value} twips)"
	// 			// );
	// 		}

	// 		if (pageSize.Height != null)
	// 		{
	// 			layout["pageHeight"] = ConvertTwipsToCentimeters((int)pageSize.Height.Value);
	// 			// Console.WriteLine(
	// 			//     $"Page Height: {layout["pageHeight"]} cm (Original: {pageSize.Height.Value} twips)"
	// 			// );
	// 		}
	// 	}
	// 	else
	// 	{
	// 		Console.WriteLine("No page size found in section properties.");
	// 	}

	// 	// Columns
	// 	var columns = sectionProps?.Elements<Columns>().FirstOrDefault();
	// 	if (columns != null)
	// 	{
	// 		int columnCount = 1;
	// 		double columnSpacing = 1.27;

	// 		if (columns.ColumnCount != null)
	// 		{
	// 			columnCount = columns.ColumnCount.Value;
	// 		}
	// 		// Console.WriteLine($"Column Count: {columnCount}");

	// 		if (columns.Space != null)
	// 		{
	// 			columnSpacing = ConvertTwipsToCentimeters(int.Parse(columns.Space.Value ?? ""));
	// 			// Console.WriteLine(
	// 			//     $"Column Spacing: {columnSpacing} cm (Original: {columns.Space.Value} twips)"
	// 			// );
	// 		}

	// 		layout["columnNum"] = columnCount;
	// 		layout["columnSpacing"] = columnSpacing;
	// 	}
	// 	else
	// 	{
	// 		// Console.WriteLine("No explicit column settings found, using defaults (1 column).");
	// 		layout["columnNum"] = 1;
	// 		layout["columnSpacing"] = 1.27;
	// 	}

	// 	// Page margins
	// 	var pageMargins = sectionProps?.Elements<PageMargin>().FirstOrDefault();
	// 	if (pageMargins != null)
	// 	{
	// 		var margins = new Dictionary<string, double>();
	// 		// Console.WriteLine("Margins found:");

	// 		if (pageMargins.Top != null)
	// 		{
	// 			margins["top"] = ConvertTwipsToCentimeters(pageMargins.Top.Value);
	// 			// Console.WriteLine(
	// 			//     $"   - Top: {margins["top"]} cm (Original: {pageMargins.Top.Value} twips)"
	// 			// );
	// 		}

	// 		if (pageMargins.Bottom != null)
	// 		{
	// 			margins["bottom"] = ConvertTwipsToCentimeters(pageMargins.Bottom.Value);
	// 			// Console.WriteLine(
	// 			//     $"   - Bottom: {margins["bottom"]} cm (Original: {pageMargins.Bottom.Value} twips)"
	// 			// );
	// 		}

	// 		if (pageMargins.Left != null)
	// 		{
	// 			margins["left"] = ConvertTwipsToCentimeters((int)pageMargins.Left.Value);
	// 			// Console.WriteLine(
	// 			//     $"   - Left: {margins["left"]} cm (Original: {pageMargins.Left.Value} twips)"
	// 			// );
	// 		}

	// 		if (pageMargins.Right != null)
	// 		{
	// 			margins["right"] = ConvertTwipsToCentimeters((int)pageMargins.Right.Value);
	// 			// Console.WriteLine(
	// 			//     $"   - Right: {margins["right"]} cm (Original: {pageMargins.Right.Value} twips)"
	// 			// );
	// 		}

	// 		if (pageMargins.Header != null)
	// 		{
	// 			margins["header"] = ConvertTwipsToCentimeters((int)pageMargins.Header.Value);
	// 			// Console.WriteLine(
	// 			//     $"   - Header: {margins["header"]} cm (Original: {pageMargins.Header.Value} twips)"
	// 			// );
	// 		}

	// 		if (pageMargins.Footer != null)
	// 		{
	// 			margins["footer"] = ConvertTwipsToCentimeters((int)pageMargins.Footer.Value);
	// 			// Console.WriteLine(
	// 			//     $"   - Footer: {margins["footer"]} cm (Original: {pageMargins.Footer.Value} twips)"
	// 			// );
	// 		}

	// 		layout["margins"] = margins;
	// 	}
	// 	else
	// 	{
	// 		Console.WriteLine("No page margins found in section properties.");
	// 	}

	// 	return layout;
	// }

	// Convert twips (1/1440 of an inch) to centimeters
	// private static double ConvertTwipsToCentimeters(int twips)
	// {
	// 	// 1 inch = 2.54 cm, and 1 inch = 1440 twips
	// 	return Math.Round((double)twips / 1440 * 2.54, 2);
	// }

	// public static List<object> ExtractDocumentContents(WordprocessingDocument doc)
	// {
	// 	var elements = new List<object>();

	// 	var body = doc.MainDocumentPart?.Document?.Body;

	// 	if (body == null)
	// 	{
	// 		Console.WriteLine("❌ Error: Document body is null.");
	// 		return elements;
	// 	}

	// 	bool haveBibliography = false;

	// 	foreach (var element in body.Elements<OpenXmlElement>())
	// 	{
	// 		// ✅ Check for a Drawing element inside the run (Extract Images)
	// 		var drawing = element
	// 			.Descendants<DocumentFormat.OpenXml.Wordprocessing.Drawing>()
	// 			.FirstOrDefault();
	// 		if (drawing != null)
	// 		{
	// 			var imageObjects = ExtractContent.ExtractImagesFromDrawing(doc, drawing);
	// 			elements.AddRange(imageObjects);
	// 		}
	// 		else if (element is DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph)
	// 		{
	// 			// ✅ Extract Paragraphs
	// 			elements.Add(ExtractContent.ExtractParagraph(paragraph, doc, ref haveBibliography));
	// 		}
	// 		else if (element is DocumentFormat.OpenXml.Wordprocessing.Table table)
	// 		{
	// 			Console.WriteLine("📝 Extracting Table");
	// 			elements.Add(ExtractContent.ExtractTable(table)); // ✅ Extract Tables
	// 		}
	// 	}
	// 	return elements;
	// }

	// Because my code broke , will shift this to some other class
	// private static Dictionary<string, object> ConvertJsonElements(Dictionary<string, object> input)
	// {
	// 	var result = new Dictionary<string, object>();
	// 	foreach (var kvp in input)
	// 	{
	// 		if (kvp.Value is JsonElement jsonElement)
	// 		{
	// 			switch (jsonElement.ValueKind)
	// 			{
	// 				case JsonValueKind.String:
	// 					result[kvp.Key] = jsonElement.GetString();
	// 					break;
	// 				case JsonValueKind.Number:
	// 					result[kvp.Key] = jsonElement.GetDouble(); // or GetInt32() depending
	// 					break;
	// 				case JsonValueKind.True:
	// 				case JsonValueKind.False:
	// 					result[kvp.Key] = jsonElement.GetBoolean();
	// 					break;
	// 				case JsonValueKind.Object:
	// 				case JsonValueKind.Array:
	// 					result[kvp.Key] = jsonElement.ToString(); // fallback as string
	// 					break;
	// 				default:
	// 					result[kvp.Key] = null;
	// 					break;
	// 			}
	// 		}
	// 		else
	// 		{
	// 			result[kvp.Key] = kvp.Value;
	// 		}
	// 	}
	// 	return result;
	// }

	// public static async Task ToSaveJson(
	// 	DocumentControl documentControl,
	// 	string filePath,
	// 	string jsonOutputPath
	// )
	// // {
	// // 	using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
	// // 	{
	// // 		// Get layout information
	// // 		var layoutInfo = GetDocumentLayout(wordDoc);

	// // 		// Extract document contents
	// // 		var documentContents = ExtractDocumentContents(wordDoc);

	// // 		// Create layout element
	// // 		var layoutElement = new Dictionary<string, object>
	// // 		{
	// // 			{ "type", "layout" },
	// // 			{ "content", "" },
	// // 			{
	// // 				"styling",
	// // 				new List<object> { layoutInfo }
	// // 			},
	// // 		};

	// // 		// Insert layout as the first element in document contents
	// // 		documentContents.Insert(0, layoutElement);

	// // 		// Create root node
	// // 		var layoutElementRoot = new Dictionary<string, object>
	// // 		{
	// // 			{ "id", 0 },
	// // 			{ "type", "root" },
	// // 			{ "content", "" },
	// // 		};
	// // 		documentContents.Insert(0, layoutElementRoot);

	// // 		var documentData = new
	// // 		{
	// // 			metadata = GetDocumentMetadata(wordDoc, filePath), // Fixed `filePath_full`
	// // 			document = documentContents,
	// // 		};

	// // 		// Convert to JSON format with UTF-8 encoding fix
	// // 		string jsonOutput = System.Text.Json.JsonSerializer.Serialize(
	// // 			documentData,
	// // 			new JsonSerializerOptions
	// // 			{
	// // 				WriteIndented = true,
	// // 				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
	// // 			}
	// // 		);

	// // 		// Write JSON to file
	// // 		File.WriteAllText(jsonOutputPath, jsonOutput);
	// // 		Console.WriteLine($"✅ JSON output saved to {jsonOutputPath}");

	// // 		// Save JSON to database (assuming `saveJsonToDatabase` is an async method)
	// // 		await documentControl.saveJsonToDatabase(jsonOutputPath);
	// // 	}
	// // }
}
