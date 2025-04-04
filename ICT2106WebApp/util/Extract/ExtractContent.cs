// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text.Encodings.Web;
// using System.Text.Json;
// using System.Text.RegularExpressions;
// using System.Xml.Linq;
// using DocumentFormat.OpenXml;
// using DocumentFormat.OpenXml.Packaging;
// using DocumentFormat.OpenXml.Wordprocessing;
// using Utilities;
// using WP = DocumentFormat.OpenXml.Drawing.Wordprocessing;

// namespace Utilities
// {
// 	public static partial class ExtractContent
// 	{
// 		// for styling
// 		public static string GetRunFontType(
// 			Run run,
// 			Paragraph paragraph,
// 			WordprocessingDocument doc
// 		)
// 		{
// 			// Default font if not found
// 			string runFontType = "Default Font";

// 			// 🔹 Step 1: Check if Run specifies a font directly
// 			if (run.RunProperties?.RunFonts?.Ascii?.Value != null)
// 			{
// 				runFontType = run.RunProperties.RunFonts.Ascii.Value;
// 			}
// 			else
// 			{
// 				// 🔹 Step 2: Check Paragraph-Level Font Style
// 				runFontType =
// 					paragraph
// 						.ParagraphProperties?.ParagraphMarkRunProperties?.GetFirstChild<RunFonts>()
// 						?.Ascii?.Value ?? "Default Font";

// 				// 🔹 Step 3: Check the style definition (if paragraph has a style)
// 				string styleId = paragraph.ParagraphProperties?.ParagraphStyleId?.Val?.Value;
// 				var stylesPart = doc.MainDocumentPart?.StyleDefinitionsPart;

// 				if (
// 					!string.IsNullOrEmpty(styleId)
// 					&& stylesPart != null
// 					&& stylesPart.Styles != null
// 				)
// 				{
// 					var paragraphStyle = stylesPart
// 						.Styles.Elements<Style>()
// 						.FirstOrDefault(s => s.StyleId == styleId);
// 					if (paragraphStyle?.StyleRunProperties?.RunFonts?.Ascii?.Value != null)
// 					{
// 						runFontType = paragraphStyle.StyleRunProperties.RunFonts.Ascii.Value;
// 					}
// 				}

// 				// 🔹 Step 4: Check Document Default Font
// 				var docDefaults = stylesPart
// 					?.Styles.Elements<Style>()
// 					.FirstOrDefault(s =>
// 						s.Type?.Value == StyleValues.Paragraph && s.Default?.Value == true
// 					);
// 				if (docDefaults?.StyleRunProperties?.RunFonts?.Ascii?.Value != null)
// 				{
// 					runFontType = docDefaults.StyleRunProperties.RunFonts.Ascii.Value;
// 				}
// 			}

// 			return runFontType;
// 		}

// 		// Get list type
// 		private static string GetListType(Paragraph paragraph)
// 		{
// 			var numberingProps = paragraph.ParagraphProperties?.NumberingProperties;

// 			// Console.WriteLine("\nGetListType:");
// 			// Console.WriteLine(numberingProps);

// 			if (numberingProps != null)
// 			{
// 				// this is the type of listing
// 				var numberingId =
// 					numberingProps?.NumberingId?.Val != null
// 						? numberingProps.NumberingId.Val.Value.ToString()
// 						: "None";

// 				// this means the depth
// 				var levelId =
// 					numberingProps?.NumberingLevelReference?.Val != null
// 						? numberingProps.NumberingLevelReference.Val.Value.ToString()
// 						: "None";
// 				// Console.WriteLine($"Numbering ID: {numberingId ?? "None\n"}");
// 				// Console.WriteLine($"Level ID: {levelId ?? "None"}");
// 			}
// 			else
// 			{
// 				// Console.WriteLine("This paragraph has no numbering properties.");
// 			}

// 			if (numberingProps == null)
// 				return "Unknown";

