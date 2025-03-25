var builder = WebApplication.CreateBuilder(args);

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

app.MapRazorPages(); // Keep this if you're using Razor Pages

app.Run();