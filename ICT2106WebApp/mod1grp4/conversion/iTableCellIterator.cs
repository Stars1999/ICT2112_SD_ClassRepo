namespace ICT2106WebApp.mod1grp4
{
    interface iTableCellIterator
    {
        TableCell Current();  // Returns the current cell
        bool IsDone();        // Checks if iteration is complete
        void Next();     // Moves to the next cell

        // // get the current object in the iteration
        // T Current();

        // // advance to the next object
        // void Next();

        // // is the iteration finished
        // bool IsDone();
    }
}