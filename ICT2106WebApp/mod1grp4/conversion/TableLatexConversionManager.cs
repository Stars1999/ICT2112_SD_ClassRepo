using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod1grp4
{



    public class TableLatexConversionManager
    // public class TableLatexConversionManager : iTableLatexConversion
    {
        // private iTableLatexConversion _latexConverter;

        // public TableLatexConversionManager(iTableLatexConversion latexConverter)
        // {
        //     _latexConverter = latexConverter;
        // }


        public async Task<string> ConvertToLatexAsync(Table table)
        {

            iTableCellCollection cellCollection = new TableCellCollection(table.GetCells());
            iTableCellIterator iterator = cellCollection.CreateIterator();

            // Start building the LaTeX table
            string latexTable = "\\begin{tabular}{|c|c|}\n"; // Adjust columns as needed

            while (!iterator.IsDone())
            {
                TableCell cell = iterator.Current(); // Get current cell
                string cellContent = cell.GetContent();
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
                // string latexCell = await _latexConverter.ConvertCellToLatexAsync(cell); // Convert cell to LaTeX
                latexTable += latexCell + " \\\\\n"; // Add the LaTeX cell to the table with a line break
                iterator.Next(); // Advance the iterator
            }

            // Close the LaTeX table
            latexTable += "\\end{tabular}";
            return latexTable;

            // // Simulate the conversion process - MOCK DATA
            // var latex = new List<string>();
            // var table = new Table(new List<List<string>>
            // {
            //     new() { "Hi", "I", "Am" },
            //     new() { "Going", "To", "Remod" }
            // });

            // // Start the LaTeX table syntax
            // latex.Add("\\begin{tabular}{|" + new string('c', table.Data[0].Length) + "|}");

            // // Convert each row into LaTeX syntax
            // foreach (var row in table.Data)
            // {
            //     string rowLatex = "\n\\hline ";
            //     foreach (var cell in row)
            //     {
            //         rowLatex += cell + " & ";
            //     }
            //     rowLatex = rowLatex.TrimEnd(new char[] { '&', ' ' }) + "\\\\ \\hline"; // Clean up row end
            //     latex.Add(rowLatex);
            // }

            // latex.Add("\\end{tabular}");
            // return latex;
        }

        public async Task<bool> UpdateLatexCheckpointAsync(int nodeId, string latexOutput)
        {
            await Task.Delay(500);  // Simulating a time-consuming operation (e.g., database or file I/O)
            Console.WriteLine($"Updated LaTeX checkpoint for Node {nodeId}.");
            return true;
        }
    }

}