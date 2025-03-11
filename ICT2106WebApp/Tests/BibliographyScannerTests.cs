using Xunit;
using System.Collections.Generic;

public class BibliographyScannerTests
{
    [Fact]
    public void APABibliographyScanner_ShouldReturnFormattedBibliographies()
    {
        var scanner = new APABibliographyScanner();
        var bibliographies = new List<BibliographyStyle>
        {
            new BibliographyStyle 
            { 
                BibliographyType = "APA",  // ✅ REQUIRED FIELD ADDED
                Author = "John Doe",
                Title = "AI Research",
                Publisher = "IEEE",
                Date = new System.DateTime(2021, 1, 1)
            }
        };

        var result = scanner.ScanBibliographies(bibliographies);

        Assert.Contains("Doe, J. (2021). AI Research. IEEE.", result);
    }
}
