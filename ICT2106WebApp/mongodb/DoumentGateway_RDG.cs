

using MongoDB.Driver;
using Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using ICT2106WebApp.mod1Grp3;
public class DocumentGateway_RDG : IDocumentRetrieve, IDocumentUpdate, ITreeUpdate, INodeRetrieve, IQueryUpdate, IQueryRetrieve
// {
//     private readonly MongoDbService _mongoDbService;
//     private readonly IMongoCollection<Docx> _docxCollection;
//     private readonly IMongoCollection<AbstractNode> _nodeCollection;
// 	private readonly IMongoCollection<AbstractNode> _treeCollection;
//     // private IDocumentUpdateNotify _docxUpdate;
//     private IDocumentRetrieveNotify _docxRetrieve;
//     private ITreeUpdateNotify _treeUpdate;

//     public DocumentGateway_RDG()
//     {
//         _mongoDbService = new MongoDbService();
//         _docxCollection = _mongoDbService.GetCollection<Docx>("wordoc");
//         _nodeCollection = _mongoDbService.GetCollection<AbstractNode>("nodes");
//         _treeCollection = _mongoDbService.GetCollection<AbstractNode>("trees");
//     }
//     private IDocumentUpdateNotify _docxUpdate;

//     // Property setter for update notification
//     public IDocumentUpdateNotify docxUpdate
//     {
//         get => _docxUpdate;
//         set => _docxUpdate = value;
//     }

//     public IDocumentRetrieveNotify docxRetrieve
//     {
//         get { return _docxRetrieve; }
//         set { _docxRetrieve = value; }
//     }

//     public ITreeUpdateNotify treeUpdate
//     {
//         get {return _treeUpdate;}
//         set {_treeUpdate = value;}
//     }

//     public async Task<Docx> getDocument(string id)
//     {
// 		Console.WriteLine("DocxRDG -> getDocument");
// 		var docx = await _docxCollection.Find(d => d.Id == id).FirstOrDefaultAsync();
// 		if (docx != null)
// 		{
// 			await _docxRetrieve.notifyRetrievedDocument(docx); // Directly notify retrieval
// 		}
// 		return docx;
//     }

// public async Task saveDocument(Docx docx)
// {
//     Console.WriteLine("DocxRDG -> saveDocument");

//     if (_docxUpdate == null)
//     {
//         throw new InvalidOperationException("Document update notifier (_docxUpdate) is not initialized.");
//     }

//     await _docxCollection.InsertOneAsync(docx);
//     await _docxUpdate.notifyUpdatedDocument(docx); // Notify about the update
// }

//     public async Task saveTree(AbstractNode rootNode)
//     {
// 		Console.WriteLine("DocxRDG -> saveTree");
// 		await _treeCollection.InsertOneAsync(rootNode);
//     }

//     	public async Task<List<Docx>> GetAllAsync()
// 	{
// 		return await _docxCollection.Find(d => true).ToListAsync();
// 	}

// 	public async Task UpdateAsync(Docx docx)
// 	{
// 		await _docxCollection.ReplaceOneAsync(d => d.Id == docx.Id, docx);
// 	}

