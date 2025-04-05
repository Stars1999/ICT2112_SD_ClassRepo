using ICT2106WebApp.mod1Grp3;

namespace ICT2106WebApp.mod1Grp3
{
	public interface INodeTraverser
	{
		// Method to traverse the node
		List<AbstractNode> TraverseNode(string nodeType);

		// Method to update tree in database
		Task UpdateLatexDocument(AbstractNode rootNode);
	}
}
