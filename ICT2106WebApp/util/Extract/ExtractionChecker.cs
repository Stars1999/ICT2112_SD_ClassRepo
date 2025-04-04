using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ICT2106WebApp.mod1Grp3;
// using ICT2106WebApp.mod1grp4;
using Microsoft.Extensions.Options;
using MongoDB.Bson; // Bson - Binary JSON
// MongoDB packages
using MongoDB.Driver;
using Newtonsoft.Json; // For JsonConvert
using Newtonsoft.Json.Linq; // Bson - Binary JSON
using Utilities;

//function to invoke to see and check data
namespace Utilities
{
	public static partial class ExtractContent
	{
		public static void checkJson(JArray documentArray)
		{
			// Parse the JSON string
			// JObject jsonObject = JObject.Parse(jsonOutput);
			// Count the number of items in the "document" array
			// JArray documentArray = (JArray)jsonObject["document"];
			int documentCount = documentArray.Count;
			Console.WriteLine("Check number of nodes");
			Console.WriteLine($"Number of items in the JSON document array: {documentCount}");

			// Check if there are "runs" in any of the document items
			var totalCounts = 0;
			var i = 0;
			foreach (var itemhere in documentArray)
			{
				var runs = itemhere["runs"];

				// Console.WriteLine(i);

				if (runs != null)
				{
					totalCounts = totalCounts + runs.Count();
					// Console.WriteLine($"This item has {runs.Count()} runs.");
				}
				else
				{
					// Console.WriteLine("This item has no runs.");
				}

				i = i + 1;
			}

			Console.WriteLine("total run count");
			Console.WriteLine(totalCounts);
			totalCounts = totalCounts + i;
			Console.WriteLine("total = ");
			Console.WriteLine(totalCounts);
		}

		public static void checkNodes(List<AbstractNode> nodesList)
		{
			Console.WriteLine("\n\n\n\n\n\n\n\n\n nodesList.Count");
			Console.WriteLine(nodesList.Count);
			int nodeNum = 0;
			foreach (var nodeInList in nodesList)
			{
				nodeNum = nodeNum + 1;
				Console.Write("nodeNum:");
				Console.Write(nodeNum);
				Console.Write("\n");

				
				Dictionary<string, object> nodeData = nodeInList.GetNodeData("NodeInfo");

				var thetypehere = nodeData["nodeType"].ToString();
				Console.WriteLine($"type:{thetypehere}");

				var thelevelhere = nodeData["nodeLevel"].ToString();
				Console.WriteLine($"level:{thelevelhere}");

				var thecontenthere = nodeData["content"].ToString();
				Console.WriteLine($"content:{thecontenthere}");

				var thestylinghere = nodeData["styling"] as List<Dictionary<string, object>>;
				string consolidatedStyling = "";
				foreach (var dict in thestylinghere)
				{
					foreach (var kvp in dict)
					{
						consolidatedStyling += $"{kvp.Key}: {kvp.Value}";
					}
				}
				Console.WriteLine($"styling:{consolidatedStyling}");
				Console.Write("\n");
			}
		}
	}
}
