using System.Collections.Generic;

namespace ICT2106WebApp.mod1grp4
{
    class TableCellIterator : iTableCellIterator
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

        // private List<TableCell_SDM> _cells;
        // private int _currentIndex = 0;

        // public TableCellIterator(ITableCellCollection collection)
        // {
        //     _collection = collection;
        //     _currentIndex = 0;
        // }

        // public TableCell Current()
        // {
        //     if (IsDone()) throw new InvalidOperationException("Iterator out of bounds.");
        //     return _collection.GetCell(_currentIndex);
        // }

        // public bool IsDone()
        // {
        //     return _currentIndex >= _collection.Count;
        // }

        // public TableCell Next()
        // {
        //     if (IsDone()) throw new InvalidOperationException("No more elements.");
        //     return _collection.GetCell(_currentIndex++);
        // }

        // public void Reset()
        // {
        //     _currentIndex = 0;
        // }



        // // the collection being iterated through
        // TableCellIterator<T> collection;

        // // counters
        // private int i, j;

        // // constructor
        // public TwoDRowMajorIteratorGeneric(TableCellIterator<T> collectionIn)
        // {
        //     collection = collectionIn;
        //     i = 0;
        //     j = 0;
        // }

        // // get the current element of the iteration
        // public T Current()
        // {
        //     return collection.Get(i, j);
        // }

        // // move to the next element of the iteration
        // public void Next()
        // {
        //     j++;
        //     if (j >= collection.Columns())
        //     {
        //         j = 0;
        //         i++;
        //     }
        // }

        // // test whether or not the iteration has finished
        // public bool IsDone()
        // {
        //     return i >= collection.Rows();
        // }
    }
}
