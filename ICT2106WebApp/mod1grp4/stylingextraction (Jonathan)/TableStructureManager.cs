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

public class TableStructureManager : iTableStructure
{
	// Extract table (Jonathan - COMPLETED)
	public Dictionary<string, object> extractTableStructure(Table table)
	{
		var tableRows = new List<Dictionary<string, object>>();

		// Run through each row in table
		foreach (var row in table.Elements<TableRow>())
		{
			//Get Row/Cell Height For later [All cells in a row are same height] (Jonathan - COMPLETED)
			var rowProperties = row.TableRowProperties; // Access the property directly

			string rowHeight =
				rowProperties != null && rowProperties.Descendants<TableRowHeight>().Any()
					? (
						(
							float.Parse(
								rowProperties.Descendants<TableRowHeight>().FirstOrDefault().Val
							) * 2.54
						) / 1440
					).ToString("#.##")
					: "auto";

			// List to hold cell dictionaries for this row.
			var cellList = new List<Dictionary<string, object>>();

			// Run through each cell in the row
			foreach (var cell in row.Elements<TableCell>())
			{
				//---Section to extract cell details START---// (Jonathan - COMPLETED)
				/*	[READ]
					Only cell contents extracted. Reasoning: Since table and rows are made of cells, only cells details need to be extracted.
				*/

				// Extract the cell text.
				string cellText = string.Join("", cell.Descendants<Text>().Select(t => t.Text));

				// Get Run (text) Properties
				var cellTextStyle = cell.Descendants<RunProperties>().FirstOrDefault();

				// Check if text underline, bold, italic
				bool underline = cellTextStyle?.GetFirstChild<Underline>() != null;
				bool bold = cellTextStyle?.GetFirstChild<Bold>() != null;
				bool italic = cellTextStyle?.GetFirstChild<Italic>() != null;

				// Get text font size and style
				float fontSize =
					cell.Descendants<FontSize>().FirstOrDefault()?.Val != null
						? float.Parse(cell.Descendants<FontSize>().FirstOrDefault().Val) / 2
						: 11;
				string fontType = cellTextStyle?.RunFonts?.Ascii?.ToString() ?? "Arial";

				// Get Text Alignment
				string horizontalalignment =
					cell.Descendants<Justification>().FirstOrDefault()?.Val?.ToString() ?? "left";
				string verticalalignment =
					cell.Descendants<TableCellVerticalAlignment>().FirstOrDefault()?.Val?.ToString()
					?? "default";

				// Get Text Color
				string textcolor = cellTextStyle?.Color?.Val ?? "auto";

				// Get Text Highlight Color
				string texthighlight = cellTextStyle?.Highlight?.Val ?? "none";

				// Extract cell borders details
				var borders = cell.Descendants<TableCellBorders>().FirstOrDefault();

				// Extract Border Style
				string bordertopstyle = borders?.TopBorder?.Val?.ToString() ?? "default";
				string borderbottomstyle = borders?.BottomBorder?.Val?.ToString() ?? "default";
				string borderleftstyle = borders?.LeftBorder?.Val?.ToString() ?? "default";
				string borderrightstyle = borders?.RightBorder?.Val?.ToString() ?? "default";

				// Extract Cell Border Width
				string bordertopwidth = (float.Parse(borders?.TopBorder?.Size ?? 8) / 8).ToString();
				string borderbottomwidth = (
					float.Parse(borders?.BottomBorder?.Size ?? 8) / 8
				).ToString();
				string borderleftwidth = (
					float.Parse(borders?.LeftBorder?.Size ?? 8) / 8
				).ToString();
				string borderrightwidth = (
					float.Parse(borders?.RightBorder?.Size ?? 8) / 8
				).ToString();

				// Extract Cell Border Colors
				string bordertopcolor = borders?.TopBorder?.Color?.Value ?? "auto";
				string borderbottomcolor = borders?.BottomBorder?.Color?.Value ?? "auto";
				string borderleftcolor = borders?.LeftBorder?.Color?.Value ?? "auto";
				string borderrightcolor = borders?.RightBorder?.Color?.Value ?? "auto";

				// Extract cell width and height
				string cellWidth =
					cell.Descendants<TableCellWidth>().FirstOrDefault().Width?.Value != null
						? (
							(
								float.Parse(
									cell.Descendants<TableCellWidth>().FirstOrDefault().Width?.Value
								) * 2.54
							) / 1440
						).ToString("#.##")
						: "auto";

				// Extract cell background color
				var cellShading = cell.Descendants<Shading>().FirstOrDefault();
				string cellColor = cellShading?.Fill?.Value;

				//---Section to extract cell details END---// (Jonathan - COMPLETED)

				// Create the cell dictionary in the desired format. [Details of cell] (Jonathan - COMPLETED)
				var cellDict = new Dictionary<string, object>
				{
					{ "type", "cell" },
					{ "content", cellText },
					{
						"styling",
						new Dictionary<string, object>
						{
							{ "underline", underline },
							{ "bold", bold },
							{ "italic", italic },
							{ "fontType", fontType },
							{ "fontsize", fontSize },
							{ "horizontalalignment", horizontalalignment },
							{ "verticalalignment", verticalalignment },
							{ "textcolor", textcolor },
							{ "highlightcolor", texthighlight },
							{ "bordertopstyle", bordertopstyle },
							{ "borderbottomstyle", borderbottomstyle },
							{ "borderleftstyle", borderleftstyle },
							{ "borderrightstyle", borderrightstyle },
							{ "bordertopwidth", bordertopwidth },
							{ "borderbottomwidth", borderbottomwidth },
							{ "borderleftwidth", borderleftwidth },
							{ "borderrightwidth", borderrightwidth },
							{ "bordertopcolor", bordertopcolor },
							{ "borderbottomcolor", borderbottomcolor },
							{ "borderleftcolor", borderleftcolor },
							{ "borderrightcolor", borderrightcolor },
							{ "cellWidth", cellWidth },
							{ "rowHeight", rowHeight },
							{ "backgroundcolor", cellColor },
						}
					},
				};

				// Save to dictionary to cell list
				cellList.Add(cellDict);
			}

			// Create a row dictionary matching the desired structure.
			var rowDict = new Dictionary<string, object>
			{
				{ "type", "row" },
				{ "content", "" },
				{ "runs", cellList },
			};

			// Save to dictionary to row list
			tableRows.Add(rowDict);
		}

		// Create the table dictionary with type "Table" and add the row dictionaries as its "runs".
		var tableDict = new Dictionary<string, object>
		{
			{ "type", "table" },
			{ "content", "" },
			{ "runs", tableRows },
		};

		// Return the table dictionary to whoever called it
		return tableDict;
	}
}
