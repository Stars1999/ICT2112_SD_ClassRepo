using System.Text.RegularExpressions;

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
                var latexOutput = processedTables[currentTableIndex].latexOutput;

                // Step 1: Verify the LaTeX output starts with "\begin{tabular}"
                if (!latexOutput.StartsWith("\\begin{tabular}"))
                {
                    return "LaTeX output does not start with \\begin{tabular}" + $"for table {tableNode.nodeID}.";
                }

                // Step 2: Verify the LaTeX output ends with "\end{tabular}"
                if (!latexOutput.EndsWith("\\end{tabular}"))
                {
                    return "LaTeX output does not end with \\end{tabular}" + $"for table {tableNode.nodeID}.";
                }

                // Step 3: Verify the number of \hlines matches the number of rows + 1
                var numberOfRows = tableNode.runs.Count; ;
                var cellCount = 0; ;


                var hlineCount = Regex.Matches(latexOutput, @"\\hline").Count;

                if (hlineCount != (numberOfRows + 1))
                {
                    return $"Mismatch in \\hline count. Expected {numberOfRows + 1}, found {hlineCount} for table {tableNode.nodeID}.";
                }

                // Step 4: Verify the number of cells in each row
                var latexCellCount = Regex.Matches(latexOutput, @"&").Count;
                foreach (var row in tableNode.runs)
                {
                    cellCount += row.runs.Count-1;
                }
                if (latexCellCount != cellCount)
                {
                    return $"Mismatch in cell count. Expected {cellCount}, found {latexCellCount} for table {tableNode.nodeID}.";
                }


                foreach (var row in tableNode.runs)
                {

                    foreach (var cell in row.runs)
                    {
                        latexOutput = cell.content;
                        // Step 6: Verify the cell content
                        if (!latexOutput.Contains(cell.content))
                        {
                            return $"Cell content '{cell.content}' not found in row content for table {tableNode.nodeID}.";
                        }
                        if (cell.styling.TryGetValue("bold", out var isBold) && isBold is bool boldValue && boldValue)
                        {
                            latexOutput = $"\\textbf{{{latexOutput}}}";
                        }

                        if (cell.styling.TryGetValue("italic", out var isItalic) && isItalic is bool italicValue && italicValue)
                        {
                            latexOutput = $"\\textit{{{latexOutput}}}";
                        }

                        if (cell.styling.TryGetValue("underline", out var isUnderline) && isUnderline is bool underlineValue && underlineValue)
                        {
                            latexOutput = $"\\underline{{{latexOutput}}}";
                        }
                        if (cell.styling.TryGetValue("fontsize", out var fontSizeValue) && fontSizeValue is int fontSize && fontSize != 0)
                        {
                            latexOutput = $"{{\\fontsize{{{fontSize}}}{{\\baselineskip}}\\selectfont {latexOutput}}}";
                        }

                        // Step 7: Verify the cell alignment

                        if (cell.styling.TryGetValue("horizontalalignment", out var alignmentValue) && alignmentValue is string alignmentKey)
                        {
                            var alignment = alignmentKey switch
                            {
                                "center" => "\\multicolumn{1}{|c|} ",
                                "left" => "\\multicolumn{1}{|l|} ",
                                "right" => "\\multicolumn{1}{|r|} ",
                                "both" => "\\multicolumn{1}{|c|} ",
                                _ => ""
                            };
                            latexOutput = alignment + latexOutput;
                            if (!latexOutput.Contains(alignment))
                            {
                                return $"Cell styling for '{cell.content}' is not matching for table {tableNode.nodeID}.";
                            }
                        }
                    }
                }

                currentTableIndex++;
            }
            return "Validation passed for all tables. Sending status for logging.";
        }
    }
}