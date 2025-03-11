// using Xunit;
// using System.Collections.Generic;

// public class BibliographyScannerTests
// {
//     [Fact]
//     public void APABibliographyScanner_ShouldReturnFormattedBibliographies()
//     {
//         var scanner = new APABibliographyScanner();
//         var bibliographies = new List<BibliographyStyle>
//         {
//             new BibliographyStyle 
//             { 
//                 BibliographyType = "APA", 
//                 Author = "John Doe",
//                 Title = "AI Research",
//                 Publisher = "IEEE",
//                 DateString = "2021-01-01" // ✅ FIX: Use DateString instead of Date
//             }
//         };

//         var result = scanner.ScanBibliographies(bibliographies);

//         Assert.Contains("John Doe (2021). AI Research. IEEE.", result);
//     }
// }
