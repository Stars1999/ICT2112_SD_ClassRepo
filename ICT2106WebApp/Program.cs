using ICT2106WebApp.Controllers;
using ICT2106WebApp.Data;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Domain;
using ICT2106WebApp.Services;
using ICT2106WebApp.Pages;
using System.IO;
using ICT2106WebApp.Control;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.Configure<IISServerOptions>(options =>
// {
//     options.MaxRequestBodySize = 30000000; // Allow up to 30 MB
// });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IRetrieveLog, LoggerGateway_TDG>();
builder.Services.AddScoped<ICT2106WebApp.Interfaces.ILogger, LoggerGateway_TDG>(); // Ensure ILogger is also registered
builder.Services.AddScoped<LoggerControl>();
builder.Services.AddScoped<Dashboard_InputController>();
builder.Services.AddScoped<Dashboard_PageController>();
builder.Services.AddScoped<ITaskScheduling, TaskSchedulerController>();
// builder.Services.AddScoped<LoggerControl>();

// Add IDocumentTestCase
builder.Services.AddScoped<IDocumentTestCase, TestCaseControl>();

// Register IParser and its implementation
builder.Services.AddScoped<IDocument, DocumentParserService>(); // Register the parser service

// Register PDF Quality Checker dependencies
builder.Services.AddSingleton<IPDFQualityChecker, PDFCheckerControl>();
builder.Services.AddSingleton<IPDFProvider>(provider => 
    new QualityCheckerModel.GeneratedPDFProvider()); // Register our inline PDF provider

builder.Services.AddRazorPages();  // Add Razor Pages support
builder.Services.AddControllers(); // Add Controllers support

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

app.MapRazorPages();  // Maps Razor Pages
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers(); 

app.Run();
