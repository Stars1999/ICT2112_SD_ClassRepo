using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;
using MathOfficeMath = DocumentFormat.OpenXml.Math.OfficeMath;
using MathRun = DocumentFormat.OpenXml.Math.Run;
using MathText = DocumentFormat.OpenXml.Math.Text;
using WordRun = DocumentFormat.OpenXml.Wordprocessing.Run;
using WordText = DocumentFormat.OpenXml.Wordprocessing.Text;

public class MathExtractor : IMathExtractor
{
	public List<Dictionary<string, object>> ExtractMathParagraphs(
		DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph
	)
	{
		var results = new List<Dictionary<string, object>>();

		string text = string.Join("", paragraph.Descendants<WordText>().Select(t => t.Text));
		if (!string.IsNullOrWhiteSpace(text))
		{
			results.Add(
				new Dictionary<string, object> { { "type", "paragraph" }, { "content", text } }
			);
		}

		foreach (var math in paragraph.Descendants<MathOfficeMath>())
		{
			results.Add(
				new Dictionary<string, object>
				{
					{ "type", "math" },
					{ "content", ConvertToReadableMath(math) },
				}
			);
		}

		return results;
	}

	public string ConvertToReadableMath(MathOfficeMath mathElement)
	{
		return GetMathString(mathElement);
	}

	private string GetMathString(OpenXmlElement element)
	{
		switch (element)
		{
			case Fraction f:
				return $"({GetMathString(f.Numerator)}/{GetMathString(f.Denominator)})";

			case Radical r:
				var baseEl = r.Elements<Base>().FirstOrDefault();
				return $"√({GetMathString(baseEl)})";

			case Nary n:
				string sup = GetMathString(n.SuperArgument);
				string sub = GetMathString(n.SubArgument);
				string main = GetMathString(n.Elements<Base>().FirstOrDefault());
				return $"∑[{sub},{sup}]({main})";

			case Superscript s:
				return $"{GetMathString(s.Base)}^{GetMathString(s.SuperArgument)}";

			case Subscript s:
				return $"{GetMathString(s.Base)}_{GetMathString(s.SubArgument)}";

			case MathRun r:
				return string.Join("", r.Descendants<MathText>().Select(t => t.Text));

			default:
				return element.HasChildren
					? string.Concat(element.ChildElements.Select(GetMathString))
					: element.InnerText;
		}
	}
}