// 			var listType = numberingProps.NumberingId?.Val?.Value switch
// 			{
// 				1 => "numbered_list",
// 				3 => "lowercase_lettered_list",
// 				9 => "lowercase_lettered_parenthesis_list",
// 				10 => "dash_bulleted_list",
// 				11 => "bulleted_list",
// 				12 => "hollow_bulleted_list",
// 				13 => "square_bulleted_list",
// 				15 => "diamond_bulleted_list",
// 				16 => "arrow_bulleted_list",
// 				17 => "checkmark_bulleted_list",
// 				18 => "numbered_parenthesis_list",
// 				19 => "roman_numeral_list",
// 				20 => "uppercase_lettered_list",
// 				21 => "lowercase_roman_numeral_list",
// 				_ => "unknown_list",
// 			};
// 			// Console.WriteLine($"List type: {listType}\n");
// 			return listType;
// 		}

// 		// extracting paragraph
// 		public static Dictionary<string, object> ExtractParagraph(
// 			DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph,
// 			WordprocessingDocument doc,
// 			ref bool haveBibliography
// 		)
// 		{
// 			string text = string.Join(
// 				"",
// 				paragraph
// 					.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
// 					.Select(t => t.Text)
// 			);
// 			string style = paragraph.ParagraphProperties?.ParagraphStyleId?.Val?.Value ?? "Normal";
// 			bool isBold = paragraph.Descendants<Bold>().Any();
// 			bool isItalic = paragraph.Descendants<Italic>().Any();
// 			var alignment = paragraph.ParagraphProperties?.Justification?.Val?.ToString() ?? "left";
// 			// ✅ Extract Font Type & Font Size from Paragraph Style
// 			string fontType = "Default Font";
// 			string? fontSizeRaw = null;
// 			string fontColor = "000000"; // Default to black color
// 			string highlightColor = "none"; // Default to no highlight

// 			string styleId =
// 				paragraph.ParagraphProperties?.ParagraphStyleId?.Val?.Value ?? "Normal";
// 			var stylesPart = doc.MainDocumentPart?.StyleDefinitionsPart;

// 			// ✅ Check if StyleDefinitionsPart exists
// 			if (stylesPart != null && stylesPart.Styles != null)
// 			{
// 				var paragraphStyle = stylesPart
// 					.Styles.Elements<Style>()
// 					.FirstOrDefault(s => s.StyleId == styleId);

// 				if (paragraphStyle != null)
// 				{
// 					fontType =
// 						paragraphStyle.StyleRunProperties?.RunFonts?.Ascii?.Value ?? "Default Font";
// 					fontSizeRaw = paragraphStyle.StyleRunProperties?.FontSize?.Val?.Value;

// 					// Extract color from style definition
// 					if (paragraphStyle.StyleRunProperties?.Color?.Val?.Value != null)
// 						fontColor = paragraphStyle.StyleRunProperties.Color.Val.Value;
// 				}
// 			}

// 			// ✅ Convert font size from half-points
// 			int fontSize = fontSizeRaw != null ? int.Parse(fontSizeRaw) / 2 : 12; // Default 12pt

// 			// Extract line spacing
// 			// Default is Multiple, 1.15x, 276 twips
// 			string lineSpacingType = "Multiple (1.15x)";
// 			double lineSpacingValue = 13.8; // 276 twips / 20 = 13.8pt

// 			if (paragraph.ParagraphProperties != null)
// 			{
// 				var spacingElement = paragraph.ParagraphProperties.SpacingBetweenLines;

// 				if (spacingElement != null)
// 				{
// 					if (spacingElement.LineRule != null)
// 					{
// 						// Retrieve initial line spacing type, "auto" for some cases
// 						lineSpacingType = spacingElement.LineRule.ToString() ?? "";
// 					}

// 					if (spacingElement.Line != null)
// 					{
// 						try
// 						{
// 							// Provided value is in twips, so need to convert to what is shown in Word
// 							int twipValue = int.Parse(spacingElement.Line.Value ?? "");
// 							lineSpacingValue = twipValue / 20.0;

