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

// this file for jonathan
namespace Utilities
{
	//  document fail safe?
	public static partial class ExtractContent
	{
		// Grp3 Johnathan's Crash Recovery
		public static async Task RunCrashRecovery(IMongoDatabase database)
		{
			Console.WriteLine("Starting Document Processing with Crash Recovery");

			DocumentControl documentControl = new DocumentControl();
			DocumentGateway_RDG documentGateway = new DocumentGateway_RDG();
			DocumentFailSafe documentFailSafe = new DocumentFailSafe();
			NodeManager nodeManager = new NodeManager();
			TreeProcessor treeProcessor = new TreeProcessor();

			string filePath = "";
			string outputPath = "";
			string jsonOutputPath = "output.json"; //

			// 1. Start by checking if the docx is stored in the database
			// By right there should only be 1 docx in the database for every conversion process.
			Console.WriteLine("Retrieving all documents...");
			var allDocuments = await documentGateway.GetAllAsync();
			Console.WriteLine($"Retrieved {allDocuments.Count} documents");

			// If there are documents, get the latest (should be only one)
			if (allDocuments.Any())
			{
				var latestDocument = allDocuments.Last();
				Console.WriteLine($"Latest document ID: {latestDocument.Id}");
				Console.WriteLine($"Latest document Title: {latestDocument.Title}");

				// Retrieve back the document
				var retrievedDocument = await documentGateway.getDocument(latestDocument.Id);
				if (retrievedDocument != null)
				{
					Console.WriteLine("Document retrieved successfully!");
					Console.WriteLine($"Retrieved Document Title: {retrievedDocument.Title}");
					outputPath = $"{retrievedDocument.Title}1.docx";
				}
				// Retrieve the saved document
				await documentFailSafe.retrieveSavedDocument(latestDocument.Id, outputPath);
				filePath = outputPath; // set the filePath to be the docx file path
			}
			else
			{
				Console.WriteLine("No documents found in the database.");
				// doesnt exist, so i want to save one docx into my db NOW !
				filePath = "Datarepository_zx_v4.docx"; // Update this with your actual file path
				await documentControl.saveDocumentToDatabase(filePath);
			}

			string currentDir = Directory.GetCurrentDirectory();
			string filePath_full = Path.Combine(currentDir, filePath);

			if (!File.Exists(filePath_full))
			{
				Console.WriteLine($"Error: File '{filePath_full}' not found.");
				return;
			}
			else
			{
				Console.WriteLine("Good Job");
			}

			// AT THIS POINT , YOU ALREADY HAVE A docx either retrieved from DB or newly read DOCX

			// STEP 2: CHECK IF JSON FILE EXISTS
			string jsonFilePath = "output.json";

			if (!File.Exists(jsonFilePath))
			{
				await ExtractContent.ToSaveJson(documentControl, filePath, jsonFilePath);
			}

			// STEP 3: CHECK FOR TREE
			var rootNode = await treeProcessor.retrieveTree();

			if (rootNode == null) // IF retrieveTree does not return me the rootNode,
			{
				// Do something when the tree is not found
				Console.WriteLine("Tree retrieval failed or no data available.");
				await ExtractContent.toSaveTree(filePath, jsonFilePath); // I will run the code to use the docx and json file to generate the tree
			}
			else
			{
				// Proceed with your logic when the tree is successfully retrieved
				Console.WriteLine(" helllloooo Tree loaded successfully.");
				CompositeNode mongoCompNode = null;
				// Further processing here

				if (rootNode is CompositeNode compnode) // Use pattern matching
				{
					Console.WriteLine("mongoRootNode is a CompositeNode!");
					mongoCompNode = compnode; // Assign to compNode
					Console.WriteLine("Typecasted rootNode from AbstractNode to CompositeNode!");
				}
				else
				{
					Console.WriteLine("mongoRootNode is not a CompositeNode!");
				}
				if (rootNode != null)
				{
					// treeProcessor.PrintTree(rootNode,0);
				}

				// TO FIX!!
				// Flatten the tree
				if (mongoCompNode != null)
				{
					List<AbstractNode> flattenedTree = treeProcessor.FlattenTree(mongoCompNode);

					string jsonOutput = File.ReadAllText("output.json");
					// Parse the JSON string
					JObject jsonObject = JObject.Parse(jsonOutput);
					JArray documentArray = (JArray)jsonObject["document"];
					int documentCount = documentArray.Count;
					Console.WriteLine(
						$"\n\n Number of items in the JSON document array: {documentCount}"
					);
					// Count the number of items in the "document" array
					// Call validation (pass the document array instead of the entire jsonObject)
					bool isContentValid = treeProcessor.ValidateContent(
						flattenedTree,
						documentArray
					);
				}
				else
				{
					Console.WriteLine("compnodeis NULL");
				}
				// // Output validation result
				// if (isContentValid)
				// 	Console.WriteLine("Content is valid!");
				// else
				// 	Console.WriteLine("Content mismatch detected!");

				// bool isValidStructure = treeProcessor.ValidateNodeStructure(compNode, -1); // Root starts at level 0

				// // Output validation result
				// if (isValidStructure)
				// 	Console.WriteLine("Tree structure is valid!");
				// else
				// 	Console.WriteLine("Invalid tree structure detected.");

				// DO NOT REMOVE FOR TESTING PURPOSES
				// INodeTraverser traverser = new NodeTraverser(rootnodehere);
				// List<AbstractNode> traverseList = traverser.TraverseNode("image");

				// //=========================FOR PRINTING ALL TRAVERSE NODES (NOT PART OF FEATURES)============================//
				// NodeTraverser traverser = new NodeTraverser(rootnodehere);
				// List<AbstractNode> traverseList = traverser.TraverseAllNodeTypes();
				// WriteToFile("traverseNodes.cs", traverseList);
				// Console.WriteLine("Traversal complete. Check traverseNodes.cs for results.");

				// GROUP 4 STUFF
				// Step 1: Get abstract nodes of table from group 3
				INodeTraverser traverser = new NodeTraverser(mongoCompNode);
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
				var tables = await tablePreprocessingManager.recoverBackupTablesIfExist(
					tablesFromNode
				);
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
				await processedTableManager.slotProcessedTableToTree(
					cleanedTables,
					tableAbstractNodes
				);

				// Will prove for Siti as we traverse the nodes again after updating
				List<AbstractNode> endingTableAbstractNodes = traverser.TraverseNode("tables");
			}
			Console.WriteLine("finish runtest");

			/*
			
		Console.WriteLine("Starting Document Processing with Crash Recovery");

		DocumentControl documentControl = new DocumentControl();
		DocumentGateway_RDG documentGateway = new DocumentGateway_RDG();
		DocumentFailSafe documentFailSafe = new DocumentFailSafe();
		NodeManager nodeManager = new NodeManager();
		TreeProcessor treeProcessor = new TreeProcessor();

		string filePath = "";
		string outputPath = "";
		string jsonOutputPath = "output.json"; //

		// 1. Start by checking if the docx is stored in the database
		// By right there should only be 1 docx in the database for every conversion process.
		Console.WriteLine("Retrieving all documents...");
		var allDocuments = await documentGateway.GetAllAsync();
		Console.WriteLine($"Retrieved {allDocuments.Count} documents");

		// If there are documents, get the latest (should be only one)
		if (allDocuments.Any())
		{
			var latestDocument = allDocuments.Last();
			Console.WriteLine($"Latest document ID: {latestDocument.Id}");
			Console.WriteLine($"Latest document Title: {latestDocument.Title}");

			// Retrieve back the document
			var retrievedDocument = await documentGateway.getDocument(latestDocument.Id);
			if (retrievedDocument != null)
			{
				Console.WriteLine("Document retrieved successfully!");
				Console.WriteLine($"Retrieved Document Title: {retrievedDocument.Title}");
				outputPath = $"{retrievedDocument.Title}1.docx";
			}
			// Retrieve the saved document
			await documentFailSafe.retrieveSavedDocument(latestDocument.Id, outputPath);
			filePath = outputPath; // set the filePath to be the docx file path
		}
		else
		{
			Console.WriteLine("No documents found in the database.");
			// doesnt exist, so i want to save one docx into my db NOW !
			filePath = "Datarepository_zx_v4.docx"; // Update this with your actual file path
			await documentControl.saveDocumentToDatabase(filePath);
		}

		string currentDir = Directory.GetCurrentDirectory();
		string filePath_full = Path.Combine(currentDir, filePath);

		if (!File.Exists(filePath_full))
		{
			Console.WriteLine($"Error: File '{filePath_full}' not found.");
			return;
		}
		else
		{
			Console.WriteLine("Good Job");
		}

		// AT THIS POINT , YOU ALREADY HAVE A docx either retrieved from DB or newly read DOCX

		// STEP 2: CHECK IF JSON FILE EXISTS
		string jsonFilePath = "output.json";

		if (!File.Exists(jsonFilePath))
		{
			await ToSaveJson(documentControl, filePath, jsonFilePath);
			await documentControl.saveJsonToDatabase(jsonOutputPath);
		}

		// STEP 3: CHECK FOR TREE
		var rootNode = await treeProcessor.retrieveTree();

		if (rootNode == null) // IF retrieveTree does not return me the rootNode,
		{
			// Do something when the tree is not found
			Console.WriteLine("Tree retrieval failed or no data available.");
			await toSaveTree(filePath, jsonFilePath); // I will run the code to use the docx and json file to generate the tree
		}
		else
		{
			// Proceed with your logic when the tree is successfully retrieved
			Console.WriteLine("Tree loaded successfully.");
			CompositeNode mongoCompNode = null;
			// Further processing here

			if (rootNode is CompositeNode compnode) // Use pattern matching
			{
				Console.WriteLine("mongoRootNode is a CompositeNode!");
				mongoCompNode = compnode; // Assign to compNode
				Console.WriteLine("Typecasted rootNode from AbstractNode to CompositeNode!");
			}
			else
			{
				Console.WriteLine("mongoRootNode is not a CompositeNode!");
			}
			if (rootNode != null)
			{
				// treeProcessor.PrintTree(rootNode,0);
			}

			// TO FIX!!
			// Flatten the tree
			if (mongoCompNode != null)
			{
				List<AbstractNode> flattenedTree = treeProcessor.FlattenTree(mongoCompNode);

				string jsonOutput = File.ReadAllText("output.json");
				// Parse the JSON string
				JObject jsonObject = JObject.Parse(jsonOutput);
				JArray documentArray = (JArray)jsonObject["document"];
				int documentCount = documentArray.Count;
				Console.WriteLine(
					$"\n\n Number of items in the JSON document array: {documentCount}"
				);
				// Count the number of items in the "document" array
				// Call validation (pass the document array instead of the entire jsonObject)
				bool isContentValid = treeProcessor.ValidateContent(flattenedTree, documentArray);
			}
			else
			{
				Console.WriteLine("compnodeis NULL");
			}
			// // Output validation result
			// if (isContentValid)
			// 	Console.WriteLine("Content is valid!");
			// else
			// 	Console.WriteLine("Content mismatch detected!");

			// bool isValidStructure = treeProcessor.ValidateNodeStructure(compNode, -1); // Root starts at level 0

			// // Output validation result
			// if (isValidStructure)
			// 	Console.WriteLine("Tree structure is valid!");
			// else
			// 	Console.WriteLine("Invalid tree structure detected.");

			// DO NOT REMOVE FOR TESTING PURPOSES
			// INodeTraverser traverser = new NodeTraverser(rootnodehere);
			// List<AbstractNode> traverseList = traverser.TraverseNode("image");
		}
		Console.WriteLine("finish runtest");
		*/
		}

