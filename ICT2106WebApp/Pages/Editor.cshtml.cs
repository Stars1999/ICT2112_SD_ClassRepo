using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Domain;
using ICT2106WebApp.DataSource;

namespace ICT2106WebApp.Pages
{
    public class EditorModel : PageModel
    {
        private readonly ILogger<EditorModel> _logger;
        private readonly IPDFQualityChecker _pdfQualityChecker;
        private readonly IPDFProvider _pdfProvider;

        public EditorModel(
            ILogger<EditorModel> logger,
            IPDFQualityChecker pdfQualityChecker,
            IPDFProvider pdfProvider)
        {
            _logger = logger;
            _pdfQualityChecker = pdfQualityChecker;
            _pdfProvider = pdfProvider;
        }

        public QualityReport QualityReport { get; set; } = new QualityReport();

        public IActionResult OnGetCheckQuality()
        {
            try 
            {
                var pdfContent = _pdfProvider.GetPDFContent();
                QualityReport = _pdfQualityChecker.CheckPDFQuality(pdfContent);
                return new JsonResult(new { 
                    success = true, 
                    qualityScore = QualityReport.QualityScore,
                    isSuccessful = QualityReport.IsSuccessful,
                    issues = QualityReport.Issues,
                    metrics = QualityReport.Metrics
                });
            }
            catch (System.IO.FileNotFoundException)
            {
                return new JsonResult(new { 
                    success = false, 
                    error = "No PDF has been generated yet. Please compile your LaTeX document first." 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking PDF quality");
                return new JsonResult(new { 
                    success = false, 
                    error = "Error checking PDF quality: " + ex.Message 
                });
            }
        }

        public void OnGet()
        {
            // Initialize page data if needed
        }
    }
}