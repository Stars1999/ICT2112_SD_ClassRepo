using ICT2106WebApp.Interfaces;

namespace ICT2106WebApp.DataSource
{
    public class MockPDFProvider : IPDFProvider
    {
        public byte[] GetPDFContent()
        {
            // Return hardcoded sample PDF content
            return new byte[0];
        }
    }
}
