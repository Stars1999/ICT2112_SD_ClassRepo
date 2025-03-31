using System.Text.RegularExpressions;
using Utilities;

namespace ICT2106WebApp.mod1grp4
{
    public class TableValidationManager : iTableValidate
    {
        // Validate the content and style of the original nodes given and the processed tables (Hiew Teng - COMPLETED)
        public string validateTableLatexOutput(List<AbstractNode> originalNodes, List<Table> processedTables)
        {
            var currentTableIndex = 0;

            foreach (var tableNode in originalNodes)
            {
                if (tableNode is CompositeNode tableCompositeNode && tableCompositeNode.GetNodeType() == "table")
                {
                    var processedTable = processedTables[currentTableIndex];
                    var latexOutput = processedTable.latexOutput;

                    // Step 1: Verify the LaTeX output starts with "\begin{tabular}"
                    if (!latexOutput.StartsWith("\\begin{tabular}"))
                    {
                        return $"LaTeX output does not start with \\begin{{tabular}} for table {tableCompositeNode.GetNodeId()}.";
                    }

                    // Step 2: Verify the LaTeX output ends with "\end{tabular}"
                    if (!latexOutput.EndsWith("\\end{tabular}"))
                    {
                        return $"LaTeX output does not end with \\end{{tabular}} for table {tableCompositeNode.GetNodeId()}.";
                    }

                    // Step 3: Verify the number of \hlines matches the number of rows + 1
                    var numberOfRows = tableCompositeNode.GetChildren().Count;
                    var hlineCount = Regex.Matches(latexOutput, @"\\hline").Count;

                    if (hlineCount != (numberOfRows + 1))
                    {
                        return $"Mismatch in \\hline count. Expected {numberOfRows + 1}, found {hlineCount} for table {tableCompositeNode.GetNodeId()}.";
                    }

                    // Step 4: Verify the number of cells in each row
                    var latexCellCount = Regex.Matches(latexOutput, @"&").Count;
                    var totalCellCount = 0;

                    foreach (var rowNode in tableCompositeNode.GetChildren())
                    {
                        if (rowNode is CompositeNode rowCompositeNode && rowCompositeNode.GetNodeType() == "row")
                        {
                            totalCellCount += rowCompositeNode.GetChildren().Count;
                        }
                    }

                    if (latexCellCount != totalCellCount)
                    {
                        return $"Mismatch in cell count. Expected {totalCellCount}, found {latexCellCount} for table {tableCompositeNode.GetNodeId()}.";
                    }

                    // Step 5: Verify cell content and styling
                    foreach (var rowNode in tableCompositeNode.GetChildren())
                    {
                        if (rowNode is CompositeNode rowCompositeNode && rowCompositeNode.GetNodeType() == "row")
                        {
                            foreach (var cellNode in rowCompositeNode.GetChildren())
                            {
                                if (cellNode is AbstractNode cellAbstractNode && cellAbstractNode.GetNodeType() == "cell")
                                {
                                    var cellContent = cellAbstractNode.GetContent();
                                    if (!latexOutput.Contains(cellContent))
                                    {
                                        return $"Cell content '{cellContent}' not found in LaTeX output for table {tableCompositeNode.GetNodeId()}.";
                                    }

                                    // Verify styling
                                    var cellStyling = cellAbstractNode.GetStyling();
                                    if (cellStyling != null)
                                    {
                                        if (cellStyling.Any(dict => dict.TryGetValue("bold", out var isBold) && isBold is bool boldValue && boldValue))
                                        {
                                            if (!latexOutput.Contains($"\\textbf{{{cellContent}}}"))
                                            {
                                                return $"Bold styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeId()}.";
                                            }
                                        }

                                        if (cellStyling.Any(dict => dict.TryGetValue("italic", out var isItalic) && isItalic is bool italicValue && italicValue))
                                        {
                                            if (!latexOutput.Contains($"\\textit{{{cellContent}}}"))
                                            {
                                                return $"Italic styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeId()}.";
                                            }
                                        }

                                        if (cellStyling.Any(dict => dict.TryGetValue("underline", out var isUnderline) && isUnderline is bool underlineValue && underlineValue))
                                        {
                                            if (!latexOutput.Contains($"\\underline{{{cellContent}}}"))
                                            {
                                                return $"Underline styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeId()}.";
                                            }
                                        }

                                        if (cellStyling.Any(dict => dict.TryGetValue("fontsize", out var fontSizeValue) && fontSizeValue is int fontSize && fontSize != 0))
                                        {
                                            if (!latexOutput.Contains($"\\fontsize{{{cellContent}}}"))
                                            {
                                                return $"Font size styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeId()}.";
                                            }
                                        }

                                        var alignmentKey = cellStyling
                                            .SelectMany(dict => dict)
                                            .Where(kv => kv.Key == "horizontalalignment" && kv.Value is string)
                                            .Select(kv => kv.Value as string)
                                            .FirstOrDefault();

                                        if (alignmentKey != null)
                                        {
                                            var alignment = alignmentKey switch
                                            {
                                                "center" => "\\multicolumn{1}{|c|}",
                                                "left" => "\\multicolumn{1}{|l|}",
                                                "right" => "\\multicolumn{1}{|r|}",
                                                _ => ""
                                            };

                                            if (!latexOutput.Contains(alignment))
                                            {
                                                return $"Alignment styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeId()}.";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    currentTableIndex++;
                }
            }

            return "Validation passed for all tables. Sending status for logging.";
        }
    }
}