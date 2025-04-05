using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using ICT2106WebApp.mod1grp4;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace ICT2106WebApp.mod1Grp3
{
	public class DocumentFailSafe : ICrashRecoveryRetrieveNotify
	{
		private readonly ICrashRecoveryRetrieve _docxRetrieve;

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

		public async Task runCrashRecovery(IMongoDatabase database, bool hasRetried = false)
		{
			string filePath = "";
			string outputPath = "";
			string jsonOutputPath = "output.json";

			Console.WriteLine("Starting Document Processing with Crash Recovery");

		// DocumentControl documentControl = new DocumentControl();
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
				await runCrashRecovery(database, true);
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
					var documentContents = DocumentProcessor.ExtractDocumentContents(wordDoc);
					var rootElement = DocumentProcessor.elementRoot();
					documentContents.Insert(0, DocumentProcessor.ExtractLayout(wordDoc));
					documentContents.Insert(0, rootElement);

					// List<AbstractNode> nodesList = NodeManager.CreateNodeList(documentContents);
					List<AbstractNode> nodesList = nodeManager.CreateNodeList(documentContents);

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

			// INodeTraverser traverser = new NodeTraverser(mongoCompNode);
			// List<AbstractNode> tableAbstractNodes = traverser.TraverseNode("tables");
			//added here onwards
			INodeTraverser traverser = new NodeTraverser(mongoCompNode);

			List<AbstractNode> tableAbstractNodes = traverser.TraverseNode("tables");
			Console.WriteLine("OFFIICALLY DOING JOEL GRP STUFF");
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

			Console.WriteLine("I WANT TO SEE SITI STUFF");

			// Will prove for Siti as we traverse the nodes again after updating
			List<AbstractNode> endingTableAbstractNodes = traverser.TraverseNode("tables");

			// Save modified latex tree back to MongoDB (query)
			await traverser.UpdateLatexDocument(mongoCompNode);

			ICompletedLatex completedLatex = new CompletedLatex();

			// // // Retrieve the non-modified tree from MongoDB (for demo query)
			// AbstractNode originalRootNode = await completedLatex.RetrieveUnmodifiedTree();
			// // CompositeNode originalMongo = null;
			// CompositeNode originalTree = (CompositeNode)originalRootNode;
			// if (originalTree != null)
			// {
			// 	treeProcessor.PrintTreeContents(originalTree);
			// 	treeProcessor.PrintTreeHierarchy(originalTree, 0);
			// }
			Console.WriteLine("ok can see all");
			//Retrieve the Latex tree from MongoDB (for demo query)
			AbstractNode latexRootNode = await completedLatex.RetrieveLatexTree();
			CompositeNode latexMongo = (CompositeNode)latexRootNode;
			if (latexMongo != null)
			{
				treeProcessor.PrintTreeContents(latexMongo);
				treeProcessor.PrintTreeHierarchy(latexMongo, 0);
			}
			// Group 4 table processing logic goes here...

			// await traverser.UpdateLatexDocument(mongoCompNode);

			Console.WriteLine("✅ Completed Error Recovery & Document Processing");
		}
	}

}