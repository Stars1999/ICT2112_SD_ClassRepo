using System.Collections.Generic;
using System.Text.Json;

namespace ICT2106WebApp.mod1grp4
{
    public class TableOrganiserManager : iTableOrganise<TableAbstractNode>
    {
        public List<Table> organiseTables(List<TableAbstractNode> abstractNodes)
        {
            Console.WriteLine("MODULE 1 GROUP 4: START");
            var tables = new List<Table>();

            foreach (var node in abstractNodes)
            {
                var tableId = int.Parse(node.nodeID);
                var rows = new List<TableRow>();

                foreach (var rowNode in node.runs)
                {
                    var cells = new List<TableCell>();

                    foreach (var cellNode in rowNode.runs)
                    {
                        var cellContent = cellNode.content;
                        var cellStylingJson = JsonSerializer.Serialize(cellNode.styling);
                        var cellStyling = JsonSerializer.Deserialize<CellStyling>(cellStylingJson);
                        cells.Add(new TableCell(cellContent, cellStyling) { type = "Cell" });
                    }

                    rows.Add(new TableRow(string.Empty, cells) {type = "Row"});
                }

                tables.Add(new Table(tableId, rows, false, node.content) { type = "Table" });
            }

            return tables;
        }
    }
}