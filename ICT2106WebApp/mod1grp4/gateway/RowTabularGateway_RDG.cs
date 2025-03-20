using MongoDB.Driver;

namespace ICT2106WebApp.mod1grp4
{
    // RDG for operations within our backup tables collection through the gateway
    class RowTabularGateway_RDG : iBackupGatewayObserver
    {
        private readonly IMongoCollection<Table> tableCollection;

        public RowTabularGateway_RDG(IMongoDatabase database)
        {
            tableCollection = database.GetCollection<Table>("tables");
        }

        // Retrieve backup tables with a given list of ids
        public async Task<List<Table>> retrieveBackupTables(List<Table> tablesFromNode)
        {
            var ids = tablesFromNode.Select(t => t.GetTableId()).ToList();
            var filter = Builders<Table>.Filter.In(t => t.GetTableId(), ids);
            return await tableCollection.Find(filter).ToListAsync();
        }

        // Save a table
        public async Task<bool> saveTable(Table table)
        {
            var filter = Builders<Table>.Filter.Eq("tableId", table.GetTableId());
            // var filter = Builders<Table>.Filter.Eq(t => t.GetTableId(), table.GetTableId());
            var result = await tableCollection.ReplaceOneAsync(filter, table, new ReplaceOptions { IsUpsert = true });
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Delete all tables in the collection
        public async Task<bool> deleteTables()
        {
            var result = await tableCollection.DeleteManyAsync(Builders<Table>.Filter.Empty);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        // Update the subjects with a message
        public async Task<T> updateSubject<T>(OperationType type, string message, object data)
        {
            Console.WriteLine($"Observer: {message}");
            switch (type)
            {
                case OperationType.RETRIEVE:
                    if (data is List<Table> tables)
                    {
                        return (T)(object)await retrieveBackupTables(tables);
                    }
                    break;
                case OperationType.SAVE:
                    if (data is Table table)
                    {
                        return (T)(object)await saveTable(table);
                    }
                    break;
                case OperationType.DELETE:
                    return (T)(object)await deleteTables();
            }
            return default(T);
        }
    }
}