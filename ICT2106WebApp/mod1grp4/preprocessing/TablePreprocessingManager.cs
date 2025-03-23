using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace ICT2106WebApp.mod1grp4 {
    class TablePreprocessingManager : iBackupTabularSubject, iPreprocessedTable
    {
        private iBackupGatewayObserver backupObserver;

        public TablePreprocessingManager() 
        {

        }

        public async Task<List<Table>> fixTableIntegrity(List<Table> tables)
        {
            // Logic to fix table integrity
            foreach (var table in tables)
            {
                await notify<bool>(OperationType.SAVE, $"Table integrity has been fixed by TablePreprocessingManager and table with id {table.tableId} saved as backup", table);
            }
            
            return tables;
        }

        public async Task<List<Table>> recoverBackupTablesIfExist(List<Table> tablesFromNode)
        {
            // Retrieve tables using backupObserver
            var backupTables = await notify<List<Table>>(OperationType.RETRIEVE, "Checking if tables exist: whether ids given to me tallies with what i have in database", tablesFromNode);

            // Check if all requested IDs are retrievable
            var ids = tablesFromNode.Select(t => t.tableId).ToList();
            if (backupTables == null || backupTables.Count != ids.Count)
            {
                Console.WriteLine("Backup Tables do not exist. No crash has occurred previously");
                return tablesFromNode;
            }

            Console.WriteLine("Backup Tables exists. Crash has occurred previously");
            return backupTables;
        }
    }

}