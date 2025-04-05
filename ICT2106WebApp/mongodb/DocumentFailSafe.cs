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
		Console.WriteLine(
			$"DocumentFailSafe -> Notify Document retrieved: {docx.GetDocxAttributeValue("title")}"
		);

		await Task.CompletedTask;
	}

	// IDocumentRetrieveNotify
	public async Task notifyRetrievedJson()
	{
		Console.WriteLine(
			"DocumentFailSafe -> notifyRetrievedJson Received json file from database."
		);
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

		DocumentProcessor documentProcessor = new DocumentProcessor();
		DocumentGateway_RDG documentGateway = new DocumentGateway_RDG();
		DocumentFailSafe documentFailSafe = new DocumentFailSafe();
		TreeProcessor treeProcessor = new TreeProcessor();
		NodeManager nodeManager = new NodeManager();

		// Step 1: Check if doc exists in DB
		Console.WriteLine(
			"DocumentFailSafe -> Querying database to check if word document exists."
		);
		var allDocuments = await documentGateway.GetAllAsync();

		Console.WriteLine($"DocumentFailSafe -> Retrieved {allDocuments.Count} documents");

		if (allDocuments.Any())
		{
			var latestDocument = allDocuments.Last();
			Console.WriteLine(
				$"Latest document ID: {(string)latestDocument.GetDocxAttributeValue("docxId")}"
			);
			Console.WriteLine(
				$"Latest document Title: {(string)latestDocument.GetDocxAttributeValue("title")}"
			);

			var retrievedDocument = await documentGateway.getDocument(
				(string)latestDocument.GetDocxAttributeValue("docxId")
			);
			if (retrievedDocument != null)
			{
				await notifyRetrievedDocument(retrievedDocument);
				Console.WriteLine(
					$"Retrieved Document Title: {(string)retrievedDocument.GetDocxAttributeValue("title")}"
				);
				outputPath = $"{(string)retrievedDocument.GetDocxAttributeValue("title")}1.docx";
			}

			await documentFailSafe.retrieveSavedDocument(
				(string)latestDocument.GetDocxAttributeValue("docxId"),
				outputPath
			);
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
			var documentProcessors = new DocumentProcessor();
			List<Object> documentContents = documentProcessors.ParseDocument(filePath).Result;
		}

		// Step 3: Check for tree in DB
		var rootNode = await treeProcessor.retrieveTree();

		if (rootNode == null)
		{
			Console.WriteLine("Tree not found. Generating new tree...");
			//ceate a list of nodes
			using (
				WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath_full, false)
			)
			{
				var documentProcessors = new DocumentProcessor();

				List<Object> documentContents = documentProcessors.ParseDocument(filePath).Result;

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

		Console.WriteLine("Tree structure loaded successfully.");
		CompositeNode mongoCompNode = (CompositeNode)rootNode;

		if (mongoCompNode != null)
		{
			List<AbstractNode> flattenedTree = treeProcessor.FlattenTree(mongoCompNode);

			string jsonOutput = File.ReadAllText("output.json");
			JObject jsonObject = JObject.Parse(jsonOutput);
			JArray documentArray = (JArray)jsonObject["document"];

			Console.WriteLine($"\n\n Number of items in JSON: {documentArray.Count}");

			bool isContentValid = nodeManager.ValidateContentRecursive(
				flattenedTree,
				documentArray,
				0
			);
			Console.WriteLine(isContentValid ? "Content is valid!" : "Content mismatch detected!");

			bool isValidStructure = treeProcessor.ValidateTreeStructure(mongoCompNode, -1);
			Console.WriteLine(
				isValidStructure ? "Tree structure is valid!" : "Invalid tree structure detected."
			);
		}

		INodeTraverser traverser = new NodeTraverser(mongoCompNode);
		List<AbstractNode> tableAbstractNodes = traverser.TraverseNode("tables");

		// Group 4 table processing logic goes here...

		// update for group 4
		await traverser.UpdateLatexDocument(mongoCompNode);

		Console.WriteLine("✅ Completed Error Recovery & Document Processing");
	}
}
