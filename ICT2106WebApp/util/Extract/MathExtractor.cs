// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DocumentFormat.OpenXml;
// using DocumentFormat.OpenXml.Math;
// using DocumentFormat.OpenXml.Wordprocessing;
// using MathOfficeMath = DocumentFormat.OpenXml.Math.OfficeMath;
// using MathRun = DocumentFormat.OpenXml.Math.Run;
// using MathText = DocumentFormat.OpenXml.Math.Text;
// // Alias to resolve ambiguity
// using WordParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
// using WordText = DocumentFormat.OpenXml.Wordprocessing.Text;

// namespace Utilities
// {
// 	public static class MathExtractor
// 	{
// 		public static List<Dictionary<string, object>> ExtractParagraphsWithMath(
// 			WordParagraph paragraph
// 		)
// 		{
// 			var results = new List<Dictionary<string, object>>();

// 			// ✅ Extract normal (non-math) text
// 			// string text = string.Join("", paragraph.Descendants<Text>().Select(t => t.Text));
// 			string text = string.Join("", paragraph.Descendants<WordText>().Select(t => t.Text));
// 			if (!string.IsNullOrWhiteSpace(text))
// 			{
// 				results.Add(
// 					new Dictionary<string, object> { { "type", "paragraph" }, { "content", text } }
// 				);
// 			}

// 			// ✅ Extract math equations inside the paragraph
// 			foreach (var mathElement in paragraph.Descendants<MathOfficeMath>())
// 			{
// 				string mathText = ExtractReadableMath(mathElement);
// 				results.Add(
// 					new Dictionary<string, object> { { "type", "math" }, { "content", mathText } }
// 				);
// 			}

// 			return results;
// 		}

// 		public static string ExtractReadableMath(MathOfficeMath mathElement)
// 		{
// 			return GetMathString(mathElement);
// 		}

// 		private static string GetMathString(OpenXmlElement element)
// 		{
// 			switch (element)
// 			{
// 				case Fraction fraction:
// 					string numerator = GetMathString(fraction.Numerator);
// 					string denominator = GetMathString(fraction.Denominator);
// 					return $"({numerator}/{denominator})";

// 				case Radical radical:
// 					var baseElement = radical.Elements<Base>().FirstOrDefault();
// 					string baseText = baseElement != null ? GetMathString(baseElement) : "?";
// 					return $"√({baseText})";

// 				case Nary nary:
// 					string naryChar = "∑";
// 					var naryProps = nary.GetFirstChild<NaryProperties>();
// 					if (naryProps?.ControlProperties?.FirstChild != null)
// 					{
// 						naryChar = naryProps.ControlProperties.FirstChild.InnerText;
// 					}

// 					string supExpression = GetMathString(nary.SuperArgument);
// 					string subExpression = GetMathString(nary.SubArgument);
// 					string mainArg = GetMathString(nary.Elements<Base>().FirstOrDefault());
// 					return $"{naryChar}[{subExpression},{supExpression}]({mainArg})";

// 				case Superscript superscript:
// 					string baseSup = GetMathString(superscript.Base);
// 					string exponent = GetMathString(superscript.SuperArgument);
// 					return $"{baseSup}^{exponent}";

// 				case Subscript subscript:
// 					string baseSub = GetMathString(subscript.Base);
// 					string sub = GetMathString(subscript.SubArgument);
// 					return $"{baseSub}_{sub}";

// 				case MathRun mathRun:
// 					return string.Join("", mathRun.Descendants<MathText>().Select(t => t.Text));

// 				default:
// 					if (element.HasChildren)
// 					{
// 						var childStrings = new List<string>();
// 						foreach (var child in element.ChildElements)
// 						{
// 							childStrings.Add(GetMathString(child));
// 						}
// 						return string.Concat(childStrings);
// 					}
// 					else
// 					{
// 						return element.InnerText;
// 					}
// 			}
// 		}
// 	}
// }
