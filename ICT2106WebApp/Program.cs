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

// Jon part's for error testing
Console.CancelKeyPress += async (sender, eventArgs) =>
{
	eventArgs.Cancel = true;
	Console.WriteLine("SIGINT received. Running crash recovery...");

	await ExtractContent.RunCrashRecovery(database);
};

var runCrashRecovery = false;

// try
// {
// 	Console.WriteLine("Running main program logic...");
// 	DocumentProcessor.RunMyProgram(database);
// }
// catch (Exception ex)
// {
// 	Console.WriteLine("Crash detected, running recovery.");
// 	await ExtractContent.RunCrashRecovery(database);
// }

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

await app.StartAsync();

// Listen for SIGINT continuously
while (true)
{
	if (runCrashRecovery)
	{
		runCrashRecovery = false;
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("SIGINT received. Running crash recovery...\n");
		Console.ResetColor();

		await ExtractContent.RunCrashRecovery(database);

		Console.WriteLine("✅ Crash recovery done. Server still running.\n");
	}

	await Task.Delay(100); // Prevent busy looping
}

// app.Run();

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
		// **
		// await documentControl.saveDocumentToDatabase(filePath);

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

			TreeProcessor treeProcessor = new TreeProcessor(); // Create instance of NodeMa

			List<AbstractNode> nodesList = new List<AbstractNode>();
			string jsonOutput = string.Empty;

			jsonOutput = await ExtractContent.CreateNodeList(
				documentContents,
				nodesList,
				jsonOutput,
				documentData,
				jsonOutputPath
			);

			JObject jsonObject = JObject.Parse(jsonOutput);
			JArray documentArray = (JArray)jsonObject["document"];
			// checkjson
			// uncomment to see consolelogs for checking purposes
			ExtractContent.checkJson(documentArray);

			// !!Break here for another function ?
			// CREATE AND PRINT TREE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			CompositeNode rootnodehere = treeProcessor.CreateTree(nodesList);

			var defaultColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine("\n\n############################## \nPrint Tree\n\n");
			treeProcessor.PrintTree(rootnodehere, 0);
			Console.ForegroundColor = defaultColor;

			// SAVE TREE TO MONGODB
			await treeProcessor.SaveTreeToDatabase(rootnodehere, "mergewithcommentedcode");

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
			{
				// Console.WriteLine("Print out tree. Commented out for now\n");

				Console.ForegroundColor = ConsoleColor.DarkYellow;
				treeProcessor.PrintTree(mongoCompNode, 0);
				Console.ForegroundColor = defaultColor;
			}
			// END TREE


			//TREE VALIDAITON
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine("\n\n############################## \nTree Validation\n\n");
			// Flatten the tree
			List<AbstractNode> flattenedTree = treeProcessor.FlattenTree(mongoCompNode);

			// Call validation (pass the document array instead of the entire jsonObject)
			bool isContentValid = treeProcessor.ValidateContent(flattenedTree, documentArray);

			// Output validation result
			if (isContentValid)
			{
				Console.WriteLine("Content is valid!");
			}
			else
			{
				Console.WriteLine("Content mismatch detected!");
			}

			bool isValidStructure = treeProcessor.ValidateNodeStructure(rootnodehere, -1); // Root starts at level 0

			// Output validation result
			if (isValidStructure)
				Console.WriteLine("Tree structure is valid!");
			else
				Console.WriteLine("Invalid tree structure detected.");

			//=========================FOR PRINTING ALL TRAVERSE NODES (NOT PART OF FEATURES)============================//

			// NodeTraverser traverser = new NodeTraverser(rootnodehere);
			// List<AbstractNode> traverseList = traverser.TraverseAllNodeTypes();
			// Console.WriteLine("Traversal complete. Check traverseNodes.cs for results.");

			//=========================FOR PRINTING ALL TRAVERSE NODES (NOT PART OF FEATURES)============================//

			// GROUP 4 STUFF
			// Step 1: Get abstract nodes of table from group 3
			INodeTraverser traverser = new NodeTraverser(rootnodehere);

			List<AbstractNode> tableAbstractNodes = traverser.TraverseNode("tables");

			// Step 2: Convert abstract node to custom table entity (JOEL)
			var tableOrganiser = new TableOrganiserManager();
			List<ICT2106WebApp.mod1grp4.Table> tablesFromNode = tableOrganiser.organiseTables(
				tableAbstractNodes
			);

			// Step 3: Preprocess tables (setup observer, recover backup tables if exist, fix table integrity) (JOEL)
			var rowTabularGateway_RDG = new RowTabularGateway_RDG(database);
			var tablePreprocessingManager = new TablePreprocessingManager();
			tablePreprocessingManager.attach(rowTabularGateway_RDG);
			var tables = await tablePreprocessingManager.recoverBackupTablesIfExist(tablesFromNode);
			List<ICT2106WebApp.mod1grp4.Table> cleanedTables =
				await tablePreprocessingManager.fixTableIntegrity(tables);

			// Step 4: Convert tables to LaTeX (ANDREA)
			var latexConversionManager = new TableLatexConversionManager();
			latexConversionManager.attach(rowTabularGateway_RDG);

			// NORMAL FLOW (this will prove for Andrea where she inserts the content to overleaf and jonathan for styling of table)
			List<ICT2106WebApp.mod1grp4.Table> processedTables =
				await latexConversionManager.convertToLatexAsync(cleanedTables);

			// JOEL CRASH RECOVERY FLOW (we will convert 2 tables then stop the program, this will prove for Joel run crash flow first then normal again)
			// List<ICT2106WebApp.mod1grp4.Table> processedTables = await latexConversionManager.convertToLatexWithLimitAsync(cleanedTables, 2);
			// Environment.Exit(0);

			// HIEW TENG VALIDATION CHECK FLOW (we will omit out some stuff in the latex conversion, will prove for hiew teng where validation is wrong)
			// List<ICT2106WebApp.mod1grp4.Table> processedTables = await latexConversionManager.convertToLatexStyleFailAsync(cleanedTables);

			// Step 5: Post-processing (validation of latex, logging of validation status, convert processed tables to nodes to send over) (HIEW TENG AND SITI)
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
			// List<AbstractNode> endingTableAbstractNodes = traverser.TraverseNode("tables");

			// Save modified latex tree back to MongoDB (query)
			await traverser.UpdateLatexDocument(rootnodehere);

			ICompletedLatex completedLatex = new CompletedLatex();

			// // Retrieve the non-modified tree from MongoDB (for demo query)
			// AbstractNode originalRootNode = await completedLatex.RetrieveTree();
			// CompositeNode originalMongo = null; // declare outside so it can be used outside of the if statement

			// if (originalRootNode is CompositeNode originalNode) // Use pattern matching
			// {
			// 	Console.WriteLine("Latex Tree retrieved!");
			// 	originalMongo = originalNode; // Assign to compNode
			// }
			// else
			// {
			// 	Console.WriteLine("Latex Tree not retrieved!");
			// }
			// if (originalMongo != null)
			// {
			// 	treeProcessor.PrintTree(originalMongo,0);
			// }


			//Retrieve the Latex tree from MongoDB (for demo query)
			AbstractNode latexRootNode = await completedLatex.RetrieveLatexTree();
			CompositeNode latexMongo = null; // declare outside so it can be used outside of the if statement

			if (latexRootNode is CompositeNode latexNode) // Use pattern matching
			{
				//Console.WriteLine("Latex Tree retrieved!");
				latexMongo = latexNode; // Assign to compNode
			}
			else
			{
				//Console.WriteLine("Latex Tree not retrieved!");
			}
			if (latexMongo != null)
			{
				treeProcessor.PrintTree(latexMongo, 0);
			}

			// foreach (var tableNode in tableAbstractNodes)
			// {
			// 	if (tableNode.GetNodeType() == "table")
			// 	{
			// 		Console.WriteLine($"Table Node Content: {tableNode.GetContent()}");
			// 	}
			// }

			// // Print tablesFromNode
			// foreach (var table in tablesFromNode)
			// {
			// 	Console.WriteLine($"{table.tableId}");
			// 	Console.WriteLine($"{table.latexOutput}");
			// 	foreach (var row in table.rows)
			// 	{
			// 		foreach (var cell in row.cells)
			// 		{
			// 			Console.WriteLine($"Cell content: {cell.content}");
			// 			Console.WriteLine($"Cell Styling: {cell.styling}");
			// 		}
			// 	}
			// }
		}
	}
}
