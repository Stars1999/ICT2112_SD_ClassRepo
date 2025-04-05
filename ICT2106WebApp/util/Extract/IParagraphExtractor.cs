using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public interface IParagraphExtractor
{
	Dictionary<string, object> Extract(
		Paragraph paragraph,
		WordprocessingDocument doc,
		ref bool haveBibliography
	);
}
