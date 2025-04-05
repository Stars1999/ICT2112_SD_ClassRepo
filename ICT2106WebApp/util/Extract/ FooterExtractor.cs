using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public class FooterExtractor : IFooterExtractor
{
	public List<string> ExtractFooters(WordprocessingDocument doc)
	{
		var footers = new List<string>();
		if (doc.MainDocumentPart?.FooterParts == null)
			return footers;

		foreach (var footerPart in doc.MainDocumentPart.FooterParts)
		{
			var footer = footerPart.Footer;
			if (footer != null)
			{
				foreach (var para in footer.Elements<Paragraph>())
				{
					string text = string.Join("", para.Descendants<Text>().Select(t => t.Text));
					var fieldCodes = para.Descendants<FieldCode>().Select(fc => fc.Text);
					var simpleFields = para.Descendants<SimpleField>()
						.SelectMany(sf => sf.Descendants<Text>())
						.Select(t => t.Text);

					string combinedText =
						$"{text} {string.Join(" ", fieldCodes)} {string.Join(" ", simpleFields)}".Trim();

					if (!string.IsNullOrWhiteSpace(combinedText))
						footers.Add(combinedText);
				}
			}
		}
		return footers;
	}
}
