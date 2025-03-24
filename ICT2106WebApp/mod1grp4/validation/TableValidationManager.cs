// using System.Collections.Generic;
// using System.Text.Json;
// using System.Text.RegularExpressions;

// namespace ICT2106WebApp.mod1grp4
// {
//     public class TableValidationManager : iTableValidate
//     {
//         public bool validateTableLatexOutput(List<TableAbstractNode> originalNodes, List<Table> processedTables)
//         {
//             foreach (var table in processedTables)
//             {
//                 var latexOutput = table.latexOutput;

//                 // Step 1: Verify the LaTeX output starts with "\begin{tabular}"
//                 if (!latexOutput.StartsWith("\\begin{tabular}"))
//                 {
//                     Console.WriteLine("LaTeX output does not start with \\begin{tabular}.");
//                     return false;
//                 }

//                 // Step 2: Verify the LaTeX output ends with "\end{tabular}"
//                 if (!latexOutput.EndsWith("\\end{tabular}"))
//                 {
//                     Console.WriteLine("LaTeX output does not end with \\end{tabular}.");
//                     return false;
//                 }

//                 // Step 3: Verify the number of \hlines matches the number of rows + 1
//                 var numberOfRows = table.rows.Count;
//                 var hlineCount = Regex.Matches(latexOutput, @"\\hline").Count;

//                 if (hlineCount != (numberOfRows + 1))
//                 {
//                     Console.WriteLine($"Mismatch in \\hline count. Expected {numberOfRows + 1}, found {hlineCount}.");
//                     return false;
//                 }

//                 // Step 4: Verify the number of cells in each row
//                 var latexCellCount = Regex.Matches(latexOutput, @"&").Count;
//                 var cellCount = 0;
//                 foreach (var row in table.rows)
//                 {
//                     cellCount += row.cells.Count - 1;
//                 }
//                 if (latexCellCount != cellCount)
//                 {
//                     Console.WriteLine($"Mismatch in cell count. Expected {cellCount}, found {latexCellCount}.");
//                     return false;
//                 }


//                 foreach (var row in table.rows)
//                 {

//                     foreach (var cell in row.cells)
//                     {
//                         latexOutput = cell.content;
//                         // Step 6: Verify the cell content
//                         if (!latexOutput.Contains(cell.content))
//                         {
//                             Console.WriteLine($"Cell content '{cell.content}' not found in row content.");
//                             return false;
//                         }

//                         if (cell.styling.bold)
//                         {
//                             latexOutput = $"\\textbf{{{latexOutput}}}";
//                         }
//                         if (cell.styling.italic)
//                         {
//                             latexOutput = $"\\textit{{{latexOutput}}}";
//                         }
//                         if (cell.styling.italic)
//                         {
//                             latexOutput = $"\\underline{{{latexOutput}}}";
//                         }
//                         if (cell.styling.fontsize != 0)
//                         {
//                             latexOutput = $"{{\\fontsize{{{cell.styling.fontsize}}}{{\\baselineskip}}\\selectfont {latexOutput}}}";
//                         }


//                         // Step 7: Verify the cell alignment
//                         var alignment = cell.styling.horizontalalignment switch
//                         {
//                             "center" => "\\multicolumn{1}{|c|} ",
//                             "left" => "\\multicolumn{1}{|l|} ",
//                             "right" => "\\multicolumn{1}{|r|} ",
//                             "both" => "\\multicolumn{1}{|c|} ",
//                             _ => ""
//                         };

//                         latexOutput = alignment + latexOutput;
//                         if (!latexOutput.Contains(alignment))
//                         {
//                             Console.WriteLine($"Cell styling for '{cell.content}' is not matching.");
//                             return false;
//                         }
//                     }
//                 }

//                 Console.WriteLine($"Validation passed for table {table.tableId}.");


//                 // {
//                 //     var rowContent = rowMatch.Groups[1].Value;
//                 //     var cellMatches = Regex.Matches(rowContent, @"(.*?)&|([^&]+)(?=\\\\|$)");
//                 //     foreach (Match cellMatch in cellMatches)
//                 //     {
//                 //         var cellContent = cellMatch.Groups[1].Value.Trim();

//                 //         // Step 6: Check if the cell content contains \textbf or \textit
//                 //         if (!cellContent.Contains("\\textbf") && !cellContent.Contains("\\textit"))
//                 //         {
//                 //             Console.WriteLine($"Cell content '{cellContent}' does not contain \\textbf or \\textit.");
//                 //             return false;
//                 //         }
//                 //     }
//                 // }
//                 // // 2nd validation code=====================================================================================

//                 // // Step 1: Verify the LaTeX output starts with "\begin{tabular}"
//                 // if (!latexOutput.StartsWith("\\begin{tabular}"))
//                 // {
//                 //     Console.WriteLine("LaTeX output does not start with \\begin{tabular}.");
//                 //     return false;
//                 // }

//                 // // Step 2: Verify the table's first row's individual cell alignment - Extracts alignment string
//                 // var alignmentMatch = Regex.Match(latexOutput, @"\\begin\{tabular\}\{(.*?)\}");
//                 // if (!alignmentMatch.Success)
//                 // {
//                 //     Console.WriteLine("Table alignment not found in LaTeX output.");
//                 //     return false;
//                 // }

