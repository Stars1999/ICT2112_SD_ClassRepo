using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;

public interface ILayoutExtractor
{
	Dictionary<string, object> ExtractLayout(WordprocessingDocument doc);
}
