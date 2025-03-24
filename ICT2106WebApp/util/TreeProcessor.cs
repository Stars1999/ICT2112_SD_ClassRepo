using System.Reflection.Metadata;

namespace Utilities
{
	public class TreeProcessor
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

		public void SaveTreeToDatabase(AbstractNode rootNode)
		{
			// TODO: Save tree to database
		}

		public bool ValidateTree(Document document, AbstractNode rootNode)
		{
			// TODO: Validate tree
			return true; // Dummy return
		}

		public void NotifyUpdatedTree()
		{
			// TODO: Notify updated tree??
		}

		// Recursive method to print the tree hierarchy
		public void PrintTree(AbstractNode node, int level)
		{
			// Print the node's content (could be its type or content)
			Console.WriteLine(
				new string(' ', level * 2) + node.GetNodeType() + ": " + node.GetContent()
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
	}
}
