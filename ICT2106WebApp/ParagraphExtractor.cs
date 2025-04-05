using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public class ParagraphExtractor : IParagraphExtractor
{
	public Dictionary<string, object> Extract(
		Paragraph paragraph,
		WordprocessingDocument doc,
		ref bool haveBibliography
	)
	{
		var paragraphData = new Dictionary<string, object>();
		string text = string.Join("", paragraph.Descendants<Text>().Select(t => t.Text));

		string style = paragraph.ParagraphProperties?.ParagraphStyleId?.Val?.Value ?? "Normal";
		string alignment = paragraph.ParagraphProperties?.Justification?.Val?.ToString() ?? "left";

		bool isBold = paragraph.Descendants<Bold>().Any();
		bool isItalic = paragraph.Descendants<Italic>().Any();

		var styling = new Dictionary<string, object>
		{
			{ "bold", isBold },
			{ "italic", isItalic },
			{ "alignment", alignment },
			{ "fonttype", "Default Font" },
			{ "fontsize", 12 },
			{ "fontcolor", "000000" },
		};

		// Detect bibliography or citations
		if (text.StartsWith("[") || text.Contains("Reference") || text.Contains("Bibliography"))
		{
			paragraphData["type"] = "citation_run";
			paragraphData["content"] = text;
			paragraphData["styling"] = new List<object> { styling };
			haveBibliography = true;
			return paragraphData;
		}

		// Empty paragraph check
		if (string.IsNullOrWhiteSpace(text))
		{
			paragraphData["type"] = "empty_paragraph";
			paragraphData["content"] = "";
			paragraphData["styling"] = new List<object> { styling };
			return paragraphData;
		}

		// Return normal paragraph
		paragraphData["type"] = GetParagraphType(style);
		paragraphData["content"] = text;
		paragraphData["styling"] = new List<object> { styling };

		return paragraphData;
	}

	private string GetParagraphType(string style)
	{
		return style switch
		{
			"Heading1" => "h1",
			"Heading2" => "h2",
			"Heading3" => "h3",
			"Heading4" => "h4",
			_ => "paragraph",
		};
	}
}
