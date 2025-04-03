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


    // GROUP 4 STUFF (look at integrated if you want to compile)
    // Step 1: Get abstract nodes of table from group 3
    INodeTraverser traverser = new NodeTraverser(rootnodehere);
    List<AbstractNode> tableAbstractNodes = traverser.TraverseNode("tables");

    // Step 2: Convert abstract node to custom table entity (JOEL)
    var tableOrganiser = new TableOrganiserManager();
    List<ICT2106WebApp.mod1grp4.Table> tablesFromNode = tableOrganiser.organiseTables(
        tableAbstractNodes
    );

    // Step 3: Preprocess tables (setup observer, recover backup tables if exist, fix table integrity) (JOEL)
    var rowTabularGateway_RDG = new RowTabularGateway_RDG(database);
    var tablePreprocessingManager = new TablePreprocessingManager();
    tablePreprocessingManager.attach(rowTabularGateway_RDG);
    var tables = await tablePreprocessingManager.recoverBackupTablesIfExist(tablesFromNode);
    List<ICT2106WebApp.mod1grp4.Table> cleanedTables =
        await tablePreprocessingManager.fixTableIntegrity(tables);

    // Step 4: Convert tables to LaTeX (ANDREA)
    var latexConversionManager = new TableLatexConversionManager();
    latexConversionManager.attach(rowTabularGateway_RDG);

    // NORMAL FLOW (this will prove for Andrea where she inserts the content to overleaf and jonathan for styling of table)
    List<ICT2106WebApp.mod1grp4.Table> processedTables =
        await latexConversionManager.convertToLatexAsync(cleanedTables);

    // JOEL CRASH RECOVERY FLOW (we will convert 2 tables then stop the program, this will prove for Joel run crash flow first then normal again)
    // List<ICT2106WebApp.mod1grp4.Table> processedTables = await latexConversionManager.convertToLatexWithLimitAsync(cleanedTables, 2);
    // Environment.Exit(0);

    // HIEW TENG VALIDATION CHECK FLOW (we will omit out some stuff in the latex conversion, will prove for hiew teng where validation is wrong)
    // List<ICT2106WebApp.mod1grp4.Table> processedTables = await latexConversionManager.convertToLatexStyleFailAsync(cleanedTables);

    // Step 5: Post-processing (validation of latex, logging of validation status, convert processed tables to nodes to send over) (HIEW TENG AND SITI)
    var tableValidationManager = new TableValidationManager();
    var validationStatus = tableValidationManager.validateTableLatexOutput(
        tableAbstractNodes,
        processedTables
    );

    var processedTableManager = new ProcessedTableManager();
    processedTableManager.attach(rowTabularGateway_RDG);
    processedTableManager.logProcessingStatus(validationStatus);
    await processedTableManager.slotProcessedTableToTree(cleanedTables, tableAbstractNodes);

    // Will prove for Siti as we traverse the nodes again after updating
    List<AbstractNode> endingTableAbstractNodes = traverser.TraverseNode("tables");
    }
  }
}