// 							// Convert "auto" into actual line spacing type names
// 							if (spacingElement.LineRule == null || lineSpacingType == "auto")
// 							{
// 								switch (twipValue)
// 								{
// 									case 240:
// 										lineSpacingType = "Single";
// 										break;
// 									case 360:
// 										lineSpacingType = "1.5 lines";
// 										break;
// 									case 480:
// 										lineSpacingType = "Double";
// 										break;
// 									default:
// 										if (twipValue > 240)
// 										{
// 											lineSpacingType =
// 												$"Multiple ({lineSpacingValue / 12:0.0}x)";
// 										}
// 										break;
// 								}
// 							}
// 							//Console.WriteLine($"Extracted line spacing: {lineSpacingType}, {lineSpacingValue}");
// 						}
// 						catch (FormatException ex)
// 						{
// 							// Console.WriteLine($"Error parsing Line value: {ex.Message}");
// 							lineSpacingValue = 1.15;
// 						}
// 					}
// 				}
// 			}

// 			var paragraphData = new Dictionary<string, object>();
// 			paragraphData["alignment"] = alignment;
// 			// paragraphData["fontType"] = fontType;
// 			// paragraphData["fontSize"] = fontSize;

// 			var havemath = false;
// 			List<Dictionary<string, object>> mathContent = new List<Dictionary<string, object>>();

// 			// ✅ Extract Paragraph-Level Font & Size Correctly
// 			string paraFontType = FormatExtractor.GetParagraphFont(paragraph);
// 			int paraFontSize = FormatExtractor.GetParagraphFontSize(paragraph);

// 			var PropertiesList = new List<object>
// 			{
// 				new Dictionary<string, object>
// 				{
// 					{ "bold", isBold },
// 					{ "italic", isItalic },
// 					{ "alignment", alignment },
// 					{ "fontsize", fontSize },
// 					{ "fonttype", paraFontType },
// 					{ "fontcolor", fontColor },
// 					{ "highlight", highlightColor },
// 					{ "lineSpacingType", lineSpacingType },
// 					{ "lineSpacingValue", lineSpacingValue },
// 				},
// 			};
// 			// the one below can grab as text
// 			// // check for internal using word. This works
// 			// string paraText = paragraph.InnerText.Trim();

// 			// // Check if it starts with "References"
// 			// if (paraText.StartsWith("References", StringComparison.OrdinalIgnoreCase))
// 			// {
// 			// 	Console.WriteLine("Found a references paragraph:");
// 			// 	Console.WriteLine(paraText);
// 			// }

// 			// Detect type of lists
// 			if (paragraph.ParagraphProperties?.NumberingProperties != null)
// 			{
// 				paragraphData["type"] = GetListType(paragraph);
// 				paragraphData["content"] = text;
// 				paragraphData["styling"] = PropertiesList;
// 				return paragraphData;
// 			}

// 			// Detect Page Breaks
// 			if (paragraph.Descendants<Break>().Any(b => b.Type?.Value == BreakValues.Page))
// 			{
// 				// Console.WriteLine("Detect page break\n");
// 				paragraphData["type"] = "page_break";
// 				paragraphData["content"] = "[PAGE BREAK]";
// 				// paragraphData["fonttype"] = paraFontType;
// 				// paragraphData["fontsize"] = paraFontSize;
// 				paragraphData["styling"] = PropertiesList;
// 				return paragraphData;
// 			}

// 			// Detect Line Breaks
// 			if (paragraph.Descendants<Break>().Any(b => b.Type?.Value == BreakValues.TextWrapping))
// 			{
// 				// Console.WriteLine("Detect line break\n");
// 				paragraphData["type"] = "line_break";
// 				paragraphData["content"] = "[LINE BREAK]";
// 				// paragraphData["fonttype"] = paraFontType;
// 				// paragraphData["fontsize"] = paraFontSize;
// 				paragraphData["styling"] = PropertiesList;
// 				return paragraphData;
// 			}

