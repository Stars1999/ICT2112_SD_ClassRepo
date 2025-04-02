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
        public async Task<AbstractNode> RetrieveLatexTree() 
        {
			AbstractNode rootNode = await _queryRetrieve.getTree("latexTree"); // Retrieve the tree from the database
			
			if (rootNode is CompositeNode compositeNode)
			{
				Console.WriteLine("Loaded tree is a CompositeNode!");
			}
			else
			{
				Console.WriteLine("Loaded tree is not a CompositeNode!");
			}
			return rootNode;
        }

        // Retrieve Original Tree (Non modified)
        public async Task<AbstractNode> RetrieveTree() 
        {
			AbstractNode rootNode = await _queryRetrieve.getTree("mergewithcommentedcode");
			Console.WriteLine("Retrieving tree from DB...");
			if (rootNode is CompositeNode compositeNode)
			{
				Console.WriteLine("Loaded tree is a CompositeNode!");
			}
			else
			{
				Console.WriteLine("Loaded tree is not a CompositeNode!");
			}
			return rootNode;
        }

		//notifty when tree is retrieved from DB
        public async Task notifyRetrieveTree()
        {
			Console.WriteLine($"Tree Retrieved!");

			await Task.CompletedTask; 
        
        }
    }
}