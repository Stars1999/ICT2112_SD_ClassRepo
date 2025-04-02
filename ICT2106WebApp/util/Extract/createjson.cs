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
using ICT2106WebApp.mod1grp4;
using Microsoft.Extensions.Options;
using MongoDB.Bson; // Bson - Binary JSON
// MongoDB packages
using MongoDB.Driver;
using Newtonsoft.Json; // For JsonConvert
using Newtonsoft.Json.Linq; // Bson - Binary JSON

namespace Utilities
{
	public static partial class ExtractContent
	{
		// returns nodeslist and json
		public static async Task<string> CreateNodeList(
			List<object> documentContents,
			List<AbstractNode> nodesList,
			string jsonOutput,
			object documentData,
			string jsonOutputPath
		)
		{
			int id = 1;
			var numberofRunNode = 0;
			var numberofMainNode = 0;
			NodeManager nodeManager = new NodeManager();
			var documentControl = new DocumentControl();

			List<AbstractNode> runListNodes = new List<AbstractNode>();
			List<AbstractNode> runRunListNodes = new List<AbstractNode>();

			// this part creates the json
			foreach (var item in documentContents)
			{
				// Going through each item's key-value pair of the object
				if (item is Dictionary<string, object> dictionary)
				{
					Console.WriteLine("Dictionary contents:");
					string nodeType = "";
					string content = "";
					List<Dictionary<string, object>> styling =
						new List<Dictionary<string, object>>();

					// Loop through the dictionary and print the key-value pairs
					foreach (var kvp in dictionary)
					{
						if (kvp.Key == "type")
						{
							nodeType = (string)kvp.Value;
							Console.WriteLine($"type: {nodeType}");
						}
						if (kvp.Key == "content")
						{
							content = (string)kvp.Value;
							Console.WriteLine($"content {content}");
						}
						if (kvp.Key == "styling")
						{
							Console.WriteLine("Styling L1:");

							if (kvp.Value is List<object> objectList)
							{
								List<Dictionary<string, object>> stylingList =
									new List<Dictionary<string, object>>();

								foreach (var itemhere in objectList)
								{
									if (itemhere is Dictionary<string, object> stylingDictionary)
									{
										stylingList.Add(
											ExtractContent.ConvertJsonElements(stylingDictionary)
										);
									}
								}

								// Assign the processed styling list to the styling variable
								styling = stylingList;
							}
							else
							{
								Console.WriteLine("The 'styling' value is not a List<object>.");
							}
						}
						// check for run
						// Check for 'runs' key
						if (kvp.Key == "runs")
						{
							var runsList = (List<Dictionary<string, object>>)kvp.Value;
							// Loop through each text_run in runs
							foreach (var run in runsList)
							{
								string runType = "";
								string runContent = "";
								Dictionary<string, object> runStyling =
									new Dictionary<string, object>();

								Console.WriteLine("JSONBUILDINGrun");
								Console.WriteLine(run);
								// Process the 'type' and 'content' for each run
								foreach (var runKvp in run)
								{
									if (runKvp.Key == "type")
									{
										runType = (string)runKvp.Value;
										Console.WriteLine($"runType: {runKvp.Value}");
										Console.WriteLine($"runType: {runType}");
									}
									if (runKvp.Key == "content")
									{
										runContent = (string)runKvp.Value;
										Console.WriteLine($"runContent: {runKvp.Value}");
										Console.WriteLine($"runContent: {runContent}");
									}
									if (runKvp.Key == "styling")
									{
										if (runKvp.Value is List<object> objectList)
										{
											List<Dictionary<string, object>> stylingList =
												new List<Dictionary<string, object>>();

											foreach (var itemhere in objectList)
											{
												if (
													itemhere
													is Dictionary<string, object> stylingDictionary
												)
												{
													stylingList.Add(
														ExtractContent.ConvertJsonElements(
															stylingDictionary
														)
													);
												}
											}

											// Assign the processed styling list to the runStyling variable
											runStyling = stylingList.FirstOrDefault(); // Assuming only one styling dictionary per run
										}
										else
										{
											Console.WriteLine(
												"The 'styling' value is not a List<object>."
											);
										}
									}

									if (runKvp.Key == "runs")
									{ //If Table Go To Cell Level
										var runRunsList =
											(List<Dictionary<string, object>>)runKvp.Value;
										// Loop through each text_run in runs
										foreach (var runRun in runRunsList)
										{
											string runRunType = "";
											string runRunContent = "";
											Dictionary<string, object> runRunStyling =
												new Dictionary<string, object>();

											Console.WriteLine("JSONBUILDINGrunrun");
											Console.WriteLine(runRun);
											// Process the 'type' and 'content' for each run
											foreach (var runRunKvp in runRun)
											{
												if (runRunKvp.Key == "type")
												{
													runRunType = (string)runRunKvp.Value;
													Console.WriteLine(
														$"runType: {runRunKvp.Value}"
													);
													Console.WriteLine($"runType: {runRunType}");
												}
												if (runRunKvp.Key == "content")
												{
													runRunContent = (string)runRunKvp.Value;
													Console.WriteLine(
														$"runContent: {runRunKvp.Value}"
													);
													Console.WriteLine(
														$"runContent: {runRunContent}"
													);
												}
												if (runRunKvp.Key == "styling") //This is where we get Cell Style
												{
													if (
														runRunKvp.Value
														is Dictionary<
															string,
															object
														> stylingDictionary
													)
													{
														List<
															Dictionary<string, object>
														> stylingList =
															new List<Dictionary<string, object>>();
														stylingList.Add(stylingDictionary);
														runRunStyling =
															stylingList.FirstOrDefault();
													}
													else
													{
														Console.WriteLine(
															"The 'styling' value is not a Dictionary<string, object>."
														);
													}
												}
											}

											// Create a node for each run (assuming it's a "text_run")
											if (runRunType != "")
											{
												var runRunNode = nodeManager.CreateNode(
													id++,
													runRunType,
													runRunContent,
													new List<Dictionary<string, object>>
													{
														runRunStyling,
													}
												);
												numberofRunNode = numberofRunNode + 1;
												Console.WriteLine(
													$"run myid:{id} {runRunType}: {runRunContent}\n"
												);
												// nodesList.Add(runNode);
												runRunListNodes.Add(runRunNode);
											}
											// nodesList.Add(runNode);
										}
									}
								}

								// Create a node for each run (assuming it's a "text_run")
								if (runType != "")
								{
									var runNode = nodeManager.CreateNode(
										id++,
										runType,
										runContent,
										new List<Dictionary<string, object>> { runStyling }
									);
									numberofRunNode = numberofRunNode + 1;
									Console.WriteLine($"run myid:{id} {runType}: {runContent}\n");
									// nodesList.Add(runNode);
									runListNodes.Add(runNode);
									foreach (var runrunnodeitem in runRunListNodes)
									{
										runListNodes.Add(runrunnodeitem);
									}
									runRunListNodes.Clear();
								}
								// nodesList.Add(runNode);
							}
							// end of run nodes
						}

						// end of an object / Parent node
					}

					if (nodeType != "" || content != "")
					{
						var node = nodeManager.CreateNode(id++, nodeType, content, styling);
						nodesList.Add(node);
					}
					else
					{
						Console.WriteLine(
							$"Weird its null\n type: {nodeType}\ncontent: {content}\n"
						);
					}

					foreach (var runnodeitem in runListNodes)
					{
						nodesList.Add(runnodeitem);
					}
					runListNodes.Clear();
				}
				// end of checking dictionary

				// Convert to JSON format with UTF-8 encoding fix (preserves emojis, math, and Chinese)
				jsonOutput = System.Text.Json.JsonSerializer.Serialize(
					documentData,
					new JsonSerializerOptions
					{
						WriteIndented = true,
						Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
					}
				);

				// ✅ Write JSON to file
				File.WriteAllText(jsonOutputPath, jsonOutput);
				//** to uncomment this line
				// await documentControl.saveJsonToDatabase(jsonOutputPath);
				Console.WriteLine($"✅ JSON output saved to {jsonOutputPath}");
			}

			// This calculates and accounts for the number of parent node and child nodes
			Console.WriteLine($"number of Main node: {numberofMainNode}\n");
			Console.WriteLine($"number of Run node: {numberofRunNode}\n");

			return jsonOutput;
		}
	}
}
