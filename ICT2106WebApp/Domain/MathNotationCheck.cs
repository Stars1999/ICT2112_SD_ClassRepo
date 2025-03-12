using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Text;
using System.Text.RegularExpressions;

namespace ICT2106WebApp.Domain
{
    public class MathNotationCheck : QualityCheck
    {
        public override string MetricName => "Math Notation";

        public override void Execute(byte[] pdfContent, QualityReport report)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(pdfContent))
                using (PdfReader reader = new PdfReader(ms))
                using (PdfDocument pdfDoc = new PdfDocument(reader))
                {
                    int potentialPlainTextMath = 0;
                    bool containsLatex = false;
                    bool containsMathML = false;

                    StringBuilder allText = new();
                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        allText.Append(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
                    }
                    string content = allText.ToString();

                    potentialPlainTextMath += Regex.Matches(content, @"[a-zA-Z]\s*[\+\-\*\/\=]\s*[a-zA-Z0-9]").Count;
                    potentialPlainTextMath += Regex.Matches(content, @"[0-9]\s*\/\s*[0-9]").Count;

                    containsLatex = content.Contains("\\begin{equation}")
                                 || content.Contains("\\sum")
                                 || content.Contains("\\frac")
                                 || content.Contains("\\alpha");

                    containsMathML = content.Contains("<math")
                                  || content.Contains("<mrow")
                                  || content.Contains("<msub")
                                  || content.Contains("<mi");

                    if (containsLatex && containsMathML)
                        report.Issues.Add("Document mixes LaTeX and MathML notations.");

                    if ((containsLatex || containsMathML) && !pdfDoc.IsTagged())
                        report.Issues.Add("Math notation present, but document is not tagged for accessibility.");

                    if (potentialPlainTextMath > 5)
                        report.Issues.Add($"{potentialPlainTextMath} instances of plain-text math found. Use proper math formatting.");

                    report.Metrics["ContainsLatexNotation"] = containsLatex.ToString();
                    report.Metrics["ContainsMathMLNotation"] = containsMathML.ToString();
                    report.Metrics["PotentialPlainTextMath"] = potentialPlainTextMath.ToString();

                    report.IsSuccessful = !report.Issues.Any();
                }
            }
            catch (Exception ex)
            {
                report.Issues.Add($"Error analyzing math notation: {ex.Message}");
                report.IsSuccessful = false;
            }
        }
    }
}
