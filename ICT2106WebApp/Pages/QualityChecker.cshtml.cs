// This file handles the PDF quality checking functionality of the web application.
// It allows users to upload a PDF file, checks its quality, and displays the results.
// This is a page used to test the PDF Quality checker feature. 

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;
using System.IO;

namespace ICT2106WebApp.Pages
{
    public class QualityCheckerModel : PageModel
    {
        private readonly IPDFQualityChecker _pdfQualityChecker;

        public QualityCheckerModel(IPDFQualityChecker pdfQualityChecker)
        {
            _pdfQualityChecker = pdfQualityChecker;
        }

        [BindProperty]
        public IFormFile? PdfFile { get; set; }

        public QualityReport Report { get; set; } = new QualityReport();
        
        public bool ShowEmptyState { get; set; } = true;

        public void OnGet()
        {
            // Always show empty state initially until user uploads a PDF
            ShowEmptyState = true;
        }

        public IActionResult OnPost()
        {
            if (PdfFile != null && PdfFile.Length > 0)
            {
                ShowEmptyState = false;
                using (var ms = new MemoryStream())
                {
                    PdfFile.CopyTo(ms);
                    var pdfContent = ms.ToArray();
                    
                    // Directly use the quality checker to analyze the PDF
                    Report = _pdfQualityChecker.CheckPDFQuality(pdfContent);
                }
            }
            else
            {
                ModelState.AddModelError("PdfFile", "Please upload a valid PDF file.");
                ShowEmptyState = true;
            }

            return Page();
        }
    }
}
