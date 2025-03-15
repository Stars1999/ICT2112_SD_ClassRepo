// // using System;
// // using System.Collections.Generic;
// // using System.Linq;
// // using DocumentFormat.OpenXml.Math; // ✅ Math Namespace
// // using DocumentFormat.OpenXml.Wordprocessing; // ✅ Wordprocessing Namespace

// // namespace Utilities
// // {
// // 	public static class MathExtractor
// // 	{
// // 		// ✅ Extract math from paragraphs (detects Unicode math symbols & OfficeMath)
// // 		public static List<Dictionary<string, object>> ExtractParagraphsWithMath(
// // 			DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph
// // 		)
// // 		{
// // 			var paragraphs = new List<Dictionary<string, object>>();

// // 			// ✅ Extract normal text from the paragraph
// // 			string text = string.Join(
// // 				"",
// // 				paragraph
// // 					.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
// // 					.Select(t => t.Text)
// // 			);

// // 			// ✅ Extract math equations inside the paragraph
// // 			var mathElements = paragraph
// // 				.Descendants<DocumentFormat.OpenXml.Math.OfficeMath>()
// // 				.ToList();
// // 			var mathContentList = new List<string>();

// // 			foreach (var mathElement in mathElements)
// // 			{
// // 				// ✅ Extract readable math content (not just MathML)
// // 				string mathText = ExtractReadableMath(mathElement);
// // 				mathContentList.Add(mathText);
// // 			}

// // 			// ✅ Store extracted text (if any)
// // 			if (!string.IsNullOrWhiteSpace(text))
// // 			{
// // 				paragraphs.Add(
// // 					new Dictionary<string, object> { { "type", "paragraph" }, { "content", text } }
// // 				);
// // 			}

// // 			// ✅ Store extracted math (if any)
// // 			foreach (var mathText in mathContentList)
// // 			{
// // 				paragraphs.Add(
// // 					new Dictionary<string, object> { { "type", "math" }, { "content", mathText } }
// // 				);
// // 			}

// // 			return paragraphs;
// // 		}

// // 		// ✅ Extract Readable Math Expression from OfficeMath
// // 		public static string ExtractReadableMath(DocumentFormat.OpenXml.Math.OfficeMath mathElement)
// // 		{
// // 			Console.WriteLine("Extract Readable Math\n");
// // 			var mathParts = new List<string>();

// // 			// ✅ Extract Fractions (m:f)
// // 			foreach (
// // 				var fraction in mathElement.Descendants<DocumentFormat.OpenXml.Math.Fraction>()
// // 			)
// // 			{
// // 				var numerator =
// // 					fraction
// // 						.Numerator?.Descendants<DocumentFormat.OpenXml.Math.Text>()
// // 						.Select(t => t.Text)
// // 						.FirstOrDefault() ?? "?";

// // 				var denominator =
// // 					fraction
// // 						.Denominator?.Descendants<DocumentFormat.OpenXml.Math.Text>()
// // 						.Select(t => t.Text)
// // 						.FirstOrDefault() ?? "?";
// // 				mathParts.Add($"({numerator}/{denominator})"); // ✅ Convert to (numerator/denominator)
// // 			}

// // 			// ✅ Extract Square Roots (m:rad)
// // 			foreach (var radical in mathElement.Descendants<DocumentFormat.OpenXml.Math.Radical>())
// // 			{
// // 				var baseElement = radical
// // 					.Elements<DocumentFormat.OpenXml.Math.Base>()
// // 					.FirstOrDefault();
// // 				var rootContent =
// // 					baseElement
// // 						?.Descendants<DocumentFormat.OpenXml.Math.Text>()
// // 						.Select(t => t.Text)
// // 						.FirstOrDefault() ?? "?"; // ✅ Use Math.Text instead of Wordprocessing.Text
// // 				mathParts.Add($"√({rootContent})"); // ✅ Convert to √(x)
// // 			}

// // 			// ✅ Extract Normal Math Text (e.g., Multiplication ×)
// // 			foreach (var run in mathElement.Descendants<DocumentFormat.OpenXml.Math.Run>())
// // 			{
// // 				string mathText = string.Join(
// // 					"",
// // 					run.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
// // 						.Select(t => t.Text)
// // 				);
// // 				if (!string.IsNullOrWhiteSpace(mathText))
// // 				{
// // 					mathParts.Add(mathText);
// // 				}
// // 			}

// // 			return mathParts.Any() ? string.Join(" ", mathParts) : "Math content not detected.";
// // 		}

