using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ICT2106WebApp.mod1Grp3;
using Microsoft.Extensions.Options;
using MongoDB.Bson; // Bson - Binary JSON
// MongoDB packages
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using Utilities;
using MathOfficeMath = DocumentFormat.OpenXml.Math.OfficeMath;
using MathRun = DocumentFormat.OpenXml.Math.Run;
using MathText = DocumentFormat.OpenXml.Math.Text;
using SixImage = SixLabors.ImageSharp.Image;
using SixPixelFormats = SixLabors.ImageSharp.PixelFormats;
using WordBreak = DocumentFormat.OpenXml.Wordprocessing.Break;
// Alias to resolve ambiguity
using WordParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using WordRun = DocumentFormat.OpenXml.Wordprocessing.Run;
using WordStyle = DocumentFormat.OpenXml.Wordprocessing.Style;
using WordText = DocumentFormat.OpenXml.Wordprocessing.Text;
using WPStyleValues = DocumentFormat.OpenXml.Wordprocessing.StyleValues;

namespace Utilities
{
	public class DocumentProcessors : IDocumentUpdateNotify
		// :
		// Iapi,
		// iCreateNode,
		// iDocument, // not sure if it there is such interface
			// IDocumentRetrieve // iDocumentRetrival,
			// iDocumentUpdate,
			// iDocumentUpdateNotify,
		// iLogger
	{
		private readonly IDocumentUpdate _dbGateway;
		private readonly Docx docxEntity;

		private static readonly string jsonOutputPath = "output.json";
		private static readonly string filePath = "Datarepository_zx_v4.docx";

		public string jsonString { get; set; }
		public JArray documentArray { get; set; }

		public DocumentProcessors()
		{
			_dbGateway = (IDocumentUpdate) new DocumentGateway_RDG();
			_dbGateway.docxUpdate = this;
		}

		// Interface
			// IDocumentUpdateNotify
	public async Task notifyUpdatedDocument(Docx docx)
	{
		// Console.WriteLine($"DocumentControl -> Notify Document updated: {docx.Title}");
		Console.WriteLine($"DocumentControl -> Notify Document updated: {docx.GetDocxAttributeValue("title")}");
		// Additional async operations if necessary
		await Task.CompletedTask; // Keeps method async-compatible
	}

	// To save local document
	public async Task saveDocumentToDatabase(string filePath)
	{
		// Validate file exists
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException($"File not found: {filePath}");
		}

		// Validate it's a .docx file
		if (!filePath.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException("File is not a .docx format");
		}

		try
		{
			// Read file into byte array
			byte[] fileData = await File.ReadAllBytesAsync(filePath);

			// Create Docx object
			// var docx = new Docx
			// {
			// 	Title = Path.GetFileNameWithoutExtension(filePath),
			// 	FileName = Path.GetFileName(filePath),
			// 	ContentType =
			// 		"application/vnd.openxmlformats-officedocument.wordprocessingml.document",
			// 	UploadDate = DateTime.UtcNow,
			// 	FileData = fileData,
			// };
			var docx = docxEntity.CreateDocx(Path.GetFileNameWithoutExtension(filePath),Path.GetFileName(filePath), fileData);
			// Check if _docxUpdate is null or not initialized
			// if (_docxUpdate == null)
			// {
			// 	Console.WriteLine("Error: _docxUpdate is not initialized.");
			// 	return;
			// }

			// Use RDG method to save document
			await _dbGateway.saveDocument(docx);

			// Console.WriteLine($"DocumentControl -> Document saved: {docx.Title}");
			Console.WriteLine($"DocumentControl -> Document saved: {docx.GetDocxAttributeValue("title")}");

		}
		catch (Exception ex)
		{
			Console.WriteLine($"DocumentControl -> Error saving document: {ex.Message}");
			Console.WriteLine(ex.StackTrace); // Log the stack trace to help identify where the issue occurred
			throw;
		}
	}

	public async Task saveJsonToDatabase(string filePath)
	{
		Console.WriteLine("DocumentControl -> saving json");
		await _dbGateway.saveJsonFile(filePath);
	}

		// Method: Return full path
		private static string ReturnFullFilePath(string fileName)
		{
			string currentDir = Directory.GetCurrentDirectory();
			return Path.Combine(currentDir, fileName);
		}

		private static List<string> ExtractHeaders(WordprocessingDocument doc)
		{
			var headers = new List<string>();

			// ✅ Check if MainDocumentPart is null
			if (doc.MainDocumentPart == null)
			{
				Console.WriteLine("Error: MainDocumentPart is null.");
				return headers;
			}

			// ✅ Check if HeaderParts exist
			if (!doc.MainDocumentPart.HeaderParts.Any())
			{
				Console.WriteLine("No headers found in the document.");
				return headers;
			}

			foreach (var headerPart in doc.MainDocumentPart.HeaderParts)
			{
				var header = headerPart.Header;

				if (header != null)
				{
					foreach (
						var paragraph in header.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
					)
					{
						// ✅ Extract normal text from headers
						string text = string.Join(
							"",
							paragraph
								.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
								.Select(t => t.Text)
						);

						if (!string.IsNullOrWhiteSpace(text))
						{
							headers.Add(text);
						}
					}
				}
			}
			return headers;
		}

