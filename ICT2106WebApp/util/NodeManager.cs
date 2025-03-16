namespace Utilities
{
	public class NodeManager
	{
		private List<string> compositeNodeList = new List<string>
		{
			"root",
			"h1",
			"h2",
			"h3",
			"h4",
			"h5",
			"h6",
			"table",
			"row",
			"runsParagraph",
			"bibliography"
		};

		public AbstractNode CreateNode(
			int id,
			int nodeLevel,
			string nodeType,
			string content,
			List<Dictionary<string, object>> styling
		)
		{
			if (compositeNodeList.Contains(nodeType))
			{
				return new CompositeNode(id, nodeLevel, nodeType, content, styling);
			}
			else
			{
				return new SimpleNode(id, nodeLevel, nodeType, content, styling);
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
	}
}
