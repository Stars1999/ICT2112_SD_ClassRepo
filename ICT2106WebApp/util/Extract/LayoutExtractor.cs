using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public class LayoutExtractor : ILayoutExtractor
{
	public Dictionary<string, object> ExtractLayout(WordprocessingDocument doc)
	{
		var layout = new Dictionary<string, object>();
		var mainDocumentPart = doc.MainDocumentPart;

		if (mainDocumentPart?.Document.Body == null)
			return layout;

		var sectionProps = mainDocumentPart
			.Document.Body.Elements<SectionProperties>()
			.FirstOrDefault();
		if (sectionProps == null)
			return layout;

		var pageSize = sectionProps.Elements<PageSize>().FirstOrDefault();
		if (pageSize != null)
		{
			bool isLandscape = pageSize.Orient?.Value == PageOrientationValues.Landscape;
			layout["orientation"] = isLandscape ? "Landscape" : "Portrait";

			if (pageSize.Width != null)
				layout["pageWidth"] = ConvertTwipsToCentimeters((int)pageSize.Width.Value);
			if (pageSize.Height != null)
				layout["pageHeight"] = ConvertTwipsToCentimeters((int)pageSize.Height.Value);
		}

		var columns = sectionProps.Elements<Columns>().FirstOrDefault();
		layout["columnNum"] = columns?.ColumnCount?.Value ?? 1;
		layout["columnSpacing"] =
			columns?.Space != null
				? ConvertTwipsToCentimeters(int.Parse(columns.Space.Value))
				: 1.27;

		var pageMargins = sectionProps.Elements<PageMargin>().FirstOrDefault();
		if (pageMargins != null)
		{
			var margins = new Dictionary<string, double>();
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

		return layout;
	}

	private static double ConvertTwipsToCentimeters(int twips)
	{
		return Math.Round((double)twips / 1440 * 2.54, 2);
	}
}
