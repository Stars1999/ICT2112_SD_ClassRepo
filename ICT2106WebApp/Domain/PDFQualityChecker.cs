using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;
using System.Text.RegularExpressions;
using ICT2106WebApp.Interfaces;

namespace ICT2106WebApp.Domain
{
    public class PDFQualityChecker : IPDFQualityChecker
    {
        private readonly List<QualityCheck> _qualityChecks;

        public PDFQualityChecker()
        {
            _qualityChecks = new List<QualityCheck>
            {
                new TextFormattingCheck(),
                new ImageResolutionCheck(),
                new DocumentStructureCheck(),
                new MathNotationCheck(),
                new BibliographyFormatCheck()
            };
        }

        public QualityReport CheckPDFQuality(byte[] pdfContent)
        {
            QualityReport report = new QualityReport();

            foreach (var check in _qualityChecks)
            {
                check.Execute(pdfContent, report);
            }

            int penaltyPerIssue = 5;
            int issuesCount = report.Issues.Count;
            report.QualityScore = Math.Max(0, 100 - issuesCount * penaltyPerIssue);
            report.Metrics["QualityScore"] = report.QualityScore.ToString();

            return report;
        }

        private class TextFormattingCheck : QualityCheck
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

                    if (fontUsage.Count > 4)
                    {
                        report.Issues.Add($"Too many different fonts used: {fontUsage.Count}. Recommended: 2-4 fonts.");
                        inconsistentFontsCount++;
                    }