// 			if (paragraph.Descendants<DocumentFormat.OpenXml.Math.OfficeMath>().Any())
// 			{
// 				// Console.WriteLine("Math extractor\n");
// 				mathContent = ExtractParagraphsWithMath(paragraph);
// 				havemath = true;
// 				paragraphData["type"] = "math";
// 			}
// 			// ✅ Check if paragraph is completely empty
// 			if (
// 				string.IsNullOrWhiteSpace(text)
// 				&& !paragraph.Elements<Break>().Any()
// 				&& havemath == false
// 			)
// 			{
// 				// Console.WriteLine("Null / white space\n");
// 				paragraphData["type"] = "empty_paragraph1";
// 				paragraphData["content"] = "";
// 				paragraphData["styling"] = PropertiesList;

// 				return paragraphData;
// 			}

// 			// Check for page/line breaks at the paragraph level
// 			if (paragraph.Descendants<Break>().Any(b => b.Type?.Value == BreakValues.Page))
// 			{
// 				// Console.WriteLine("break\n");
// 				return new Dictionary<string, object>
// 				{
// 					{ "type", "page_break" },
// 					{ "content", "[PAGE BREAK]" },
// 					{ "fonttype", paraFontType },
// 				};
// 			}

// 			if (paragraph.Descendants<Break>().Any(b => b.Type?.Value == BreakValues.TextWrapping))
// 			{
// 				// Console.WriteLine("line break\n");
// 				return new Dictionary<string, object>
// 				{
// 					{ "type", "line_break" },
// 					{ "content", "[LINE BREAK]" },
// 				};
// 			}

// 			// Collect each run's text and formatting
// 			var runsList = new List<Dictionary<string, object>>();
// 			var runs = paragraph.Elements<Run>().ToList(); // Convert to List to get index
// 			bool bracket = false;

// 			for (int i = 0; i < runs.Count; i++)
// 			{
// 				var run = runs[i];
// 				string runText = string.Join("", run.Descendants<Text>().Select(t => t.Text));

// 				if (string.IsNullOrWhiteSpace(runText))
// 				{
// 					// Console.WriteLine("Continue\n");
// 					continue;
// 				}

// 				// Extract Font Type
// 				string runfontType = run.RunProperties?.RunFonts?.Ascii?.Value;
// 				if (string.IsNullOrEmpty(runfontType))
// 				{
// 					// Fallback: Check paragraph font style if run doesn't specify one
// 					runfontType =
// 						paragraph
// 							.ParagraphProperties?.ParagraphMarkRunProperties?.GetFirstChild<RunFonts>()
// 							?.Ascii?.Value ?? "Default Font";
// 				}

// 				// string? runFontSizeRaw = run.RunProperties?.FontSize?.Val?.Value;
// 				// int runFontSize = runFontSizeRaw != null ? int.Parse(runFontSizeRaw) / 2 : 12; // Default to 12pt

// 				string? runFontSizeRaw = run.RunProperties?.FontSize?.Val?.Value;
// 				int runFontSize = 12; // Default to 12pt

// 				if (int.TryParse(runFontSizeRaw, out int parsedSize))
// 					runFontSize = parsedSize / 2; // Convert from half-points

// 				string runFontColor = fontColor;
// 				if (run.RunProperties?.Color?.Val?.Value != null)
// 					runFontColor = run.RunProperties.Color.Val.Value;

// 				// Extract highlight color from run
// 				var runHighlightColor = highlightColor; // Default to paragraph highlight
// 				if (run.RunProperties?.Highlight?.Val?.Value != null)
// 					runHighlightColor = run.RunProperties.Highlight.Val;

// 				// for bold
// 				var boldElement = run.RunProperties?.Bold;
// 				if (boldElement != null)
// 					isBold = true;
// 				else
// 					isBold = false;