// // 		// ✅ Extract MathML and readable content from OfficeMath equations
// // 		// public static Dictionary<string, object> ExtractMathEquation(DocumentFormat.OpenXml.Math.OfficeMath mathElement)
// // 		// {

// // 		// 	if (mathElement == null)
// // 		// 	{
// // 		// 		Console.WriteLine("No math element found.");
// // 		// 		return new Dictionary<string, object>
// // 		// 		{
// // 		// 			{ "type", "math" },
// // 		// 			{ "content", "No math content detected" },
// // 		// 			{ "mathML", "" }
// // 		// 		};
// // 		// 	}

// // 		// 	// ✅ Extract raw MathML XML representation
// // 		// 	string mathML = mathElement.OuterXml;
// // 		// 	Console.WriteLine("Extracted MathML: " + mathML); // Debugging

// // 		// 	// ✅ Extract readable math content
// // 		// 	string mathText = ExtractReadableMath(mathElement);
// // 		// 	Console.WriteLine("Extracted Math Text: " + mathText); // Debugging

// // 		// 	return new Dictionary<string, object>
// // 		// 	{
// // 		// 		{ "type", "math" },
// // 		// 		{ "content", mathText },
// // 		// 		{ "mathML", mathML }
// // 		// 	};
// // 		// }

// // 		// ✅ Extract Math Expressions for JSON Output
// // 		private static string ExtractMathContent(DocumentFormat.OpenXml.Math.OfficeMath mathElement)
// // 		{
// // 			var mathParts = mathElement
// // 				.Descendants<DocumentFormat.OpenXml.Math.Run>()
// // 				.Select(r =>
// // 					string.Join(
// // 						"",
// // 						r.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
// // 							.Select(t => t.Text)
// // 					)
// // 				)
// // 				.ToList();

// // 			if (!mathParts.Any())
// // 			{
// // 				Console.WriteLine("No Math text found.");
// // 				return "Math content not detected.";
// // 			}

// // 			return string.Join(" ", mathParts);
// // 		}
// // 	}
// // }

