using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ICT2106WebApp.mod1grp4
{
    class TableLatexConversionManager : iBackupTabularSubject, iTableLatexConversion
    // public class TableLatexConversionManager
    // public class TableLatexConversionManager : iTableLatexConversion
    {
        // private iTableLatexConversion _latexConverter;

        // public TableLatexConversionManager(iTableLatexConversion latexConverter)
        // {
        //     _latexConverter = latexConverter;
        // }

        private iBackupGatewayObserver backupObserver;

        public TableLatexConversionManager()
        {

        }

        public async Task<List<Table>> convertToLatexAsync(List<Table> tableList)
        {

            // iTableCellCollection cellCollection = new TableCellCollection(table.GetCells());
            // iTableCellIterator iterator = cellCollection.CreateIterator();

            // Get numebr of columns


            foreach (var table in tableList)
            {
                if (table.tableCompletionState == false)
                {
                    int columns = 0;
                    foreach (var row in table.rows)
                    {
                        if (row.cells.Count > columns)
                        {
                            columns = row.cells.Count;
                        }
                    }

                    // Start building the LaTeX table
                    string latexTable = "\\begin{tabular}{|" + new string('c', columns).Replace("c", "c|") + "}";
                    latexTable += "\n\\hline\n";

                    foreach (var row in table.rows)
                    {
                        iTableCellCollection cellCollection = new TableCellCollection(row.cells);
                        iTableCellIterator iterator = cellCollection.createIterator();

                        if (table.rows.IndexOf(row) != 0) // Check if it's not the first row
                        {
                            latexTable = latexTable.TrimEnd(' ', '&') + " \\\\"; // End the previous row
                            latexTable += "\n\\hline\n"; // Add a horizontal line
                        }

                        while (!iterator.isDone())
                        {


                            TableCell cell = iterator.current(); // Get current cell
                            string cellContent = cell.content;
                            Dictionary<string, string> latexEscapes = new Dictionary<string, string>
                    {
                        { "&", "\\&" },
                        { "%", "\\%" },
                        { "$", "\\$" },
                        { "#", "\\#" },
                        { "_", "\\_" },
                        { "{", "\\{" },
                        { "}", "\\}" },
                        { "~", "\\~" },
                        { "^", "\\^" },
                        { "\\", "\\\\" }
                    };

                            foreach (var pair in latexEscapes)
                            {
                                cellContent = cellContent.Replace(pair.Key, pair.Value); // Escape special characters
                            }
                            string latexCell = cellContent; // Convert cell to LaTeX



                            if (cell.styling.bold)
                            {
                                latexCell = $"\\textbf{{{latexCell}}}";
                            }
                            if (cell.styling.italic)
                            {
                                latexCell = $"\\textit{{{latexCell}}}";
                            }
                            if (cell.styling.underline)
                            {
                                latexCell = $"\\underline{{{latexCell}}}";
                            }
                            if (cell.styling.fontsize != 0)
                            {
                                int fontSize = cell.styling.fontsize;
                                latexCell = $"{{\\fontsize{{{fontSize}}}{{\\baselineskip}}\\selectfont {latexCell}}}";
                            }
                            if (!string.IsNullOrEmpty(cell.styling.horizontalalignment))
                            {
                                string alignment = cell.styling.horizontalalignment;
                                string alignmentChar = alignment == "right" ? "r" : alignment == "left" ? "l" : "c";
                                latexCell = $" \\multicolumn{{1}}{{|{alignmentChar}|}} {{{latexCell}}}";
                            }

                            latexTable += latexCell + " & ";
                            iterator.next(); // Advance the iterator
                        }
                    }

                    latexTable = latexTable.TrimEnd(' ', '&') + " \\\\\n"; // Finalize the last row
                    latexTable += "\\hline\n"; // Add a horizontal line
                    latexTable += "\\end{tabular}"; // Close the LaTeX table
                    table.latexOutput = latexTable;
                    if (await updateLatexCheckpointAsync(table))
                    {
                        Console.WriteLine("Latex output:");
                        Console.WriteLine(latexTable);
                    }
                }
                else
                {
                    Console.WriteLine($"Table with id ${table.tableId}has already been converted before the crash.");
                    if (table == tableList[^1]) // Check if it's the last table in the list
                    {
                        Console.WriteLine($"All tables have been processed.");
                    }
                    else
                    {
                        Console.WriteLine($"Moving on to process the next table.");
                    }
                }

            }

            return tableList;
        }

        public async Task<bool> updateLatexCheckpointAsync(Table table)
        {
            await notify<bool>(OperationType.SAVE, "Updated LaTeX checkpoint for table", table);
            return true;
        }

        // public async Task<bool> UpdateLatexCheckpointAsync(int nodeId, string latexOutput)
        // {
        //     await Task.Delay(500);  // Simulating a time-consuming operation (e.g., database or file I/O)
        //     Console.WriteLine($"Updated LaTeX checkpoint for Node {nodeId}.");
        //     return true;
        // }
    }

}