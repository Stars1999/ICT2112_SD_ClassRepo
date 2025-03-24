using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

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

      // Temporary JSON file for testing abstract node
      string jsonFilePath = "./dummyTableNode.json";
      string jsonData = await File.ReadAllTextAsync(jsonFilePath);
      var abstractNodes = JsonSerializer.Deserialize<List<TableAbstractNode>>(jsonData);
      
      // Step 1: Organize tables
      var tableOrganiser = new TableOrganiserManager();
      List<Table> tablesFromNode = tableOrganiser.organiseTables(abstractNodes);

      // // Step 2: Preprocess tables (setup observer, recover backup tables if exist, fix table integrity)
      var rowTabularGateway_RDG = new RowTabularGateway_RDG(database);
      var tablePreprocessingManager = new TablePreprocessingManager();
      tablePreprocessingManager.attach(rowTabularGateway_RDG);
      var tables = await tablePreprocessingManager.recoverBackupTablesIfExist(tablesFromNode);
      List<Table> cleanedTables = await tablePreprocessingManager.fixTableIntegrity(tables);

      // Step 3: Convert tables to LaTeX
      var latexConversionManager = new TableLatexConversionManager();
      latexConversionManager.attach(rowTabularGateway_RDG);
      List<Table> processedTables = await latexConversionManager.ConvertToLatexAsync(cleanedTables);


      // // Step 4: Post-processing (e.g., prepare LaTeX output to pass to node, validation of latex)
      var tableValidationManager = new TableValidationManager();
      // var validationResults = tableValidationManager.validateTableLatexOutput(processedTables);

      var processedTableManager = new ProcessedTableManager();
      processedTableManager.attach(rowTabularGateway_RDG);
      // processedTableManager.logProcessingStatus(validationResults);
      await processedTableManager.slotProcessedTableToTree(cleanedTables);
    }
  }
}