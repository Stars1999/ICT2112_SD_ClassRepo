using ICT2106WebApp.mod1Grp3;

namespace ICT2106WebApp.mod1Grp3
{
	public class CompletedLatex : ICompletedLatex, IQueryRetrieveNotify
	{
		private readonly IQueryRetrieve _queryRetrieve;

		public CompletedLatex()
		{
			_queryRetrieve = (IQueryRetrieve)new DocumentGateway_RDG();
			_queryRetrieve.queryRetrieve = this;
		}

		// Retrieve Latex Tree
		public async Task<AbstractNode> RetrieveLatexTree()
		{
			AbstractNode rootNode = await _queryRetrieve.getTree("latexTree"); // Retrieve the tree from the database
			Console.WriteLine("Retrieving tree from DB...");

			if (rootNode is CompositeNode compositeNode)
			{
				Console.WriteLine("Latex Tree Retrieved Sucessfully!");
			}
			else
			{
				Console.WriteLine("Latex Tree Retrieved Unsucessfully!");
			}
			return rootNode;
		}

		// Retrieve Original Tree (Non modified)
		public async Task<AbstractNode> RetrieveUnmodifiedTree()
		{
			AbstractNode rootNode = await _queryRetrieve.getTree("mergewithcommentedcode");
			Console.WriteLine("Retrieving tree from DB...");

			if (rootNode is CompositeNode compositeNode)
			{
				Console.WriteLine("Non Modified Tree Retrieved Sucessfully!");
			}
			else
			{
				Console.WriteLine("Non Modified Tree Retrieved Unsucessfully!");
			}
			return rootNode;
		}

		//notify when tree is retrieved from DB
		public async Task notifyRetrievedTree()
		{
			Console.WriteLine($"Tree Retrieved!");

			await Task.CompletedTask;
		}
	}
}
