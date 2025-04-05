using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;

public interface IFooterExtractor
{
	List<string> ExtractFooters(WordprocessingDocument doc);
}
