namespace ICT2106WebApp.mod1grp4 {
    // iBackupGatewayObserver (Joel - COM
    public interface iBackupGatewayObserver
    {
        Task<List<Table>> retrieveBackupTables(List<Table> tables);
        Task<bool> saveTable(Table table);
        Task<bool> deleteTable(Table table);
        Task<T> updateSubject<T>(OperationType type, string message, object data);
    }
} 