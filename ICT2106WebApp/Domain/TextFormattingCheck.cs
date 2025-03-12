using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace ICT2106WebApp.Domain
{
    public class TextFormattingCheck : QualityCheck
    {
        public override string MetricName => "Text Formatting";

        public override void Execute(byte[] pdfContent, QualityReport report)
        {
            try
            {
                using MemoryStream ms = new MemoryStream(pdfContent);
                using PdfReader reader = new PdfReader(ms);
                using PdfDocument pdfDoc = new PdfDocument(reader);
                int inconsistentFontsCount = 0;
                int inconsistentSpacingCount = 0;
                Dictionary<string, int> fontUsage = new();

                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    var page = pdfDoc.GetPage(i);
                    var text = PdfTextExtractor.GetTextFromPage(page, new SimpleTextExtractionStrategy());
                    var resources = page.GetResources();
                    var fontNames = resources.GetResourceNames(PdfName.Font);

                    if (fontNames != null)
                    {
                        foreach (var fontName in fontNames)
                        {
                            string name = fontName.ToString();
                            // Canonicalize font name: if it includes a '+' sign then use the part after it.
                            string canonicalFontName = name;
                            int plusIndex = name.IndexOf('+');
                            if (plusIndex != -1 && plusIndex < name.Length - 1)
                            {
                                canonicalFontName = name.Substring(plusIndex + 1);
                            }
                            if (!fontUsage.TryAdd(canonicalFontName, 1))
                                fontUsage[canonicalFontName]++;
                        }
                    }
                    if (text.Contains("  "))
                        inconsistentSpacingCount++;
                }

                if (fontUsage.Count > 3)
                {
                    report.Issues.Add($"Too many different fonts used: {fontUsage.Count}. Recommended: 2-3 fonts.");
                    inconsistentFontsCount++;
                }

                report.Metrics["FontCount"] = fontUsage.Count.ToString();
                report.Metrics["InconsistentSpacingPages"] = inconsistentSpacingCount.ToString();
                // Consider the check successful if neither spacing nor font issues were found.
                report.IsSuccessful = (inconsistentFontsCount == 0 && inconsistentSpacingCount == 0);
            }
            catch (Exception ex)
            {
                report.Issues.Add($"Error analyzing text formatting: {ex.Message}");
                report.IsSuccessful = false;
            }
        }
    }
}
