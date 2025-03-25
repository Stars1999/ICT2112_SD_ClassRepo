using MongoDB.Driver;
using Microsoft.Extensions.Options;
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
      var abstractNodes = JsonSerializer.Deserialize<List<AbstractNode>>(jsonData);

      
      // Step 1: Convert abstract node to custom table entity
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
      List<Table> processedTables = await latexConversionManager.convertToLatexAsync(cleanedTables);

      // // Step 4: Post-processing (validation of latex, logging of validation status, convert processed tables to nodes to send over)
      var tableValidationManager = new TableValidationManager();
      var validationStatus = tableValidationManager.validateTableLatexOutput(abstractNodes, processedTables);

      var processedTableManager = new ProcessedTableManager();
      processedTableManager.attach(rowTabularGateway_RDG);
      processedTableManager.logProcessingStatus(validationStatus);
      await processedTableManager.slotProcessedTableToTree(cleanedTables);
    }
  }
}