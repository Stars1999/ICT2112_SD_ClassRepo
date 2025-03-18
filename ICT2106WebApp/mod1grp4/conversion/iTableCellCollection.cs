namespace ICT2106WebApp.mod1grp4
{
    interface iTableCellCollection<T>
    {
        private List<TableCell_SDM> _cells;

        public TableCellCollection(List<TableCell_SDM> cells)
        {
            _cells = cells;
        }

        public iTableCellIterator CreateIterator()
        {
            return new TableCellIterator(_cells);
        }
        
        // ITableCellIterator CreateIterator(); // Factory method for iterator
        // int Count { get; } // Returns the number of cells
        // TableCell GetCell(int index); // Retrieves a cell at an index


        // // get an iterator through the array
        // iTableCellIterator<T> CreateRowMajorIterator();

        // // get the element at position (i, j)
        // T Get(int i, int j);

        // // set the element at position (i, j)
        // void Set(int i, int j, T elem);

        // // get the number of rows in the array
        // int Rows();

        // // get the number of columns in the array
        // int Columns();
    }
}
