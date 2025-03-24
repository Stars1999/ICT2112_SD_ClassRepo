using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ICT2106WebApp.mod1grp4
{
    class ProcessedTableManager : iBackupTabularSubject
    {
        public ProcessedTableManager()
        {
        }

        public bool logProcessingStatus(string description) {
            // Log to module 3
            var logID = Guid.NewGuid().ToString();
            var logTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var logLocation = "Table";
            Console.WriteLine($"insertLog({logID}, {logTimeStamp}, {description}, {logLocation})");
            return true;
        }

        public async Task<bool> slotProcessedTableToTree(List<Table> processedTables)
        {
            // Convert back to node and slot into tree
            var abstractNodes = tableToNode(processedTables);
            
            // Create JSON for debugging
            var json = JsonSerializer.Serialize(abstractNodes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync("debug_abstract_nodes.json", json);
            Console.WriteLine("Debug JSON created: debug_abstract_nodes.json");

            // use group 3 updateLatexDocument function to slot back into tree
            // updateLatexDocument(abstractNodes);

            Console.WriteLine("Processed tables have been converted back to node format and slotted back into the tree.");
            
            // Delete tables that are no longer needed
            await deleteTables(processedTables);
            Console.WriteLine("MODULE 1 GROUP 4: END");
            return true;
        }

        public List<TableAbstractNode> tableToNode(List<Table> processedTables)
        {
            var abstractNodes = new List<TableAbstractNode>();

            foreach (var table in processedTables)
            {
                var rowNodes = new List<RowNode>();

                foreach (var row in table.rows)
                {
                    var cellNodes = new List<CellNode>();

                    foreach (var cell in row.cells)
                    {
                        var cellNode = new CellNode
                        {
                            type = "Cell",
                            content = cell.content,
                            styling = new Dictionary<string, object>
                            {
                                { "bold", cell.styling.bold },
                                { "italic", cell.styling.italic },
                                { "border", cell.styling.border },
                                { "size", cell.styling.size },
                                { "backgroundcolor", cell.styling.backgroundcolor }
                            }
                        };
                        cellNodes.Add(cellNode);
                    }

                    var rowNode = new RowNode
                    {
                        type = "Row",
                        content = row.content,
                        runs = cellNodes
                    };
                    rowNodes.Add(rowNode);
                }

                var tableNode = new TableAbstractNode
                {
                    nodeID = table.tableId.ToString(),
                    type = "Table",
                    content = table.latexOutput,
                    runs = rowNodes
                };
                abstractNodes.Add(tableNode);
            }

            return abstractNodes;
        }

        public async Task<bool> deleteTables(List<Table> tables)
        {
            foreach (var table in tables)
            {
                await notify<bool>(OperationType.DELETE, "Deleting tables since no longer needed.", table);
            }
            return true;
        }
    }
}