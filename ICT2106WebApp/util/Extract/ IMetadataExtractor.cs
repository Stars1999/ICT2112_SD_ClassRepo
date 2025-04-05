using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;

public interface IMetadataExtractor
{
	Dictionary<string, string> ExtractMetadata(WordprocessingDocument doc, string filePath);
}
