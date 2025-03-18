namespace ICT2106WebApp.mod1grp4 {
    public interface iBackupGatewayObserver
    {
        Task<List<Table>> retrieveTables(List<int> ids);
        Task<bool> saveTable(int id, Table table);
        Task<bool> deleteTables(int collectionId);
    }
} 