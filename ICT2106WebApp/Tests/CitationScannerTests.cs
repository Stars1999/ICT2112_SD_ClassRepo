using Xunit;
using System.Collections.Generic;

public class CitationScannerTests
{
    [Fact]
    public void APACitationScanner_ShouldReturnFormattedCitations()
    {
        var scanner = new APACitationScanner();
        var citations = new List<CitationStyle>
        {
            new CitationStyle 
            { 
                CitationType = "APA", 
                Author = "John Doe",
                Date = DateTime.Parse("2021-01-01")
            }
        };


        var result = scanner.ScanCitations(citations);

        Assert.Contains("(Doe, 2021)", result);
    }

    [Fact]
    public void MLACitationScanner_ShouldReturnFormattedCitations()
    {
        var scanner = new MLACitationScanner();
        var citations = new List<CitationStyle>
        {
            new CitationStyle 
            { 
                CitationType = "MLA", // 
                Author = "Jane Smith",
                Date = DateTime.Parse("2021-01-01")
            }
        };


        var result = scanner.ScanCitations(citations);

        Assert.Contains("Smith 2020.", result);
    }
}
