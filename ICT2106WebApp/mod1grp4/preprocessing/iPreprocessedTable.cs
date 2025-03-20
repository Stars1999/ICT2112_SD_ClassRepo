namespace ICT2106WebApp.mod1grp4
{
    public interface iPreprocessedTable
    {
        Task<List<Table>> fixTableIntegrity(List<Table> tables);
        Task<bool> backupTablesExists(List<int> ids);
        Task<List<Table>> recoverTables(List<int> ids);
    }
}