		/* Footer below. But need to fix the page number not being picked up*/
		public static List<string> ExtractFooters(WordprocessingDocument doc)
		{
			var footers = new List<string>();

			// ✅ Check if MainDocumentPart is null
			if (doc.MainDocumentPart == null)
			{
				Console.WriteLine("Error: MainDocumentPart is null.");
				return footers;
			}

			// ✅ Check if FooterParts exist
			if (!doc.MainDocumentPart.FooterParts.Any())
			{
				Console.WriteLine("No footers found in the document.");
				return footers;
			}

			foreach (var footerPart in doc.MainDocumentPart.FooterParts)
			{
				var footer = footerPart.Footer;

				if (footer != null)
				{
					foreach (
						var paragraph in footer.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
					)
					{
						// ✅ Extract normal text
						string text = string.Join(
							"",
							paragraph
								.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
								.Select(t => t.Text)
						);

						// ✅ Extract FieldCode elements (e.g., { PAGE })
						var fieldCodes = paragraph
							.Descendants<DocumentFormat.OpenXml.Wordprocessing.FieldCode>()
							.Select(fc => fc.Text);

						// ✅ Extract SimpleField elements (for dynamic fields like page numbers)
						var simpleFields = paragraph
							.Descendants<DocumentFormat.OpenXml.Wordprocessing.SimpleField>()
							.SelectMany(sf =>
								sf.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
							)
							.Select(t => t.Text);

						// ✅ Combine extracted text
						string combinedText =
							$"{text} {string.Join(" ", fieldCodes)} {string.Join(" ", simpleFields)}".Trim();

						if (!string.IsNullOrWhiteSpace(combinedText))
						{
							footers.Add(combinedText);
						}
					}
				}
			}

			return footers;
		}

		public static List<Dictionary<string, object>> ExtractImagesFromDrawing(
			WordprocessingDocument doc,
			DocumentFormat.OpenXml.Wordprocessing.Drawing drawing
		)
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
				// Console.WriteLine("No Blip found in Drawing.");
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
			// 1. Define the folder path (relative or absolute).
			//    Here, we create a subfolder called "Images" in the current directory.
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

			// 2. Make sure the folder exists (this call is safe if it already exists).
			Directory.CreateDirectory(folderPath);

			// 3. Build the full file path with Path.Combine.
			string fileName = Path.Combine(folderPath, $"Image_{embed}.png");
			// string fileName = $"/image/Image_{embed}.png";
			using (var stream = imagePart.GetStream())
			using (var fileStream = new FileStream(fileName, FileMode.Create))
			{
				stream.CopyTo(fileStream);
			}

			// 7. Extract image dimensions from the XML (EMUs)
			// For inline images, the extent is usually found in wp:extent element.
			long cx = 0;
			long cy = 0;
			var inline = drawing
				.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline>()
				.FirstOrDefault();

			if (inline != null && inline.Extent != null)
			{
				cx = inline.Extent.Cx;
				cy = inline.Extent.Cy;
			}
			else
			{
				// For floating images, look in wp:anchor
				var anchor = drawing
					.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Anchor>()
					.FirstOrDefault();
				if (anchor != null && anchor.Extent != null)
				{
					cx = anchor.Extent.Cx;
					cy = anchor.Extent.Cy;
				}
			}

			// Convert EMUs to pixels (1 pixel = 9525 EMUs at 96 DPI)
			double widthPixels = cx / 9525.0;
			double heightPixels = cy / 9525.0;

