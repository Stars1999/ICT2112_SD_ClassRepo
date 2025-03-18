using MongoDB.Driver;

namespace ICT2106WebApp.mod1grp4
{
    // RDG for operations within our backup tables collection through the gateway
    public class RowTabularGateway_RDG : iBackupGatewayObserver
    {
        private readonly IMongoCollection<Table> tableCollection;
        private readonly iBackupTabularSubject ibackupTabularSubject;

        public RowTabularGateway_RDG(IMongoDatabase database, iBackupTabularSubject backupTabularSubject)
        {
            tableCollection = database.GetCollection<Table>("tables");
            ibackupTabularSubject = backupTabularSubject;
            ibackupTabularSubject.attach(this);
        }

        // Retrieve tables by the given ids
        public async Task<List<Table>> retrieveTables(List<int> ids)
        {
            var filter = Builders<Table>.Filter.In(t => t.GetTableId(), ids);
            return await tableCollection.Find(filter).ToListAsync();
        }

        // Save a table with a given id
        public async Task<bool> saveTable(int id, Table table)
        {
            var filter = Builders<Table>.Filter.Eq(t => t.GetTableId(), id);
            var result = await tableCollection.ReplaceOneAsync(filter, table, new ReplaceOptions { IsUpsert = true });
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Delete all tables in the collection
        public async Task<bool> deleteTables(int collectionId)
        {
            var filter = Builders<Table>.Filter.Eq(t => t.GetTableId(), collectionId);
            var result = await tableCollection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}