                    report.Metrics["FontCount"] = fontUsage.Count.ToString();
                    report.Metrics["InconsistentSpacingPages"] = inconsistentSpacingCount.ToString();
                    report.IsSuccessful = (inconsistentFontsCount == 0 && inconsistentSpacingCount == 0);
                }
                catch (Exception ex)
                {
                    report.Issues.Add($"Error analyzing text formatting: {ex.Message}");
                    report.IsSuccessful = false;
                }
            }
        }

        private class ImageResolutionCheck : QualityCheck
        {
            public override string MetricName => "Image Resolution";
            private const int MinimumDPI = 300;

            public override void Execute(byte[] pdfContent, QualityReport report)
            {
                try
                {
                    using var ms = new MemoryStream(pdfContent);
                    using var reader = new PdfReader(ms);
                    using var pdfDoc = new PdfDocument(reader);
                    int lowResImageCount = 0;
                    int totalImageCount = 0;

                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        var page = pdfDoc.GetPage(i);
                        var resources = page.GetResources();
                        var xobjects = resources.GetResource(PdfName.XObject);

                        if (xobjects == null) continue;
                        foreach (var xName in xobjects.KeySet())
                        {
                            var xObj = xobjects.GetAsStream(xName);
                            var xDict = xobjects.GetAsDictionary(xName);
                            if (xDict == null || !PdfName.Image.Equals(xDict.Get(PdfName.Subtype))) continue;

                            totalImageCount++;
                            float width = xDict.GetAsNumber(PdfName.Width).FloatValue();
                            float height = xDict.GetAsNumber(PdfName.Height).FloatValue();
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
                catch (Exception ex)
                {
                    report.Issues.Add($"Error analyzing image resolution: {ex.Message}");
                    report.IsSuccessful = false;
                }
            }
        }

        private class DocumentStructureCheck : QualityCheck
        {
            public override string MetricName => "Document Structure";

            public override void Execute(byte[] pdfContent, QualityReport report)
            {
                try
                {
                    using var ms = new MemoryStream(pdfContent);
                    using var reader = new PdfReader(ms);
                    using var pdfDoc = new PdfDocument(reader);

                    bool hasBookmarks = pdfDoc.GetCatalog().GetPdfObject().ContainsKey(PdfName.Outlines);
                    if (!hasBookmarks)
                        report.Issues.Add("Document lacks bookmarks or table of contents.");

                    bool isTagged = pdfDoc.IsTagged();
                    // Commented out accessibility check for IsTagged
                    //if (!isTagged)
                    //    report.Issues.Add("Document is not tagged for accessibility.");

                    var info = pdfDoc.GetDocumentInfo();
                    bool missingMetadata = string.IsNullOrEmpty(info.GetTitle())
                                       || string.IsNullOrEmpty(info.GetAuthor())
                                       || string.IsNullOrEmpty(info.GetSubject());
                    if (missingMetadata)
                        report.Issues.Add("Document metadata is incomplete (title, author, or subject missing).");

                    bool hasHeaders = HasAppropriateHeaders(pdfDoc);
                    if (!hasHeaders)
                        report.Issues.Add("Document may lack proper headers/section titles.");

                    report.Metrics["IsTagged"] = isTagged.ToString();
                    report.Metrics["HasBookmarks"] = hasBookmarks.ToString();
                    report.Metrics["HasCompleteMetadata"] = (!missingMetadata).ToString();
                    // Modified success criteria to not require isTagged
                    report.IsSuccessful = (hasBookmarks && !missingMetadata && hasHeaders);
                }
                catch (Exception ex)
                {
                    report.Issues.Add($"Error analyzing structure: {ex.Message}");
                    report.IsSuccessful = false;
                }
            }

            private bool HasAppropriateHeaders(PdfDocument doc)
            {
                int headerCount = 0;
                int limit = Math.Min(5, doc.GetNumberOfPages());
                for (int i = 1; i <= limit; i++)
                {
                    string text = PdfTextExtractor.GetTextFromPage(doc.GetPage(i), new SimpleTextExtractionStrategy());
                    var lines = text.Split('\n');
                    for (int j = 0; j < lines.Length - 1; j++)
                    {
                        string line = lines[j].Trim();
                        string next = lines[j + 1].Trim();
                        if (line.Length > 0 && line.Length < 80 && next.Length == 0)
                            headerCount++;
                    }
                }
                return (headerCount >= 2);
            }
        }

        private class MathNotationCheck : QualityCheck
        {
            public override string MetricName => "Math Notation";

            public override void Execute(byte[] pdfContent, QualityReport report)
            {
                try
                {
                    using var ms = new MemoryStream(pdfContent);
                    using var reader = new PdfReader(ms);
                    using var pdfDoc = new PdfDocument(reader);

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

                    // Commented out accessibility check for math notation
                    //if ((containsLatex || containsMathML) && !pdfDoc.IsTagged())
                    //    report.Issues.Add("Math notation present, but document is not tagged for accessibility.");

                    if (potentialPlainTextMath > 5)
                        report.Issues.Add($"{potentialPlainTextMath} instances of plain-text math found. Use proper math formatting.");

                    report.Metrics["ContainsLatexNotation"] = containsLatex.ToString();
                    report.Metrics["ContainsMathMLNotation"] = containsMathML.ToString();
                    report.Metrics["PotentialPlainTextMath"] = potentialPlainTextMath.ToString();

                    report.IsSuccessful = !report.Issues.Any();
                }
                catch (Exception ex)
                {
                    report.Issues.Add($"Error analyzing math notation: {ex.Message}");
                    report.IsSuccessful = false;
                }
            }
        }

        private class BibliographyFormatCheck : QualityCheck
        {
            public override string MetricName => "Bibliography Format";

            public override void Execute(byte[] pdfContent, QualityReport report)
            {
                try
                {
                    using var ms = new MemoryStream(pdfContent);
                    using var reader = new PdfReader(ms);
                    using var pdfDoc = new PdfDocument(reader);

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
                                biblioText = pageText[(lowerPageText.IndexOf(title) + title.Length)..].TrimStart();
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
                catch (Exception ex)
                {
                    report.Issues.Add($"Error analyzing bibliography format: {ex.Message}");
                    report.IsSuccessful = false;
                }
            }
        }
    }
}
