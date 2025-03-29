namespace ICT2106WebApp.Interfaces
{
    public interface IDocumentTestCase
    {
        // Runs the test cases for the specified module and returns the results
        bool runModTestCases(int testCaseModNum, string citationFormat = null);

        // Retrieves the test status ("Pass" or "Fail") for the specified module
        string getTestStatus(int testCaseModNum);
    }
}