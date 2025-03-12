namespace ICT2106WebApp.Domain
{
    using iText.Kernel.Pdf;
    using iText.Kernel.Pdf.Canvas.Parser;
    using iText.Kernel.Pdf.Canvas.Parser.Listener;

    public class DocumentStructureCheck : QualityCheck
    {
        public override string MetricName => "Document Structure";

        public override void Execute(byte[] pdfContent, QualityReport report)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(pdfContent))
                using (PdfReader reader = new PdfReader(ms))
                using (PdfDocument pdfDoc = new PdfDocument(reader))
                {
                    bool hasBookmarks = pdfDoc.GetCatalog().GetPdfObject().ContainsKey(iText.Kernel.Pdf.PdfName.Outlines);
                    if (!hasBookmarks)
                        report.Issues.Add("Document lacks bookmarks or table of contents.");

                    bool isTagged = pdfDoc.IsTagged(); 
                    if (!isTagged)
                        report.Issues.Add("Document is not tagged for accessibility.");

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
                    report.IsSuccessful = (isTagged && hasBookmarks && !missingMetadata && hasHeaders);
                }
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
}
