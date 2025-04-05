using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public interface IImageExtractor
{
	List<Dictionary<string, object>> ExtractImages(WordprocessingDocument doc, Drawing drawing);
}
