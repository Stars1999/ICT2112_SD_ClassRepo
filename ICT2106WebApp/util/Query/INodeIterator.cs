using ICT2106WebApp.mod1Grp3;

namespace ICT2106WebApp.mod1Grp3
{
	public interface INodeIterator
	{
		//Method to get the current node
		AbstractNode current();

		//Method to get the next node
		AbstractNode next();

		//Method to check if the iteration is done
		bool isDone();
	}
}
