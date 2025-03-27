using ICT2106WebApp.Interfaces;

namespace ICT2106WebApp.DataSource
{
    public class GeneratedPDFProvider : IPDFProvider
    {
        private readonly PDFGenerator _pdfGenerator;
        
        public GeneratedPDFProvider(PDFGenerator pdfGenerator)
        {
            _pdfGenerator = pdfGenerator;
        }

        public byte[] GetPDFContent()
        {
            string relativePdfPath = _pdfGenerator.GetGeneratedPDFUrl().TrimStart('/');
            string fullPath = System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(), 
                "wwwroot",
                relativePdfPath
            );

            if (!System.IO.File.Exists(fullPath))
            {
                throw new System.IO.FileNotFoundException("No PDF has been generated yet.");
            }

            return System.IO.File.ReadAllBytes(fullPath);
        }
    }
}