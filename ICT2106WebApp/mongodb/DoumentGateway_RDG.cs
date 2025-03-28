

using MongoDB.Driver;
using Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
public class DocumentGateway_RDG : IDocumentRetrieve, IDocumentUpdate, INodeUpdate
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
    private readonly IMongoCollection<BsonDocument> _treeCollection;


    // Nullable properties with null checks
    private IDocumentUpdateNotify _docxUpdate;
    private IDocumentRetrieveNotify _docxRetrieve;

    private ITreeUpdateNotify _treeUpdate;
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

    public DocumentGateway_RDG()
    {
        _mongoDbService = new MongoDbService();
        _docxCollection = _mongoDbService.GetCollection<Docx>("wordox");
        // _treeCollection = _mongoDbService.GetCollection<AbstractNode>("trees");
        _treeCollection = _mongoDbService.GetCollection<BsonDocument>("trees");


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

//     public BsonDocument ConvertNodeToDetailedBsonDocument(AbstractNode node)
//     {
//         var bsonDocument = new BsonDocument
//         {
//             { "nodeId", node.GetNodeId() },
//             { "nodeLevel", node.GetNodeLevel() },
//             { "nodeType", node.GetNodeType() },
//             { "content", node.GetContent() },
//             { "converted", node.IsConverted() }
//         };


// var styling = node.GetStyling(); // Get the list of dictionaries
// var stylingBsonArray = new BsonArray();
// foreach (var dictionary in styling)
// {
//     foreach(var kvp in dictionary)
//     {
//         var styleBson = new BsonDocument();
//         styleBson.Add(kvp.Key, (BsonValue)kvp.Value);
//     }
//     stylingBsonArray.Add(styleBson);
// }
//     bsonDocument.Add("styling", stylingBsonArray); // Add to the final BSON document

//         // If it's a CompositeNode, add children
//         if (node is CompositeNode compositeNode)
//         {
//             var children = compositeNode.GetChildren();
//             if (children != null && children.Any())
//             {
//                 var childrenBsonArray = new BsonArray();
//                 foreach (var child in children)
//                 {
//                     childrenBsonArray.Add(ConvertNodeToDetailedBsonDocument(child));
//                 }
//                 bsonDocument.Add("children", childrenBsonArray);
//             }
//         }
//         Console.WriteLine($"Final BSON Document: {bsonDocument.ToJson()}");

//         return bsonDocument;
//     }
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

    // Get the list of dictionaries for styling
    var styling = node.GetStyling();
    if (styling != null && styling.Any())
    {
        var stylingBsonArray = new BsonArray();

        foreach (var styleDict in styling)
        {
            var styleBson = new BsonDocument();

            foreach (var kvp in styleDict)
            {
                // Handle null values by adding BsonNull
                if (kvp.Value == null)
                {
                    styleBson.Add(kvp.Key, BsonNull.Value);
                }
                else
                {
                    styleBson.Add(kvp.Key, BsonValue.Create(kvp.Value));
                }
            }

            stylingBsonArray.Add(styleBson);
        }

        bsonDocument.Add("styling", stylingBsonArray);
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

    Console.WriteLine($"Final BSON Document: {bsonDocument.ToJson()}");

    return bsonDocument;
}

public async Task saveTree(AbstractNode rootNode)
{
    Console.WriteLine("DocxRDG -> saveTree");
    
    // var bsonDocument = rootNode is CompositeNode compositeNode 
    //     ? ToRecursiveBsonDocument(compositeNode) 
    //     : rootNode.ToBsonDocument();
    var bsonDocument = ConvertNodeToDetailedBsonDocument(rootNode);

    await _treeCollection.InsertOneAsync(bsonDocument);
    Console.WriteLine("added rootNode into MongoDB!");
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

    // retrieve tree
    public async Task<AbstractNode> RetrieveTree()
{
    // Find the root node (assuming the root node has no parent)
    var rootDocument = await _treeCollection.Find(Builders<BsonDocument>.Filter.Exists("nodeId"))
                                            .FirstOrDefaultAsync();

    if (rootDocument == null)
    {
        return null;
    }

    return RecursivelyCreateNode(rootDocument);
}
// private AbstractNode RecursivelyCreateNode(BsonDocument bsonDocument)
// {
//     string nodeType = bsonDocument["nodeType"].AsString;

//     // Validate styling based on node type
//     List<Dictionary<string, object>> styling;

//     if (nodeType == "root")
//     {
//         // For root node, styling is optional or can be empty
//         styling = new List<Dictionary<string, object>>();
//     }
//     else
//     {
//         // For all other node types, require non-null, non-empty styling
//         if (!bsonDocument.Contains("styling") || 
//             bsonDocument["styling"].IsBsonNull || 
//             bsonDocument["styling"].AsBsonArray.Count == 0)
//         {
//             throw new ArgumentException($"Node of type '{nodeType}' must have non-empty styling");
//         }

//         // Convert styling to List<Dictionary<string, object>>
//         styling = bsonDocument["styling"].AsBsonArray
//             .Select(styleElement =>
//             {
//                 // Ensure each style element is a non-null dictionary with non-null values
//                 var styleDictionary = styleElement.AsBsonDocument
//                     .Where(kvp => !kvp.Value.IsBsonNull)
//                     .ToDictionary(
//                         kvp => kvp.Name, 
//                         kvp => BsonTypeMapper.MapToDotNetValue(kvp.Value)
//                     );

//                 // Additional validation to ensure the dictionary is not empty
//                 if (styleDictionary.Count == 0)
//                 {
//                     throw new ArgumentException($"Style element for node type '{nodeType}' cannot be empty");
//                 }

//                 return styleDictionary;
//             })
//             .ToList();
//     }

//     AbstractNode node;
//     switch (nodeType)
//     {
//         case "CompositeNode":
//         case "root":
//             node = new CompositeNode(
//                 bsonDocument["nodeId"].AsInt32, 
//                 bsonDocument["nodeLevel"].AsInt32, 
//                 bsonDocument["nodeType"].AsString,
//                 bsonDocument["content"].AsString,
//                 styling
//             );
//             break;
//         default:
//             node = new SimpleNode(
//                 bsonDocument["nodeId"].AsInt32, 
//                 bsonDocument["nodeLevel"].AsInt32, 
//                 bsonDocument["nodeType"].AsString,
//                 bsonDocument["content"].AsString,
//                 styling
//             );
//             break;
//     }

//     // Rest of the method remains the same...
//     return node;
// }


// // Overload to retrieve by specific criteria if needed
// public async Task<AbstractNode> RetrieveTreeByNodeId(int specificNodeId)
// {
//     var filter = Builders<BsonDocument>.Filter.Eq("nodeId", specificNodeId);
//     var rootDocument = await _treeCollection.Find(filter).FirstOrDefaultAsync();

//     return rootDocument != null ? RecursivelyCreateNode(rootDocument) : null;
// }
// }

private AbstractNode RecursivelyCreateNode(BsonDocument bsonDocument)
{
    // Logging the input document
    Console.WriteLine($"Processing BsonDocument: {bsonDocument.ToJson()}");

    string nodeType = bsonDocument["nodeType"].AsString;
    Console.WriteLine($"Node Type: {nodeType}");

    // Validate styling based on node type
    List<Dictionary<string, object>> styling;

    if (nodeType == "root")
    {
        // For root node, styling is optional or can be empty
        styling = new List<Dictionary<string, object>>();
        Console.WriteLine("Root node: Creating empty styling list");
    }
    else
    {
        // For all other node types, require non-null, non-empty styling
        if (!bsonDocument.Contains("styling") || 
            bsonDocument["styling"].IsBsonNull || 
            bsonDocument["styling"].AsBsonArray.Count == 0)
        {
            Console.WriteLine($"Error: Node of type '{nodeType}' lacks styling");
            throw new ArgumentException($"Node of type '{nodeType}' must have non-empty styling");
        }

        // Convert styling to List<Dictionary<string, object>>
        styling = bsonDocument["styling"].AsBsonArray
            .Select((styleElement, index) =>
            {
                Console.WriteLine($"Processing style element {index}");

                // Ensure each style element is a non-null dictionary with non-null values
                var styleDictionary = styleElement.AsBsonDocument
                    .Where(kvp => !kvp.Value.IsBsonNull)
                    .ToDictionary(
                        kvp => kvp.Name, 
                        kvp => BsonTypeMapper.MapToDotNetValue(kvp.Value)
                    );

                // Log the processed style dictionary
                Console.WriteLine($"Processed Style Dictionary {index}: {string.Join(", ", styleDictionary.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");

                // Additional validation to ensure the dictionary is not empty
                if (styleDictionary.Count == 0)
                {
                    Console.WriteLine($"Error: Style element {index} for node type '{nodeType}' is empty");
                    throw new ArgumentException($"Style element for node type '{nodeType}' cannot be empty");
                }

                return styleDictionary;
            })
            .ToList();

        Console.WriteLine($"Total style elements processed: {styling.Count}");
    }

    AbstractNode node;
    try 
    {
        switch (nodeType)
        {
            case "CompositeNode":
            case "root":
                node = new CompositeNode(
                    bsonDocument["nodeId"].AsInt32, 
                    bsonDocument["nodeLevel"].AsInt32, 
                    bsonDocument["nodeType"].AsString,
                    bsonDocument["content"].AsString,
                    styling
                );
                Console.WriteLine($"Created CompositeNode with ID: {node.GetNodeId()}");
                break;
            default:
                node = new SimpleNode(
                    bsonDocument["nodeId"].AsInt32, 
                    bsonDocument["nodeLevel"].AsInt32, 
                    bsonDocument["nodeType"].AsString,
                    bsonDocument["content"].AsString,
                    styling
                );
                Console.WriteLine($"Created SimpleNode with ID: {node.GetNodeId()}");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating node: {ex.Message}");
        Console.WriteLine($"Exception Details: {ex}");
        throw;
    }

    // Logging node creation
    Console.WriteLine($"Node created successfully. Type: {nodeType}, NodeId: {node.GetNodeId()}, NodeLevel: {node.GetNodeLevel()}");

    return node;
}

// Overload with additional logging
public async Task<AbstractNode> RetrieveTreeByNodeId(int specificNodeId)
{
    specificNodeId  = 1;
    Console.WriteLine($"Retrieving tree for NodeId: {specificNodeId}");

    var filter = Builders<BsonDocument>.Filter.Eq("nodeId", specificNodeId);
    var rootDocument = await _treeCollection.Find(filter).FirstOrDefaultAsync();

    if (rootDocument == null)
    {
        Console.WriteLine($"No document found for NodeId: {specificNodeId}");
        return null;
    }

    try 
    {
        var node = RecursivelyCreateNode(rootDocument);
        Console.WriteLine($"Successfully retrieved and created node for NodeId: {specificNodeId}");
        return node;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving tree for NodeId {specificNodeId}: {ex.Message}");
        Console.WriteLine($"Exception Details: {ex}");
        throw;
    }
}
}