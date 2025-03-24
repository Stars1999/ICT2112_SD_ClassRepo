using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ICT2106WebApp.mod1grp4
{



    class TableLatexConversionManager : iBackupTabularSubject
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

        public async Task<List<Table>> ConvertToLatexAsync(List<Table> tableList)
        {

            // iTableCellCollection cellCollection = new TableCellCollection(table.GetCells());
            // iTableCellIterator iterator = cellCollection.CreateIterator();

            // Get numebr of columns


            foreach (var table in tableList)
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
                    iTableCellIterator iterator = cellCollection.CreateIterator();

                    if (table.rows.IndexOf(row) != 0) // Check if it's not the first row
                    {
                        latexTable = latexTable.TrimEnd(' ', '&') + " \\\\"; // End the previous row
                        latexTable += "\n\\hline\n"; // Add a horizontal line
                    }

                    while (!iterator.IsDone())
                    {


                        TableCell cell = iterator.Current(); // Get current cell
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
                        // if (cell.styling.underline)
                        // {
                        //     latexCell = $"\\underline{{{latexCell}}}";
                        // }
                        // if (!string.IsNullOrEmpty(cell.styling.fontsize))
                        // {
                        //     string fontSize = cell.styling.fontsize;
                        //     latexCell = $"{{\\fontsize{{{fontSize}}}{{\\baselineskip}}\\selectfont {latexCell}}}";
                        // }
                        // if (!string.IsNullOrEmpty(cell.styling.alignment))
                        // {
                        //     string fontSize = cell.styling.alignment;
                        //     string alignmentChar = alignment == "right" ? "r" : alignment == "center" ? "c" : "l";
                        //     latexCell = $" \\multicolumn{{1}}{{|{alignmentChar}|}} {{{latexCell}}}";
                        // }

                        latexTable += latexCell + " & ";
                        iterator.Next(); // Advance the iterator
                    }


                }




                // int currentRow = -1; // Variable to track the current row

                // while (!iterator.IsDone())
                // {
                //     TableCell cell = iterator.Current(); // Get current cell
                //     if (cell.GetRowSpan() != currentRow) // Check if it's a new row
                //     {
                //         // latexTable += " \\\\\n";
                //         if (currentRow != -1) latexTable = latexTable.TrimEnd(' ', '&') + " \\\\"; // End the previous row
                //         latexTable += "\n\\hline\n"; // Add a horizontal line
                //         currentRow = cell.GetRowSpan(); // Update the current row
                //     }

                //     string cellContent = cell.GetContent();
                //     Dictionary<string, string> latexEscapes = new Dictionary<string, string>
                //     {
                //         { "&", "\\&" },
                //         { "%", "\\%" },
                //         { "$", "\\$" },
                //         { "#", "\\#" },
                //         { "_", "\\_" },
                //         { "{", "\\{" },
                //         { "}", "\\}" },
                //         { "~", "\\~" },
                //         { "^", "\\^" },
                //         { "\\", "\\\\" }
                //     };

                //     foreach (var pair in latexEscapes)
                //     {
                //         cellContent = cellContent.Replace(pair.Key, pair.Value); // Escape special characters
                //     }
                //     string latexCell = cellContent; // Convert cell to LaTeX

                //     // Apply styles based on content style
                //     List<string> contentStyles = cell.GetContentStyle();
                //     if (contentStyles.Contains("bold"))
                //     {
                //         latexCell = $"\\textbf{{{latexCell}}}";
                //     }
                //     if (contentStyles.Contains("italic"))
                //     {
                //         latexCell = $"\\textit{{{latexCell}}}";
                //     }
                //     if (contentStyles.Contains("underline"))
                //     {
                //         latexCell = $"\\underline{{{latexCell}}}";
                //     }
                //     if (contentStyles.Exists(style => style.StartsWith("fontsize:")))
                //     {
                //         string fontSizeStyle = contentStyles.Find(style => style.StartsWith("fontsize:"));
                //         string fontSize = fontSizeStyle.Split(':')[1];
                //         latexCell = $"{{\\fontsize{{{fontSize}}}{{\\baselineskip}}\\selectfont {latexCell}}}";
                //     }
                //     if (contentStyles.Exists(style => style.StartsWith("alignment:")))
                //     {
                //         string alignmentStyle = contentStyles.Find(style => style.StartsWith("alignment:"));
                //         string alignment = alignmentStyle.Split(':')[1];
                //         string alignmentChar = alignment == "right" ? "r" : alignment == "center" ? "c" : "l";
                //         latexCell = $" \\multicolumn{{1}}{{|{alignmentChar}|}} {{{latexCell}}}";
                //     }

                //     // Add the LaTeX cell to the table
                //     latexTable += latexCell + " & ";
                //     iterator.Next(); // Advance the iterator
                // }

                latexTable = latexTable.TrimEnd(' ', '&') + " \\\\\n"; // Finalize the last row
                latexTable += "\\hline\n"; // Add a horizontal line
                latexTable += "\\end{tabular}"; // Close the LaTeX table
                table.latexOutput = latexTable;
                if (await UpdateLatexCheckpointAsync(table))
                {
                    Console.WriteLine("yay");
                }

            }

            return tableList;
        }

        public async Task<bool> UpdateLatexCheckpointAsync(Table table)
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