using Utilities;

namespace ICT2106WebApp.mod1Grp3
{
	public interface ITreeUpdateNotify
	{
		Task NotifyUpdatedTree(AbstractNode node);
	}
}