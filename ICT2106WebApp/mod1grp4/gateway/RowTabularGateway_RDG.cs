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

        // Retrieve backup tables with a given list of ids (Joel - COMPLETED)
        public async Task<List<Table>> retrieveBackupTables(List<Table> tablesFromNode)
        {
            var ids = tablesFromNode.Select(t => t.tableId).ToList();
            var filter = Builders<Table>.Filter.In("tableId", ids);
            return await tableCollection.Find(filter).ToListAsync();
        }

        // Save a table (Joel - COMPLETED)
        public async Task<bool> saveTable(Table table)
        {
            var filter = Builders<Table>.Filter.Eq("tableId", table.tableId);
            var result = await tableCollection.ReplaceOneAsync(filter, table, new ReplaceOptions { IsUpsert = true });
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Delete specified tables in the collection (Joel - COMPLETED)
        public async Task<bool> deleteTable(Table table)
        {
            var filter = Builders<Table>.Filter.Eq("tableId", table.tableId);
            var result = await tableCollection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        // Update the subjects with a message (Joel - COMPLETED)
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
                    if (data is Table tableToDelete)
                    {
                        return (T)(object)await deleteTable(tableToDelete);
                    }
                    break;
            }
            return default(T);
        }
    }
}