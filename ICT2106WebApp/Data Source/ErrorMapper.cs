using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ErrorMapper
{
    private readonly IMongoCollection<ErrorStyle> _collection;

    public ErrorMapper(MongoDbContext context)
    {
        _collection = context.ErrorStyles;

    }

    //Insert new error record
    public async Task InsertErrorAsync(ErrorStyle error)
    {
        await _collection.InsertOneAsync(error);
    }
    //Replace existing error based on ErrorId
    public async Task UpdateErrorAsync(string id, ErrorStyle updatedError)
    {
        var filter = Builders<ErrorStyle>.Filter.Eq(e => e.ErrorId, id);
        await _collection.ReplaceOneAsync(filter, updatedError);
    }

    //Updates only the error message field of existing error document
    public async Task EditErrorMessageAsync(string id, string newMessage)
    {
        var filter = Builders<ErrorStyle>.Filter.Eq(e => e.ErrorId, id);
        var update = Builders<ErrorStyle>.Update.Set(e => e.Message, newMessage);
        await _collection.UpdateOneAsync(filter, update);
    }


    //Retrieves the latest error record by timestamp.
    public async Task<ErrorStyle> GetLatestErrorAsync()
    {
        return await _collection
            .Find(FilterDefinition<ErrorStyle>.Empty)
            .SortByDescending(e => e.Timestamp)
            .FirstOrDefaultAsync();
    }

    public async Task<List<ErrorStyle>> GetAllErrorsAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
}
