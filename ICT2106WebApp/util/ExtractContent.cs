using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Utilities;

namespace Utilities
{

	public static class ExtractContent
	{
		public static Dictionary<string, object> ExtractParagraph
		(
			DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph,
			WordprocessingDocument doc
		)
		{
			string text = string.Join(
				"",
				paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().Select(t => t.Text)
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

            string styleId = paragraph.ParagraphProperties?.ParagraphStyleId?.Val?.Value ?? "Normal";
			var stylesPart = doc.MainDocumentPart?.StyleDefinitionsPart;

			// ✅ Check if StyleDefinitionsPart exists
			if (stylesPart != null && stylesPart.Styles != null)
			{
				var paragraphStyle = stylesPart.Styles.Elements<Style>()
					.FirstOrDefault(s => s.StyleId == styleId);

				if (paragraphStyle != null)
				{
					fontType = paragraphStyle.StyleRunProperties?.RunFonts?.Ascii?.Value ?? "Default Font";
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
					{ "italic", isItalic } ,
					{ "alignment", alignment },
					{"fontsize", fontSize},
					{"fonttype", paraFontType},
                    {"fontcolor", fontColor},
                    {"highlight", highlightColor},
                },
			};
            // Console.WriteLine(paraFontSize);
            // Console.WriteLine(paraFontType);

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
                            Console.WriteLine("Lowercase Lettered with parenthesis list item detected.");
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
			// ✅ Check if paragraph is completely empty
			if (string.IsNullOrWhiteSpace(text) && !paragraph.Elements<Break>().Any())
			{
				Console.WriteLine("Completely empty\n");

				paragraphData["type"] = "empty_paragraph1";
				paragraphData["content"] = "";
				paragraphData["styling"] = PropertiesList;

				return paragraphData;
			}

			if (paragraph.Descendants<DocumentFormat.OpenXml.Math.OfficeMath>().Any())
			{
				Console.WriteLine("Goes to math extractor\n");

				mathContent = MathExtractor.ExtractParagraphsWithMath(paragraph);
				havemath = true;
				// var mathContent = MathExtractor.ExtractParagraphsWithMath(paragraph);
				// elements.AddRange(MathExtractor.ExtractParagraphsWithMath(paragraph)); // ✅ Extract paragraphs & Unicode math
				// return mathContent;
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
					{ "content", "[LINE BREAK]" }
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
				string runfontType = run.RunProperties?.RunFonts?.Ascii?.Value ?? "Default Font";

				string? runFontSizeRaw = run.RunProperties?.FontSize?.Val?.Value;
				int runFontSize = runFontSizeRaw != null ? int.Parse(runFontSizeRaw) / 2 : 12; // Default to 12pt

                // Extract font color from run
                string runFontColor = fontColor; // Default to paragraph color
                if (run.RunProperties?.Color?.Val?.Value != null)
                {
                    runFontColor = run.RunProperties.Color.Val.Value;
                }

                // Extract highlight color from run
                var runHighlightColor = highlightColor; // Default to paragraph highlight
                if (run.RunProperties?.Highlight?.Val?.Value != null)
                {
                    runHighlightColor = run.RunProperties.Highlight.Val;
                }

                // Console.WriteLine("run run:");
                // Console.WriteLine(runText);
                // Console.WriteLine("\n");

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

				if (PropertiesList.Count > 0 && PropertiesList[0] is Dictionary<string, object> firstDict)
				{
					string json = JsonSerializer.Serialize(firstDict, new JsonSerializerOptions { WriteIndented = true });
					// Console.WriteLine("Serialized JSON:\n" + json);

					// Convert JSON back to a dictionary (deserialize)
					var modifiedDict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

					// Modify values after deserializing
					if (modifiedDict != null)
					{
						// in runs and we need to modify it due to changes in the run portion
						modifiedDict["bold"] = isBold;
						modifiedDict["italic"] = isItalic;
						modifiedDict["alignment"] = alignment;
						modifiedDict["fontsize"] = fontSize;
						modifiedDict["fonttype"] = runfontType;
                        modifiedDict["fontcolor"] = runFontColor;
                        modifiedDict["highlight"] = runHighlightColor;
                        // Assign it back to PropertiesList[0]
                        PropertiesList[0] = modifiedDict;
					}
					// Print modified dictionary
					Console.WriteLine("Modified JSON:\n" + JsonSerializer.Serialize(PropertiesList[0], new JsonSerializerOptions { WriteIndented = true }));
				}
				runsList.Add(new Dictionary<string, object>
				{
					{ "type", "text_run" },
					{ "content", runText },
					{ "styling", PropertiesList[0]}
				});
			}

			Console.WriteLine($"Total runs found: {runsList.Count}");
			if (!runsList.Any())
			{
				if (runsList.Count == 0)
				{
					return new Dictionary<string, object>
					{
						{ "type", "paragraph_run?" },
						{ "content", text }
					};
				}
				else
				{
					return new Dictionary<string, object>
					{
						{ "type", "paragraph_run" },
						{ "content", runsList }
					};
				}

			}
			else if (runsList.Count > 1)
			{
				Console.WriteLine("If there is runs\n");
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
				var finalDictionary = new Dictionary<string, object>
					{
						{ "type", FormatExtractor.GetParagraphType(style) },
						{ "content", text },
						{ "runs", runsList } // ✅ Store the entire runsList as a key-value pair
					};
				return finalDictionary;
			}
			else
			{
				// Console.WriteLine("last test case of ExtractParagraph function\n");

				if (havemath == true)
				{
					var mathstring = "";
					Console.WriteLine("Getting back the result and we see what is inside the for loop\n");

					foreach (var mathEntry in mathContent)
					{
						Console.WriteLine(mathEntry["content"]);
						mathstring = mathEntry["content"] + mathstring;
					}

					return new Dictionary<string, object>
					{
						{ "type", FormatExtractor.GetParagraphType(style) },
						{ "content", mathstring },
						{ "styling", PropertiesList}
					};
				}
				else
				{
					Console.WriteLine("else2\n");

					Console.WriteLine("this is my text:");
					Console.WriteLine(text);
					Console.WriteLine("\nend of my text");

					return new Dictionary<string, object>
					{
					{ "type", FormatExtractor.GetParagraphType(style) },
					{ "content", text },
					{ "styling", PropertiesList}

					};
				}
			}
		}



		public static Dictionary<string, object> ExtractTable
		(
			Table table
		)
		{
			var tableData = new List<List<string>>();

			foreach (var row in table.Elements<TableRow>())
			{
				var rowData = row.Elements<TableCell>()
					.Select(cell =>
						string.Join(
							"",
							cell.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
								.Select(t => t.Text)
						)
					)
					.ToList(); // ✅ Fixed ambiguous reference
				tableData.Add(rowData);
			}

			return new Dictionary<string, object> { { "type", "table" }, { "content", tableData } };
		}

		public static List<Dictionary<string, object>> ExtractImagesFromDrawing
		(
			WordprocessingDocument doc,
			DocumentFormat.OpenXml.Wordprocessing.Drawing drawing)
		{
			var imageList = new List<Dictionary<string, object>>();

			// 1. Ensure MainDocumentPart is not null
			var mainPart = doc.MainDocumentPart;
			if (mainPart == null)
			{
				Console.WriteLine("Error: MainDocumentPart is null.");
				return imageList;
			}

			// 2. Find the Blip element
			var blip = drawing.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().FirstOrDefault();
			if (blip == null)
			{
				Console.WriteLine("No Blip found in Drawing.");
				return imageList;
			}

			// 3. Get the relationship ID (embed)
			string? embed = blip.Embed?.Value;
			if (string.IsNullOrEmpty(embed))
			{
				Console.WriteLine("Embed is null or empty.");
				return imageList;
			}

			// 4. Retrieve the ImagePart using the relationship ID
			var part = mainPart.GetPartById(embed);
			if (part == null)
			{
				Console.WriteLine($"No part found for embed ID: {embed}");
				return imageList;
			}

			// 5. Cast part to ImagePart
			if (part is not ImagePart imagePart)
			{
				Console.WriteLine("Part is not an ImagePart.");
				return imageList;
			}

			// 6. Save the image locally
			string fileName = $"Image_{embed}.png";
			using (var stream = imagePart.GetStream())
			using (var fileStream = new FileStream(fileName, FileMode.Create))
			{
				stream.CopyTo(fileStream);
			}

			// 7. Add image info to the result list
			imageList.Add(new Dictionary<string, object>
			{
				{ "type", "image" },
				{ "filename", fileName }
			});

			return imageList;
		}

	}
}
