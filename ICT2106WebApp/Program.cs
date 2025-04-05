using DocumentFormat.OpenXml.Packaging;
using ICT2106WebApp.mod1Grp3;
using Microsoft.Extensions.Options;

// MongoDB packages
using MongoDB.Driver;
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

// singleton
builder.Services.AddSingleton(serviceProvider =>
{
	var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
	var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
	return mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
});

var serviceProvider = builder.Services.BuildServiceProvider();
var database = serviceProvider.GetRequiredService<IMongoDatabase>();

var app = builder.Build();

// ahJon part's for error testing
Console.CancelKeyPress += async (sender, eventArgs) =>
{
	eventArgs.Cancel = true;
	Console.WriteLine("SIGINT received. Running crash recovery...");

	// await ExtractContent.RunCrashRecovery(database);
	DocumentFailSafe documentFailSafe = new DocumentFailSafe();
	await documentFailSafe.runCrashRecovery(false);
};
var runCrashRecovery = false;
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

RunMyProgram(database);
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
		DocumentFailSafe documentFailSafe = new DocumentFailSafe();
		await documentFailSafe.runCrashRecovery(false);

		Console.WriteLine("✅ Crash recovery done. Server still running.\n");
	}

	await Task.Delay(100); // Prevent busy looping
}

// Running whole program
async static void RunMyProgram(IMongoDatabase database)
{
	string filePath = "Datarepository_zx_v4 - demo.docx"; // Change this to your actual file path
	string jsonOutputPath = "output.json"; // File where JSON will be saved

	using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
	{
		NodeManager nodeManager = new NodeManager();
		TreeProcessor treeProcessor = new TreeProcessor(); 
		DocumentProcessor documentProcessors;
		CompositeNode rootnodehere = null;
		bool isValid = false;

		while (!isValid)
		{
			documentProcessors = new DocumentProcessor();

			List<Object> documentContents = documentProcessors
				.ParseDocument(filePath)
				.Result;

			//ceate a list of nodes
			List<AbstractNode> nodesList = NodeManager.CreateNodeList(documentContents);
			rootnodehere = treeProcessor.CreateTree(nodesList);

			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine("\n\n############################## \nTree Validation\n\n");

			List<AbstractNode> flattenedTree = treeProcessor.FlattenTree(rootnodehere);
			bool isContentValid = nodeManager.ValidateContentRecursive(
				flattenedTree,
				documentProcessors.documentArray,
				0
			);
			if (!isContentValid)
			{
				Console.WriteLine("Content mismatch detected! Retrying...\n");
				continue;
			}

			bool isValidStructure = treeProcessor.ValidateTreeStructure(rootnodehere, -1);
			if (!isValidStructure)
			{
				Console.WriteLine("Invalid tree structure detected. Retrying...\n");
				continue;
			}

			Console.WriteLine("Tree content and structure are valid!");
			isValid = true; // exit loop
		}

		var defaultColor = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.DarkCyan;
		Console.WriteLine("\n\n############################## \nPrint Tree Contents\n\n");
		treeProcessor.PrintTreeContents(rootnodehere);
		Console.WriteLine("\n\n############################## \nPrint Tree Hierarchy\n\n");
		treeProcessor.PrintTreeHierarchy(rootnodehere, 0);
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
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			treeProcessor.PrintTreeContents(mongoCompNode);
			treeProcessor.PrintTreeHierarchy(mongoCompNode, 0);
			Console.ForegroundColor = defaultColor;
		}
		// END TREE

		INodeTraverser traverser = new NodeTraverser(rootnodehere);
		// Save modified latex tree back to MongoDB (query)
		await traverser.UpdateLatexDocument(rootnodehere);

		ICompletedLatex completedLatex = new CompletedLatex();

		// // Retrieve the non-modified tree from MongoDB (for demo query)
		AbstractNode originalRootNode = await completedLatex.RetrieveUnmodifiedTree();
		CompositeNode originalTree = (CompositeNode)originalRootNode;
		if (originalTree != null)
		{
			treeProcessor.PrintTreeContents(originalTree);
			treeProcessor.PrintTreeHierarchy(originalTree, 0);
		}

		//Retrieve the Latex tree from MongoDB (for demo query)
		AbstractNode latexRootNode = await completedLatex.RetrieveLatexTree();
		CompositeNode latexMongo = (CompositeNode)latexRootNode;
		if (latexMongo != null)
		{
			treeProcessor.PrintTreeContents(latexMongo);
			treeProcessor.PrintTreeHierarchy(latexMongo, 0);
		}

		Console.WriteLine("Program Main Flow COMPLETED");
	}
}
