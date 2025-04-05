using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;

public interface IHeaderExtractor
{
	List<string> ExtractHeaders(WordprocessingDocument doc);
}
