namespace ICT2106WebApp.Domain
{
    public static class QualityCheckerFactory
    {
        public static PDFQualityChecker CreateDefaultChecker()
        {
            var checks = new List<QualityCheck>
            {
                new TextFormattingCheck(),
                new ImageResolutionCheck(),
                new DocumentStructureCheck(),
                new MathNotationCheck(),
                new BibliographyFormatCheck()
            };
            return new PDFQualityChecker(checks);
        }
    }
}
