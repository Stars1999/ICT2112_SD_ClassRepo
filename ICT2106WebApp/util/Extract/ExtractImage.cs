using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using SixImage = SixLabors.ImageSharp.Image;
using SixPixelFormats = SixLabors.ImageSharp.PixelFormats;

using Utilities;

namespace Utilities
{

	public static partial class ExtractContent
	{
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

			// 7. Extract image dimensions from the XML (EMUs)
			// For inline images, the extent is usually found in wp:extent element.
			long cx = 0;
			long cy = 0;
			var inline = drawing.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline>().FirstOrDefault();

			if (inline != null && inline.Extent != null)
			{
				cx = inline.Extent.Cx;
				cy = inline.Extent.Cy;
			}
			else
			{
				// For floating images, look in wp:anchor
				var anchor = drawing.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Anchor>().FirstOrDefault();
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
				_ => "Unknown"
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
			var parentParagraph = drawing.Ancestors<Paragraph>().FirstOrDefault();
			if (parentParagraph != null &&
				parentParagraph.ParagraphProperties?.Justification != null &&
				parentParagraph.ParagraphProperties.Justification.Val != null)
			{
				// Extract the underlying enum value
				var justValue = parentParagraph.ParagraphProperties.Justification.Val.Value;

				if (justValue == DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Left)
				{
					alignment = "Left Align (Ctrl + L)";
				}
				else if (justValue == DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Center)
				{
					alignment = "Center Align (Ctrl + E)";
				}
				else if (justValue == DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Right)
				{
					alignment = "Right Align (Ctrl + R)";
				}
				else if (justValue == DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Both)
				{
					alignment = "Justify (Ctrl + J)";
				}
				else
				{
					alignment = justValue.ToString();
				}
			}
			Console.WriteLine("Image Alignment: " + alignment);

			// 11.Get image position(for floating images)
			string imagePosition = "Inline (position determined by text flow)";
			// var anchorElement = drawing.Descendants<WP.Anchor>().FirstOrDefault();
			var anchorElement = drawing.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Anchor>().FirstOrDefault();

			if (anchorElement != null)
			{
				// var posH = anchorElement.PositionH;
				// var posV = anchorElement.PositionV;
				var horizontalPosElem = anchorElement.HorizontalPosition;
				var verticalPosElem = anchorElement.VerticalPosition;

				string horizontalPos = horizontalPosElem?.PositionOffset?.Text ?? "Not specified";
				string verticalPos = verticalPosElem?.PositionOffset?.Text ?? "Not specified";
				imagePosition = $"Horizontal Offset: {horizontalPos}, Vertical Offset: {verticalPos}";
			}


			// 12. Add all the extracted information into a dictionary
			var imageInfo = new Dictionary<string, object>
		{
			{ "type", "image" },
			{ "filename", fileName },
			{ "widthEMU", cx },
			{ "heightEMU", cy },
			{ "widthPixels", widthPixels },
			{ "heightPixels", heightPixels },
			{ "horizontalResolution", horizontalResolution },
			{ "verticalResolution", verticalResolution },
			{ "format", imageFormat },
			{ "alignment", alignment },
			{ "position", imagePosition }
		};

			imageList.Add(imageInfo);


			return imageList;
		}
	}
}
