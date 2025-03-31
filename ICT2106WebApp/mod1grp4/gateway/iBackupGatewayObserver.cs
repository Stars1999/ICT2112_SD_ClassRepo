namespace ICT2106WebApp.mod1grp4 {
    // iBackupGatewayObserver (Joel - COMPLETED)
    public interface iBackupGatewayObserver
    {
        public Task<List<Table>> retrieveBackupTables(List<Table> tables);
        public Task<bool> saveTable(Table table);
        public Task<bool> deleteTable(Table table);
        public Task<T> updateSubject<T>(OperationType type, string message, object data);
    }
} 