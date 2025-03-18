// Program.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod1grp4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Step 1: Organize tables
            var tableOrganiser = new TableOrganiserManager();
            List<Table_SDM> tables = tableOrganiser.OrganiseTables(GetSampleTables());

            // Step 2: Extract table structure
            var tableStructureManager = new TableStructureManager();
            foreach (var table in tables)
            {
                tableStructureManager.ExtractTableStructure(table);
            }

            // Step 3: Convert tables to LaTeX
            var latexConverter = new MockLatexConverter();
            var latexConversionManager = new TableLatexConversionManager(latexConverter);

            foreach (var table in tables)
            {
                // Step 3a: Convert table to LaTeX
                string latexOutput = await latexConversionManager.ConvertToLatexAsync(table);

                // Step 3b: Update LaTeX checkpoint
                await latexConversionManager.UpdateLatexCheckpointAsync(table.TableId, latexOutput);

                Console.WriteLine($"LaTeX output for Table {table.TableId}:\n{latexOutput}\n");
            }

            // Step 4: Post-processing (e.g., prepare LaTeX output to pass to node)
            Console.WriteLine("Post-processing completed. LaTeX output ready for node.");
        }

        // Helper method to generate sample tables
        private static string GetSampleTables()
        {
            return @"
    {
        ""document"": [
            {
                ""type"": ""table"",
                ""content"": [
                    [""Table1"", ""i"", ""Am""],
                    [""Going"", ""To"", ""Remod""]
                ]
            },
            {
                ""type"": ""table"",
                ""content"": [
                    [""Table2"", ""i"", ""Am""],
                    [""Going"", ""To"", ""Remod""]
                ]
            },
            {
                ""type"": ""table"",
                ""content"": [
                    [""Table3"", ""i"", ""Am""],
                    [""Going"", ""To"", ""Remod""]
                ]
            },
            {
                ""type"": ""table"",
                ""content"": [
                    [""Table4"", ""i"", ""Am""],
                    [""Going"", ""To"", ""Remod""]
                ]
            }
        ]
    }";
        }

    }
}