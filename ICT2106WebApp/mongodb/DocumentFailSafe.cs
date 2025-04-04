using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using ICT2106WebApp.mod1Grp3;
using Newtonsoft.Json.Linq;
using Utilities;

public class DocumentFailSafe : ICrashRecoveryRetrieveNotify
{
	// private readonly Lazy<IDocumentRetrieve> _docxRetrieve;
	private readonly ICrashRecoveryRetrieve _docxRetrieve;

	// // private readonly DocxRDG _docxRDG;
	// public DocumentFailSafe(IServiceProvider serviceProvider)
	// {
	// 	_docxRetrieve = new Lazy<IDocumentRetrieve>(
	// 		() => serviceProvider.GetRequiredService<IDocumentRetrieve>()
	// 	);
	// }
	public DocumentFailSafe()
	{
		_docxRetrieve = (ICrashRecoveryRetrieve)new DocumentGateway_RDG();
		_docxRetrieve.docxRetrieve = this;
	}

	// Retrieve Saved Document
	public async Task retrieveSavedDocument(string id, string outputPath)
	{
		var docx = await _docxRetrieve.getDocument(id);
		if (docx == null)
		{
			throw new FileNotFoundException($"Document with ID {id} not found in database.");
		}

		// Write the bytes to a file
		// await File.WriteAllBytesAsync(outputPath, docx.FileData);
		await File.WriteAllBytesAsync(outputPath, (byte[])docx.GetDocxAttributeValue("fileData"));
		Console.WriteLine($"DocumentFailSafe -> Document retrieved and saved to: {outputPath}");
	}

	// IDocumentRetrieveNotify
	public async Task notifyRetrievedDocument(Docx docx)
	{
		// Console.WriteLine($"DocumentFailSafe -> Notify Document retrieved: {docx.Title}");
		Console.WriteLine($"DocumentFailSafe -> Notify Document retrieved: {docx.GetDocxAttributeValue("title")}");

		await Task.CompletedTask;
	}

	// IDocumentRetrieveNotify
	public async Task notifyRetrievedJson()
	{
		Console.WriteLine("DocumentFailSafe -> notifyRetrievedJson Received json file from database.");
		await Task.CompletedTask;
	}

