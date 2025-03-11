// using Xunit;
// using System.Collections.Generic;

// public class CitationScannerTests
// {
//     [Fact]
//     public void APACitationScanner_ShouldReturnFormattedCitations()
//     {
//         var scanner = new APACitationScanner();
//         var citations = new List<CitationStyle>
//         {
//             new CitationStyle 
//             { 
//                 CitationType = "APA", 
//                 Author = "John Doe",
//                 DateString = "2021-01-01" // ✅ Change this
//             }
//         };

//         var result = scanner.ScanCitations(citations);

//         Assert.Contains("(John Doe, 2021)", result);
//     }

//     [Fact]
//     public void MLACitationScanner_ShouldReturnFormattedCitations()
//     {
//         var scanner = new MLACitationScanner();
//         var citations = new List<CitationStyle>
//         {
//             new CitationStyle 
//             { 
//                 CitationType = "MLA", 
//                 Author = "Jane Smith",
//                 DateString = "2020-06-15" // ✅ Change this
//             }
//         };

//         var result = scanner.ScanCitations(citations);

//         Assert.Contains("Smith, J. \"AI Research\". 2020.", result);
//     }
// }
   
