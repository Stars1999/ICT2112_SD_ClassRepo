using Newtonsoft.Json.Linq;

namespace Utilities
{
	public class NodeManager
	{

		// Method to create nodes
		public AbstractNode CreateNode(
			int id,
			string nodeType,
			string content,
			List<Dictionary<string, object>> styling
		)
		{
			int nodeLevel = FindNodeLevel(nodeType);
			AbstractNode newNode;

			if (nodeLevel >= 0)
			{
				newNode = new CompositeNode(id, nodeLevel, nodeType, content, styling);
				return newNode;
			}
			else
			{
				newNode = new SimpleNode(id, nodeLevel, nodeType, content, styling);
				return newNode;
			}
		}

		private int FindNodeLevel(string nodeType)
		{
			switch (nodeType)
			{
				case "root":
					return 0;
				case "h1":
					return 1;
				case "bibliography":
					return 1;
				case "h2":
					return 2;
				case "h3":
					return 3;
				case "h4":
					return 4;
				case "h5":
					return 5;
				case "h6":
					return 6;
				case "paragraph":
					return 7;
				case "table":
					return 7;
				case "row":
					return 8;
				default:
					return -1;
			}
		}

		// -- node content validation --
		public bool ValidateContentRecursive(
			List<AbstractNode> treeNodes,
			JArray documentArray,
			int startIndex
		)
		{
			int treeNodeIndex = startIndex;

			for (int jsonIndex = 0; jsonIndex < documentArray.Count; jsonIndex++)
			{
				JObject jsonItem = (JObject)documentArray[jsonIndex];
				string jsonType = jsonItem["type"]?.ToString();

				// Handle nested structures (tables, rows, cells)
				if (HasNestedRuns(jsonItem))
				{
					// Validate current node type and content
					if (
						!CompareNodeTypeAndContent(
							treeNodes[treeNodeIndex],
							jsonType,
							GetJsonNodeContent(jsonItem)
						)
					)
					{
						return false;
					}
					treeNodeIndex++;

					// Recursively validate nested runs
					var nestedRuns = (JArray)jsonItem["runs"];
					if (!ValidateNestedRuns(treeNodes, ref treeNodeIndex, nestedRuns))
					{
						return false;
					}
				}
				// Handle simple nodes with regular runs
				else if (jsonItem["runs"] is JArray runs && runs.Count > 0)
				{
					// Validate main node
					if (
						!CompareNodeTypeAndContent(
							treeNodes[treeNodeIndex],
							jsonType,
							GetJsonNodeContent(jsonItem)
						)
					)
					{
						return false;
					}
					treeNodeIndex++;

					// Validate individual runs
					foreach (var run in runs)
					{
						string runContent = run["content"]?.ToString() ?? "";
						if (
							!CompareNodeTypeAndContent(
								treeNodes[treeNodeIndex],
								"text_run",
								runContent
							)
						)
						{
							return false;
						}
						treeNodeIndex++;
					}
				}
				// Handle simple nodes without runs
				else
				{
					if (
						!CompareNodeTypeAndContent(
							treeNodes[treeNodeIndex],
							jsonType,
							jsonItem["content"]?.ToString() ?? ""
						)
					)
					{
						return false;
					}
					treeNodeIndex++;
				}
			}

			return true;
		}

		private bool ValidateNestedRuns(
			List<AbstractNode> treeNodes,
			ref int treeNodeIndex,
			JArray nestedRuns
		)
		{
			foreach (var nestedRun in nestedRuns)
			{
				JObject nestedRunObj = (JObject)nestedRun;
				string nestedType = nestedRunObj["type"]?.ToString();
				string nestedContent = nestedRunObj["content"]?.ToString() ?? "";

				// Validate current nested node
				if (!CompareNodeTypeAndContent(treeNodes[treeNodeIndex], nestedType, nestedContent))
				{
					return false;
				}
				treeNodeIndex++;

				// Recursively handle further nested runs
				if (HasNestedRuns(nestedRunObj))
				{
					var deeperRuns = (JArray)nestedRunObj["runs"];
					if (!ValidateNestedRuns(treeNodes, ref treeNodeIndex, deeperRuns))
					{
						return false;
					}
				}
			}
			return true;
		}

		private bool HasNestedRuns(JObject jsonItem)
		{
			return jsonItem["runs"] is JArray runs && runs.Count > 0;
		}

