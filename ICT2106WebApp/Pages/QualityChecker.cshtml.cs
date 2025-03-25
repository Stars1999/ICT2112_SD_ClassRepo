using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Domain;
using ICT2106WebApp.DataSource;

namespace ICT2106WebApp.Pages
{
    public class QualityCheckerModel : PageModel
    {
        private readonly IPDFQualityChecker _pdfQualityChecker;
        private readonly IPDFProvider _pdfProvider;
        private readonly PDFQualityRepository _repository;

        public QualityCheckerModel(IPDFQualityChecker pdfQualityChecker, IPDFProvider pdfProvider, PDFQualityRepository repository)
        {
            _pdfQualityChecker = pdfQualityChecker;
            _pdfProvider = pdfProvider;
            _repository = repository;
        }

        [BindProperty]
        public IFormFile? PdfFile { get; set; }

        public QualityReport Report { get; set; } = new QualityReport();

        public void OnGet()
        {
            // Fallback: Use default sample PDF content from provider
            var pdfContent = _pdfProvider.GetPDFContent();
            Report = _pdfQualityChecker.CheckPDFQuality(pdfContent);
            _repository.SaveReport(Report);
        }

        public IActionResult OnPost()
        {
            if (PdfFile != null && PdfFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    PdfFile.CopyTo(ms);
                    var pdfContent = ms.ToArray();
                    Report = _pdfQualityChecker.CheckPDFQuality(pdfContent);
                    _repository.SaveReport(Report);
                }
            }
            else
            {
                ModelState.AddModelError("PdfFile", "Please upload a valid PDF file.");
            }

            return Page();
        }
    }
}
