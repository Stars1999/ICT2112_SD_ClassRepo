using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            // Step 1: Organize tables
            // var tableOrganiser = new TableOrganiserManager();
            // List<Table> tables = tableOrganiser.OrganiseTables(GetSampleTables());

            // // Step 3: Convert tables to LaTeX
            // var latexConverter = new MockLatexConverter();
            // var latexConversionManager = new TableLatexConversionManager(latexConverter);

            // foreach (var table in tables)
            // {
            //     // Step 3a: Convert table to LaTeX
            //     string latexOutput = await latexConversionManager.ConvertToLatexAsync(table);

            //     // Step 3b: Update LaTeX checkpoint
            //     await latexConversionManager.UpdateLatexCheckpointAsync(table.TableId, latexOutput);

            //     Console.WriteLine($"LaTeX output for Table {table.TableId}:\n{latexOutput}\n");
            // }

            // // Step 4: Post-processing (e.g., prepare LaTeX output to pass to node)
            Console.WriteLine("Post-processing completed. LaTeX output ready for node.");

            app.Run();
        }

        // Helper method to generate sample tables
        private static string GetSampleTables()
        {
            return @"
    {
        ""document"": [
            {
                ""type"": ""table"",
                ""content"": [
                    [""Table1"", ""i"", ""Am""],
                    [""Going"", ""To"", ""Remod""]
                ]
            },
            {
                ""type"": ""table"",
                ""content"": [
                    [""Table2"", ""i"", ""Am""],
                    [""Going"", ""To"", ""Remod""]
                ]
            },
            {
                ""type"": ""table"",
                ""content"": [
                    [""Table3"", ""i"", ""Am""],
                    [""Going"", ""To"", ""Remod""]
                ]
            },
            {
                ""type"": ""table"",
                ""content"": [
                    [""Table4"", ""i"", ""Am""],
                    [""Going"", ""To"", ""Remod""]
                ]
            }
        ]
    }";
        }
    }
}