namespace ICT2106WebApp.mod1grp4
{
    // iPreprocessedTable (Joel - COMPLETED)
    public interface iPreprocessedTable
    {
        Task<List<Table>> fixTableIntegrity(List<Table> tables);
        Task<List<Table>> recoverBackupTablesIfExist(List<Table> tablesFromNode);
    }
}