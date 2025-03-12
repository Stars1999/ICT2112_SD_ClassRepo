using iText.Kernel.Pdf;

namespace ICT2106WebApp.Domain
{
    public class ImageResolutionCheck : QualityCheck
    {
        public override string MetricName => "Image Resolution";
        
        private const int MinimumDPI = 300;

        public override void Execute(byte[] pdfContent, QualityReport report)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(pdfContent))
                using (PdfReader reader = new PdfReader(ms))
                using (PdfDocument pdfDoc = new PdfDocument(reader))
                {
                    int lowResImageCount = 0;
                    int totalImageCount = 0;

                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        var page = pdfDoc.GetPage(i);
                        var resources = page.GetResources();
                        var xobjects = resources.GetResource(iText.Kernel.Pdf.PdfName.XObject);

                        if (xobjects == null) continue;
                        foreach (var xName in xobjects.KeySet())
                        {
                            var xObj = xobjects.GetAsStream(xName);
                            var xDict = xobjects.GetAsDictionary(xName);
                            if (xDict == null || !iText.Kernel.Pdf.PdfName.Image.Equals(xDict.Get(iText.Kernel.Pdf.PdfName.Subtype))) continue;

                            totalImageCount++;
                            float width = xDict.GetAsNumber(iText.Kernel.Pdf.PdfName.Width).FloatValue();
                            float height = xDict.GetAsNumber(iText.Kernel.Pdf.PdfName.Height).FloatValue();
                            float pageWidth = page.GetMediaBox().GetWidth();
                            float pageHeight = page.GetMediaBox().GetHeight();

                            float estimatedDPI = Math.Min(width / pageWidth, height / pageHeight) * 72; 
                            if (estimatedDPI < MinimumDPI)
                            {
                                lowResImageCount++;
                                report.Issues.Add($"Low resolution image on page {i} (~{Math.Round(estimatedDPI)} DPI).");
                            }
                        }
                    }

                    report.Metrics["TotalImages"] = totalImageCount.ToString();
                    report.Metrics["LowResolutionImages"] = lowResImageCount.ToString();
                    report.IsSuccessful = (lowResImageCount == 0);
                }
            }
            catch (Exception ex)
            {
                report.Issues.Add($"Error analyzing image resolution: {ex.Message}");
                report.IsSuccessful = false;
            }
        }
    }
}
