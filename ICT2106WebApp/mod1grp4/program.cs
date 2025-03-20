using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ICT2106WebApp.mod1grp4
{
  class Program
  {
    static async Task Main(string[] args)
    {
      // MongoDB Setup
      var builder = WebApplication.CreateBuilder(args);
      builder.Services.AddRazorPages();
      builder.Services.AddLogging();
      builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
      builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
      {
        var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
        return mongoClient;
      });
      builder.Services.AddSingleton(serviceProvider =>
      {
        var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
        return mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
      });

      var serviceProvider = builder.Services.BuildServiceProvider();
      var database = serviceProvider.GetRequiredService<IMongoDatabase>();

      // public TableCell(string content, List<string> contentStyle, int rowSpan, int colSpan, List<string> cellStyle)
      // public Table(int tableId, int rows, int columns, List<TableCell> cells, List<string> style, int lastProcessedNode, bool tableCompletionState, Table table, string latexOutput)
      // mockTable DATA
      var cells = new List<TableCell>
            {
                new TableCell("Cell1 - Bold", new List<string> { "bold", "fontsize:17" }, 1, 1, new List<string> { "border" }),
                new TableCell("Cell2 - italic", new List<string> { "italic" }, 1, 2, new List<string> { "border" }),
                new TableCell("Cell3 - underline", new List<string> { "underline" }, 1, 3, new List<string> { "border" }),
                new TableCell("Cell4 - Bold", new List<string> { "bold","underline","alignment:left" }, 2, 1, new List<string> { "border" }),
                new TableCell("Cell5 - italic", new List<string> { "italic","bold" }, 2, 2, new List<string> { "border" }),
                new TableCell("Cell6 - underline", new List<string> { "underline","italic" }, 2, 3, new List<string> { "border" }),
                new TableCell("Cell7 - Bold", new List<string> { "bold", "italic","alignment:right" }, 3, 1, new List<string> { "border" }),
                new TableCell("Cell8 - italic", new List<string> { "italic","underline" }, 3, 2, new List<string> { "border" }),
                new TableCell("Cell9 - underline", new List<string> { "underline","bold" }, 3, 3, new List<string> { "border" })
            };

      var mockTable = new Table(
          tableId: 1,
          rows: 2,
          columns: 3,
          cells: cells,
          style: new List<string> { "tableStyle" },
          lastProcessedNode: 0,
          tableCompletionState: true,
          table: null,
          latexOutput: ""
      );


      // var app = builder.Build();

      // // Configure the HTTP request pipeline.
      // if (!app.Environment.IsDevelopment())
      // {
      //     app.UseExceptionHandler("/Error");
      //     app.UseHsts();
      // }

      // app.UseHttpsRedirection();
      // app.UseStaticFiles();

      // app.UseRouting();

      // app.UseAuthorization();

      // app.MapRazorPages();

      // Step 1: Organize tables
      // var tableOrganiser = new TableOrganiserManager();
      // List<Table> tables = tableOrganiser.OrganiseTables(GetSampleTables());

      // // Step 2: Preprocess tables
      // mocktable
      // var tablePreprocessingManager = new TablePreprocessingManager();
      // var mockTables = new List<Table>
      // {
      //   new Table(
      //     tableId: 1,
      //     rows: 2,
      //     columns: 3,
      //     cells: cells,
      //     style: new List<string> { "tableStyle" },
      //     lastProcessedNode: 0,
      //     tableCompletionState: true,
      //     table: null,
      //     latexOutput: ""
      //   ),
      //   new Table(
      //     tableId: 2,
      //     rows: 2,
      //     columns: 3,
      //     cells: cells,
      //     style: new List<string> { "tableStyle" },
      //     lastProcessedNode: 0,
      //     tableCompletionState: true,
      //     table: null,
      //     latexOutput: ""
      //   ),
      //   new Table(
      //     tableId: 3,
      //     rows: 2,
      //     columns: 3,
      //     cells: cells,
      //     style: new List<string> { "tableStyle" },
      //     lastProcessedNode: 0,
      //     tableCompletionState: true,
      //     table: null,
      //     latexOutput: ""
      //   ),
      //   new Table(
      //     tableId: 4,
      //     rows: 2,
      //     columns: 3,
      //     cells: cells,
      //     style: new List<string> { "tableStyle" },
      //     lastProcessedNode: 0,
      //     tableCompletionState: true,
      //     table: null,
      //     latexOutput: ""
      //   )
      // };

      // CRASH RECOVERY
      // RowTabularGateway_RDG rowTabularGateway_RDG = new RowTabularGateway_RDG(database);
      // TablePreprocessingManager tablePreprocessingManager = new TablePreprocessingManager();
      // tablePreprocessingManager.attach(rowTabularGateway_RDG);
      // tables = await tablePreprocessingManager.recoverBackupTablesIfExist(mockTables);
      // List<Table> cleanedTables = await tablePreprocessingManager.fixTableIntegrity(tables);

      // // Step 3: Convert tables to LaTeX
      // var latexConverter = new MockLatexConverter();
      // var latexConversionManager = new TableLatexConversionManager(latexConverter);

      // foreach (var table in tables)
      // {
      //     // Step 3a: Convert table to LaTeX
      //     string latexOutput = await latexConversionManager.ConvertToLatexAsync(table);

      //     // Step 3b: Update LaTeX checkpoint
      //     await latexConversionManager.UpdateLatexCheckpointAsync(table.TableId, latexOutput);

      //     Console.WriteLine($"LaTeX output for Table {table.TableId}:\n{latexOutput}\n");
      // }



      var latexConversionManager = new TableLatexConversionManager();
      latexConversionManager.attach(new RowTabularGateway_RDG(database));
      string latexOutput = await latexConversionManager.ConvertToLatexAsync(mockTable);
      Console.WriteLine("LaTeX Output:");
      Console.WriteLine(latexOutput);
      // // Step 4: Post-processing (e.g., prepare LaTeX output to pass to node)
      Console.WriteLine("Post-processing completed. LaTeX output ready for node.");

      // Console.WriteLine(GetMockJSONTables());

      // app.Run();
    }

    // Helper method to generate sample tables
    private static string GetMockJSONTables()
    {
      return @"
    {
        ""document"": [
         {
      ""type"": ""Table"",
      ""content"": """",
      ""runs"": [
        {
                ""type"": ""Row"",
          ""content"": """",
          ""runs"": [
            {
                    ""type"": ""Cell"",
              ""content"": ""Hi"",
              ""styling"": {
                        ""bold"": false,
                ""italic"": false
              }
                },
            {
                    ""type"": ""Cell"",
              ""content"": ""i"",
              ""styling"": {
                        ""bold"": false,
                ""italic"": false
              }
                },
            {
                    ""type"": ""Cell"",
              ""content"": ""Am"",
              ""styling"": {
                        ""bold"": false,
                ""italic"": false
              }
                }
          ],
          ""styling"": {
                    ""bold"": false,
            ""italic"": true,
            ""alignment"": ""right"",
            ""fontsize"": 12,
            ""fonttype"": ""Aptos"",
            ""fontcolor"": ""0E2841"",
            ""highlight"": ""none""
          }
            },
        {
                ""type"": ""Row"",
          ""content"": "",
          ""runs"": [
            {
                    ""type"": ""Cell"",
              ""content"": ""Going"",
              ""styling"": {
                        ""bold"": false,
                ""italic"": false
              }
                },
            {
                    ""type"": ""Cell"",
              ""content"": ""To"",
              ""styling"": {
                        ""bold"": false,
                ""italic"": false
              }
                },
            {
                    ""type"": ""Cell"",
              ""content"": ""Remod"",
              ""styling"": {
                        ""bold"": false,
                ""italic"": false
              }
                }
          ],
          ""styling"": {
                    ""bold"": false,
            ""italic"": true,
            ""alignment"": ""right"",
            ""fontsize"": 12,
            ""fonttype"": ""Aptos"",
            ""fontcolor"": ""0E2841"",
            ""highlight"": ""none""
          }
            }
      ]
    }
        


        ]
    }";
    }

    // Method to create a 2x3 table with mock data
    private static Table CreateMockTable()
    {
      var cells = new List<TableCell>
            {
                new TableCell("Cell1", new List<string> { "bold" }, 1, 1, new List<string> { "border" }),
                new TableCell("Cell2", new List<string> { "italic" }, 1, 1, new List<string> { "border" }),
                new TableCell("Cell3", new List<string> { "underline" }, 1, 1, new List<string> { "border" }),
                new TableCell("Cell4", new List<string> { "bold" }, 1, 1, new List<string> { "border" }),
                new TableCell("Cell5", new List<string> { "italic" }, 1, 1, new List<string> { "border" }),
                new TableCell("Cell6", new List<string> { "underline" }, 1, 1, new List<string> { "border" })
            };

      return new Table(
          tableId: 1,
          rows: 2,
          columns: 3,
          cells: cells,
          style: new List<string> { "tableStyle" },
          lastProcessedNode: 0,
          tableCompletionState: true,
          table: null,
          latexOutput: string.Empty
      );
    }
  }
}


// {
//     ""type"": ""table"",
//     ""content"": [
//         [""Table1"", ""i"", ""Am""],
//         [""Going"", ""To"", ""Remod""]
//     ]
// },
// {
//     ""type"": ""table"",
//     ""content"": [
//         [""Table2"", ""i"", ""Am""],
//         [""Going"", ""To"", ""Remod""]
//     ]
// },
// {
//     ""type"": ""table"",
//     ""content"": [
//         [""Table3"", ""i"", ""Am""],
//         [""Going"", ""To"", ""Remod""]
//     ]
// },
// {
//     ""type"": ""table"",
//     ""content"": [
//         [""Table4"", ""i"", ""Am""],
//         [""Going"", ""To"", ""Remod""]
//     ]
// }