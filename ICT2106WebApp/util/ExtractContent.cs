using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Utilities;
using WP = DocumentFormat.OpenXml.Drawing.Wordprocessing;

namespace Utilities
{
	public static partial class ExtractContent
	{
		public static Dictionary<string, object> ExtractParagraph(
			DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph,
			WordprocessingDocument doc,
			ref bool haveBibliography
		)
		{
			string text = string.Join(
				"",
				paragraph
					.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
					.Select(t => t.Text)
			);
			string style = paragraph.ParagraphProperties?.ParagraphStyleId?.Val?.Value ?? "Normal";
			bool isBold = paragraph.Descendants<Bold>().Any();
			bool isItalic = paragraph.Descendants<Italic>().Any();
			var alignment = paragraph.ParagraphProperties?.Justification?.Val?.ToString() ?? "left";

			// ✅ Extract Font Type & Font Size from Paragraph Style
			string fontType = "Default Font";
			string? fontSizeRaw = null;
			string fontColor = "000000"; // Default to black color
			string highlightColor = "none"; // Default to no highlight

			string styleId =
				paragraph.ParagraphProperties?.ParagraphStyleId?.Val?.Value ?? "Normal";
			var stylesPart = doc.MainDocumentPart?.StyleDefinitionsPart;

			// ✅ Check if StyleDefinitionsPart exists
			if (stylesPart != null && stylesPart.Styles != null)
			{
				var paragraphStyle = stylesPart
					.Styles.Elements<Style>()
					.FirstOrDefault(s => s.StyleId == styleId);

				if (paragraphStyle != null)
				{
					fontType =
						paragraphStyle.StyleRunProperties?.RunFonts?.Ascii?.Value ?? "Default Font";
					fontSizeRaw = paragraphStyle.StyleRunProperties?.FontSize?.Val?.Value;

					// Extract color from style definition
					if (paragraphStyle.StyleRunProperties?.Color?.Val?.Value != null)
					{
						fontColor = paragraphStyle.StyleRunProperties.Color.Val.Value;
					}
				}
			}

			// ✅ Convert font size from half-points
			int fontSize = fontSizeRaw != null ? int.Parse(fontSizeRaw) / 2 : 12; // Default 12pt
			var paragraphData = new Dictionary<string, object>();

			paragraphData["alignment"] = alignment;
			// paragraphData["fontType"] = fontType;
			// paragraphData["fontSize"] = fontSize;

			var havemath = false;
			// var haveBibliography = false;
			// List<Dictionary<string, object>> mathContent = null;
			List<Dictionary<string, object>> mathContent = new List<Dictionary<string, object>>();

			// ✅ Extract Paragraph-Level Font & Size Correctly
			string paraFontType = FormatExtractor.GetParagraphFont(paragraph);
			int paraFontSize = FormatExtractor.GetParagraphFontSize(paragraph);

			var PropertiesList = new List<object>
			{
				new Dictionary<string, object>
				{
					{ "bold", isBold },
					{ "italic", isItalic },
					{ "alignment", alignment },
					{ "fontsize", fontSize },
					{ "fonttype", paraFontType },
					{ "fontcolor", fontColor },
					{ "highlight", highlightColor },
				},
			};
			// Console.WriteLine(paraFontSize);
			// Console.WriteLine(paraFontType);

			// the one below can grab as text
			// // check for internal using word. This works
			// string paraText = paragraph.InnerText.Trim();

			// // Check if it starts with "References"
			// if (paraText.StartsWith("References", StringComparison.OrdinalIgnoreCase))
			// {
			// 	Console.WriteLine("Found a references paragraph:");
			// 	Console.WriteLine(paraText);
			// }

			// Detect type of lists (will probably add more in the future)
			if (paragraph.ParagraphProperties?.NumberingProperties != null)
			{
				var numberingProperties = paragraph.ParagraphProperties.NumberingProperties;
				var numberingId = numberingProperties.NumberingId?.Val?.Value;

				Console.WriteLine($"Numbering ID: {numberingId}");

				if (numberingId != null)
				{
					// Detect the type of list based on NumberingId
					switch (numberingId)
					{
						case 1:
							// Numbered list
							Console.WriteLine("Numbered list item detected.");
							paragraphData["type"] = "numbered_list";
							break;

						case 2:
							// Bulleted list
							Console.WriteLine("Bulleted list item detected.");
							paragraphData["type"] = "bulleted_list";
							break;

						case 3:
							// Lowercase Lettered list (a, b, c, ...)
							Console.WriteLine("Lowercase Lettered list item detected.");
							paragraphData["type"] = "lowercase_lettered_list";
							break;

						case 5:
							// Roman Numeral (I, II, III, ...)
							Console.WriteLine("Roman numeral list item detected.");
							paragraphData["type"] = "roman_numeral_list";
							break;

						case 6:
							// Uppercase Lettered list (A, B, C, ...)
							Console.WriteLine("Uppercase Lettered list item detected.");
							paragraphData["type"] = "uppercase_lettered_list";
							break;

						case 8:
							// Numbered with parenthesis list 1), 2), 3), ...
							Console.WriteLine("Numbered with parenthesis list item detected.");
							paragraphData["type"] = "numbered_parenthesis_list";
							break;

						case 9:
							// Lowercase Lettered with parenthesis list a), b), c), ...
							Console.WriteLine(
								"Lowercase Lettered with parenthesis list item detected."
							);
							paragraphData["type"] = "lowercase_lettered_parenthesis_list";
							break;

						default:
							// Handle other cases if needed
							Console.WriteLine("Unrecognized list type detected.");
							paragraphData["type"] = "unknown_list";
							break;
					}

					paragraphData["content"] = text;
					paragraphData["styling"] = PropertiesList;

					return paragraphData;
				}
			}

			// ✅ Detect Page Breaks
			if (paragraph.Descendants<Break>().Any(b => b.Type?.Value == BreakValues.Page))
			{
				Console.WriteLine("Detect page break\n");
				paragraphData["type"] = "page_break";
				paragraphData["content"] = "[PAGE BREAK]";
				// paragraphData["fonttype"] = paraFontType;
				// paragraphData["fontsize"] = paraFontSize;
				paragraphData["styling"] = PropertiesList;
				return paragraphData;
			}

			// ✅ Detect Line Breaks
			if (paragraph.Descendants<Break>().Any(b => b.Type?.Value == BreakValues.TextWrapping))
			{
				Console.WriteLine("Detect line break\n");
				paragraphData["type"] = "line_break";
				paragraphData["content"] = "[LINE BREAK]";
				// paragraphData["fonttype"] = paraFontType;
				// paragraphData["fontsize"] = paraFontSize;
				paragraphData["styling"] = PropertiesList;

				return paragraphData;
			}
			if (paragraph.Descendants<DocumentFormat.OpenXml.Math.OfficeMath>().Any())
			{
				Console.WriteLine("Goes to math extractor\n");

				mathContent = MathExtractor.ExtractParagraphsWithMath(paragraph);
				havemath = true;
				paragraphData["type"] = "math";

				// var mathContent = MathExtractor.ExtractParagraphsWithMath(paragraph);
				// elements.AddRange(MathExtractor.ExtractParagraphsWithMath(paragraph)); // ✅ Extract paragraphs & Unicode math
				// return mathContent;
			}
			// ✅ Check if paragraph is completely empty
			if (
				string.IsNullOrWhiteSpace(text)
				&& !paragraph.Elements<Break>().Any()
				&& havemath == false
			)
			{
				Console.WriteLine("Completely empty\n");
				paragraphData["type"] = "empty_paragraph1";
				paragraphData["content"] = "";
				paragraphData["styling"] = PropertiesList;

				return paragraphData;
			}

			// Check for page/line breaks at the paragraph level
			if (paragraph.Descendants<Break>().Any(b => b.Type?.Value == BreakValues.Page))
			{
				Console.WriteLine("break value\n");
				return new Dictionary<string, object>
				{
					{ "type", "page_break" },
					{ "content", "[PAGE BREAK]" },
					{ "fonttype", paraFontType },
				};
			}

			if (paragraph.Descendants<Break>().Any(b => b.Type?.Value == BreakValues.TextWrapping))
			{
				Console.WriteLine("line break\n");
				return new Dictionary<string, object>
				{
					{ "type", "line_break" },
					{ "content", "[LINE BREAK]" },
				};
			}

			// Collect each run's text and formatting
			var runsList = new List<Dictionary<string, object>>();
			var runs = paragraph.Elements<Run>().ToList(); // Convert to List to get index

			for (int i = 0; i < runs.Count; i++)
			{
				var run = runs[i];
				string runText = string.Join("", run.Descendants<Text>().Select(t => t.Text));

				if (string.IsNullOrWhiteSpace(runText))
				{
					Console.WriteLine("Continue\n");
					continue; // Skip empty runs
				}

				// Extract Font Type
				string runfontType = run.RunProperties?.RunFonts?.Ascii?.Value;
				if (string.IsNullOrEmpty(runfontType))
				{
					// Fallback: Check paragraph font style if run doesn't specify one
					runfontType =
						paragraph
							.ParagraphProperties?.ParagraphMarkRunProperties?.GetFirstChild<RunFonts>()
							?.Ascii?.Value ?? "Default Font";
				}

				// string? runFontSizeRaw = run.RunProperties?.FontSize?.Val?.Value;
				// int runFontSize = runFontSizeRaw != null ? int.Parse(runFontSizeRaw) / 2 : 12; // Default to 12pt

				string? runFontSizeRaw = run.RunProperties?.FontSize?.Val?.Value;
				int runFontSize = 12; // Default to 12pt

				if (int.TryParse(runFontSizeRaw, out int parsedSize))
					runFontSize = parsedSize / 2; // Convert from half-points
												  // Extract font color from run
				string runFontColor = fontColor; // Default to paragraph color
				if (run.RunProperties?.Color?.Val?.Value != null)
					runFontColor = run.RunProperties.Color.Val.Value;
				// Extract highlight color from run
				var runHighlightColor = highlightColor; // Default to paragraph highlight
				if (run.RunProperties?.Highlight?.Val?.Value != null)
					runHighlightColor = run.RunProperties.Highlight.Val;
				// for bold
				var boldElement = run.RunProperties?.Bold;
				if (boldElement != null)
					isBold = true;
				else
					isBold = false;
				// for italic
				var italicElement = run.RunProperties?.Italic;
				if (italicElement != null)
					isItalic = true;
				else
					isItalic = false;

				if (
					PropertiesList.Count > 0
					&& PropertiesList[0] is Dictionary<string, object> firstDict
				)
				{
					string json = JsonSerializer.Serialize(
						firstDict,
						new JsonSerializerOptions { WriteIndented = true }
					);
					// Console.WriteLine("Serialized JSON:\n" + json);
					// Console.WriteLine("modifeid dict\n");
					// Console.WriteLine(runText); // Here is where Reference will apeapr

					// Convert JSON back to a dictionary (deserialize)
					var modifiedDict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

					// Modify values after deserializing
					if (modifiedDict != null)
					{
						// in runs and we need to modify it due to changes in the run portion
						modifiedDict["bold"] = isBold;
						modifiedDict["italic"] = isItalic;
						modifiedDict["alignment"] = alignment;
						modifiedDict["fontsize"] = runFontSize;
						modifiedDict["fonttype"] = runfontType;
						modifiedDict["fontcolor"] = runFontColor;
						modifiedDict["highlight"] = runHighlightColor;
						// Assign it back to PropertiesList[0]
						PropertiesList[0] = modifiedDict;

						Console.WriteLine("check the font type:");
						Console.WriteLine(runfontType);
					}
					// Print modified dictionary
					// Console.WriteLine(
					//     "Modified JSON:\n"
					//         + JsonSerializer.Serialize(
					//             PropertiesList[0],
					//             new JsonSerializerOptions { WriteIndented = true }
					//         )
					// );
				}

				if (PropertiesList[0] is Dictionary<string, object> dict)
				{
					// Now you can safely call dict.ContainsKey(...)
					if (dict.ContainsKey("fontsize") && dict.ContainsKey("fonttype"))
					{
						// Retrieve and compare values
						if (
							dict["fontsize"] is int size
							&& size == 10
							&& dict["fonttype"] is string font
							&& font == "Times New Roman"
						)
						{
							runsList.Add(
								new Dictionary<string, object>
								{
									{ "type", "citation_run" },
									{ "content", runText },
									{ "styling", PropertiesList[0] },
								}
							);
						}
						else
						{
							runsList.Add(
								new Dictionary<string, object>
								{
									{ "type", "text_run" },
									{ "content", runText },
									{ "styling", PropertiesList[0] },
								}
							);
						}
					}
				}
				else
				{
					runsList.Add(
						new Dictionary<string, object>
						{
							{ "type", "text_run" },
							{ "content", runText },
							{ "styling", PropertiesList[0] },
						}
					);
				}
			}


			string pattern = @"\b(Reference|Bibliography)\b";
			Console.WriteLine($"Total runs found: {runsList.Count}");
			Console.WriteLine($"bib:{haveBibliography}");


			Console.WriteLine("check before citation run");
			Console.WriteLine(text);
			if (text.Length > 0 && text[0] == '[')
			{

				if (runsList.Count == 1)
				{
					return new Dictionary<string, object>
					{
						{ "type", "citation_run" },
						{ "content", text },
						{ "styling", PropertiesList },
					};
				}
				// else if 
				else
				// (runsList.Count > 1)
				{
					return new Dictionary<string, object>
					{
						{ "type", "citation_run" },
						{ "content", text },
						{ "styling", PropertiesList },
					};
				}
			}
			else
			{
				haveBibliography = false;

				// Check for references and biblography
				if (
					 Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase)
					&& haveBibliography == false
				)
				{
					haveBibliography = true;
					return new Dictionary<string, object>
				{
					{ "type", "Bibliography" },
					{ "content", text },
					{ "styling", PropertiesList },
				};
				}
				else if (!runsList.Any() && havemath == false)
				{
					if (runsList.Count == 0)
					{
						return new Dictionary<string, object>
					{
						{ "type", "paragraph_run?" },
						{ "content", text },
					};
					}
					else
					{
						return new Dictionary<string, object>
					{
						{ "type", "paragraph_run" },
						{ "content", runsList },
					};
					}
				}
				else if (runsList.Count > 1)
				{
					Console.WriteLine("\nRuns > 1:");
					// to see the content.
					// foreach (var run in runsList) // `run` is a Dictionary<string, object>
					// {
					// 	Console.WriteLine("Run Details:");
					// 	foreach (var kvp in run) // `kvp` is KeyValuePair<string, object>
					// 	{
					// 		if (kvp.Value is List<object> styleObjects) // Check if value is List<object>
					// 		{
					// 			Console.WriteLine($"{kvp.Key}:");
					// 			foreach (var styleObject in styleObjects) // Iterate over list items
					// 			{
					// 				if (styleObject is Dictionary<string, object> styleDict) // Ensure it's a dictionary
					// 				{
					// 					Console.WriteLine("if styling is true:");
					// 					foreach (var styleKvp in styleDict) // Iterate dictionary key-value pairs
					// 					{
					// 						Console.WriteLine($"  - {styleKvp.Key}: {styleKvp.Value}");
					// 					}
					// 				}
					// 				else
					// 				{
					// 					Console.WriteLine($"  - Unexpected type: {styleObject.GetType()}");
					// 				}
					// 			}
					// 		}
					// 		else
					// 		{
					// 			Console.WriteLine($"{kvp.Key}: {kvp.Value}");
					// 		}
					// 	}
					// 	Console.WriteLine("------------");
					// }

					Console.WriteLine(FormatExtractor.GetParagraphType(style));
					var finalDictionary = new Dictionary<string, object>
				{
					{ "type", FormatExtractor.GetParagraphType(style) },
					{ "content", text },
					{ "runs", runsList }, // ✅ Store the entire runsList as a key-value pair
                };
					return finalDictionary;
				}
				else
				{
					Console.WriteLine("HEREEEEE:");
					if (havemath == true)
					{
						var mathstring = "";
						Console.WriteLine(
							"Getting back the result and we see what is inside the for loop\n"
						);

						foreach (var mathEntry in mathContent)
						{
							Console.WriteLine(mathEntry["content"]);
							mathstring = mathEntry["content"] + mathstring;
						}

						return new Dictionary<string, object>
					{
                        // { "type", FormatExtractor.GetParagraphType(style) },
                        { "type", "math" },
						{ "content", mathstring },
						{ "styling", PropertiesList },
					};
					}
					else
					{
						Console.WriteLine("else, else\n");
						Console.WriteLine("this is my text:");
						Console.WriteLine(text);
						Console.WriteLine("\nend of my text");

						return new Dictionary<string, object>
					{
						{ "type", FormatExtractor.GetParagraphType(style) },
						{ "content", text },
						{ "styling", PropertiesList },
					};
					}
				}
			}
		}

