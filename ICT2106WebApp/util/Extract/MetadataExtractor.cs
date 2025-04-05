using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;

public class MetadataExtractor : IMetadataExtractor
{
	public Dictionary<string, string> ExtractMetadata(WordprocessingDocument doc, string filepath)
	{
		var metadata = new Dictionary<string, string>();
		if (doc.PackageProperties.Title != null)
			metadata["Title"] = doc.PackageProperties.Title;
		if (doc.PackageProperties.Creator != null)
			metadata["Author"] = doc.PackageProperties.Creator;
		if (doc.PackageProperties.Created != null)
			metadata["CreatedDate_Internal"] = doc.PackageProperties.Created.Value.ToString("u");
		if (doc.PackageProperties.Modified != null)
			metadata["LastModified_Internal"] = doc.PackageProperties.Modified.Value.ToString("u");

		FileInfo fileInfo = new FileInfo(filepath);
		metadata["filename"] = fileInfo.Name;
		metadata["size"] = fileInfo.Length.ToString();

		return metadata;
	}
}
