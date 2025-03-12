using ICT2106WebApp.Interfaces;

namespace ICT2106WebApp.DataSource
{
    public class FilePDFProvider : IPDFProvider
    {
        private readonly string _filePath;

        public FilePDFProvider(string filePath)
        {
            _filePath = filePath;
        }

        public byte[] GetPDFContent()
        {
            return File.ReadAllBytes(_filePath);
        }
    }
}