	// IDocumentRetrieve
	public async Task retrieveSavedJson()
	{
		await _docxRetrieve.getJsonFile();
	}
	public async Task runCrashRecovery(bool hasRetried = false)
	{
		string filePath = "";
		string outputPath = "";
		string jsonOutputPath = "output.json";

		Console.WriteLine("Starting Document Processing with Crash Recovery");

		// DocumentControl documentControl = new DocumentControl();
		DocumentProcessors documentProcessor = new DocumentProcessors();
		DocumentGateway_RDG documentGateway = new DocumentGateway_RDG();
		DocumentFailSafe documentFailSafe = new DocumentFailSafe();
		TreeProcessor treeProcessor = new TreeProcessor();
		NodeManager nodeManager = new NodeManager();

		// Step 1: Check if doc exists in DB
		Console.WriteLine("DocumentFailSafe -> Querying database to check if word document exists.");
		var allDocuments = await documentGateway.GetAllAsync();

		Console.WriteLine($"DocumentFailSafe -> Retrieved {allDocuments.Count} documents");

		if (allDocuments.Any())
		{
			var latestDocument = allDocuments.Last();
			Console.WriteLine($"Latest document ID: {(string)latestDocument.GetDocxAttributeValue("docxId")}");
			Console.WriteLine($"Latest document Title: {(string)latestDocument.GetDocxAttributeValue("title")}");

			var retrievedDocument = await documentGateway.getDocument((string)latestDocument.GetDocxAttributeValue("docxId"));
			if (retrievedDocument != null)
			{
				await notifyRetrievedDocument(retrievedDocument);
				Console.WriteLine($"Retrieved Document Title: {(string)retrievedDocument.GetDocxAttributeValue("title")}");
				outputPath = $"{(string)retrievedDocument.GetDocxAttributeValue("title")}1.docx";
			}

			await documentFailSafe.retrieveSavedDocument((string)latestDocument.GetDocxAttributeValue("docxId"), outputPath);
			filePath = outputPath;
		}
		else
		{
			if (hasRetried)
			{
				Console.WriteLine("Error: No documents found even after retry.");
				return;
			}

			Console.WriteLine("No documents found. Saving default document into the database...");
			filePath = "Datarepository_zx_v4.docx"; // your fallback document
			await documentProcessor.saveDocumentToDatabase(filePath);

			// Retry the process after saving
			await runCrashRecovery(true);
			return;
		}

		// ----- Main document processing starts here -----

		string currentDir = Directory.GetCurrentDirectory();
		string filePath_full = Path.Combine(currentDir, filePath);

		if (!File.Exists(filePath_full))
		{
			Console.WriteLine($"Error: File '{filePath_full}' not found.");
			return;
		}

		Console.WriteLine("Word document found, continuing with processing...");

		// Step 2: Check if JSON exists
		if (!File.Exists(jsonOutputPath))
		{
			// await DocumentProcessors.ToSaveJson(documentProcessor, filePath, jsonOutputPath);
						var documentProcessors = new DocumentProcessors();

			List<Object> documentContents = documentProcessors
				.ParseDocument(filePath)
				.Result;
		}

		// Step 3: Check for tree in DB
		var rootNode = await treeProcessor.retrieveTree();

		if (rootNode == null)
		{
			Console.WriteLine("Tree not found. Generating new tree...");
			//ceate a list of nodes
			using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath_full, false))
			{
				var documentContents = DocumentProcessors.ExtractDocumentContents(wordDoc);
				var rootElement = DocumentProcessors.elementRoot();
				documentContents.Insert(0, DocumentProcessors.ExtractLayout(wordDoc));
				documentContents.Insert(0, rootElement);
			
			List<AbstractNode> nodesList = NodeManager.CreateNodeList(documentContents);

			CompositeNode rootnodehere = treeProcessor.CreateTree(nodesList);

			var defaultColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine("\n\n############################## \nPrint Tree Contents\n\n");
			treeProcessor.PrintTreeContents(rootnodehere);
			Console.WriteLine("\n\n############################## \nPrint Tree Hierarchy\n\n");
			treeProcessor.PrintTreeHierarchy(rootnodehere, 0);
			Console.ForegroundColor = defaultColor;

			// SAVE TREE TO MONGODB
			await treeProcessor.SaveTreeToDatabase(rootnodehere, "mergewithcommentedcode");

			// Retry retrieving the tree after generating it
			rootNode = await treeProcessor.retrieveTree();

			if (rootNode == null)
			{
				Console.WriteLine("❌ Failed to retrieve tree even after saving.");
				return;
			}
			}
		}

		// 🟢 From here, rootNode is guaranteed to be not null

		Console.WriteLine("Tree structure loaded successfully.");
		CompositeNode mongoCompNode = (CompositeNode)rootNode;

		if (mongoCompNode != null)
		{
			List<AbstractNode> flattenedTree = treeProcessor.FlattenTree(mongoCompNode);

			string jsonOutput = File.ReadAllText("output.json");
			JObject jsonObject = JObject.Parse(jsonOutput);
			JArray documentArray = (JArray)jsonObject["document"];

			Console.WriteLine($"\n\n Number of items in JSON: {documentArray.Count}");

			bool isContentValid = nodeManager.ValidateContentRecursive(flattenedTree, documentArray, 0);
			Console.WriteLine(isContentValid ? "Content is valid!" : "Content mismatch detected!");

			bool isValidStructure = treeProcessor.ValidateTreeStructure(mongoCompNode, -1);
			Console.WriteLine(isValidStructure ? "Tree structure is valid!" : "Invalid tree structure detected.");
		}

		INodeTraverser traverser = new NodeTraverser(mongoCompNode);
		List<AbstractNode> tableAbstractNodes = traverser.TraverseNode("tables");

		// Group 4 table processing logic goes here...

		await traverser.UpdateLatexDocument(mongoCompNode);

		Console.WriteLine("✅ Completed Error Recovery & Document Processing");
	}


	// 	public async Task runCrashRecovery()
	// 	{
	// Console.WriteLine("doing test with the tree");
	// TreeProcessor treeProcessor = new TreeProcessor();
	// var node = await treeProcessor.retrieveTree();
	// CompositeNode compositeNodes = (CompositeNode) node;
	// treeProcessor.PrintTreeContents(compositeNodes);
	// 	}
	// public async Task runCrashRecovery()
	// {
	// 	string filePath = "";
	// 	string outputPath = "";
	// 	string jsonOutputPath = "output.json"; //

	// 	Console.WriteLine("Starting Document Processing with Crash Recovery");

	// 	DocumentControl documentControl = new DocumentControl();
	// 	DocumentGateway_RDG documentGateway = new DocumentGateway_RDG();
	// 	DocumentFailSafe documentFailSafe = new DocumentFailSafe();
	// 	TreeProcessor treeProcessor = new TreeProcessor();
	// 	NodeManager nodeManager = new NodeManager();
	// 	// 1. Start by checking if the word document is stored in the database
	// 	Console.WriteLine("DocumentFailSafe -> Querying database to check if word document exists.");
	// 	var allDocuments = await documentGateway.GetAllAsync();

	// 	Console.WriteLine($"DocumentFailSafe -> Retrieved {allDocuments.Count} documents");

	// 	// If there are documents, get the latest (should be only one)
	// 	if (allDocuments.Any())
	// 	{
	// 		var latestDocument = allDocuments.Last();
	// 		Console.WriteLine($"Latest document ID: {(string)latestDocument.GetDocxAttributeValue("docxId")}");
	// 		Console.WriteLine($"Latest document Title: {(string)latestDocument.GetDocxAttributeValue("title")}");
	// 		// Retrieve back the document
	// 		var retrievedDocument = await documentGateway.getDocument((string)latestDocument.GetDocxAttributeValue("docxId"));

	// 		if (retrievedDocument != null)
	// 		{
	// 			await notifyRetrievedDocument(retrievedDocument);
	// 			// Console.WriteLine($"Retrieved Document Title: {retrievedDocument.Title}");
	// 			Console.WriteLine($"Retrieved Document Title: {(string)retrievedDocument.GetDocxAttributeValue("title")}");
	// 			// outputPath = $"{retrievedDocument.Title}1.docx";
	// 			outputPath = $"{(string)retrievedDocument.GetDocxAttributeValue("title")}1.docx";

	// 		}
	// 		// Retrieve the saved document
	// 		await documentFailSafe.retrieveSavedDocument((string)latestDocument.GetDocxAttributeValue("docxId"), outputPath);
	// 		// await documentFailSafe.retrieveSavedDocument(latestDocument.Id, outputPath);

	// 		filePath = outputPath; // set the filePath to be the docx file path
	// 	}
	// 	else
	// 	{
	// 		Console.WriteLine("No documents found in the database.");
	// 		// doesnt exist, so i want to save one docx into my db NOW !
	// 		filePath = "Datarepository_zx_v4.docx"; // Update this with your actual file path
	// 		await documentControl.saveDocumentToDatabase(filePath); // will be stored locally. 
	// 	}

	// 	// Here, a docx will either be generated from the database or stored locally 

	// 	string currentDir = Directory.GetCurrentDirectory();
	// 	string filePath_full = Path.Combine(currentDir, filePath);

	// 	if (!File.Exists(filePath_full))
	// 	{
	// 		Console.WriteLine($"Error: File '{filePath_full}' not found.");
	// 		return;
	// 	}
	// 	else
	// 	{
	// 		Console.WriteLine("Error: word document missing");
	// 	}

	// 	// AT THIS POINT , YOU ALREADY HAVE A docx either retrieved from DB or newly read DOCX

	// 	// STEP 2: Check if json exists locally, if not retrieve from database.
	// 	string jsonFilePath = "output.json";

	// 	if (!File.Exists(jsonFilePath))
	// 	{
	// 		await ExtractContent.ToSaveJson(documentControl, filePath, jsonFilePath);
	// 	}

	// 	// Step 3: Check if tree exists
	// 	var rootNode = await treeProcessor.retrieveTree();

	// 	if (rootNode == null) // indicates that there is no existing tree stored in the database.
	// 	{
	// 		Console.WriteLine("Tree retrieval failed or no data available.");
	// 		await ExtractContent.toSaveTree(filePath, jsonFilePath); // Run the method using the docx and json file to generate the tree
	// 	}
	// 	else
	// 	{ // The word document, json file, and tree structure exists in the database.
	// 	  // Can proceed with the rest of the porgram.
	// 	  // Proceed with your logic when the tree is successfully retrieved
	// 		Console.WriteLine(" Tree structure loaded successfully.");

	// 		// Typecast the AbstractNode rootNode that was retrieved from the database as a CompositeNode
	// 		CompositeNode mongoCompNode = (CompositeNode)rootNode;

	// 		// Tree Validation Portion
	// 		if (mongoCompNode != null)
	// 		{
	// 			List<AbstractNode> flattenedTree = treeProcessor.FlattenTree(mongoCompNode);

	// 			string jsonOutput = File.ReadAllText("output.json");
	// 			// Parse the JSON string
	// 			JObject jsonObject = JObject.Parse(jsonOutput);
	// 			JArray documentArray = (JArray)jsonObject["document"];
	// 			int documentCount = documentArray.Count;
	// 			Console.WriteLine(
	// 				$"\n\n Number of items in the JSON document array: {documentCount}"
	// 			);
	// 			// Count the number of items in the "document" array
	// 			// Call validation (pass the document array instead of the entire jsonObject)
	// 			bool isContentValid = nodeManager.ValidateContentRecursive(
	// 				flattenedTree,
	// 				documentArray,0
	// 			);

	// 			// Output validation result
	// 			if (isContentValid)
	// 				Console.WriteLine("Content is valid!");
	// 			else
	// 				Console.WriteLine("Content mismatch detected!");

	// 			bool isValidStructure = treeProcessor.ValidateTreeStructure(mongoCompNode, -1); // Root starts at level 0

	// 			// Output validation result
	// 			if (isValidStructure)
	// 				Console.WriteLine("Tree structure is valid!");
	// 			else
	// 				Console.WriteLine("Invalid tree structure detected.");
	// 		}

	// 		// Node Traverser / Querying of nodes to other modules 

	// 		// Step 1: Get abstract nodes of table from group 3
	// 		INodeTraverser traverser = new NodeTraverser(mongoCompNode);
	// 		List<AbstractNode> tableAbstractNodes = traverser.TraverseNode("tables");

	// 		// Start of Module 1 Group 4 
	// 		// Group 4 Table Extraction and Updating of Nodes method here!
	// 		// End of Module 1 Group 4

	// 		// Save modified latex tree back to MongoDB (query)
	// 		await traverser.UpdateLatexDocument(mongoCompNode);
	// 	}

	// 	Console.WriteLine("Completed Error Recovery");
	// }

	public static async Task toSaveTree(string filePath, string jsonOutputPath)
	{
		using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
		{
			// Get layout information
			var layoutInfo = DocumentProcessors.GetDocumentLayout(wordDoc);

			// Extract document contents
			var documentContents = DocumentProcessors.ExtractDocumentContents(wordDoc);

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
				metadata = DocumentProcessors.GetDocumentMetadata(wordDoc, filePath_full),
				// headers = DocumentHeadersFooters.ExtractHeaders(wordDoc),
				// !!footer still exists issues. Commented for now
				// footers = DocumentHeadersFooters.ExtractFooters(wordDoc),
				// documentContents is what i need
				document = documentContents,
			};

			List<AbstractNode> nodesList = new List<AbstractNode>();
			string jsonOutput = string.Empty;

			// jsonOutput = await NodeManager.CreateNodeList(
			// 	documentContents,
			// 	nodesList,
			// 	jsonOutput,
			// 	documentData,
			// 	jsonOutputPath
			// );

			// JObject jsonObject = JObject.Parse(jsonOutput);
			// JArray documentArray = (JArray)jsonObject["document"];
			// checkjson
			// uncomment to see consolelogs for checking purposes
			// ExtractContent.checkJson(documentArray);

			TreeProcessor treeProcessor = new TreeProcessor(); // Create instance of NodeMa

			// !!Break here for another function ?
			// CREATE AND PRINT TREE HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			CompositeNode rootnodehere = treeProcessor.CreateTree(nodesList);

			var defaultColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine("\n\n############################## \nPrint Tree Contents\n\n");
			treeProcessor.PrintTreeContents(rootnodehere);
			Console.WriteLine("\n\n############################## \nPrint Tree Hierarchy\n\n");
			treeProcessor.PrintTreeHierarchy(rootnodehere, 0);
			Console.ForegroundColor = defaultColor;

			// SAVE TREE TO MONGODB
			await treeProcessor.SaveTreeToDatabase(rootnodehere, "mergewithcommentedcode");
		}
	}
}


