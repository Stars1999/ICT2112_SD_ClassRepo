namespace ICT2106WebApp.mod1grp4 {
    public interface iBackupGatewayObserver
    {
        Task<List<Table>> retrieveBackupTables(List<Table> tables);
        Task<bool> saveTable(Table table);
        Task<bool> deleteTables();
        Task<T> updateSubject<T>(OperationType type, string message, object data);
    }
} 