		// END OF CRASH RECOVERY! TO FIX ARADHANA PART...OR ASK HER HEHE

		// take json -> build list of nodes -> build Tree
		public static async Task toSaveTree(string filePath, string jsonOutputPath)
		{
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
				string currentDir = Directory.GetCurrentDirectory();
				string filePath_full = Path.Combine(currentDir, filePath);
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
										if (
											itemhere is Dictionary<string, object> stylingDictionary
										)
										{
											stylingList.Add(
												ExtractContent.ConvertJsonElements(
													stylingDictionary
												)
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
									Dictionary<string, object> runStyling =
										new Dictionary<string, object>();

									// Console.WriteLine("JSONBUILDINGrun");
									// Console.WriteLine(run);
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
														is Dictionary<
															string,
															object
														> stylingDictionary
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

												// Console.WriteLine("JSONBUILDINGrunrun");
												// Console.WriteLine(runRun);
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
																new List<
																	Dictionary<string, object>
																>();
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
										Console.WriteLine(
											$"run myid:{id} {runType}: {runContent}\n"
										);
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
				jsonOutput = File.ReadAllText(jsonOutputPath);
				// Parse the JSON string
				JObject jsonObject = JObject.Parse(jsonOutput);
				// Count the number of items in the "document" array
				JArray documentArray = (JArray)jsonObject["document"];
				int documentCount = documentArray.Count;
				Console.WriteLine(
					$"\n\n Number of items in the JSON document array: {documentCount}"
				);

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
				// CompositeNode rootnodehere = treeProcessor.CreateTree(nodesList);
				// await treeProcessor.SaveTreeToDatabase(rootnodehere);
				CompositeNode rootnodehere = treeProcessor.CreateTree(nodesList);
				await treeProcessor.SaveTreeToDatabase(rootnodehere, "mergewithcommentedcode");
			}
		}
	}
}