//                 // var alignment = alignmentMatch.Groups[1].Value;
//                 // // int columnCount = alignment.Split('c').Length - 1; // Count the number of 'c' in the alignment
//                 // int columnCount = Regex.Matches(alignment, "[clr]").Count;
//                 // // string expectedAlignment = string.Concat(Enumerable.Repeat("|c", columnCount)) + "|";
//                 // string expectedAlignment = "|";
//                 // foreach (var cell in table.rows[0].cells) // Assuming the first row defines the alignment
//                 // {
//                 //     switch (cell.alignment)
//                 //     {
//                 //         case "center":
//                 //             expectedAlignment += "c|";
//                 //             break;
//                 //         case "left":
//                 //             expectedAlignment += "l|";
//                 //             break;
//                 //         case "right":
//                 //             expectedAlignment += "r|";
//                 //             break;
//                 //         default:
//                 //             Console.WriteLine($"Unknown alignment '{cell.alignment}' in TableCell.");
//                 //             return false;
//                 //     }
//                 // }

//                 // if (alignment != expectedAlignment)
//                 // {
//                 //     Console.WriteLine($"Table alignment does not match expected format {expectedAlignment}.");
//                 //     return false;
//                 // }

//                 // // Step 3: Loop through table rows
//                 // var rowMatches = Regex.Matches(latexOutput, @"\\hline\s*(.*?)\s*(?=\\hline|\\end\{tabular\})");
//                 // foreach (Match rowMatch in rowMatches)
//                 // {
//                 //     var rowContent = rowMatch.Groups[1].Value;

//                 //     // Step 4: Check that each row has \hline
//                 //     if (!rowContent.Contains("\\hline"))
//                 //     {
//                 //         Console.WriteLine("Row does not contain \\hline.");
//                 //         return false;
//                 //     }

//                 //     // Step 5: Loop through cells in the row
//                 //     var cellMatches = Regex.Matches(rowContent, @"\{(.*?)\}");
//                 //     foreach (Match cellMatch in cellMatches)
//                 //     {
//                 //         var cellContent = cellMatch.Groups[1].Value;

//                 //         // Step 6: Check for \textbf or \textit if bold or italic
//                 //         if (!cellContent.Contains("\\textbf") && !cellContent.Contains("\\textit"))
//                 //         {
//                 //             Console.WriteLine($"Cell content '{cellContent}' does not contain \\textbf or \\textit.");
//                 //             return false;
//                 //         }
//                 //     }
//                 // }

//                 // // Step 7: Verify closing table line and tag
//                 // if (!latexOutput.EndsWith("\\hline\n\\end{tabular}"))
//                 // {
//                 //     Console.WriteLine("LaTeX output does not end with \\hline and \\end{tabular}.");
//                 //     return false;
//                 // }




//                 // initial validation code=====================================================================================

//                 // var originalNode = originalNodes.Find(node => int.Parse(node.nodeID) == table.tableId);
//                 // if (originalNode == null)
//                 // {
//                 //     Console.WriteLine($"Table ID {table.tableId} not found in original nodes.");
//                 //     return false; // Table ID not found in original nodes
//                 // }

//                 // // Extract content and styling from LaTeX output
//                 // var latexContentMatches = Regex.Matches(table.latexOutput, @"\\cellcontent\{(.*?)\}");
//                 // var latexStylingMatches = Regex.Matches(table.latexOutput, @"\\cellstyling\{(.*?)\}");

//                 // var latexContents = new HashSet<string>();
//                 // var latexStylings = new HashSet<string>();

//                 // foreach (Match match in latexContentMatches)
//                 // {
//                 //     latexContents.Add(match.Groups[1].Value.Trim().ToLowerInvariant());
//                 // }

//                 // foreach (Match match in latexStylingMatches)
//                 // {
//                 //     latexStylings.Add(match.Groups[1].Value.Trim().ToLowerInvariant());
//                 // }

//                 // // Print the contents of the sets (for debugging atm)
//                 // Console.WriteLine("Latex Contents:");
//                 // foreach (var content in latexContents)
//                 // {
//                 //     Console.WriteLine(content);
//                 // }

//                 // Console.WriteLine("Latex Stylings:");
//                 // foreach (var styling in latexStylings)
//                 // {
//                 //     Console.WriteLine(styling);
//                 // }

//                 // foreach (var rowNode in originalNode.runs)
//                 // {
//                 //     foreach (var cellNode in rowNode.runs)
//                 //     {
//                 //         if (!latexContents.Contains(cellNode.content.Trim().ToLowerInvariant()))
//                 //         {
//                 //             Console.WriteLine($"Cell content '{cellNode.content}' not found in LaTeX output.");
//                 //             return false; // Cell content not found in LaTeX output
//                 //         }

//                 //         var cellStylingJson = JsonSerializer.Serialize(cellNode.styling);
//                 //         if (!latexStylings.Contains(cellStylingJson.Trim().ToLowerInvariant()))
//                 //         {
//                 //             Console.WriteLine($"Cell styling '{cellStylingJson}' not found in LaTeX output.");
//                 //             return false; // Cell styling not found in LaTeX output
//                 //         }
//                 //     }
//                 // }

//                 // if (!table.latexOutput.Contains("\\begin{tabular}") || !table.latexOutput.Contains("\\end{tabular}"))
//                 // {
//                 //     Console.WriteLine("LaTeX output does not contain expected table structure.");
//                 //     return false; // LaTeX output does not contain expected table structure
//                 // }
//             }
//             Console.WriteLine("Validation finished.");
//             return true;
//         }
//     }
// }