			// 8. Get image resolution (DPI) using System.Drawing
			double horizontalResolution = 0;
			double verticalResolution = 0;
			try
			{
				// using (var img = Image.FromFile(fileName))
				// using (var img = System.Drawing.Image.FromFile(fileName))
				using (var img = SixImage.Load<SixPixelFormats.Rgba32>(fileName))
				{
					// horizontalResolution = img.HorizontalResolution;
					// verticalResolution = img.VerticalResolution;
					horizontalResolution = img.Metadata.HorizontalResolution;
					verticalResolution = img.Metadata.VerticalResolution;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error reading image resolution [step 8]: " + ex.Message);
			}

			// 9. Determine image format from the ImagePart ContentType
			string imageFormat = imagePart.ContentType.ToLower() switch
			{
				"image/png" => "PNG",
				"image/jpeg" or "image/jpg" => "JPG",
				"image/gif" => "GIF",
				_ => "Unknown",
			};

			// 10. Get image alignment from the parent Paragraph (if available)
			// string alignment = "Not specified (?)";
			// var parentParagraph = drawing.Ancestors<Paragraph>().FirstOrDefault();
			// if (parentParagraph != null && parentParagraph.ParagraphProperties?.Justification != null)
			// {
			// 	// The Justification value is an enum (e.g., left, center, right, both)
			// 	// alignment = parentParagraph.ParagraphProperties.Justification.Val.Value;
			// 	Console.WriteLine(parentParagraph.ParagraphProperties.Justification.Val);
			// 	alignment = parentParagraph.ParagraphProperties.Justification.Val.ToString();
			// }

			// 10. Get image alignment from the parent Paragraph (if available)
			string alignment = "Left Align (Ctrl + L)";
			var parentParagraph = drawing.Ancestors<WordParagraph>().FirstOrDefault();
			if (
				parentParagraph != null
				&& parentParagraph.ParagraphProperties?.Justification != null
				&& parentParagraph.ParagraphProperties.Justification.Val != null
			)
			{
				// Extract the underlying enum value
				var justValue = parentParagraph.ParagraphProperties.Justification.Val.Value;

				if (justValue == DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Left)
				{
					alignment = "Left Align (Ctrl + L)";
				}
				else if (
					justValue == DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Center
				)
				{
					alignment = "Center Align (Ctrl + E)";
				}
				else if (
					justValue == DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Right
				)
				{
					alignment = "Right Align (Ctrl + R)";
				}
				else if (
					justValue == DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Both
				)
				{
					alignment = "Justify (Ctrl + J)";
				}
				else
				{
					alignment = justValue.ToString();
				}
			}
			// Console.WriteLine("Image Alignment: " + alignment);

			// 11.Get image position(for floating images)
			string imagePosition = "Inline (position determined by text flow)";
			// var anchorElement = drawing.Descendants<WP.Anchor>().FirstOrDefault();
			var anchorElement = drawing
				.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Anchor>()
				.FirstOrDefault();

			if (anchorElement != null)
			{
				// var posH = anchorElement.PositionH;
				// var posV = anchorElement.PositionV;
				var horizontalPosElem = anchorElement.HorizontalPosition;
				var verticalPosElem = anchorElement.VerticalPosition;

				string horizontalPos = horizontalPosElem?.PositionOffset?.Text ?? "Not specified";
				string verticalPos = verticalPosElem?.PositionOffset?.Text ?? "Not specified";
				imagePosition =
					$"Horizontal Offset: {horizontalPos}, Vertical Offset: {verticalPos}";
			}

			// 12. Add all the extracted information into a dictionary
			var imageInfo = new Dictionary<string, object>
			{
				{ "type", "image" },
				{ "content", fileName },
				{
					"styling",
					new Dictionary<string, object>
					{
						{ "widthEMU", cx },
						{ "heightEMU", cy },
						{ "widthPixels", widthPixels },
						{ "heightPixels", heightPixels },
						{ "horizontalResolution", horizontalResolution },
						{ "verticalResolution", verticalResolution },
						{ "format", imageFormat },
						{ "alignment", alignment },
						{ "position", imagePosition },
					}
				},
			};
			imageList.Add(imageInfo);
			return imageList;
		}

		// get my meta data
		public static Dictionary<string, string> GetDocumentMetadata(
			WordprocessingDocument doc,
			string filepath
		)
		{
			var metadata = new Dictionary<string, string>();
			if (doc.PackageProperties.Title != null)
				metadata["Title"] = doc.PackageProperties.Title;
			if (doc.PackageProperties.Creator != null)
				metadata["Author"] = doc.PackageProperties.Creator;

			// Created & Modified (from the DOCX metadata, not the OS timestamps)
			if (doc.PackageProperties.Created != null)
				metadata["CreatedDate_Internal"] = doc.PackageProperties.Created.Value.ToString(
					"u"
				);
			if (doc.PackageProperties.Modified != null)
				metadata["LastModified_Internal"] = doc.PackageProperties.Modified.Value.ToString(
					"u"
				);

			FileInfo fileInfo = new FileInfo(filepath);

			string fileName = fileInfo.Name; // "Example.docx"
			long fileSize = fileInfo.Length; // size in bytes

			metadata["filename"] = fileName;
			metadata["size"] = fileSize.ToString();

			Console.WriteLine(metadata);
			return metadata;
		}

		// Extract layout
		public static object ExtractLayout(WordprocessingDocument wordDoc)
		{
			var layoutInfo = GetDocumentLayout(wordDoc);
			// Create layout element
			var layoutElement = new Dictionary<string, object>
			{
				{ "type", "layout" },
				{ "content", "" },
				{
					"styling",
					new List<object> { layoutInfo }
				},
			};

			return layoutElement;
		}

		public static object elementRoot()
		{
			var elementRoot = new Dictionary<string, object>
			{
				{ "id", 0 },
				{ "type", "root" },
				{ "content", "" },
			};
			return elementRoot;
		}