// 				// for italic
// 				var italicElement = run.RunProperties?.Italic;
// 				if (italicElement != null)
// 					isItalic = true;
// 				else
// 					isItalic = false;

// 				if (
// 					PropertiesList.Count > 0
// 					&& PropertiesList[0] is Dictionary<string, object> firstDict
// 				)
// 				{
// 					string json = JsonSerializer.Serialize(
// 						firstDict,
// 						new JsonSerializerOptions { WriteIndented = true }
// 					);
// 					// Console.WriteLine("Serialized JSON:\n" + json);

// 					// Convert JSON back to a dictionary (deserialize)
// 					var modifiedDict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

// 					// Modify values after deserializing
// 					if (modifiedDict != null)
// 					{
// 						// in runs and we need to modify it due to changes in the run portion
// 						modifiedDict["bold"] = isBold;
// 						modifiedDict["italic"] = isItalic;
// 						modifiedDict["alignment"] = alignment;
// 						modifiedDict["fontsize"] = runFontSize;
// 						modifiedDict["fonttype"] = runfontType;
// 						modifiedDict["fontcolor"] = runFontColor;
// 						modifiedDict["highlight"] = runHighlightColor;
// 						// Assign it back to PropertiesList[0]
// 						PropertiesList[0] = modifiedDict;

// 						// Console.WriteLine("check the font type:");
// 						// Console.WriteLine(runfontType);
// 					}
// 					// Print modified dictionary
// 					// Console.WriteLine(
// 					//     "Modified JSON:\n"
// 					//         + JsonSerializer.Serialize(
// 					//             PropertiesList[0],
// 					//             new JsonSerializerOptions { WriteIndented = true }
// 					//         )
// 					// );
// 				}

// 				if (PropertiesList[0] is Dictionary<string, object> dict)
// 				{
// 					// get fonts and size
// 					// Console.WriteLine("\nS3");
// 					//Check font and font size
// 					string? runFontSizeRawS1 = run.RunProperties?.FontSize?.Val?.Value;
// 					int runFontSizeS1 = 12; // Default to 12pt if not found

// 					if (int.TryParse(runFontSizeRawS1, out int parsedSizeS1))
// 						runFontSizeS1 = parsedSizeS1 / 2; // Convert from half-points to standard points
// 					// Console.WriteLine($"Run Font Size: {runFontSizeS1}pt");

// 					string? paraFontSizeRawS1 = paragraph
// 						.ParagraphProperties?.ParagraphMarkRunProperties?.GetFirstChild<FontSize>()
// 						?.Val?.Value;
// 					int paraFontSizeS1 = 12;

// 					if (int.TryParse(paraFontSizeRawS1, out int paraParsedSizeS1))
// 						paraFontSizeS1 = paraParsedSizeS1 / 2; // Convert from half-points

// 					string fontTypexx = GetRunFontType(run, paragraph, doc);
// 					// Console.WriteLine($"Paragraph Font Size1: {runFontSizeS1}pt {fontTypexx}");
// 					// Console.WriteLine(runText);
// 					//end

// 					// Now you can safely call dict.ContainsKey(...)
// 					if (
// 						dict.ContainsKey("fontsize")
// 						&& dict.ContainsKey("fonttype")
// 						&& haveBibliography == true
// 					)
// 					{
// 						// Retrieve and compare values | Check if citation
// 						if (
// 							dict["fontsize"] is int size
// 							&& size == 10
// 							&& dict["fonttype"] is string font
// 							&& runfontType == "Times New Roman"
// 						)
// 						{
// 							runsList.Add(
// 								new Dictionary<string, object>
// 								{
// 									{ "temp", "citation_run" },
// 									{ "content", runText },
// 									{ "styling", PropertiesList[0] },
// 								}
// 							);
// 						}
// 					}
// 					else if (dict.ContainsKey("fontsize") && dict.ContainsKey("fonttype"))
// 					{
// 						// if (
// 						// 	runFontSizeS1 == 10 && fontTypexx == "Times New Roman"
// 						// )
// 						// {