// 	public async Task DeleteAsync(string id)
// 	{
// 		await _docxCollection.DeleteOneAsync(d => d.Id == id);
// 	}
// }
{
    private readonly MongoDbService _mongoDbService;
    private readonly IMongoCollection<Docx> _docxCollection;
    // private readonly IMongoCollection<AbstractNode> _treeCollection;
    private readonly IMongoCollection<AbstractNode> _treeCollection;


    // Nullable properties with null checks
    private IDocumentUpdateNotify _docxUpdate;
    private IDocumentRetrieveNotify _docxRetrieve;

    private ITreeUpdateNotify _treeUpdate;
    
    private IQueryUpdateNotify _queryUpdate;

    private IQueryRetrieveNotify _queryRetrieve;

    public IDocumentUpdateNotify docxUpdate
    {
        get => _docxUpdate;
        set => _docxUpdate = value;
    }

    public IDocumentRetrieveNotify docxRetrieve
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
        _treeCollection = _mongoDbService.GetCollection<AbstractNode>("zxTrees");
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
        var docx = await _docxCollection.Find(d => d.Id == id).FirstOrDefaultAsync();
        
        // Null check for retrieve notifier
        if (_docxRetrieve != null && docx != null)
        {
            await _docxRetrieve.notifyRetrievedDocument(docx);
        }
        
        return docx;
    }

        // Extension method for converting to BsonDocument
    private BsonDocument ToBsonDocument(AbstractNode node)
    {
        return node.ToBsonDocument();

        // If you need a more complex recursive conversion for CompositeNodes:
        // return node is CompositeNode compositeNode 
        //     ? ToRecursiveBsonDocument(compositeNode) 
        //     : node.ToBsonDocument();
    }

    // Optional recursive method for CompositeNodes
private BsonDocument ToRecursiveBsonDocument(CompositeNode compositeNode)
{
    var bsonDocument = compositeNode.ToBsonDocument();
    
    var children = compositeNode.GetChildren();
    if (children != null && children.Any())
    {
        var childrenBsonArray = new BsonArray();
        foreach (var child in children)
        {
            childrenBsonArray.Add(
                child is CompositeNode childComposite 
                    ? ToRecursiveBsonDocument(childComposite) 
                    : child.ToBsonDocument()
            );
        }
        
        bsonDocument.Add("children", childrenBsonArray);
    }

    return bsonDocument;
}
// private BsonDocument ConvertNodeToDetailedBsonDocument(AbstractNode node)
// {
// var bsonDocument = new BsonDocument
// {
//     { "nodeId", node.GetNodeId() },
//     { "nodeLevel", node.GetNodeLevel() },
//     { "nodeType", node.GetNodeType() },
//     { "content", node.GetContent() },
//     { "converted", node.IsConverted() },
//     { "styling", new BsonArray(node.GetStyling().Select(s => 
//         new BsonDocument(s.Select(kvp => 
//             new BsonElement(kvp.Key, BsonValue.Create(kvp.Value))
//         ))
//     ) )}
// };  // Added closing parenthesis here

//     // If it's a CompositeNode, add children
//     if (node is CompositeNode compositeNode)
//     {
//         var children = compositeNode.GetChildren();
//         if (children != null && children.Any())
//         {
//             var childrenBsonArray = new BsonArray();
//             foreach (var child in children)
//             {
//                 childrenBsonArray.Add(ConvertNodeToDetailedBsonDocument(child));
//             }
//             bsonDocument.Add("children", childrenBsonArray);
//         }
//     }

//     return bsonDocument;
// }
// private BsonDocument ConvertNodeToDetailedBsonDocument(AbstractNode node)
// {
//     var bsonDocument = new BsonDocument
//     {
//         { "nodeId", node.GetNodeId() },
//         { "nodeLevel", node.GetNodeLevel() },
//         { "nodeType", node.GetNodeType() },
//         { "content", node.GetContent() },
//         { "converted", node.IsConverted() }
//     };

//     // Improved styling conversion
//     var styling = node.GetStyling();
//     if (styling != null && styling.Count > 0)
//     {
//         var stylingBsonArray = new BsonArray();
//         foreach (var styleDict in styling)
//         {
//             var styleBsonDocument = new BsonDocument();
//             foreach (var kvp in styleDict)
//             {
//                 // Explicitly handle different value types
//                 if (kvp.Value == null)
//                 {
//                     styleBsonDocument.Add(kvp.Key, BsonNull.Value);
//                 }
//                 else
//                 {
//                     styleBsonDocument.Add(kvp.Key, BsonValue.Create(kvp.Value));
//                 }
//             }
//             stylingBsonArray.Add(styleBsonDocument);
//         }
//         bsonDocument.Add("styling", stylingBsonArray);
//     }
//     else
//     {
//         // Add an empty styling array if no styling is present
//         bsonDocument.Add("styling", new BsonArray());
//     }

