var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

// ✅ Manually Test the Function Before Running API Server
var testProcessor = new MathProcessor();

// Test Math Conversion with Different Inputs
string[] testEquations = 
{
    "(1/2) √(4)",
};

Console.WriteLine("\n==== TESTING MATH CONVERSION ====");
foreach (var equation in testEquations)
{
    Console.WriteLine($"Input: {equation}");
    Console.WriteLine($"LaTeX Output: {testProcessor.ConvertContent(equation)[0]}\n");
}
Console.WriteLine("================================\n");

app.Run();