		//extract table
		public static Dictionary<string, object> ExtractTable(Table table)
		{
			var tableRows = new List<Dictionary<string, object>>();

			foreach (var row in table.Elements<TableRow>())
			{
				// List to hold cell dictionaries for this row.
				var cellList = new List<Dictionary<string, object>>();

				foreach (var cell in row.Elements<TableCell>())
				{
					// Extract the cell text.
					string cellText = string.Join("", cell.Descendants<Text>().Select(t => t.Text));

					// Extract basic text formatting from the first run.
					var firstRun = cell.Descendants<RunProperties>().FirstOrDefault();
					bool isBold = firstRun?.GetFirstChild<Bold>() != null;
					bool isItalic = firstRun?.GetFirstChild<Italic>() != null;

					// Create the cell dictionary in the desired format.
					var cellDict = new Dictionary<string, object>
					{
						{ "type", "Cell" },
						{ "content", cellText },
						{
							"styling",
							new Dictionary<string, object>
							{
								{ "bold", isBold },
								{ "italic", isItalic },
							}
						},
					};

					cellList.Add(cellDict);
				}

				// You can also adjust the row styling as needed.
				var rowStyling = new Dictionary<string, object>
				{
					{ "bold", false },
					{ "italic", true },
					{ "alignment", "right" },
					{ "fontsize", 12 },
					{ "fonttype", "Aptos" },
					{ "fontcolor", "0E2841" },
					{ "highlight", "none" },
				};

				// Create a row dictionary matching the desired structure.
				var rowDict = new Dictionary<string, object>
				{
					{ "type", "Row" },
					{ "content", "" },
					{ "runs", cellList },
					{ "styling", rowStyling },
				};

				tableRows.Add(rowDict);
			}

			// Create the table dictionary with type "Table" and add the row dictionaries as its "runs".
			var tableDict = new Dictionary<string, object>
			{
				{ "type", "Table" },
				{ "content", "" },
				{ "runs", tableRows },
			};

			return tableDict;
		}
	}
}