		// for styling
		public static string GetRunFontType(
			WordRun run,
			WordParagraph paragraph,
			WordprocessingDocument doc
		)
		{
			// Default font if not found
			string runFontType = "Default Font";

			// 🔹 Step 1: Check if Run specifies a font directly
			if (run.RunProperties?.RunFonts?.Ascii?.Value != null)
			{
				runFontType = run.RunProperties.RunFonts.Ascii.Value;
			}
			else
			{
				// 🔹 Step 2: Check Paragraph-Level Font Style
				runFontType =
					paragraph
						.ParagraphProperties?.ParagraphMarkRunProperties?.GetFirstChild<RunFonts>()
						?.Ascii?.Value ?? "Default Font";

				// 🔹 Step 3: Check the style definition (if paragraph has a style)
				string styleId = paragraph.ParagraphProperties?.ParagraphStyleId?.Val?.Value;
				var stylesPart = doc.MainDocumentPart?.StyleDefinitionsPart;

				if (
					!string.IsNullOrEmpty(styleId)
					&& stylesPart != null
					&& stylesPart.Styles != null
				)
				{
					var paragraphStyle = stylesPart
						.Styles.Elements<WordStyle>()
						.FirstOrDefault(s => s.StyleId == styleId);
					if (paragraphStyle?.StyleRunProperties?.RunFonts?.Ascii?.Value != null)
					{
						runFontType = paragraphStyle.StyleRunProperties.RunFonts.Ascii.Value;
					}
				}

				// 🔹 Step 4: Check Document Default Font
				var docDefaults = stylesPart
					?.Styles.Elements<WordStyle>()
					.FirstOrDefault(s =>
						s.Type?.Value == WPStyleValues.Paragraph && s.Default?.Value == true
					);
				if (docDefaults?.StyleRunProperties?.RunFonts?.Ascii?.Value != null)
				{
					runFontType = docDefaults.StyleRunProperties.RunFonts.Ascii.Value;
				}
			}

			return runFontType;
		}

		// Get list type
		private static string GetListType(WordParagraph paragraph)
		{
			var numberingProps = paragraph.ParagraphProperties?.NumberingProperties;

			// Console.WriteLine("\nGetListType:");
			// Console.WriteLine(numberingProps);

			if (numberingProps != null)
			{
				// this is the type of listing
				var numberingId =
					numberingProps?.NumberingId?.Val != null
						? numberingProps.NumberingId.Val.Value.ToString()
						: "None";

				// this means the depth
				var levelId =
					numberingProps?.NumberingLevelReference?.Val != null
						? numberingProps.NumberingLevelReference.Val.Value.ToString()
						: "None";
				// Console.WriteLine($"Numbering ID: {numberingId ?? "None\n"}");
				// Console.WriteLine($"Level ID: {levelId ?? "None"}");
			}
			else
			{
				// Console.WriteLine("This paragraph has no numbering properties.");
			}

			if (numberingProps == null)
				return "Unknown";

			var listType = numberingProps.NumberingId?.Val?.Value switch
			{
				1 => "numbered_list",
				3 => "lowercase_lettered_list",
				9 => "lowercase_lettered_parenthesis_list",
				10 => "dash_bulleted_list",
				11 => "bulleted_list",
				12 => "hollow_bulleted_list",
				13 => "square_bulleted_list",
				15 => "diamond_bulleted_list",
				16 => "arrow_bulleted_list",
				17 => "checkmark_bulleted_list",
				18 => "numbered_parenthesis_list",
				19 => "roman_numeral_list",
				20 => "uppercase_lettered_list",
				21 => "lowercase_roman_numeral_list",
				_ => "unknown_list",
			};
			// Console.WriteLine($"List type: {listType}\n");
			return listType;
		}

		// extracting paragraph
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
					.Styles.Elements<WordStyle>()
					.FirstOrDefault(s => s.StyleId == styleId);

