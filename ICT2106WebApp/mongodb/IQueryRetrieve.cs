using ICT2106WebApp.mod1Grp3;

namespace ICT2106WebApp.mod1Grp3
{
	public interface IQueryRetrieve
	{
		IQueryRetrieveNotify queryRetrieve { get; set; }

		Task<AbstractNode> getTree(string collectionName);
	}
}
