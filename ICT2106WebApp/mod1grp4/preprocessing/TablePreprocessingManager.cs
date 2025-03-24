namespace ICT2106WebApp.mod1grp4 {
    public class TablePreprocessingManager : iBackupTabularSubject, iPreprocessedTable
    {
        public TablePreprocessingManager() 
        {

        }

        // Fix table integrity and fixing corrupted tables then save as backup (Jing Kai - NOT COMPLETED)
        public async Task<List<Table>> fixTableIntegrity(List<Table> tables)
        {
            // Logic to fix table integrity -- insert here
            foreach (var table in tables)
            {
                await notify<bool>(OperationType.SAVE, $"Table integrity has been fixed by TablePreprocessingManager and table with id {table.tableId} saved as backup", table);
            }
            
            return tables;
        }

        // Recover backup tables if ids tally with abstract node and whats in our database collection (Joel - COMPLETED)
        public async Task<List<Table>> recoverBackupTablesIfExist(List<Table> tablesFromNode)
        {
            // Retrieve tables using backupObserver
            var backupTables = await notify<List<Table>>(OperationType.RETRIEVE, "Checking if backup tables exist: whether ids given to me tallies with what i have in database", tablesFromNode);

            // Check if all requested IDs are retrievable
            var ids = tablesFromNode.Select(t => t.tableId).ToList();
            if (backupTables == null || backupTables.Count != ids.Count)
            {
                Console.WriteLine("Backup Tables do not exist. No crash has occurred previously. Proceeding with tables from node.");
                return tablesFromNode;
            }

            Console.WriteLine("Backup Tables exists. Crash has occurred previously. Retrieving backup tables instead.");
            return backupTables;
        }
    }

}