				if (paragraphStyle != null)
				{
					fontType =
						paragraphStyle.StyleRunProperties?.RunFonts?.Ascii?.Value ?? "Default Font";
					fontSizeRaw = paragraphStyle.StyleRunProperties?.FontSize?.Val?.Value;

					// Extract color from style definition
					if (paragraphStyle.StyleRunProperties?.Color?.Val?.Value != null)
						fontColor = paragraphStyle.StyleRunProperties.Color.Val.Value;
				}
			}

			// ✅ Convert font size from half-points
			int fontSize = fontSizeRaw != null ? int.Parse(fontSizeRaw) / 2 : 12; // Default 12pt

			// Extract line spacing
			// Default is Multiple, 1.15x, 276 twips
			string lineSpacingType = "Multiple (1.15x)";
			double lineSpacingValue = 13.8; // 276 twips / 20 = 13.8pt

			if (paragraph.ParagraphProperties != null)
			{
				var spacingElement = paragraph.ParagraphProperties.SpacingBetweenLines;

				if (spacingElement != null)
				{
					if (spacingElement.LineRule != null)
					{
						// Retrieve initial line spacing type, "auto" for some cases
						lineSpacingType = spacingElement.LineRule.ToString() ?? "";
					}

					if (spacingElement.Line != null)
					{
						try
						{
							// Provided value is in twips, so need to convert to what is shown in Word
							int twipValue = int.Parse(spacingElement.Line.Value ?? "");
							lineSpacingValue = twipValue / 20.0;

							// Convert "auto" into actual line spacing type names
							if (spacingElement.LineRule == null || lineSpacingType == "auto")
							{
								switch (twipValue)
								{
									case 240:
										lineSpacingType = "Single";
										break;
									case 360:
										lineSpacingType = "1.5 lines";
										break;
									case 480:
										lineSpacingType = "Double";
										break;
									default:
										if (twipValue > 240)
										{
											lineSpacingType =
												$"Multiple ({lineSpacingValue / 12:0.0}x)";
										}
										break;
								}
							}
							//Console.WriteLine($"Extracted line spacing: {lineSpacingType}, {lineSpacingValue}");
						}
						catch (FormatException ex)
						{
							// Console.WriteLine($"Error parsing Line value: {ex.Message}");
							lineSpacingValue = 1.15;
						}
					}
				}
			}

			var paragraphData = new Dictionary<string, object>();
			paragraphData["alignment"] = alignment;
			// paragraphData["fontType"] = fontType;
			// paragraphData["fontSize"] = fontSize;

			var havemath = false;
			List<Dictionary<string, object>> mathContent = new List<Dictionary<string, object>>();

			// ✅ Extract Paragraph-Level Font & Size Correctly
			string paraFontType = GetParagraphFont(paragraph);
			int paraFontSize = GetParagraphFontSize(paragraph);

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
					{ "lineSpacingType", lineSpacingType },
					{ "lineSpacingValue", lineSpacingValue },
				},
			};
			// the one below can grab as text
			// // check for internal using word. This works
			// string paraText = paragraph.InnerText.Trim();

			// // Check if it starts with "References"
			// if (paraText.StartsWith("References", StringComparison.OrdinalIgnoreCase))
			// {
			// 	Console.WriteLine("Found a references paragraph:");
			// 	Console.WriteLine(paraText);
			// }

			// Detect type of lists
			if (paragraph.ParagraphProperties?.NumberingProperties != null)
			{
				paragraphData["type"] = GetListType(paragraph);
				paragraphData["content"] = text;
				paragraphData["styling"] = PropertiesList;
				return paragraphData;
			}

			// Detect Page Breaks
			if (paragraph.Descendants<WordBreak>().Any(b => b.Type?.Value == BreakValues.Page))
			{
				// Console.WriteLine("Detect page break\n");
				paragraphData["type"] = "page_break";
				paragraphData["content"] = "[PAGE BREAK]";
				// paragraphData["fonttype"] = paraFontType;
				// paragraphData["fontsize"] = paraFontSize;
				paragraphData["styling"] = PropertiesList;
				return paragraphData;
			}

			// Detect Line Breaks
			if (
				paragraph
					.Descendants<WordBreak>()
					.Any(b => b.Type?.Value == BreakValues.TextWrapping)
			)
			{
				// Console.WriteLine("Detect line break\n");
				paragraphData["type"] = "line_break";
				paragraphData["content"] = "[LINE BREAK]";
				// paragraphData["fonttype"] = paraFontType;
				// paragraphData["fontsize"] = paraFontSize;
				paragraphData["styling"] = PropertiesList;
				return paragraphData;
			}

			if (paragraph.Descendants<DocumentFormat.OpenXml.Math.OfficeMath>().Any())
			{
				// Console.WriteLine("Math extractor\n");
				mathContent = ExtractParagraphsWithMath(paragraph);
				havemath = true;
				paragraphData["type"] = "math";
			}
			// ✅ Check if paragraph is completely empty
			if (
				string.IsNullOrWhiteSpace(text)
				&& !paragraph.Elements<WordBreak>().Any()
				&& havemath == false
			)
			{
				// Console.WriteLine("Null / white space\n");
				paragraphData["type"] = "empty_paragraph1";
				paragraphData["content"] = "";
				paragraphData["styling"] = PropertiesList;

				return paragraphData;
			}

			// Check for page/line breaks at the paragraph level
			if (paragraph.Descendants<WordBreak>().Any(b => b.Type?.Value == BreakValues.Page))
			{
				// Console.WriteLine("break\n");
				return new Dictionary<string, object>
				{
					{ "type", "page_break" },
					{ "content", "[PAGE BREAK]" },
					{ "fonttype", paraFontType },
				};
			}

			if (
				paragraph
					.Descendants<WordBreak>()
					.Any(b => b.Type?.Value == BreakValues.TextWrapping)
			)
			{
				// Console.WriteLine("line break\n");
				return new Dictionary<string, object>
				{
					{ "type", "line_break" },
					{ "content", "[LINE BREAK]" },
				};
			}

			// Collect each run's text and formatting
			var runsList = new List<Dictionary<string, object>>();
			var runs = paragraph.Elements<WordRun>().ToList(); // Convert to List to get index
			bool bracket = false;

			for (int i = 0; i < runs.Count; i++)
			{
				var run = runs[i];
				string runText = string.Join("", run.Descendants<WordText>().Select(t => t.Text));

				if (string.IsNullOrWhiteSpace(runText))
				{
					// Console.WriteLine("Continue\n");
					continue;
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

				string runFontColor = fontColor;
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

						// Console.WriteLine("check the font type:");
						// Console.WriteLine(runfontType);
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
					// get fonts and size
					// Console.WriteLine("\nS3");
					//Check font and font size
					string? runFontSizeRawS1 = run.RunProperties?.FontSize?.Val?.Value;
					int runFontSizeS1 = 12; // Default to 12pt if not found

					if (int.TryParse(runFontSizeRawS1, out int parsedSizeS1))
						runFontSizeS1 = parsedSizeS1 / 2; // Convert from half-points to standard points
					// Console.WriteLine($"Run Font Size: {runFontSizeS1}pt");

					string? paraFontSizeRawS1 = paragraph
						.ParagraphProperties?.ParagraphMarkRunProperties?.GetFirstChild<FontSize>()
						?.Val?.Value;
					int paraFontSizeS1 = 12;

					if (int.TryParse(paraFontSizeRawS1, out int paraParsedSizeS1))
						paraFontSizeS1 = paraParsedSizeS1 / 2; // Convert from half-points

					string fontTypexx = GetRunFontType(run, paragraph, doc);
					// Console.WriteLine($"Paragraph Font Size1: {runFontSizeS1}pt {fontTypexx}");
					// Console.WriteLine(runText);
					//end

					// Now you can safely call dict.ContainsKey(...)
					if (
						dict.ContainsKey("fontsize")
						&& dict.ContainsKey("fonttype")
						&& haveBibliography == true
					)
					{
						// Retrieve and compare values | Check if citation
						if (
							dict["fontsize"] is int size
							&& size == 10
							&& dict["fonttype"] is string font
							&& runfontType == "Times New Roman"
						)
						{
							runsList.Add(
								new Dictionary<string, object>
								{
									{ "temp", "citation_run" },
									{ "content", runText },
									{ "styling", PropertiesList[0] },
								}
							);
						}
					}
					else if (dict.ContainsKey("fontsize") && dict.ContainsKey("fonttype"))
					{
						// if (
						// 	runFontSizeS1 == 10 && fontTypexx == "Times New Roman"
						// )
						// {

						string bracketPattern = @"\([^)]*\)"; // Matches entire ( ... )
						string firstBracketPattern = @"\("; // Matches first "("
						string secondBracketPattern = @"\)"; // Matches first ")"

						Match bracketMatch = Regex.Match(runText, bracketPattern);
						Match firstBracketMatch = Regex.Match(runText, firstBracketPattern);
						Match secondBracketMatch = Regex.Match(runText, secondBracketPattern);

						// matches whole bracket
						if (
							bracketMatch.Success
							&& runFontSizeS1 == 10
							&& fontTypexx == "Times New Roman"
						)
						{
							// Console.WriteLine("bracket set\n");
							// Console.WriteLine(runFontSizeS1);
							// Console.WriteLine(fontTypexx);
							runsList.Add(
								new Dictionary<string, object>
								{
									{ "type", "intext-citation" },
									{ "content", runText },
									{ "styling", PropertiesList[0] },
								}
							);
						}
						// found first bracket
						else if (
							bracket == false
							&& firstBracketMatch.Success
							&& runFontSizeS1 == 10
							&& fontTypexx == "Times New Roman"
						)
						{
							runsList.Add(
								new Dictionary<string, object>
								{
									{ "type", "intext-citation" },
									{ "content", runText },
									{ "styling", PropertiesList[0] },
								}
							);
							bracket = true;
						}
						// second bracket
						else if (
							bracket == true
							&& secondBracketMatch.Success
							&& runFontSizeS1 == 10
							&& fontTypexx == "Times New Roman"
						)
						{
							runsList.Add(
								new Dictionary<string, object>
								{
									{ "type", "intext-citation" },
									{ "content", runText },
									{ "styling", PropertiesList[0] },
								}
							);
							bracket = false;
						}
						// else if got content just add
						else
						{
							if (bracket == true)
							{
								// runFontSizeS1 == 10 && fontTypexx == "Times New Roman"
								// Console.WriteLine("debug\n");
								// Console.WriteLine(runFontSizeS1);
								// Console.WriteLine(fontTypexx);
								// Console.WriteLine(runText);

								runsList.Add(
									new Dictionary<string, object>
									{
										{ "type", "intext-citation" },
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
			// Console.WriteLine($"Total runs found: {runsList.Count}");
			// Console.WriteLine($"bib:{haveBibliography}");
			// Console.WriteLine("check the text:");
			// Console.WriteLine(text);

			// check if it is still references / citation at the bottom
			if (text.Length > 0 && text[0] == '[')
			{
				return new Dictionary<string, object>
				{
					{ "type", "citation_run" },
					{ "content", text },
					{ "styling", PropertiesList },
				};
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
						{ "type", "bibliography" },
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
							{ "styling", new List<object>() }, // Ensure no comma here after the last item
						};
					}
					else
					{
						return new Dictionary<string, object>
						{
							{ "type", "paragraph_run" },
							{ "content", runsList },
							{ "styling", new List<object>() }, // Ensure no comma here after the last item
						};
					}
				}
				else if (runsList.Count > 1)
				{
					// Console.WriteLine("\nRuns > 1:");
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
						{ "type", GetParagraphType(style) },
						{ "content", text },
						{ "runs", runsList },
						{ "styling??", new List<object>() }, // Ensure no comma here after the last item
					};
					return finalDictionary;
				}
				else
				{
					if (havemath == true)
					{
						var mathstring = "";
						// Console.WriteLine(
						// 	"Getting back the result and we see what is inside the for loop\n"
						// );
						// Go through the math and re-assemble it back from the run
						foreach (var mathEntry in mathContent)
						{
							// Console.WriteLine(mathEntry["content"]);
							mathstring = mathEntry["content"] + mathstring;
						}
						return new Dictionary<string, object>
						{
							{ "type", "math" },
							{ "content", mathstring },
							{ "styling", PropertiesList },
						};
					}
					else
					{
						return new Dictionary<string, object>
						{
							{ "type", GetParagraphType(style) },
							{ "content", text },
							{ "styling", PropertiesList },
						};
					}
				}
			}
		}

		// Method: Save content as JSON
		// Return A string containing the serialized JSON for checking purposes later
		private static string SaveDocumentDataToJsonFile(object documentData)
		{
			var jsonOutput = JsonSerializer.Serialize(
				documentData,
				new JsonSerializerOptions
				{
					WriteIndented = true,
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				}
			);

			File.WriteAllText(jsonOutputPath, jsonOutput);
			Console.WriteLine($"✅ JSON saved to {jsonOutputPath}");

			return jsonOutput;
		}

		//Method: Parse the document
		// public async Task<List<object>> ParseDocument(IMongoDatabase database, string fileName)
		public async Task<List<object>> ParseDocument(string fileName)

		{
			// var documentControl = new DocumentControl(); // Must be declared inside the method

			var fullFilePath = ReturnFullFilePath(fileName);

			using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, false))
			{
				var documentContents = ExtractDocumentContents(wordDoc);
				var rootElement = elementRoot();
				documentContents.Insert(0, ExtractLayout(wordDoc));
				documentContents.Insert(0, rootElement);

				// for the other contents
				var documentData = new
				{
					// metadata = DocumentMetadataExtractor.GetMetadata(wordDoc),
					metadata = GetDocumentMetadata(wordDoc, ReturnFullFilePath(fileName)),
					headers = ExtractHeaders(wordDoc),
					footers = ExtractFooters(wordDoc),
					document = documentContents,
				};

				// save extracted content to json and return json string
				this.jsonString = SaveDocumentDataToJsonFile(documentData);
				JObject jsonObject = JObject.Parse(jsonString);
				this.documentArray = (JArray)jsonObject["document"];

				// uncomment to see consolelogs for checking purposes
				// checkJson((jsonString);

				return documentContents;
			}
		}

		public static Dictionary<string, object> GetDocumentLayout(WordprocessingDocument doc)
		{
			var layout = new Dictionary<string, object>();
			var mainDocumentPart = doc.MainDocumentPart;

			if (mainDocumentPart == null || mainDocumentPart.Document.Body == null)
				return layout;

			// Get all section properties in the document
			var allSectionProps = mainDocumentPart
				.Document.Body.Descendants<SectionProperties>()
				.ToList();

			if (allSectionProps.Count == 0)
				return layout;

			// Get the first section only
			var sectionProps = allSectionProps.FirstOrDefault();

			// Page size
			var pageSize = sectionProps?.Elements<PageSize>().FirstOrDefault();
			if (pageSize != null)
			{
				bool isLandscape =
					pageSize.Orient != null
					&& pageSize.Orient.Value == PageOrientationValues.Landscape;

				layout["orientation"] = isLandscape ? "Landscape" : "Portrait";
				// Console.WriteLine($"Orientation: {(isLandscape ? "Landscape" : "Portrait")}");

				if (pageSize.Width != null)
				{
					layout["pageWidth"] = ConvertTwipsToCentimeters((int)pageSize.Width.Value);
					// Console.WriteLine(
					//     $"Page Width: {layout["pageWidth"]} cm (Original: {pageSize.Width.Value} twips)"
					// );
				}

				if (pageSize.Height != null)
				{
					layout["pageHeight"] = ConvertTwipsToCentimeters((int)pageSize.Height.Value);
					// Console.WriteLine(
					//     $"Page Height: {layout["pageHeight"]} cm (Original: {pageSize.Height.Value} twips)"
					// );
				}
			}
			else
				Console.WriteLine("No page size found in section properties.");

			// Columns
			var columns = sectionProps?.Elements<Columns>().FirstOrDefault();
			if (columns != null)
			{
				int columnCount = 1;
				double columnSpacing = 1.27;

				if (columns.ColumnCount != null)
					columnCount = columns.ColumnCount.Value;

				if (columns.Space != null)
					columnSpacing = ConvertTwipsToCentimeters(int.Parse(columns.Space.Value ?? ""));

				layout["columnNum"] = columnCount;
				layout["columnSpacing"] = columnSpacing;
			}
			else
			{
				layout["columnNum"] = 1;
				layout["columnSpacing"] = 1.27;
			}

			// Page margins
			var pageMargins = sectionProps?.Elements<PageMargin>().FirstOrDefault();
			if (pageMargins != null)
			{
				var margins = new Dictionary<string, double>();
				// Console.WriteLine("Margins found:");

				if (pageMargins.Top != null)
					margins["top"] = ConvertTwipsToCentimeters(pageMargins.Top.Value);

				if (pageMargins.Bottom != null)
					margins["bottom"] = ConvertTwipsToCentimeters(pageMargins.Bottom.Value);

				if (pageMargins.Left != null)
					margins["left"] = ConvertTwipsToCentimeters((int)pageMargins.Left.Value);

				if (pageMargins.Right != null)
					margins["right"] = ConvertTwipsToCentimeters((int)pageMargins.Right.Value);

				if (pageMargins.Header != null)
					margins["header"] = ConvertTwipsToCentimeters((int)pageMargins.Header.Value);

				if (pageMargins.Footer != null)
					margins["footer"] = ConvertTwipsToCentimeters((int)pageMargins.Footer.Value);

				layout["margins"] = margins;
			}
			else
				Console.WriteLine("No page margins found in section properties.");

			return layout;
		}

		// extract document content
		public static List<object> ExtractDocumentContents(WordprocessingDocument doc)
		{
			var elements = new List<object>();
			var body = doc.MainDocumentPart?.Document?.Body;

			if (body == null)
			{
				Console.WriteLine("❌ Error: Document body is null.");
				return elements;
			}

			bool haveBibliography = false;

			foreach (var element in body.Elements<OpenXmlElement>())
			{
				// ✅ Check for a Drawing element inside the run (Extract Images)
				var drawing = element
					.Descendants<DocumentFormat.OpenXml.Wordprocessing.Drawing>()
					.FirstOrDefault();
				if (drawing != null)
				{
					var imageObjects = ExtractImagesFromDrawing(doc, drawing);
					elements.AddRange(imageObjects);
				}
				else if (element is DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph)
				{
					// ✅ Extract Paragraphs
					elements.Add(ExtractParagraph(paragraph, doc, ref haveBibliography));
				}
				else if (element is DocumentFormat.OpenXml.Wordprocessing.Table table)
				{
					// Console.WriteLine("📝 Extracting Table by another modue");
					elements.Add(ExtractContent.ExtractTable(table)); // ✅ Extract Tables
				}
			}
			return elements;
		}

		// json related stuff
		public static Dictionary<string, object> ConvertJsonElements(
			Dictionary<string, object> input
		)
		{
			var result = new Dictionary<string, object>();
			foreach (var kvp in input)
			{
				if (kvp.Value is JsonElement jsonElement)
				{
					switch (jsonElement.ValueKind)
					{
						case JsonValueKind.String:
							result[kvp.Key] = jsonElement.GetString();
							break;
						case JsonValueKind.Number:
							result[kvp.Key] = jsonElement.GetDouble(); // or GetInt32() depending
							break;
						case JsonValueKind.True:
						case JsonValueKind.False:
							result[kvp.Key] = jsonElement.GetBoolean();
							break;
						case JsonValueKind.Object:
						case JsonValueKind.Array:
							result[kvp.Key] = jsonElement.ToString(); // fallback as string
							break;
						default:
							result[kvp.Key] = null;
							break;
					}
				}
				else
				{
					result[kvp.Key] = kvp.Value;
				}
			}
			return result;
		}

		public static async Task ToSaveJson(
			DocumentProcessors documentControl,
			string filePath,
			string jsonOutputPath
		)
		{
			using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
			{
				// Get layout information
				var layoutInfo = GetDocumentLayout(wordDoc);

				// Extract document contents
				var documentContents = ExtractDocumentContents(wordDoc);

				// Create layout element
				var layoutElement = new Dictionary<string, object>
				{
					{ "type", "layout" },
					{ "content", "" },
					{
						"styling",
						new List<object> { layoutInfo }
					},
				};

				// Insert layout as the first element in document contents
				documentContents.Insert(0, layoutElement);

				// Create root node
				var layoutElementRoot = new Dictionary<string, object>
				{
					{ "id", 0 },
					{ "type", "root" },
					{ "content", "" },
				};
				documentContents.Insert(0, layoutElementRoot);

				var documentData = new
				{
					metadata = GetDocumentMetadata(wordDoc, filePath), // Fixed `filePath_full`
					document = documentContents,
				};

				// Convert to JSON format with UTF-8 encoding fix
				string jsonOutput = System.Text.Json.JsonSerializer.Serialize(
					documentData,
					new JsonSerializerOptions
					{
						WriteIndented = true,
						Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
					}
				);

				// Write JSON to file
				File.WriteAllText(jsonOutputPath, jsonOutput);
				Console.WriteLine($"✅ New data saved to {jsonOutputPath}");

				// Save JSON to database (assuming `saveJsonToDatabase` is an async method)
				await documentControl.saveJsonToDatabase(jsonOutputPath);
			}
		}

		// Convert twips (1/1440 of an inch) to centimeters
		private static double ConvertTwipsToCentimeters(int twips)
		{
			// 1 inch = 2.54 cm, and 1 inch = 1440 twips
			return Math.Round((double)twips / 1440 * 2.54, 2);
		}

		public static string GetParagraphFont(
			DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph
		)
		{
			if (
				paragraph.ParagraphProperties != null
				&& paragraph.ParagraphProperties.ParagraphStyleId != null
			)
			{
				return paragraph.ParagraphProperties.ParagraphStyleId.Val?.Value ?? "Default Font";
			}
			return "Default Font";
		}

		public static int GetParagraphFontSize(
			DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph
		)
		{
			string? fontSizeRaw = paragraph
				.ParagraphProperties?.ParagraphMarkRunProperties?.Elements<FontSize>()
				.FirstOrDefault()
				?.Val?.Value;

			return fontSizeRaw != null ? int.Parse(fontSizeRaw) / 2 : 12; // Default 12pt
		}

		// Change the type to header
		public static string GetParagraphType(string style)
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
}