// 						string bracketPattern = @"\([^)]*\)"; // Matches entire ( ... )
// 						string firstBracketPattern = @"\("; // Matches first "("
// 						string secondBracketPattern = @"\)"; // Matches first ")"

// 						Match bracketMatch = Regex.Match(runText, bracketPattern);
// 						Match firstBracketMatch = Regex.Match(runText, firstBracketPattern);
// 						Match secondBracketMatch = Regex.Match(runText, secondBracketPattern);

// 						// matches whole bracket
// 						if (
// 							bracketMatch.Success
// 							&& runFontSizeS1 == 10
// 							&& fontTypexx == "Times New Roman"
// 						)
// 						{
// 							// Console.WriteLine("bracket set\n");
// 							// Console.WriteLine(runFontSizeS1);
// 							// Console.WriteLine(fontTypexx);
// 							runsList.Add(
// 								new Dictionary<string, object>
// 								{
// 									{ "type", "intext-citation" },
// 									{ "content", runText },
// 									{ "styling", PropertiesList[0] },
// 								}
// 							);
// 						}
// 						// found first bracket
// 						else if (
// 							bracket == false
// 							&& firstBracketMatch.Success
// 							&& runFontSizeS1 == 10
// 							&& fontTypexx == "Times New Roman"
// 						)
// 						{
// 							runsList.Add(
// 								new Dictionary<string, object>
// 								{
// 									{ "type", "intext-citation" },
// 									{ "content", runText },
// 									{ "styling", PropertiesList[0] },
// 								}
// 							);
// 							bracket = true;
// 						}
// 						// second bracket
// 						else if (
// 							bracket == true
// 							&& secondBracketMatch.Success
// 							&& runFontSizeS1 == 10
// 							&& fontTypexx == "Times New Roman"
// 						)
// 						{
// 							runsList.Add(
// 								new Dictionary<string, object>
// 								{
// 									{ "type", "intext-citation" },
// 									{ "content", runText },
// 									{ "styling", PropertiesList[0] },
// 								}
// 							);
// 							bracket = false;
// 						}
// 						// else if got content just add
// 						else
// 						{
// 							if (bracket == true)
// 							{
// 								// runFontSizeS1 == 10 && fontTypexx == "Times New Roman"
// 								// Console.WriteLine("debug\n");
// 								// Console.WriteLine(runFontSizeS1);
// 								// Console.WriteLine(fontTypexx);
// 								// Console.WriteLine(runText);

// 								runsList.Add(
// 									new Dictionary<string, object>
// 									{
// 										{ "type", "intext-citation" },
// 										{ "content", runText },
// 										{ "styling", PropertiesList[0] },
// 									}
// 								);
// 							}
// 							else
// 							{
// 								runsList.Add(
// 									new Dictionary<string, object>
// 									{
// 										{ "type", "text_run" },
// 										{ "content", runText },
// 										{ "styling", PropertiesList[0] },
// 									}
// 								);
// 							}
// 						}
// 					}
// 				}
// 				else
// 				{
// 					runsList.Add(
// 						new Dictionary<string, object>
// 						{
// 							{ "type", "text_run" },
// 							{ "content", runText },
// 							{ "styling", PropertiesList[0] },
// 						}
// 					);
// 				}
// 			}

// 			string pattern = @"\b(Reference|Bibliography)\b";
// 			// Console.WriteLine($"Total runs found: {runsList.Count}");
// 			// Console.WriteLine($"bib:{haveBibliography}");
// 			// Console.WriteLine("check the text:");
// 			// Console.WriteLine(text);

