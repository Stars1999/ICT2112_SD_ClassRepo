namespace ICT2106WebApp.mod1grp4
{
    // TableLatexConversionManager (Andrea - COMPLETED)
    public class TableLatexConversionManager : iBackupTabularSubject, iTableLatexConversion
    {

        private iBackupGatewayObserver backupObserver;

        public TableLatexConversionManager()
        {

        }

        // Convert table to latex (Andrea - COMPLETED)
        public async Task<List<Table>> convertToLatexAsync(List<Table> tableList)
        {
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

                    // Prepare to store background colour
                    string predefinedColours = "";

                    // Start building the LaTeX table
                    string latexTable = "\\begin{tabular}{|" + string.Join("|", table.rows[0].cells.Select(cell => $"m{{{cell.styling.cellWidth}cm}}")) + "|}";
                    latexTable += "\n\\hline";

                    foreach (var row in table.rows)
                    {
                        iTableCellCollection cellCollection = new TableCellCollection(row.cells);
                        iTableCellIterator iterator = cellCollection.createIterator();

                        if (table.rows.IndexOf(row) != 0) // Check if it's not the first row
                        {
                            latexTable = latexTable.TrimEnd(' ', '&') + " \\\\"; // End the previous row
                            latexTable += "\n\\hline"; // Add a horizontal line
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
                            if (!string.IsNullOrEmpty(cell.styling.highlight) && cell.styling.highlight != "auto")
                            {
                                if (!predefinedColours.Contains(cell.styling.highlight))
                                {
                                    predefinedColours += $"\\definecolor{{{cell.styling.highlight}}}{{HTML}}{{{cell.styling.highlight}}}\n";
                                }
                                latexCell = $"\\hl{{{latexCell}}}";
                            }
                            if (!string.IsNullOrEmpty(cell.styling.textcolor) && cell.styling.textcolor != "auto")
                            {
                                if (!predefinedColours.Contains(cell.styling.textcolor))
                                {
                                    predefinedColours += $"\\definecolor{{{cell.styling.textcolor}}}{{HTML}}{{{cell.styling.textcolor}}}\n";
                                }
                                latexCell = $"\\textcolor{{{cell.styling.textcolor}}}{{{latexCell}}}";
                            }
                            // if (cell.styling.fontsize != 0)
                            // {
                            //     int fontSize = cell.styling.fontsize;
                            //     string rowHeightLatex = string.IsNullOrEmpty(cell.styling.rowHeight) || cell.styling.rowHeight == "auto"
                            //         ? string.Empty
                            //         : $"\\rule{{0pt}}{{{cell.styling.rowHeight}cm}}";
                            //     latexCell = $"{{{rowHeightLatex}\\fontsize{{{fontSize}}}{{\\baselineskip}}\\selectfont {latexCell}}}";
                            // }
                            if (cell.styling.fontsize != 0)
                            {
                                int fontSize = cell.styling.fontsize;
                                // string rowHeightLatex = string.IsNullOrEmpty(cell.styling.rowHeight) || cell.styling.rowHeight == "auto"
                                //     ? string.Empty
                                //     : $"\\rule{{0pt}}{{{cell.styling.rowHeight}cm}}";
                                latexCell = $"{{\\fontsize{{{fontSize}}}{{\\baselineskip}}\\selectfont {latexCell}}}";
                            }
                            if (!string.IsNullOrEmpty(cell.styling.backgroundcolor) && cell.styling.backgroundcolor != "auto")
                            {
                                if (!predefinedColours.Contains(cell.styling.backgroundcolor))
                                {
                                    predefinedColours += $"\\definecolor{{{cell.styling.backgroundcolor}}}{{HTML}}{{{cell.styling.backgroundcolor}}}\n";
                                }
                                latexCell = $"\\cellcolor{{{cell.styling.backgroundcolor}}}{{{latexCell}}}";
                            }
                            // Console.WriteLine(cell.styling.rowHeight);
                            if (!string.IsNullOrEmpty(cell.styling.verticalalignment) && cell.styling.rowHeight != "auto")
                            {
                                double rowHeight;
                                if (!string.IsNullOrEmpty(cell.styling.rowHeight) && double.TryParse(cell.styling.rowHeight, out double parsedRowHeight))
                                {

                                    rowHeight = parsedRowHeight;
                                }
                                else
                                {
                                    rowHeight = 0;
                                }
                                double topSpace;
                                double bottomSpace;
                                switch (cell.styling.verticalalignment.ToLower())
                                {
                                    case "top":
                                        // Handle top alignment
                                        topSpace = 0;
                                        bottomSpace = rowHeight;
                                        break;

                                    case "bottom":
                                        // Handle bottom alignment
                                        topSpace = rowHeight;
                                        bottomSpace = 0;
                                        break;

                                    case "center":
                                        // Handle center alignment
                                        topSpace = rowHeight / 2;
                                        bottomSpace = rowHeight / 2;
                                        break;

                                    default:
                                        // Handle default
                                        topSpace = rowHeight / 2;
                                        bottomSpace = rowHeight / 2;
                                        break;
                                }
                                latexCell = $" {{ \\rule{{0pt}}{{{topSpace}cm}} \\vspace{{{bottomSpace}cm}} {latexCell}}}";
                            }
                            if (!string.IsNullOrEmpty(cell.styling.horizontalalignment))
                            {
                                string alignment = cell.styling.horizontalalignment;
                                string alignmentChar = alignment == "right" ? "r" : alignment == "left" ? "l" : "c";
                                string alignmentRagged = alignment == "right" ? "raggedleft" : alignment == "left" ? "raggedright" : alignment == "both" ? "justifying" : "centering";
                                // latexCell = $" \\multicolumn{{1}}{{|{alignmentChar}|}} {{{latexCell}}}";
                                latexCell = $"\n\\multicolumn{{1}}{{|{alignmentChar}|}}{{\\parbox{{{cell.styling.cellWidth}cm}}{{\\{alignmentRagged} {latexCell}}}}}";
                            }

                            if (!string.IsNullOrEmpty(cell.styling.highlight) && cell.styling.highlight != "auto")
                            {
                                latexCell = $"\\sethlcolor {cell.styling.highlight}";
                            }
                            latexTable += latexCell + " & ";
                            iterator.next(); // Advance the iterator
                        }
                    }

                    latexTable = latexTable.TrimEnd(' ', '&') + " \\\\\n"; // Finalize the last row
                    latexTable += "\\hline\n"; // Add a horizontal line
                    latexTable += "\\end{tabular}"; // Close the LaTeX table
                    latexTable = predefinedColours + latexTable;
                    table.latexOutput = latexTable;
                    table.tableCompletionState = true;
                    if (await updateLatexCheckpointAsync(table))
                    {
                        Console.WriteLine("Latex output:");
                        Console.WriteLine(latexTable);
                    }
                }
                else
                {
                    Console.WriteLine($"Table with id {table.tableId} has already been converted before the crash. (ANDREA)");
                    if (table == tableList[^1]) // Check if it's the last table in the list
                    {
                        Console.WriteLine($"All tables have been processed. (ANDREA)");
                    }
                    else
                    {
                        Console.WriteLine($"Moving on to process the next table. (ANDREA)");
                    }
                }

            }

            return tableList;
        }

        // FOR HIEW TENG STYLE FAIL VALIDATION SIMILATION
        public async Task<List<Table>> convertToLatexStyleFailAsync(List<Table> tableList)
        {
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

                    // Prepare to store background colour
                    string predefinedColours = "";

                    // Start building the LaTeX table
                    string latexTable = "\\begin{tabular}{|" + string.Join("|", table.rows[0].cells.Select(cell => $"m{{{cell.styling.cellWidth}cm}}")) + "|}";
                    latexTable += "\n\\hline";

                    foreach (var row in table.rows)
                    {
                        iTableCellCollection cellCollection = new TableCellCollection(row.cells);
                        iTableCellIterator iterator = cellCollection.createIterator();

                        if (table.rows.IndexOf(row) != 0) // Check if it's not the first row
                        {
                            latexTable = latexTable.TrimEnd(' ', '&') + " \\\\"; // End the previous row
                            latexTable += "\n\\hline"; // Add a horizontal line
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

                            latexTable += latexCell + " & ";
                            iterator.next(); // Advance the iterator
                        }
                    }

                    latexTable = latexTable.TrimEnd(' ', '&') + " \\\\\n"; // Finalize the last row
                    latexTable += "\\hline\n"; // Add a horizontal line
                    latexTable += "\\end{tabular}"; // Close the LaTeX table
                    latexTable = predefinedColours + latexTable;
                    table.latexOutput = latexTable;
                    table.tableCompletionState = true;
                    if (await updateLatexCheckpointAsync(table))
                    {
                        Console.WriteLine("Latex output:");
                        Console.WriteLine(latexTable);
                    }
                }
                else
                {
                    Console.WriteLine($"Table with id {table.tableId} has already been converted before the crash. (ANDREA)");
                    if (table == tableList[^1]) // Check if it's the last table in the list
                    {
                        Console.WriteLine($"All tables have been processed. (ANDREA)");
                    }
                    else
                    {
                        Console.WriteLine($"Moving on to process the next table. (ANDREA)");
                    }
                }

            }

            return tableList;
        }




        // FOR JOEL CRASH RECOVERY SIMULATION
        public async Task<List<Table>> convertToLatexWithLimitAsync(List<Table> tableList, int limit)
        {
            int processedCount = 0;

            foreach (var table in tableList)
            {
                // Stop processing after hitting limit
                if (processedCount >= limit)
                {
                    Console.WriteLine($"Stopping after processing {limit} tables out of a total of {tableList.Count} (ANDREA).");
                    break;
                }


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
                    table.tableCompletionState = true;
                    if (await updateLatexCheckpointAsync(table))
                    {
                        Console.WriteLine("Latex output:");
                        Console.WriteLine(latexTable);
                    }

                    processedCount++;
                }
                else
                {
                    Console.WriteLine($"Table with id {table.tableId} has already been converted before the crash. (ANDREA)");
                    if (table == tableList[^1]) // Check if it's the last table in the list
                    {
                        Console.WriteLine($"All tables have been processed. (ANDREA)");
                    }
                    else
                    {
                        Console.WriteLine($"Moving on to process the next table. (ANDREA)");
                    }
                }

            }

            return tableList;
        }

        public async Task<bool> updateLatexCheckpointAsync(Table table)
        {
            await notify<bool>(OperationType.SAVE, $"Updated LaTeX checkpoint for table ID: {table.tableId} (ANDREA)", table);
            return true;
        }
    }

}