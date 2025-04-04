using Newtonsoft.Json.Linq;
namespace Utilities
{
	public class NodeManager
	{
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
				InsertNodeToDatabase(newNode);
				return newNode;
			}
			else
			{
				newNode = new SimpleNode(id, nodeLevel, nodeType, content, styling);
				InsertNodeToDatabase(newNode);
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

		public void InsertNodeToDatabase(AbstractNode node)
		{
			// TODO: Insert node to database
		}

		public AbstractNode GetLastSavedNode()
		{
			// TODO: Retrieve last saved node from database
			return new CompositeNode(0, 0, "root", "root", new List<Dictionary<string, object>>()); // Dummy return
		}

		public void NotifyUpdatedNode()
		{
			// TODO: Notify updated node??
		}

		public void NotifyRetrievedNode()
		{
			// TODO: Notify retrieved node??
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
	}
}
