using Utilities;

namespace ICT2106WebApp.mod1Grp3
{
	public interface ICompletedLatex
	{
		// Method to get the completed latex string
		Task<AbstractNode> RetrieveLatexTree();

		// Method to get the original tree
		Task<AbstractNode> RetrieveTree();
	}
}
