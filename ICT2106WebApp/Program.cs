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

// Test Math Conversion with Different Inputs
string[] testEquations =
{
    "(1/2) √(4)",
    "(1/2) × √(4)=1",
    //"s=√(([i=1,N](x^2)/N-1))",    //doesnt work
    "(1/2)cos2x+ (3/8)sin4x =3x",
    "log_246y ≥ (π/2)",
    "∴ ∞ ≠ ±α",
    //"∃xPersonx∧∀yTimey→Happyx,y", //doesnt work
    "F=ma",
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
    ""type"": ""lowercase_lettered_parenthesis_list"",
    ""content"": ""lowercase lettered with bracket list"",
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
        string latex = ConvertToLatex(jsonObject);
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


static string ConvertToLatex(Dictionary<string, object> json)
{
    if (!json.ContainsKey("type") || !json.ContainsKey("content"))
    {
        return "Invalid JSON structure.";
    }

    string type = json["type"]?.ToString();
    string content = json["content"]?.ToString();

    string latexListType = type switch
    {
        "bulleted_list" => "itemize",
        "hollow_bulleted_list" => "itemize",
        "square_bulleted_list" => "itemize",
        "diamond_bulleted_list" => "itemize",
        "arrow_bulleted_list" => "itemize",
        "checkmark_bulleted_list" => "itemize",
        "dash_bulleted_list" => "itemize",
        "numbered_list" => "enumerate",
        "numbered_parenthesis_list" => "enumerate",
        "roman_numeral_list" => "enumerate",
        "lowercase_roman_numeral_list" => "enumerate",
        "uppercase_lettered_list" => "enumerate",
        "lowercase_lettered_list" => "enumerate",
        "lowercase_lettered_parenthesis_list" => "enumerate",
        _ => "unknown"
    };

    if (latexListType == "unknown")
    {
        return "Unsupported list type.";
    }

    if (type == "lowercase_lettered_parenthesis_list")
    {
        // Special case for lowercase lettered list with parentheses (a), b), c), ...)
        return $@"\begin{{enumerate}}
    \renewcommand{{\labelenumi}}{{\alph{{enumi}})}}
    \item {content}
\end{{enumerate}}";
    }

    return $"\\begin{{{latexListType}}}\n    \\item {content}\n\\end{{{latexListType}}}";
}
