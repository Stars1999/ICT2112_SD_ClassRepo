using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace ICT2106WebApp.mod1grp4 {
    public class TablePreprocessingManager : iBackupTabularSubject, iPreprocessedTable
    {
        private iBackupGatewayObserver backupObserver;

        public TablePreprocessingManager(iBackupGatewayObserver observer)
        {
            this.backupObserver = observer;
            this.attach(observer);
        }

        public async Task<List<Table>> fixTableIntegrity(List<Table> tables)
        {
            // Logic to fix table integrity using backupObserver
            foreach (var table in tables)
            {
                // Logic to fix table integrity

                await backupObserver.saveTable(table.GetTableId(), table);
            }
            return tables;
        }

        public async Task<bool> backupTablesExists(List<int> ids)
        {
            // Retrieve tables using backupObserver
            var tables = await backupObserver.retrieveTables(ids);
            this.notify("Checking if tables exist");

            // Check if all requested IDs are retrievable
            if (tables == null || tables.Count != ids.Count)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Table>> recoverTables(List<int> ids)
        {
            // Logic to recover tables using backupObserver then notify all observers
            this.notify("Tables recovered");  
            return await backupObserver.retrieveTables(ids);
        }
    }

}