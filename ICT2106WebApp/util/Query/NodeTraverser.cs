using System.Threading.Tasks;
using Newtonsoft.Json;
using Utilities;

namespace ICT2106WebApp.mod1Grp3
{
	public class NodeTraverser : INodeTraverser, IQueryUpdateNotify
	{
		private CompositeNode _rootNode;
		private readonly IQueryUpdate _QueryUpdate;

		public NodeTraverser(CompositeNode rootNode)
		{
			_rootNode = rootNode;
			_QueryUpdate = (IQueryUpdate)new DocumentGateway_RDG(); 
			_QueryUpdate.queryUpdate = this; 
		}

		// Define grouped node types
		private static Dictionary<string, HashSet<string>> nodeTypeGroups = new Dictionary<
			string,
			HashSet<string>
		>
		{
			{
				"headers",
				new HashSet<string> { "h1", "h2", "h3" }
			},
			{
				"layouts",
				new HashSet<string> { "layout", "page_break" }
			},
			{
				"lists",
				new HashSet<string>
				{
					"bulleted_list",
					"hollow_bulleted_list",
					"square_bulleted_list",
					"diamond_bulleted_list",
					"arrow_bulleted_list",
					"checkmark_bulleted_list",
					"dash_bulleted_list",
					"numbered_list",
					"numbered_parenthesis_list",
					"roman_numeral_list",
					"lowercase_roman_numeral_list",
					"uppercase_lettered_list",
					"lowercase_lettered_list",
					"lowercase_lettered_parenthesis_list",
				}
			},
			{
				"paragraphs",
				new HashSet<string> { "paragraph", "paragraph_run?", "empty_paragraph1" }
			},
			{
				"tables",
				new HashSet<string> { "table", "cell", "row" }
			},
			{
				"citationAndbibliographys",
				new HashSet<string> { "bibliography", "citation_run", "intext-citation" }
			},
			{
				"allNodes",
				new HashSet<string>
				{
					"headers",
					"layouts",
					"lists",
					"paragraphs",
					"tables",
					"citationAndbibliographys",
				}
			},
		};

		/*NODETYPE LIST (for reference)
		metadata (not avaliable)
		headers: h1, h2, h3
		layouts: layout, page_break
		lists: bulleted_list, hollow_bulleted_list, square_bulleted_list, diamond_bulleted_list, arrow_bulleted_list, checkmark_bulleted_list, dash_bulleted_list, numbered_list, numbered_parenthesis_list, roman_numeral_list, lowercase_roman_numeral_list, uppercase_lettered_list, lowercase_lettered_list, lowercase_lettered_parenthesis_list
		paragraphs: paragraph, paragraph_run?, empty_paragraph1
		table : table, cell, row
		citationAndbibliographys: bibliography, citation_run, intext-citation
		text_run
		Image
		math*/

		public List<AbstractNode> TraverseNode(string nodeType)
		{
			if (string.IsNullOrWhiteSpace(nodeType))
				throw new ArgumentException("Node type cannot be null or empty", nameof(nodeType));

			List<AbstractNode> matchingNodesList = new List<AbstractNode>();

			void TraverseWithIterator(
				AbstractNode node,
				string nodeType,
				List<AbstractNode> matchingNodes
			)
			{
				Dictionary<string, object> nodeData = node.GetNodeData("NodeTraversal");
				string nt = nodeData["nodeType"].ToString();
				// Check if current node matches the type
				if (
					nt == nodeType
					|| (
						nodeTypeGroups.ContainsKey(nodeType)
						&& nodeTypeGroups[nodeType].Contains(nt)
					)
				)
				{
					matchingNodes.Add(node);
				}

				// If node can provide an iterator, process its children
				if (node is INodeCollection collection)
				{
					INodeIterator iterator = collection.CreateIterator();

					while (!iterator.isDone())
					{
						AbstractNode childNode = iterator.next();
						if (childNode != null)
						{
							TraverseWithIterator(childNode, nodeType, matchingNodes);
						}
					}
				}
			}

			// Process all nodes starting from root
			TraverseWithIterator(_rootNode, nodeType, matchingNodesList);

			foreach (var node in matchingNodesList)
			{
				Dictionary<string, object> nodeData = node.GetNodeData("NodeInfo");
				Console.WriteLine(
					$"Matching Node: ID={nodeData["nodeId"]} \nType={nodeData["nodeType"]} \nContent={nodeData["content"]} \nstyling={JsonConvert.SerializeObject(nodeData["styling"])} \nconverted={nodeData["converted"]} \n"
				);
			}
			Console.WriteLine(matchingNodesList.Count + " nodes found of type " + nodeType);

			return matchingNodesList;
		}

		public async Task UpdateLatexDocument(AbstractNode rootNode)
		{
			Console.WriteLine("LATEX DOCUMENT UPDATING...");
			await _QueryUpdate.saveTree(rootNode, "latexTree"); // Save the updated tree to the database
		}

		// Notify when the tree is updated in the database
		public async Task notifyUpdatedTree()
		{
			Console.WriteLine("Tree updated...");

			await Task.CompletedTask;
		}

	}
}
