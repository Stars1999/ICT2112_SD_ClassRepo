using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
var listProcessor = new ListProcessor();

// Test Math Conversion with Different Inputs
string[] testEquations =
{
    "(1/2) √(4)",
    //"(1/2) × √(4)=1",
    //"s=√(([i=1,N](x^2)/N-1))",    //doesnt work
    //"(1/2)cos2x+ (3/8)sin4x =3x",
    //"log_246y ≥ (π/2)",
    //"∴ ∞ ≠ ±α",
    //"∃xPersonx∧∀yTimey→Happyx,y", //doesnt work
    //"F=ma",
    //"limn→∞1+(1/n)^n=e",    //doesnt work

};

Console.WriteLine("\n==== TESTING MATH CONVERSION ====");
foreach (var equation in testEquations)
{
    Console.WriteLine($"Input: {equation}");
    Console.WriteLine($"LaTeX Output: {testProcessor.ConvertContent(equation)[0]}\n");
}
Console.WriteLine("================================\n");

// ✅ Manually Test JSON to LaTeX Conversion Before Running API Server
string jsonInput = @"{
    ""alignment"": ""left"",
    ""type"": ""dash_bulleted_list"",
    ""content"": ""bulleted type 7"",
    ""styling"": [
        {
            ""bold"": false,
            ""italic"": false,
            ""alignment"": ""left"",
            ""fontsize"": 12,
            ""fonttype"": ""ListParagraph"",
            ""fontcolor"": ""000000"",
            ""highlight"": ""none""
        }
    ]
}";

try
{
    var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonInput);

    if (jsonObject == null)
    {
        Console.WriteLine("Failed to deserialize JSON.");
    }
    else
    {
        string latex = listProcessor.ConvertToLatex(jsonObject);
        Console.WriteLine("\n==== TESTING JSON TO LaTeX CONVERSION ====");
        Console.WriteLine("Generated LaTeX:\n" + latex);
        Console.WriteLine("================================\n");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Invalid JSON input: " + ex.Message);
}

app.Run();