//     // If it's a CompositeNode, add children
//     if (node is CompositeNode compositeNode)
//     {
//         var children = compositeNode.GetChildren();
//         if (children != null && children.Any())
//         {
//             var childrenBsonArray = new BsonArray();
//             foreach (var child in children)
//             {
//                 childrenBsonArray.Add(ConvertNodeToDetailedBsonDocument(child));
//             }
//             bsonDocument.Add("children", childrenBsonArray);
//         }
//     }

//     return bsonDocument;
// }
    public BsonDocument ConvertNodeToDetailedBsonDocument(AbstractNode node)
    {
        var bsonDocument = new BsonDocument
        {
            { "nodeId", node.GetNodeId() },
            { "nodeLevel", node.GetNodeLevel() },
            { "nodeType", node.GetNodeType() },
            { "content", node.GetContent() },
            { "converted", node.IsConverted() }
        };

        // Improved styling conversion
        var styling = node.GetStyling();
        if (styling != null && styling.Count > 0)
        {
            var stylingBsonArray = new BsonArray();
            foreach (var styleDict in styling)
            {
                var styleBsonDocument = new BsonDocument();
                foreach (var kvp in styleDict)
                {
                    // Explicitly handle different value types
                    if (kvp.Value == null)
                    {
                        styleBsonDocument.Add(kvp.Key, BsonNull.Value);
                    }
                    else
                    {
                        styleBsonDocument.Add(kvp.Key, BsonValue.Create(kvp.Value));
                    }
                }
                stylingBsonArray.Add(styleBsonDocument);
            }
            bsonDocument.Add("styling", stylingBsonArray);
        }
        else
        {
            // Add an empty styling array if no styling is present
            bsonDocument.Add("styling", new BsonArray());
        }

        // If it's a CompositeNode, add children
        if (node is CompositeNode compositeNode)
        {
            var children = compositeNode.GetChildren();
            if (children != null && children.Any())
            {
                var childrenBsonArray = new BsonArray();
                foreach (var child in children)
                {
                    childrenBsonArray.Add(ConvertNodeToDetailedBsonDocument(child));
                }
                bsonDocument.Add("children", childrenBsonArray);
            }
        }

        return bsonDocument;
    }

// public async Task saveTree(AbstractNode rootNode)
// {
//     Console.WriteLine("DocxRDG -> saveTree");
    
//     // var bsonDocument = rootNode is CompositeNode compositeNode 
//     //     ? ToRecursiveBsonDocument(compositeNode) 
//     //     : rootNode.ToBsonDocument();
//     var bsonDocument = ConvertNodeToDetailedBsonDocument(rootNode);

//     await _treeCollection.InsertOneAsync(bsonDocument);
//     Console.WriteLine("added rootNode into MongoDB!");
// }
public async Task saveTree(AbstractNode rootNode)
{
    Console.WriteLine("DocxRDG -> saveTree");
    
    // var bsonDocument = rootNode is CompositeNode compositeNode 
    //     ? ToRecursiveBsonDocument(compositeNode) 
    //     : rootNode.ToBsonDocument();
    var bsonDocument = rootNode.ToBsonDocument(); 

	// string jsonDoc = Newtonsoft.Json.JsonConvert.SerializeObject(rootNode, Newtonsoft.Json.Formatting.Indented);
    // await _treeCollection.InsertOneAsync(bsonDocument);
    await _treeCollection.InsertOneAsync(rootNode);

    Console.WriteLine("added rootNode into MongoDB!");
}


public async Task<AbstractNode> getTree()
{
    Console.WriteLine("Loading tree from MongoDB...");
    
    var node = await _treeCollection.Find(_ => true).FirstOrDefaultAsync();
    return node ?? throw new Exception("No tree found in database.");
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
        await _docxCollection.ReplaceOneAsync(d => d.Id == docx.Id, docx);
    }

    public async Task DeleteAsync(string id)
    {
        await _docxCollection.DeleteOneAsync(d => d.Id == id);
    }
}