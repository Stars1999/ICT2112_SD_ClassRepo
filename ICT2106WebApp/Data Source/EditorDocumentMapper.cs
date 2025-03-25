using MongoDB.Driver;
using System.Threading.Tasks;

public class EditorDocumentMapper : iRetrieveEditorDoc
{
    private readonly IMongoCollection<EditorDocument> _collection;

    public EditorDocumentMapper(MongoDbContext context)
    {
        _collection = context.Database.GetCollection<EditorDocument>("EditorDocuments");
    }

    // Update existing document with DocumentID = 1
    public async Task UpsertAsync(EditorDocument doc)
{
    var filter = Builders<EditorDocument>.Filter.Eq(d => d.DocumentID, 1);
    var options = new ReplaceOptions { IsUpsert = true };

    await _collection.ReplaceOneAsync(filter, doc, options);
}

    public async Task<EditorDocument> GetLatestAsync()
    {
        return await _collection
            .Find(FilterDefinition<EditorDocument>.Empty)
            .SortByDescending(d => d.DocumentID)
            .FirstOrDefaultAsync();
    }
}
