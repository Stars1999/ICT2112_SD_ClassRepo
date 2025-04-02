using Utilities;

namespace ICT2106WebApp.mod1Grp3
{
	public interface IQueryUpdate
	{
		IQueryUpdateNotify queryUpdate { get; set; }

		Task saveTree(AbstractNode rootNode);
	}
}
