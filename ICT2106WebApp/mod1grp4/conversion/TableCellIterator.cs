namespace ICT2106WebApp.mod1grp4
{
    // TableCellIterator (Andrea - COMPLETED)
    public class TableCellIterator : iTableCellIterator
    {
        private List<TableCell> _cells;
        private int _currentIndex = 0;
        
        public TableCellIterator(List<TableCell> cells)
        {
            _cells = cells;
        }

        public TableCell current()
        {
            if (_currentIndex < _cells.Count)
            {
                return _cells[_currentIndex];
            }
            return null;
        }

        public bool isDone()
        {
            return _currentIndex >= _cells.Count;
        }

        public void next()
        {
            if (!isDone())
            {
                _currentIndex++;
            }
        }
    }
}
