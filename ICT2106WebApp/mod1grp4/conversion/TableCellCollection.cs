using System.Collections.Generic;

namespace ICT2106WebApp.mod1grp4
{
    // class TableCellCollection<T> : ITableCellCollection<T>
    class TableCellCollection : iTableCellCollection
    {

        private List<TableCell> _cells;

        public TableCellCollection(List<TableCell> cells)
        {
            _cells = cells;
        }

        public iTableCellIterator createIterator()
        {
            return new TableCellIterator(_cells);
        }

        // private List<TableCell> _cells = new();

        // public void AddCell(string content)
        // {
        //     _cells.Add(new TableCell(content));
        // }

        // public ITableCellIterator CreateIterator()
        // {
        //     return new TableCellIterator(this);
        // }

        // public int Count => _cells.Count;

        // public TableCell GetCell(int index)
        // {
        //     return index >= 0 && index < _cells.Count ? _cells[index] : null;
        // }





        // // the array
        // private T[][] array;

        // // constructor
        // public TableCellCollection(int rows, int columns)
        // {
        //     // allocate memory for the array
        //     array = new T[rows][];
        //     for (int i = 0; i < rows; i++)
        //         array[i] = new T[columns];
        // }

        // // get the element at (i, j)
        // public T Get(int i, int j)
        // {
        //     return array[i][j];
        // }

        // // set the element at (i, j)
        // public void Set(int i, int j, T elem)
        // {
        //     array[i][j] = elem;
        // }

        // // get an iterator that will traverse the array in row major order
        // public ITwoDIterator<T> CreateRowMajorIterator()
        // {
        //     return new TableCellIterator<T>(this);
        // }

        // // get the number of columns in the array
        // public int Columns()
        // {
        //     return array[0].Length;
        // }

        // // get the number of rows in the array
        // public int Rows()
        // {
        //     return array.Length;
        // }

    }
}
