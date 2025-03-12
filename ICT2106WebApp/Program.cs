using ICT2106WebApp.DataSource;
using ICT2106WebApp.Domain;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<LoggerService>(); // Register LoggerService

// Register PDF Quality Checker dependencies
builder.Services.AddSingleton<IPDFQualityChecker>(provider => QualityCheckerFactory.CreateDefaultChecker());
builder.Services.AddSingleton<IPDFProvider, MockPDFProvider>(); // Change to FilePDFProvider if needed
builder.Services.AddSingleton<PDFQualityRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
