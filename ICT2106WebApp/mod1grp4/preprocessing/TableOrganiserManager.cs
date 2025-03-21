using System.Collections.Generic;
using System.Linq;

namespace ICT2106WebApp.mod1grp4 {
    class TableOrganiserManager : iTableOrganise<TableAbstractNode>
    {

        public TableOrganiserManager() 
        {

        }

        public List<Table> organiseTables(List<TableAbstractNode> abstractNode)
        {
            var tables = new List<Table>();

            foreach (var node in abstractNode)
            {
                var tableId = int.Parse(node.nodeID);
                var cells = new List<TableCell>();

                foreach (var row in node.runs)
                {
                    foreach (var cell in row.runs)
                    {
                        cells.Add(new TableCell(cell.content, cell.styling, new List<string>())); // awaiting jonathan to extract cell style NOT CONTENT
                    }
                }

                var table = new Table(
                    tableId: tableId,
                    cells: cells,
                    style: new List<string>(), // awaiting jonathan to extract table style
                    tableCompletionState: false,
                    latexOutput: string.Empty
                );

                tables.Add(table);
            }

            Console.WriteLine("Tables have been organised from abstract node type to Table entity type");
            return tables;
        }
    }
}