using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;
using MathOfficeMath = DocumentFormat.OpenXml.Math.OfficeMath;
using MathRun = DocumentFormat.OpenXml.Math.Run;
using MathText = DocumentFormat.OpenXml.Math.Text;
// Alias to resolve ambiguity
using WordParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using WordText = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Utilities
{
	public static class MathExtractor
	{
		public static List<Dictionary<string, object>> ExtractParagraphsWithMath(
			WordParagraph paragraph
		)
		{
			var results = new List<Dictionary<string, object>>();

			// ✅ Extract normal (non-math) text
			// string text = string.Join("", paragraph.Descendants<Text>().Select(t => t.Text));
			string text = string.Join("", paragraph.Descendants<WordText>().Select(t => t.Text));
			if (!string.IsNullOrWhiteSpace(text))
			{
				results.Add(
					new Dictionary<string, object> { { "type", "paragraph" }, { "content", text } }
				);
			}

			// ✅ Extract math equations inside the paragraph
			foreach (var mathElement in paragraph.Descendants<MathOfficeMath>())
			{
				string mathText = ExtractReadableMath(mathElement);
				results.Add(
					new Dictionary<string, object> { { "type", "math" }, { "content", mathText } }
				);
			}

			return results;
		}

		public static string ExtractReadableMath(MathOfficeMath mathElement)
		{
			return GetMathString(mathElement);
		}

		private static string GetMathString(OpenXmlElement element)
		{
			switch (element)
			{
				case Fraction fraction:
					string numerator = GetMathString(fraction.Numerator);
					string denominator = GetMathString(fraction.Denominator);
					return $"({numerator}/{denominator})";

				case Radical radical:
					var baseElement = radical.Elements<Base>().FirstOrDefault();
					string baseText = baseElement != null ? GetMathString(baseElement) : "?";
					return $"√({baseText})";

				case Nary nary:
					string naryChar = "∑";
					var naryProps = nary.GetFirstChild<NaryProperties>();
					if (naryProps?.ControlProperties?.FirstChild != null)
					{
						naryChar = naryProps.ControlProperties.FirstChild.InnerText;
					}

					// string subscript = GetMathString(nary.SubArgument);
					// string superscript = GetMathString(nary.SuperArgument);
					// string mainArg = GetMathString(nary.Argument);
					string supExpression = GetMathString(nary.SuperArgument);
					string subExpression = GetMathString(nary.SubArgument);
					string mainArg = GetMathString(nary.Elements<Base>().FirstOrDefault());
					return $"{naryChar}[{subExpression},{supExpression}]({mainArg})";
				// return $"{naryChar}[{subscript},{superscript}]({mainArg})";

				case Superscript superscript:
					string baseSup = GetMathString(superscript.Base);
					string exponent = GetMathString(superscript.SuperArgument);
					return $"{baseSup}^{exponent}";

				case Subscript subscript:
					string baseSub = GetMathString(subscript.Base);
					string sub = GetMathString(subscript.SubArgument);
					return $"{baseSub}_{sub}";

				case MathRun mathRun:
					return string.Join("", mathRun.Descendants<MathText>().Select(t => t.Text));

				// ✅ Handle Grouping Symbols (Parentheses, Brackets)
				// this is causing issue, Should be used for bracket
				// case Delimiter delimiter:
				// 	var delimiterProps = delimiter.GetFirstChild<DelimiterProperties>();
				// 	string leftDelimiter = delimiterProps?.FirstChild?.InnerText ?? "(";  // Default to "("
				// 	string rightDelimiter = delimiterProps?.LastChild?.InnerText ?? ")";  // Default to ")"

				// 	// Extract the expression inside the delimiters
				// 	string insideContent = GetMathString(delimiter.Elements<MathRun>().FirstOrDefault());

				// 	return $"{leftDelimiter}{insideContent}{rightDelimiter}";

				default:
					if (element.HasChildren)
					{
						var childStrings = new List<string>();
						foreach (var child in element.ChildElements)
						{
							childStrings.Add(GetMathString(child));
						}
						return string.Concat(childStrings);
					}
					else
					{
						return element.InnerText;
					}
			}
		}
	}

	// private static string GetMathString(OpenXmlElement element)
	// {
	//     if (element == null) return ""; // ✅ Prevents null exceptions

	//     switch (element)
	//     {
	//         case Fraction fraction:
	//             string numerator = GetMathString(fraction.Numerator ?? new Run());
	//             string denominator = GetMathString(fraction.Denominator ?? new Run());
	//             return $"({numerator}/{denominator})";

	//         case Radical radical:
	//             var baseElement = radical.Elements<Base>().FirstOrDefault();
	//             string baseText = baseElement != null ? GetMathString(baseElement) : "?";
	//             return $"√({baseText})";

	//         case Nary nary:
	//             string naryChar = "∑"; // Default to summation
	//             var naryProps = nary.GetFirstChild<NaryProperties>();
	//             if (naryProps?.ControlProperties?.FirstChild != null)
	//             {
	//                 naryChar = naryProps.ControlProperties.FirstChild.InnerText;
	//             }

	//             string subscript = GetMathString(nary.SubArgument ?? new Run());
	//             string superscript = GetMathString(nary.SuperArgument ?? new Run());
	//             string mainArg = GetMathString(nary.Elements<Base>().FirstOrDefault() ?? new Run());

	//             return $"{naryChar}[{subscript},{superscript}]({mainArg})";

	//         case Superscript superscript:
	//             string baseSup = GetMathString(superscript.Base ?? new Run());
	//             string exponent = GetMathString(superscript.SuperArgument ?? new Run());
	//             return $"{baseSup}^{exponent}";

	//         case Subscript subscript:
	//             string baseSub = GetMathString(subscript.Base ?? new Run());
	//             string sub = GetMathString(subscript.SubArgument ?? new Run());
	//             return $"{baseSub}_{sub}";

	//         case Delimiter delimiter:
	//             var delimiterProps = delimiter.GetFirstChild<DelimiterProperties>();
	//             string leftDelimiter = delimiterProps?.FirstChild?.InnerText ?? "(";
	//             string rightDelimiter = delimiterProps?.LastChild?.InnerText ?? ")";
	//             string insideContent = GetMathString(delimiter.Elements<MathRun>().FirstOrDefault() ?? new Run());
	//             return $"{leftDelimiter}{insideContent}{rightDelimiter}";

	//         case MathRun mathRun:
	//             return string.Join("", mathRun.Descendants<MathText>().Select(t => t.Text));

	//         default:
	//             if (element.HasChildren)
	//             {
	//                 var childStrings = new List<string>();
	//                 foreach (var child in element.ChildElements)
	//                 {
	//                     childStrings.Add(GetMathString(child));
	//                 }
	//                 return string.Concat(childStrings);
	//             }
	//             else
	//             {
	//                 return element.InnerText;
	//             }
	//     }
	// }
}
