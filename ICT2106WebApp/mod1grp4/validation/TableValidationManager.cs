using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ICT2106WebApp.mod1grp4
{
    public class TableValidationManager : iTableValidate
    {
        public bool validateTableLatexOutput(List<TableAbstractNode> originalNodes, List<Table> processedTables)
        {
            foreach (var table in processedTables)
            {
                var originalNode = originalNodes.Find(node => int.Parse(node.nodeID) == table.tableId);
                if (originalNode == null)
                {
                    Console.WriteLine($"Table ID {table.tableId} not found in original nodes.");
                    return false; // Table ID not found in original nodes
                }

                // Extract content and styling from LaTeX output
                var latexContentMatches = Regex.Matches(table.latexOutput, @"\\cellcontent\{(.*?)\}");
                var latexStylingMatches = Regex.Matches(table.latexOutput, @"\\cellstyling\{(.*?)\}");

                var latexContents = new HashSet<string>();
                var latexStylings = new HashSet<string>();

                foreach (Match match in latexContentMatches)
                {
                    latexContents.Add(match.Groups[1].Value.Trim().ToLowerInvariant());
                }

                foreach (Match match in latexStylingMatches)
                {
                    latexStylings.Add(match.Groups[1].Value.Trim().ToLowerInvariant());
                }

                // Print the contents of the sets (for debugging atm)
                Console.WriteLine("Latex Contents:");
                foreach (var content in latexContents)
                {
                    Console.WriteLine(content);
                }

                Console.WriteLine("Latex Stylings:");
                foreach (var styling in latexStylings)
                {
                    Console.WriteLine(styling);
                }

                foreach (var rowNode in originalNode.runs)
                {
                    foreach (var cellNode in rowNode.runs)
                    {
                        if (!latexContents.Contains(cellNode.content.Trim().ToLowerInvariant()))
                        {
                            Console.WriteLine($"Cell content '{cellNode.content}' not found in LaTeX output.");
                            return false; // Cell content not found in LaTeX output
                        }

                        var cellStylingJson = JsonSerializer.Serialize(cellNode.styling);
                        if (!latexStylings.Contains(cellStylingJson.Trim().ToLowerInvariant()))
                        {
                            Console.WriteLine($"Cell styling '{cellStylingJson}' not found in LaTeX output.");
                            return false; // Cell styling not found in LaTeX output
                        }
                    }
                }

                if (!table.latexOutput.Contains("\\begin{tabular}") || !table.latexOutput.Contains("\\end{tabular}"))
                {
                    Console.WriteLine("LaTeX output does not contain expected table structure.");
                    return false; // LaTeX output does not contain expected table structure
                }
            }
            Console.WriteLine("Validation passed.");
            return true;
        }
    }
}