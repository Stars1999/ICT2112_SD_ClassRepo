namespace ICT2106WebApp.mod1grp4
{
    // TableCellCollection (Andrea - COMPLETED)
    public class TableCellCollection : iTableCellCollection
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
    }
}
