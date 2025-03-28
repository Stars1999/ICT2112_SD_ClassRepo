using MongoDB.Driver;

namespace Utilities
{
	public class NodeManager : INodeUpdateNotify
	{


	private readonly IMongoCollection<AbstractNode> _nodeCollection;
	private readonly Lazy<INodeUpdate> _nodeUpdate;
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
			//     if (node == null)
    		// {
        	// throw new ArgumentNullException(nameof(node), "Node cannot be null");
    		// }
			// if (node != null)
			// {
			// // TODO: Insert node to database
			// _nodeUpdate.Value.saveTree(node);
			// }
		}

		public AbstractNode GetLastSavedNode()
		{
			// TODO: Retrieve last saved node from database
			return new CompositeNode(0, 0, "root", "root", new List<Dictionary<string, object>>()); // Dummy return
		}

		// INodeUpdateNotify
		public async Task notifyUpdatedNode(AbstractNode node)
		{
			//TODO: notifyNode
			Console.WriteLine($"NodeManager -> Notify Node Updated: {node}");
			await Task.CompletedTask;

		}
		
		public void notifyRetrievedNode()
		{
			// TODO: Notify retrieved node??
		}
	}
}
