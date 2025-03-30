using Utilities;

namespace ICT2106WebApp.mod1Grp3
{
    public class NodeIterator : INodeIterator
    {
        private List<AbstractNode> _collection;
        private int _currentIndex;

        public NodeIterator (List<AbstractNode> collection)
        {
            _collection = collection;
            _currentIndex = -1;
        }

        public AbstractNode current()
        {
            if (_currentIndex >= 0 && _currentIndex < _collection.Count)
            {
                return _collection[_currentIndex];
            }
            return null;
        }

        public AbstractNode next()
        {
            if (!isDone())
            {
                _currentIndex++;
                return _collection[_currentIndex];
            }
            return null;
        }

        public bool isDone()
        {
            return _currentIndex >= _collection.Count - 1;
        }
    }
}