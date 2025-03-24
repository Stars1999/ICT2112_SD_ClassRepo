namespace ICT2106WebApp.mod1grp4
{
    interface iTableCellIterator
    {
        TableCell current();  // Returns the current cell
        bool isDone();        // Checks if iteration is complete
        void next();     // Moves to the next cell

        // // get the current object in the iteration
        // T Current();

        // // advance to the next object
        // void Next();

        // // is the iteration finished
        // bool IsDone();
    }
}