// 			// check if it is still references / citation at the bottom
// 			if (text.Length > 0 && text[0] == '[')
// 			{
// 				return new Dictionary<string, object>
// 				{
// 					{ "type", "citation_run" },
// 					{ "content", text },
// 					{ "styling", PropertiesList },
// 				};
// 			}
// 			else
// 			{
// 				haveBibliography = false;
// 				// Check for references and biblography
// 				if (
// 					Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase)
// 					&& haveBibliography == false
// 				)
// 				{
// 					haveBibliography = true;
// 					return new Dictionary<string, object>
// 					{
// 						{ "type", "bibliography" },
// 						{ "content", text },
// 						{ "styling", PropertiesList },
// 					};
// 				}
// 				else if (!runsList.Any() && havemath == false)
// 				{
// 					if (runsList.Count == 0)
// 					{
// 						return new Dictionary<string, object>
// 						{
// 							{ "type", "paragraph_run?" },
// 							{ "content", text },
// 							{ "styling", new List<object>() }, // Ensure no comma here after the last item
// 						};
// 					}
// 					else
// 					{
// 						return new Dictionary<string, object>
// 						{
// 							{ "type", "paragraph_run" },
// 							{ "content", runsList },
// 							{ "styling", new List<object>() }, // Ensure no comma here after the last item
// 						};
// 					}
// 				}
// 				else if (runsList.Count > 1)
// 				{
// 					// Console.WriteLine("\nRuns > 1:");
// 					// to see the content.
// 					// foreach (var run in runsList) // `run` is a Dictionary<string, object>
// 					// {
// 					// 	Console.WriteLine("Run Details:");
// 					// 	foreach (var kvp in run) // `kvp` is KeyValuePair<string, object>
// 					// 	{
// 					// 		if (kvp.Value is List<object> styleObjects) // Check if value is List<object>
// 					// 		{
// 					// 			Console.WriteLine($"{kvp.Key}:");
// 					// 			foreach (var styleObject in styleObjects) // Iterate over list items
// 					// 			{
// 					// 				if (styleObject is Dictionary<string, object> styleDict) // Ensure it's a dictionary
// 					// 				{
// 					// 					Console.WriteLine("if styling is true:");
// 					// 					foreach (var styleKvp in styleDict) // Iterate dictionary key-value pairs
// 					// 					{
// 					// 						Console.WriteLine($"  - {styleKvp.Key}: {styleKvp.Value}");
// 					// 					}
// 					// 				}
// 					// 				else
// 					// 				{
// 					// 					Console.WriteLine($"  - Unexpected type: {styleObject.GetType()}");
// 					// 				}
// 					// 			}
// 					// 		}
// 					// 		else
// 					// 		{
// 					// 			Console.WriteLine($"{kvp.Key}: {kvp.Value}");
// 					// 		}
// 					// 	}
// 					// 	Console.WriteLine("------------");
// 					// }
// 					var finalDictionary = new Dictionary<string, object>
// 					{
// 						{ "type", FormatExtractor.GetParagraphType(style) },
// 						{ "content", text },
// 						{ "runs", runsList },
// 						{ "styling??", new List<object>() }, // Ensure no comma here after the last item
// 					};
// 					return finalDictionary;
// 				}
// 				else
// 				{
// 					if (havemath == true)
// 					{
// 						var mathstring = "";
// 						// Console.WriteLine(
// 						// 	"Getting back the result and we see what is inside the for loop\n"
// 						// );
// 						// Go through the math and re-assemble it back from the run
// 						foreach (var mathEntry in mathContent)
// 						{
// 							// Console.WriteLine(mathEntry["content"]);
// 							mathstring = mathEntry["content"] + mathstring;
// 						}
// 						return new Dictionary<string, object>
// 						{
// 							{ "type", "math" },
// 							{ "content", mathstring },
// 							{ "styling", PropertiesList },
// 						};
// 					}
// 					else
// 					{
// 						return new Dictionary<string, object>
// 						{
// 							{ "type", FormatExtractor.GetParagraphType(style) },
// 							{ "content", text },
// 							{ "styling", PropertiesList },
// 						};
// 					}
// 				}
// 			}
// 		}
// 	}
// }
