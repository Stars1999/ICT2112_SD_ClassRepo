using System.Text.Json;
using DocumentFormat.OpenXml.EMMA;
using Utilities;

namespace ICT2106WebApp.mod1grp4
{
    public class TableOrganiserManager : iTableOrganise<AbstractNode>
    {
        // Convert the abstract nodes given into custom table entity (Joel - COMPLETED)
        public List<Table> organiseTables(List<AbstractNode> abstractNodes)
        {
            Console.WriteLine("MODULE 1 GROUP 4: START");
            var tables = new Dictionary<int, Table>(); // Use a dictionary to group tables by their ID

            foreach (var node in abstractNodes)
            {
                if (node is CompositeNode tableCompositeNode && tableCompositeNode.GetNodeType() == "table")
                {
                    var tableId = tableCompositeNode.GetNodeId();

                    // Check if the table already exists in the dictionary
                    if (!tables.TryGetValue(tableId, out var existingTable))
                    {
                        // Create a new table if it doesn't already exist
                        existingTable = new Table(tableId, new List<TableRow>(), false, tableCompositeNode.GetContent()) { type = "table" };
                        tables[tableId] = existingTable;
                    }

                    // Process rows and add them to the existing table
                    foreach (var rowNode in tableCompositeNode.GetChildren())
                    {
                        if (rowNode is CompositeNode rowCompositeNode && rowCompositeNode.GetNodeType() == "row")
                        {
                            var cells = new List<TableCell>();

                            // Process cells within the row
                            foreach (var cellNode in rowCompositeNode.GetChildren())
                            {
                                if (cellNode is AbstractNode cellAbstractNode && cellAbstractNode.GetNodeType() == "cell")
                                {
                                    var cellContent = cellAbstractNode.GetContent();
                                    var cellStyling = cellAbstractNode.GetStyling();
                                    cells.Add(new TableCell(cellContent, new CellStyling()) { type = "cell" });
                                }
                            }

                            // Add the row to the table
                            existingTable.rows.Add(new TableRow(string.Empty, cells) { type = "row" });
                        }
                    }
                }
            }

            // Return the list of tables
            return tables.Values.ToList();
        }
    }
}