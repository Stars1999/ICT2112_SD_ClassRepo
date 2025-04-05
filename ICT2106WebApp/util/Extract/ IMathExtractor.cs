using System.Collections.Generic;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;

public interface IMathExtractor
{
	List<Dictionary<string, object>> ExtractMathParagraphs(
		DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph
	);
	string ConvertToReadableMath(DocumentFormat.OpenXml.Math.OfficeMath mathElement);
}
