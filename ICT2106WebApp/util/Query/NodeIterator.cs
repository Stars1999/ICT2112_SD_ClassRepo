using Utilities;

namespace ICT2106WebApp.mod1Grp3
{
	public class NodeIterator : INodeIterator
	{
		private List<AbstractNode> _collection;
		private int _currentIndex;

		// Constructor to initialize the iterator with a collection of nodes
		public NodeIterator(List<AbstractNode> collection)
		{
			_collection = collection;
			_currentIndex = -1; // Start before the first element
		}

		// Method to get the current node
		public AbstractNode current()
		{
			if (_currentIndex >= 0 && _currentIndex < _collection.Count)
			{
				return _collection[_currentIndex];
			}
			return null;
		}

		// Method to get the next node
		public AbstractNode next()
		{
			if (!isDone())
			{
				_currentIndex++;
				return _collection[_currentIndex];
			}
			return null;
		}

		// Method to check if the iteration is done
		public bool isDone()
		{
			return _currentIndex >= _collection.Count - 1;
		}
	}
}
