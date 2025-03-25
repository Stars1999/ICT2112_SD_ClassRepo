using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Text.RegularExpressions;

namespace ICT2106WebApp.Domain
{
    public class BibliographyFormatCheck : QualityCheck
    {
        public override string MetricName => "Bibliography Format";

        public override void Execute(byte[] pdfContent, QualityReport report)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(pdfContent))
                using (PdfReader reader = new PdfReader(ms))
                using (PdfDocument pdfDoc = new PdfDocument(reader))
                {
                    List<string> bibleTitles = new() { "references", "bibliography", "works cited", "literature" };
                    string biblioText = "";
                    bool foundBiblio = false;
                    int biblioPage = -1;

                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        string pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                        string lowerPageText = pageText.ToLower();
                        foreach (var title in bibleTitles)
                        {
                            if (lowerPageText.Contains(title))
                            {
                                foundBiblio = true;
                                biblioPage = i;
                                biblioText = pageText[(lowerPageText.IndexOf(title) + title.Length)..]
                                              .TrimStart();
                                break;
                            }
                        }
                        if (foundBiblio) break;
                    }

                    if (!foundBiblio)
                    {
                        report.Issues.Add("Bibliography or references section not found.");
                    }
                    else
                    {
                        int referenceCount = Regex.Matches(biblioText, @"\n").Count;
                        if (referenceCount < 2)
                            report.Issues.Add("Bibliography references seem too few or incorrectly formatted.");

                        report.Metrics["BibliographyPage"] = biblioPage.ToString();
                        report.Metrics["ReferenceCount"] = referenceCount.ToString();
                    }

                    report.IsSuccessful = !report.Issues.Any();
                }
            }
            catch (Exception ex)
            {
                report.Issues.Add($"Error analyzing bibliography format: {ex.Message}");
                report.IsSuccessful = false;
            }
        }
    }
}
