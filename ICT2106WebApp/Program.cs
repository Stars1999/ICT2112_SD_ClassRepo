using ICT2106WebApp.Class;
using ICT2106WebApp.Control;
using ICT2106WebApp.Controllers;
using ICT2106WebApp.Data;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.DataSource;
using ICT2106WebApp.Domain;

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
builder.Services.AddScoped<DashboardController>();
// builder.Services.AddScoped<LoggerControl>();

// Register PDF Quality Checker dependencies
builder.Services.AddSingleton<IPDFQualityChecker>(provider => QualityCheckerFactory.CreateDefaultChecker());
builder.Services.AddSingleton<IPDFProvider, MockPDFProvider>(); // Change to FilePDFProvider if needed
builder.Services.AddSingleton<PDFQualityRepository>();

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


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers(); // ✅ Ensures API Controllers work

app.Run();
