using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public class HeaderExtractor : IHeaderExtractor
{
	public List<string> ExtractHeaders(WordprocessingDocument doc)
	{
		var headers = new List<string>();
		if (doc.MainDocumentPart?.HeaderParts == null)
			return headers;

		foreach (var headerPart in doc.MainDocumentPart.HeaderParts)
		{
			var header = headerPart.Header;
			if (header != null)
			{
				foreach (var para in header.Elements<Paragraph>())
				{
					string text = string.Join("", para.Descendants<Text>().Select(t => t.Text));
					if (!string.IsNullOrWhiteSpace(text))
						headers.Add(text);
				}
			}
		}
		return headers;
	}
}