		private bool CompareNodeTypeAndContent(
			AbstractNode treeNode,
			string jsonType,
			string jsonContent
		)
		{
			Dictionary<string, object> nodeData = treeNode.GetNodeData("ContentValidation");
			string treeType = nodeData["nodeType"].ToString();
			string treeContent = nodeData["content"].ToString();

			if (treeType != jsonType || treeContent != jsonContent)
			{
				Console.WriteLine(
					$"❌ Mismatch:"
						+ $"\n  Tree   - Type: {treeType}, Content: '{treeContent}'"
						+ $"\n  JSON   - Type: {jsonType}, Content: '{jsonContent}'"
				);
				return false;
			}
			return true;
		}

		private string GetJsonNodeContent(JObject jsonItem)
		{
			// If the item has 'runs', concatenate their content
			if (jsonItem["runs"] is JArray runs && runs.Count > 0)
			{
				return string.Concat(runs.Select(run => run["content"]?.ToString() ?? ""));
			}

			// Otherwise, return the content directly
			return jsonItem["content"]?.ToString() ?? "";
		}

		public static List<AbstractNode> CreateNodeList(List<object> documentContents)
		{
			List<AbstractNode> nodesList = new List<AbstractNode>();
			int id = 1;
			var numberofRunNode = 0;
			var numberofMainNode = 0;
			NodeManager nodeManager = new NodeManager();
			var documentProcessor = new DocumentProcessor();


			List<AbstractNode> runListNodes = new List<AbstractNode>();
			List<AbstractNode> runRunListNodes = new List<AbstractNode>();

			foreach (var item in documentContents)
			{
				// Going through each item's key-value pair of the object
				if (item is Dictionary<string, object> dictionary)
				{
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
						}
						if (kvp.Key == "content")
						{
							content = (string)kvp.Value;
						}
						if (kvp.Key == "styling")
						{
							if (kvp.Value is List<object> objectList)
							{
								List<Dictionary<string, object>> stylingList =
									new List<Dictionary<string, object>>();

								foreach (var itemhere in objectList)
								{
									if (itemhere is Dictionary<string, object> stylingDictionary)
									{
										stylingList.Add(
											DocumentProcessor.ConvertJsonElements(
												stylingDictionary
											)
										);
									}
								}
								// Assign the processed styling list to the styling variable
								styling = stylingList;
							}
						}
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
								// Process the 'type' and 'content' for each run
								foreach (var runKvp in run)
								{
									if (runKvp.Key == "type")
									{
										runType = (string)runKvp.Value;
									}
									if (runKvp.Key == "content")
									{
										runContent = (string)runKvp.Value;
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
														DocumentProcessor.ConvertJsonElements(
															stylingDictionary
														)
													);
												}
											}
											// Assign the processed styling list to the runStyling variable
											runStyling = stylingList.FirstOrDefault(); // Assuming only one styling dictionary per run
										}
									}

									if (runKvp.Key == "runs")
									{
										//If Table Go To Cell Level
										var runRunsList =
											(List<Dictionary<string, object>>)runKvp.Value;
										// Loop through each text_run in runs
										foreach (var runRun in runRunsList)
										{
											string runRunType = "";
											string runRunContent = "";
											Dictionary<string, object> runRunStyling =
												new Dictionary<string, object>();
											// Process the 'type' and 'content' for each run
											foreach (var runRunKvp in runRun)
											{
												if (runRunKvp.Key == "type")
												{
													runRunType = (string)runRunKvp.Value;
												}
												if (runRunKvp.Key == "content")
												{
													runRunContent = (string)runRunKvp.Value;
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
												// numberofRunNode = numberofRunNode + 1;
												// Console.WriteLine(
												// 	$"run myid:{id} {runRunType}: {runRunContent}\n"
												// );
												// nodesList.Add(runNode);
												runRunListNodes.Add(runRunNode);
											}
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
									runListNodes.Add(runNode);
									foreach (var runrunnodeitem in runRunListNodes)
									{
										runListNodes.Add(runrunnodeitem);
									}
									runRunListNodes.Clear();
								}
							}
							// end of run nodes
						}
						// end of an object / Parent node
					}

					if (nodeType != "" || content != "")
					{
						var nodeOutside = nodeManager.CreateNode(id++, nodeType, content, styling);
						numberofMainNode = numberofMainNode + 1;
						nodesList.Add(nodeOutside);
					}

					foreach (var runnodeitem in runListNodes)
					{
						nodesList.Add(runnodeitem);
					}
					runListNodes.Clear();
				}
				// end of checking dictionary
				Console.WriteLine($"✅ Node created");
			}

			// This calculates and accounts for the number of parent node and child nodes
			Console.WriteLine($"number of Main node: {numberofMainNode}");
			Console.WriteLine($"number of Run node: {numberofRunNode}\n");
			return nodesList;
		}
	}
}
