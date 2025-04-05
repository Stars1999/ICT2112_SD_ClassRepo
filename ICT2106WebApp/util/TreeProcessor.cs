using System.Reflection.Metadata;
using System.Text;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace ICT2106WebApp.mod1Grp3
{
	public class TreeProcessor : ITreeUpdateNotify
	{
		private readonly ITreeUpdate _treeUpdate;

		public TreeProcessor()
		{
			// _docxRetrieve = (IDocumentRetrieve) new DocumentGateway_RDG();
			_treeUpdate = (ITreeUpdate)new DocumentGateway_RDG();
			_treeUpdate.treeUpdate = this;
			// _docxRetrieve.docxRetrieve = this;
		}

		// Method to create relational tree using nodes
		public CompositeNode CreateTree(List<AbstractNode> sequentialList)
		{
			Stack<AbstractNode> nodeStack = new Stack<AbstractNode>();
			AbstractNode rootNode = sequentialList[0];
			nodeStack.Push(rootNode);

			foreach (AbstractNode node in sequentialList)
			{
				Dictionary<string, object> nodeData = node.GetNodeData("TreeCreation");
				string nodeType = nodeData["nodeType"].ToString();
				int nodeLevel = (int)nodeData["nodeLevel"];

				if (nodeType == "root")
				{
					continue;
				}

				int currentNodeLevel = nodeLevel;
				int currentCompositeNodeLevel = (int)
					((CompositeNode)nodeStack.Peek()).GetNodeData("Peek")["nodeLevel"];

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
						currentCompositeNodeLevel = (int)
							((CompositeNode)nodeStack.Peek()).GetNodeData("Peek")["nodeLevel"];
					}
					((CompositeNode)nodeStack.Peek()).AddChild(node);
					nodeStack.Push(node);
				}
			}
			return (CompositeNode)rootNode;
		}

		// Method to save relational tree to database
		public async Task SaveTreeToDatabase(AbstractNode rootNode, string collectionName)
		{
			Console.WriteLine("inside this function now");
			// TODO: Save tree to database
			await _treeUpdate.saveTree(rootNode, collectionName);
		}

		// ----------------------------------TREE VALIDATION CODES-----------------------------------------
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

		public bool ValidateTreeStructure(AbstractNode node, int parentLevel)
		{
			Dictionary<string, object> nodeData = node.GetNodeData("TreeStructureValidation");
			int nodeLevel = (int)nodeData["nodeLevel"]; // Get the level of the current node
			int expectedLevel = parentLevel + 1; // Expected level based on parent

			if (nodeLevel < expectedLevel && nodeLevel != -1)
			{
				Console.WriteLine(
					$"Structural error: '{nodeData["nodeType"]}' at level {nodeLevel}, expected {expectedLevel}."
				);
				return false;
			}

			// Check children if the node is composite
			if (node is CompositeNode compositeNode)
			{
				foreach (var child in compositeNode.GetChildren())
				{
					if (!ValidateTreeStructure(child, nodeLevel)) // Pass current node level as parentLevel
						return false;
				}
			}
			return true;
		}

		// Recursive method to print the tree contents
		public void PrintTreeContents(AbstractNode node)
		{
			Dictionary<string, object> nodeData = node.GetNodeData("TreePrint");
			// Print the node's content (could be its type or content)
			// var nodeStyles = node.GetStyling();
			List<Dictionary<string, object>> result =
				(List<Dictionary<string, object>>)nodeData["styling"]; // This returns List<Dictionary<string, object>>

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
				new string(' ', 1)
					+ "\nNode ID: "
					+ nodeData["nodeId"]
					+ "\nNode Type: "
					+ nodeData["nodeType"]
					+ "\nContent: "
					+ nodeData["content"]
					+ "\nStyling: "
					+ consolidatedStyling
			);

			if (node is CompositeNode compositeNode)
			{
				// Recursively print children of composite nodes
				foreach (var child in compositeNode.GetChildren())
				{
					PrintTreeContents(child);
				}
			}
		}

		// Recursive method to print the tree hierarchy
		public void PrintTreeHierarchy(
			AbstractNode node,
			int level,
			bool isLastChild = true,
			List<bool> isLastChildHistory = null
		)
		{
			// Initialize history tracking for the first call
			if (isLastChildHistory == null)
				isLastChildHistory = new List<bool>();

			Dictionary<string, object> nodeData = node.GetNodeData("TreePrint");

			// Build the prefix based on the hierarchy history
			StringBuilder prefix = new StringBuilder();

			// Add appropriate symbols based on history of last children
			for (int i = 0; i < level; i++)
			{
				if (i == level - 1)
				{
					// For the current level
					prefix.Append(isLastChild ? "└── " : "├── ");
				}
				else
				{
					// For parent levels
					prefix.Append(
						isLastChildHistory.Count > i && !isLastChildHistory[i] ? "│   " : "    "
					);
				}
			}

			// Print the current node
			Console.WriteLine(
				prefix + "Node ID: " + nodeData["nodeId"] + ", Node Type: " + nodeData["nodeType"]
			);

			// If this is a composite node, print its children
			if (node is CompositeNode compositeNode)
			{
				var children = compositeNode.GetChildren().ToList();

				// Track the last child status in history for child nodes
				List<bool> childHistory = new List<bool>(isLastChildHistory);
				while (childHistory.Count < level)
					childHistory.Add(isLastChild);

				// Process each child
				for (int i = 0; i < children.Count; i++)
				{
					bool childIsLast = (i == children.Count - 1);
					PrintTreeHierarchy(children[i], level + 1, childIsLast, childHistory);
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
			AbstractNode rootNode = await _treeUpdate.getTree("mergewithcommentedcode");

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
