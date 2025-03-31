namespace ICT2106WebApp.mod1grp4
{
    // iTableCellIterator (Andrea - COMPLETED)
    public interface iTableCellIterator
    {
        public TableCell current();  // Returns the current cell
        public bool isDone();        // Checks if iteration is complete
        public void next();     // Moves to the next cell
    }
}