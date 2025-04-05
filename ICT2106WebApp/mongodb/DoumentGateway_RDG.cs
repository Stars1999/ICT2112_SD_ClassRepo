using ICT2106WebApp.mod1Grp3;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ICT2106WebApp.mod1Grp3;

public class DocumentGateway_RDG
	: ICrashRecoveryRetrieve,
		IDocumentUpdate,
		ITreeUpdate,
		INodeRetrieve,
		IQueryUpdate,
		IQueryRetrieve
{
	private readonly MongoDbService _mongoDbService;
	private readonly IMongoCollection<Docx> _docxCollection;

	// private readonly IMongoCollection<AbstractNode> _treeCollection;
	private readonly IMongoCollection<AbstractNode> _treeCollection;

	private readonly IMongoCollection<BsonDocument> _jsonCollection;

	private readonly IMongoCollection<AbstractNode> _latexCollection;

	// Nullable properties with null checks
	private IDocumentUpdateNotify _docxUpdate;
	private ICrashRecoveryRetrieveNotify _docxRetrieve;

	private ITreeUpdateNotify _treeUpdate;

	private IQueryUpdateNotify _queryUpdate;

	private IQueryRetrieveNotify _queryRetrieve;

	public IDocumentUpdateNotify docxUpdate
	{
		get => _docxUpdate;
		set => _docxUpdate = value;
	}

	public ICrashRecoveryRetrieveNotify docxRetrieve
	{
		get => _docxRetrieve;
		set => _docxRetrieve = value;
	}

	public ITreeUpdateNotify treeUpdate
	{
		get => _treeUpdate;
		set => _treeUpdate = value;
	}

	public IQueryUpdateNotify queryUpdate
	{
		get => _queryUpdate;
		set => _queryUpdate = value;
	}

	public IQueryRetrieveNotify queryRetrieve
	{
		get => _queryRetrieve;
		set => _queryRetrieve = value;
	}

	public DocumentGateway_RDG()
	{
		_mongoDbService = new MongoDbService();
		_docxCollection = _mongoDbService.GetCollection<Docx>("wordox");
		// _treeCollection = _mongoDbService.GetCollection<AbstractNode>("trees");
		_treeCollection = _mongoDbService.GetCollection<AbstractNode>("mergewithcommentedcode");
		_jsonCollection = _mongoDbService.GetCollection<BsonDocument>("jsonn");
		_latexCollection = _mongoDbService.GetCollection<AbstractNode>("latexTree");
		// Docx.RegisterMongoSerializer();

		// if (!BsonClassMap.IsClassMapRegistered(typeof(Docx)))
        // {
        //     BsonClassMap.RegisterClassMap<Docx>(cm =>
        //     {
        //         cm.AutoMap(); // maps public methods/properties
        //         cm.MapIdMember(c => c.GetType().GetProperty("DocId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
        //         cm.MapMember(c => c.GetType().GetProperty("Title", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
        //         cm.MapMember(c => c.GetType().GetProperty("FileName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
        //         cm.MapMember(c => c.GetType().GetProperty("FileData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
        //     });
        // }
	}

	public async Task saveDocument(Docx docx)
	{
		Console.WriteLine("DocxRDG -> saveDocument");

		// Null check for update notifier
		if (_docxUpdate == null)
		{
			Console.WriteLine("Warning: No document update notifier set.");
			await _docxCollection.InsertOneAsync(docx);
			return;
		}

		await _docxCollection.InsertOneAsync(docx);
		await _docxUpdate.notifyUpdatedDocument(docx);
	}

	public async Task<Docx> getDocument(string id)
	{
		Console.WriteLine("DocxRDG -> getDocument");
		// var docx = await _docxCollection.Find(d => d.Id == id).FirstOrDefaultAsync();

		// var docx = await _docxCollection.Find(d => (string)d.GetDocxAttributeValue("docxId") == id).FirstOrDefaultAsync();
		var docx = await _docxCollection.Find(_ => true).FirstOrDefaultAsync();

		// Null check for retrieve notifier
		if (_docxRetrieve != null && docx != null)
		{
			await _docxRetrieve.notifyRetrievedDocument(docx);
		}

		return docx;
	}

	public async Task saveJsonFile(string filepath)
	{
		string jsonData = File.ReadAllText(filepath);

		// Convert JSON string to BsonDocument
		var bsonDocument = BsonDocument.Parse(jsonData);

		// Insert into MongoDB
		_jsonCollection.InsertOne(bsonDocument);
		Console.WriteLine("RDG -> json saved into DB!");
	}

	public async Task getJsonFile()
	{
		var documents = _jsonCollection.Find(new BsonDocument()).ToList();
		// Convert BSON to JSON
		string jsonOutput = Newtonsoft.Json.JsonConvert.SerializeObject(
			documents,
			Newtonsoft.Json.Formatting.Indented
		);

		// Save JSON to a file
		string filePath = "output1.json";
		File.WriteAllText(filePath, jsonOutput);
		Console.WriteLine($"Data successfully saved to {filePath}");

		await _docxRetrieve.notifyRetrievedJson();
	}

	public async Task saveTree(AbstractNode rootNode, string collectionName)
	{
		Console.WriteLine("DocxRDG -> saveTree");

		// var bsonDocument = rootNode is CompositeNode compositeNode
		//     ? ToRecursiveBsonDocument(compositeNode)
		//     : rootNode.ToBsonDocument();
		var bsonDocument = rootNode.ToBsonDocument();

		// string jsonDoc = Newtonsoft.Json.JsonConvert.SerializeObject(rootNode, Newtonsoft.Json.Formatting.Indented);
		// await _treeCollection.InsertOneAsync(bsonDocument);

		if (collectionName == "latexTree")
		{
			await _latexCollection.DeleteManyAsync(_ => true); // Clear existing data
			await _latexCollection.InsertOneAsync(rootNode);
			Console.WriteLine("added LatexTree into MongoDB!");
			return;
		}
		else if (collectionName == "mergewithcommentedcode")
		{
			await _treeCollection.DeleteManyAsync(_ => true); // Clear existing data
			await _treeCollection.InsertOneAsync(rootNode);
		}

		Console.WriteLine("added DocxTree into MongoDB!");
	}

	public async Task<AbstractNode> getTree(string collectionName)
	{
		Console.WriteLine("Loading tree from MongoDB...");
		AbstractNode node = null;

		if (collectionName == "latexTree")
		{
			node = await _latexCollection.Find(_ => true).FirstOrDefaultAsync();
			Console.WriteLine(node == null ? "No data found" : "Data found!");
			// return node;
		}
		else if (collectionName == "mergewithcommentedcode")
		{
			node = await _treeCollection.Find(_ => true).FirstOrDefaultAsync();
			Console.WriteLine(node == null ? "No data found" : "Data found!");
			// return node;
		}

		// var node = await _treeCollection.Find(_ => true).FirstOrDefaultAsync();
		Console.WriteLine(node == null ? "No data found" : "Data found!");

		return node;
		// return node ?? throw new Exception("No tree found in database.");
	}

	// public async Task saveTree(AbstractNode rootNode)
	// {
	// 	Console.WriteLine("DocxRDG -> saveTree");
	// 	await _treeCollection.InsertOneAsync(rootNode);
	//     Console.WriteLine("ADDDED");
	// }

	public async Task<List<Docx>> GetAllAsync()
	{
		return await _docxCollection.Find(d => true).ToListAsync();
	}

	public async Task UpdateAsync(Docx docx)
	{
		// await _docxCollection.ReplaceOneAsync(d => d.getDocxAttributeValue("docxId") == docx.Id, docx);
				await _docxCollection.ReplaceOneAsync(d => (string)d.GetDocxAttributeValue("docxId") == (string)docx.GetDocxAttributeValue("docxId"), docx);

	}

	public async Task DeleteAsync(string id)
	{
		// await _docxCollection.DeleteOneAsync(d => d.Id == id);
		await _docxCollection.DeleteOneAsync(d => (string)d.GetDocxAttributeValue("docxId") == id);

	}
}
