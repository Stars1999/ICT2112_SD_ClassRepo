using Utilities; 

namespace ICT2106WebApp.mod1Grp3
{
    public class CompletedLatex: ICompletedLatex, IQueryRetrieveNotify 
    {
        private readonly IQueryRetrieve _queryRetrieve;

        public CompletedLatex()
        {
            _queryRetrieve = (IQueryRetrieve) new DocumentGateway_RDG();
            _queryRetrieve.queryRetrieve = this;
        }

        // Retrieve Latex Tree
        public async Task<AbstractNode> RetrieveLatexTree() //idk HELP LA
        {
			AbstractNode rootNode = await _queryRetrieve.getTree();
			
			if (rootNode is CompositeNode compositeNode)
			{
				Console.WriteLine("Loaded tree is a CompositeNode!");
				// Console.WriteLine("printing tree FROM DB\n");
				// PrintTree(rootNode,0);
			}
			else
			{
				Console.WriteLine("Loaded tree is not a CompositeNode!");
			}
			return rootNode;
        }

        // Retrieve Original Tree (Non modified)
        public async Task<AbstractNode> RetrieveTree() //idk WORKING SAME AS RETRIEVELATEXTREE()
        {
			AbstractNode rootNode = await _queryRetrieve.getTree();
			
			if (rootNode is CompositeNode compositeNode)
			{
				Console.WriteLine("Loaded tree is a CompositeNode!");
				// Console.WriteLine("printing tree FROM DB\n");
				// PrintTree(rootNode,0);
			}
			else
			{
				Console.WriteLine("Loaded tree is not a CompositeNode!");
			}
			return rootNode;
        }

        public async Task notifyRetrieveTree()
        {
			Console.WriteLine($" CompletedLaTeX -> Tree Retrieved!");
		// Additional async operations if necessary
			await Task.CompletedTask; // Keeps method async-compatible
        
        }
    }
}