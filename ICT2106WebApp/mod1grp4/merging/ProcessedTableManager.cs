using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod1grp4
{
    class ProcessedTableManager : iBackupTabularSubject
    {
        private readonly iTableValidate _tableValidator;

        public ProcessedTableManager(iTableValidate tableValidator)
        {
            _tableValidator = tableValidator;
        }

        public async Task<bool> slotProcessedTableToTree(List<Table> processedTables, List<TableAbstractNode> originalNodes)
        {
            // Validate tables first before converting back to node
            if (!_tableValidator.validateTableLatexOutput(originalNodes, processedTables))
            {
                // Log back to module 3
                var logID = Guid.NewGuid().ToString();
                var logTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var logDescription = "Validation failed";
                var logLocation = "Table";
                Console.WriteLine($"insertLog({logID}, {logTimeStamp}, {logDescription}, {logLocation})");
            }

            // Convert back to node and slot into tree
            var abstractNodes = tableToNode(processedTables);
            // use group 3 updateLatexDocument function
            // updateLatexDocument(abstractNodes);
            Console.WriteLine("Processed tables have been converted back to nodes and slotted back into the tree.");
            
            // Delete tables that are no longer needed
            await deleteTables(processedTables);
            Console.WriteLine("MODULE 1 GROUP 4: END");
            return true;
        }

        public List<TableAbstractNode> tableToNode(List<Table> processedTables) //HELPPPPPPPPPPPPPPPPPPPP
        {
            var abstractNodes = new List<TableAbstractNode>();

            foreach (var table in processedTables)
            {
                var rowNodes = new List<RowNode>();

                // Assuming cells are already grouped by rows in table.cells
                var groupedCells = table.cells.GroupBy(cell => cell);
                // need to add back their row here

                foreach (var row in groupedCells)
                {
                    var cellNodes = new List<CellNode>();

                    foreach (var cell in row)
                    {
                        var cellNode = new CellNode
                        {
                            content = cell.content,
                            styling = new Dictionary<string, object>
                            {
                                { "bold", cell.styling.bold },
                                { "italic", cell.styling.italic },
                            }
                        };
                        cellNodes.Add(cellNode);
                    }

                    rowNodes.Add(new RowNode { runs = cellNodes });
                }

                abstractNodes.Add(new TableAbstractNode
                {
                    nodeID = table.tableId.ToString(),
                    runs = rowNodes
                });
            }

            return abstractNodes;
        }

        public async Task<bool> deleteTables(List<Table> tables)
        {
            foreach (var table in tables)
            {
                await notify<bool>(OperationType.DELETE, $"Deleting table with id {table.tableId} since no longer needed.", table);
            }

            return true;
        }
    }
}