using ICT2106WebApp.Class;
using ICT2106WebApp.Controllers;
using ICT2106WebApp.Data;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.DataSource;
using ICT2106WebApp.Domain;
using ICT2106WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IRetrieveLog, LoggerGateway_TDG>();
builder.Services.AddScoped<ICT2106WebApp.Interfaces.ILogger, LoggerGateway_TDG>(); // Ensure ILogger is also registered
builder.Services.AddScoped<LoggerControl>();
builder.Services.AddScoped<Dashboard_InputController>();
builder.Services.AddScoped<Dashboard_PageController>();
// builder.Services.AddScoped<LoggerControl>();

// Register IParser and its implementation
builder.Services.AddScoped<IDocument, DocumentParserService>(); // Register the parser service

// Register PDF Quality Checker dependencies
builder.Services.AddSingleton<IPDFQualityChecker>(provider => QualityCheckerFactory.CreateDefaultChecker());
builder.Services.AddSingleton<IPDFProvider, MockPDFProvider>(); // Change to FilePDFProvider if needed
builder.Services.AddSingleton<PDFQualityRepository>();

// Add services to the container.
builder.Services.AddControllers(); // <-- Ensure this line is here
builder.Services.AddRazorPages();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<iConversionStatus, LatexCompiler>();
builder.Services.AddSingleton<iGetGeneratedLatex, LatexGenerator>();
builder.Services.AddSingleton<iErrorAnalyser, ErrorAnalyser>();
builder.Services.AddSingleton<iErrorPresenter, ErrorPresenter>();
builder.Services.AddSingleton<ErrorCheckingFacade>();
builder.Services.AddSingleton<PDFGenerator>();
builder.Services.AddSingleton<EditorDoc>();
builder.Services.AddSingleton<LatexGenerator>();
builder.Services.AddSingleton<IScannerFactory, CitationScannerFactory>();
builder.Services.AddSingleton<IScannerFactory, BibliographyScannerFactory>();
builder.Services.AddSingleton<BibTeXConverter>();
builder.Services.AddSingleton<LatexCompiler>();
builder.Services.AddSingleton<EditorDocumentMapper>();
builder.Services.AddSingleton<BibTexMapper>();
builder.Services.AddSingleton<IInsertBibTex, BibTexMapper>();
builder.Services.AddSingleton<iErrorAnalyser, ErrorAnalyser>();
builder.Services.AddSingleton<iErrorPresenter>(provider =>
{
    var errorAnalyser = provider.GetRequiredService<iErrorAnalyser>();
    return new ErrorPresenter(errorAnalyser);
});
builder.Services.AddSingleton<ErrorCheckingFacade>(provider =>
{
    var errorAnalyser = provider.GetRequiredService<iErrorAnalyser>();
    var errorPresenter = provider.GetRequiredService<iErrorPresenter>();
    return new ErrorCheckingFacade(errorAnalyser, errorPresenter);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Dashboard");  // Redirect to Dashboard page
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Ensure Controllers work properly
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/pdf"
});

app.MapControllers();

app.MapRazorPages(); // Keep this if you're using Razor Pages

app.Run();