using ICT2106WebApp.mod1Grp3;
namespace ICT2106WebApp.mod1Grp3
{
	public interface INodeUpdateNotify
	{
		Task notifyUpdatedNode(AbstractNode node);
	}
}
