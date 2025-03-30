using System.Reflection.Metadata;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Utilities
{
	public class TreeProcessor: ITreeUpdateNotify
	{
		// private List<string> nodeOrder = new List<string>
		// {
		// 	"root",
		// 	"h1",
		// 	"h2",
		// 	"h3",
		// 	"h4",
		// 	"h5",
		// 	"h6",
		// 	"table",
		// 	"row",
		// 	"runsParagraph",
		// };
		// Save Tree to Database 
		private readonly ITreeUpdate _treeUpdate;

		public TreeProcessor()
		{
			// _docxRetrieve = (IDocumentRetrieve) new DocumentGateway_RDG();
			_treeUpdate = (ITreeUpdate) new DocumentGateway_RDG();
			_treeUpdate.treeUpdate = this;
			// _docxRetrieve.docxRetrieve = this;
		}
        public CompositeNode CreateTree(List<AbstractNode> sequentialList)
		{
			Stack<AbstractNode> nodeStack = new Stack<AbstractNode>();
			AbstractNode rootNode = sequentialList[0];
			nodeStack.Push(rootNode);

			foreach (AbstractNode node in sequentialList)
			{
				if (node.GetNodeType() == "root")
				{
					continue;
				}

				int currentNodeLevel = node.GetNodeLevel();
				int currentCompositeNodeLevel = ((CompositeNode)nodeStack.Peek()).GetNodeLevel();

				// Set level of runs to be +1 of runsParagraph
				// if (node.GetNodeType() == "text_run")
				// {
				// 	currentNodeLevel = nodeOrder.IndexOf("runsParagraph") + 1;
				// }

				// if (node.GetNodeType() == "cell")
				// {
				// 	currentNodeLevel = nodeOrder.IndexOf("row") + 1;
				// }

				if (currentNodeLevel == -1)
				{
					((CompositeNode)nodeStack.Peek()).AddChild(node);
				}
				else if (currentNodeLevel > currentCompositeNodeLevel)
				{
					((CompositeNode)nodeStack.Peek()).AddChild(node);
					nodeStack.Push(node);
				}
				else
				{
					while (currentNodeLevel <= currentCompositeNodeLevel)
					{
						nodeStack.Pop();
						currentCompositeNodeLevel = ((CompositeNode)nodeStack.Peek()).GetNodeLevel();
					}
					((CompositeNode)nodeStack.Peek()).AddChild(node);
					nodeStack.Push(node);
				}
			}
			return (CompositeNode)rootNode;
		}

		public async Task SaveTreeToDatabase(AbstractNode rootNode)
		{
			Console.WriteLine("inside this function now");
			// TODO: Save tree to database
			await _treeUpdate.saveTree(rootNode);
		}

		// public bool ValidateTree(Document document, AbstractNode rootNode)
		// {
		// 	// TODO: Validate tree
		// 	return true; // Dummy return
		// }

		public List<AbstractNode> FlattenTree(AbstractNode root)
		{
			List<AbstractNode> flatList = new List<AbstractNode>();
			int count = 0;

			void Traverse(AbstractNode node)
			{
				flatList.Add(node); // Add current node to list
				if (node is CompositeNode compositeNode)
				{
					foreach (var child in compositeNode.GetChildren())
					{
						Traverse(child); // Recursively add children
					}
				}
			}

			Traverse(root);
			// Print the flatList by iterating over the nodes
			// Console.WriteLine("flatList contents:");
			// foreach (var node in flatList)
			// {
				// Console.WriteLine($"{count}. Node Type: {node.GetNodeType()}, Node Content: {node.GetContent()}");
			// 	count++;
			// }
			return flatList;
		}

		public bool ValidateContent(List<AbstractNode> treeNodes, JArray documentArray)
		{
			return ValidateContentRecursive(treeNodes, documentArray, 0);
		}

		private bool ValidateContentRecursive(List<AbstractNode> treeNodes, JArray documentArray, int startIndex)
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
					if (!CompareNodeTypeAndContent(treeNodes[treeNodeIndex], jsonType, GetJsonNodeContent(jsonItem)))
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
					if (!CompareNodeTypeAndContent(treeNodes[treeNodeIndex], jsonType, GetJsonNodeContent(jsonItem)))
					{
						return false;
					}
					treeNodeIndex++;

					// Validate individual runs
					foreach (var run in runs)
					{
						string runContent = run["content"]?.ToString() ?? "";
						if (!CompareNodeTypeAndContent(treeNodes[treeNodeIndex], "text_run", runContent))
						{
							return false;
						}
						treeNodeIndex++;
					}
				}
				// Handle simple nodes without runs
				else
				{
					if (!CompareNodeTypeAndContent(treeNodes[treeNodeIndex], jsonType, jsonItem["content"]?.ToString() ?? ""))
					{
						return false;
					}
					treeNodeIndex++;
				}
			}

			return true;
		}

		private bool ValidateNestedRuns(List<AbstractNode> treeNodes, ref int treeNodeIndex, JArray nestedRuns)
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

		private bool CompareNodeTypeAndContent(AbstractNode treeNode, string jsonType, string jsonContent)
		{
			string treeType = treeNode.GetNodeType();
			string treeContent = treeNode.GetContent();

			if (treeType != jsonType || treeContent != jsonContent)
			{
				Console.WriteLine($"❌ Mismatch:" +
					$"\n  Tree   - Type: {treeType}, Content: '{treeContent}'" +
					$"\n  JSON   - Type: {jsonType}, Content: '{jsonContent}'");
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
		
		public bool ValidateNodeStructure(AbstractNode node, int parentLevel)
		{
			int nodeLevel = node.GetNodeLevel(); // Get the level of the current node
			int expectedLevel = parentLevel + 1; // Expected level based on parent

			if (nodeLevel < expectedLevel && nodeLevel != -1)
			{
				Console.WriteLine($"Structural error: '{node.GetNodeType()}' at level {nodeLevel}, expected {expectedLevel}.");
				return false;
			}

			// Check children if the node is composite
			// if(nodeLevel != -1 ) 
			// {
				if (node is CompositeNode compositeNode)
			{
				foreach (var child in compositeNode.GetChildren()) 
				{
					if (!ValidateNodeStructure(child, nodeLevel)) // Pass current node level as parentLevel
						return false;
				}
			}
			// }

			return true;
		}

		// Recursive method to print the tree hierarchy
		public void PrintTree(AbstractNode node, int level)
		{
			// Print the node's content (could be its type or content)
			// var nodeStyles = node.GetStyling();
			var result = node.GetStyling(); // This returns List<Dictionary<string, object>>
			string consolidatedStyling = "";

			// Loop through each dictionary in the list
			foreach (var dict in result)
			{
				// Loop through each key-value pair in the dictionary
				foreach (var kvp in dict)
				{
        		consolidatedStyling += $"{kvp.Key}: {kvp.Value}, ";
				}
			}

			Console.WriteLine(
				new string(' ', level * 2) + node.GetNodeType() + ": " + node.GetContent() + " | styling: " + consolidatedStyling
				
			);


			if (node is CompositeNode compositeNode)
			{
				// Recursively print children of composite nodes
				foreach (var child in compositeNode.GetChildren())
				{
					PrintTree(child, level + 1);
				}
			}
		}


        public Task NotifyUpdatedTree(AbstractNode node)
        {
			return Task.CompletedTask;
        }

		// public async Task retrieveTree()
		// {
		// 	// await _treeUpdate.loadTree();
		// 	await CompositeNode compNode = (CompositeNode) _treeUpdate.loadTree()
		// }
		public async Task<AbstractNode> retrieveTree()
		{
			AbstractNode rootNode = await _treeUpdate.getTree();
			
			if (rootNode is CompositeNode compositeNode)
			{
				Console.WriteLine("Loaded tree is a CompositeNode!");
				// Console.WriteLine("printing tree FROM DB\n");
				// PrintTree(rootNode,0);
				// Process the tree
			}
			else
			{
				Console.WriteLine("Loaded tree is not a CompositeNode!");
			}
			return rootNode;
		}
    }
}
