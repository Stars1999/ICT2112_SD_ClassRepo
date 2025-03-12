using ICT2106WebApp.Class;
using ICT2106WebApp.Control;
using ICT2106WebApp.Data;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.DataSource;
using ICT2106WebApp.Domain;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IRetrieveLog, LoggerGateway_TDG>();
builder.Services.AddScoped<ICT2106WebApp.Interfaces.ILogger, LoggerGateway_TDG>(); // Ensure ILogger is also registered
builder.Services.AddScoped<LoggerControl>();

builder.Services.AddScoped<LoggerControl>();

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
