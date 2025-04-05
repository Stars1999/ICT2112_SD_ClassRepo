// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using DocumentFormat.OpenXml.Drawing;
// using DocumentFormat.OpenXml.Packaging;
// using DocumentFormat.OpenXml.Wordprocessing;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.PixelFormats;
// using IOPath = System.IO.Path;

// public class ImageExtractor : IImageExtractor
// {
// 	public List<Dictionary<string, object>> ExtractImages(
// 		WordprocessingDocument doc,
// 		Drawing drawing
// 	)
// 	{
// 		var imageList = new List<Dictionary<string, object>>();
// 		var mainPart = doc.MainDocumentPart;
// 		if (mainPart == null)
// 			return imageList;

// 		var blip = drawing.Descendants<Blip>().FirstOrDefault();
// 		string embed = blip?.Embed?.Value;
// 		if (string.IsNullOrEmpty(embed))
// 			return imageList;

// 		var part = mainPart.GetPartById(embed);
// 		if (part is not ImagePart imagePart)
// 			return imageList;

// 		// string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
// 		// string fileName = Path.Combine(folderPath, $"Image_{embed}.png");

// 		string folderPath = IOPath.Combine(Directory.GetCurrentDirectory(), "Images");
// 		Directory.CreateDirectory(folderPath);
// 		string fileName = IOPath.Combine(folderPath, $"Image_{embed}.png");

// 		using (var stream = imagePart.GetStream())
// 		using (var fileStream = new FileStream(fileName, FileMode.Create))
// 		{
// 			stream.CopyTo(fileStream);
// 		}

// 		long cx = 0,
// 			cy = 0;
// 		var inline = drawing
// 			.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline>()
// 			.FirstOrDefault();
// 		if (inline?.Extent != null)
// 		{
// 			cx = inline.Extent.Cx;
// 			cy = inline.Extent.Cy;
// 		}

// 		double widthPixels = cx / 9525.0;
// 		double heightPixels = cy / 9525.0;

// 		double hRes = 0,
// 			vRes = 0;
// 		try
// 		{
// 			using var img = Image.Load<Rgba32>(fileName);
// 			hRes = img.Metadata.HorizontalResolution;
// 			vRes = img.Metadata.VerticalResolution;
// 		}
// 		catch { }

// 		string format = imagePart.ContentType.ToLower() switch
// 		{
// 			"image/png" => "PNG",
// 			"image/jpeg" or "image/jpg" => "JPG",
// 			"image/gif" => "GIF",
// 			_ => "Unknown",
// 		};

// 		string alignment = "Left Align";
// 		var parentParagraph = drawing.Ancestors<Paragraph>().FirstOrDefault();
// 		var justification = parentParagraph?.ParagraphProperties?.Justification?.Val?.Value;
// 		if (justification != null)
// 			alignment = justification.ToString();

// 		var anchor = drawing
// 			.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Anchor>()
// 			.FirstOrDefault();
// 		string position = "Inline";
// 		if (anchor != null)
// 		{
// 			string h = anchor.HorizontalPosition?.PositionOffset?.Text ?? "N/A";
// 			string v = anchor.VerticalPosition?.PositionOffset?.Text ?? "N/A";
// 			position = $"H: {h}, V: {v}";
// 		}

// 		imageList.Add(
// 			new Dictionary<string, object>
// 			{
// 				{ "type", "image" },
// 				{ "content", fileName },
// 				{
// 					"styling",
// 					new Dictionary<string, object>
// 					{
// 						{ "widthEMU", cx },
// 						{ "heightEMU", cy },
// 						{ "widthPixels", widthPixels },
// 						{ "heightPixels", heightPixels },
// 						{ "horizontalResolution", hRes },
// 						{ "verticalResolution", vRes },
// 						{ "format", format },
// 						{ "alignment", alignment },
// 						{ "position", position },
// 					}
// 				},
// 			}
// 		);

// 		return imageList;
// 	}
// }

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
// 👇 Disambiguate Path
using IOPath = System.IO.Path;
using WordParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;

public class ImageExtractor : IImageExtractor
{
	public List<Dictionary<string, object>> ExtractImages(
		WordprocessingDocument doc,
		Drawing drawing
	)
	{
		var imageList = new List<Dictionary<string, object>>();
		var mainPart = doc.MainDocumentPart;
		if (mainPart == null)
			return imageList;

		var blip = drawing.Descendants<Blip>().FirstOrDefault();
		string embed = blip?.Embed?.Value;
		if (string.IsNullOrEmpty(embed))
			return imageList;

		var part = mainPart.GetPartById(embed);
		if (part is not ImagePart imagePart)
			return imageList;

		// ✅ Fixed Path.Combine usage
		string folderPath = IOPath.Combine(Directory.GetCurrentDirectory(), "Images");
		Directory.CreateDirectory(folderPath);
		string fileName = IOPath.Combine(folderPath, $"Image_{embed}.png");

		using (var stream = imagePart.GetStream())
		using (var fileStream = new FileStream(fileName, FileMode.Create))
		{
			stream.CopyTo(fileStream);
		}

		long cx = 0,
			cy = 0;
		var inline = drawing
			.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline>()
			.FirstOrDefault();
		if (inline?.Extent != null)
		{
			cx = inline.Extent.Cx;
			cy = inline.Extent.Cy;
		}

		double widthPixels = cx / 9525.0;
		double heightPixels = cy / 9525.0;

		double hRes = 0,
			vRes = 0;
		try
		{
			using var img = Image.Load<Rgba32>(fileName);
			hRes = img.Metadata.HorizontalResolution;
			vRes = img.Metadata.VerticalResolution;
		}
		catch { }

		string format = imagePart.ContentType.ToLower() switch
		{
			"image/png" => "PNG",
			"image/jpeg" or "image/jpg" => "JPG",
			"image/gif" => "GIF",
			_ => "Unknown",
		};

		string alignment = "Left Align";
		var parentParagraph = drawing.Ancestors<WordParagraph>().FirstOrDefault();
		var justification = parentParagraph?.ParagraphProperties?.Justification?.Val?.Value;
		if (justification != null)
			alignment = justification.ToString();

		var anchor = drawing
			.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.Anchor>()
			.FirstOrDefault();
		string position = "Inline";
		if (anchor != null)
		{
			string h = anchor.HorizontalPosition?.PositionOffset?.Text ?? "N/A";
			string v = anchor.VerticalPosition?.PositionOffset?.Text ?? "N/A";
			position = $"H: {h}, V: {v}";
		}

		imageList.Add(
			new Dictionary<string, object>
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
						{ "horizontalResolution", hRes },
						{ "verticalResolution", vRes },
						{ "format", format },
						{ "alignment", alignment },
						{ "position", position },
					}
				},
			}
		);

		return imageList;
	}
}
