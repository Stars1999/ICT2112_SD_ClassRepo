using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;
using System.IO;

namespace ICT2106WebApp.Pages
{
    public class EditorModel : PageModel
    {
        private readonly ILogger<EditorModel> _logger;
        private readonly IPDFQualityChecker _pdfQualityChecker;
        private readonly PDFGenerator _pdfGenerator;

        public EditorModel(
            ILogger<EditorModel> logger,
            IPDFQualityChecker pdfQualityChecker,
            PDFGenerator pdfGenerator)
        {
            _logger = logger;
            _pdfQualityChecker = pdfQualityChecker;
            _pdfGenerator = pdfGenerator;
        }

        public QualityReport QualityReport { get; set; } = new QualityReport();

        public IActionResult OnGetCheckQuality()
        {
            try 
            {
                // Get PDF path directly
                string pdfRelativePath = _pdfGenerator.GetGeneratedPDFUrl().TrimStart('/');
                string pdfFullPath = Path.Combine(
                    Directory.GetCurrentDirectory(), 
                    "wwwroot",
                    pdfRelativePath
                );

                if (!System.IO.File.Exists(pdfFullPath))
                {
                    throw new FileNotFoundException("No PDF has been generated yet.");
                }

                var pdfContent = System.IO.File.ReadAllBytes(pdfFullPath);
                QualityReport = _pdfQualityChecker.CheckPDFQuality(pdfContent);
                
                // Use the GetQualityReportDetails method to access the encapsulated data
                var (isSuccessful, issues, metrics, qualityScore) = QualityReport.GetQualityReportDetails();
                
                return new JsonResult(new { 
                    success = true, 
                    qualityScore = qualityScore,
                    isSuccessful = isSuccessful,
                    issues = issues,
                    metrics = metrics
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