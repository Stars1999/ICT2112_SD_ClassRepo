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
		//extract table
		public static Dictionary<string, object> ExtractTable(Table table)
		{
			// var TS = new TableStructureManager();
			// var tableDict = TS.extractTableStructure(table);

			// return tableDict;

			return new TableStructureManager().extractTableStructure(table);


			//Old Version, to verify 
			// var tableRows = new List<Dictionary<string, object>>();

			// foreach (var row in table.Elements<TableRow>())
			// {
			// 	// List to hold cell dictionaries for this row.
			// 	var cellList = new List<Dictionary<string, object>>();

			// 	foreach (var cell in row.Elements<TableCell>())
			// 	{
			// 		// Extract the cell text.
			// 		string cellText = string.Join("", cell.Descendants<Text>().Select(t => t.Text));

			// 		// //Example Extracter
			// 		// var tester = cell.Descendants<Font>().FirstOrDefault();
			// 		// string test = tester?.Name?.ToString();

			// 		//Get Run Properties
			// 		var cellTextStyle = cell.Descendants<RunProperties>().FirstOrDefault();

			// 		bool underline = cellTextStyle?.GetFirstChild<Underline>() != null;
			// 		bool bold = cellTextStyle?.GetFirstChild<Bold>() != null;
			// 		bool italic = cellTextStyle?.GetFirstChild<Italic>() != null;
			// 		//Get font size and style
			// 		float fontSize = cell.Descendants<FontSize>().FirstOrDefault()?.Val != null 
			// 		? float.Parse(cell.Descendants<FontSize>().FirstOrDefault().Val) / 2 
			// 		: 11;
			// 		string fontType = cellTextStyle?.RunFonts?.Ascii?.ToString() ?? "Arial";
			// 		//Get Text Alignment
			// 		string horizontalalignment = cell.Descendants<Justification>().FirstOrDefault()?.Val?.ToString() ?? "left";
					
			// 		//Dump
			// 		// var tester = cell.Descendants<FontSize>().FirstOrDefault();
			// 		// string test = tester?.Val?.ToString() ?? "11";

			// 		// Extract cell borders details
			// 		var borders = cell.Descendants<TableCellBorders>().FirstOrDefault();
			// 		var cellBorderDetails = new Dictionary<string, object>
			// 		{
						
			// 			//Extract Cell Border Styles
			// 			{ "topstyle", borders?.TopBorder?.Val?.ToString() ?? "default" },
			// 			{ "bottomstyle", borders?.BottomBorder?.Val?.ToString() ?? "default" },
			// 			{ "leftstyle", borders?.LeftBorder?.Val?.ToString() ?? "default" },
			// 			{ "rightstyle", borders?.RightBorder?.Val?.ToString() ?? "default" },

			// 			//Extract Cell Border Width	
			// 			{ "topwidth", (float.Parse(borders?.TopBorder?.Size ?? 8)/8).ToString() },
			// 			{ "bottomwidth", (float.Parse(borders?.BottomBorder?.Size ?? 8)/8).ToString() },
			// 			{ "leftwidth", (float.Parse(borders?.LeftBorder?.Size ?? 8)/8).ToString() },
			// 			{ "rightwidth", (float.Parse(borders?.RightBorder?.Size ?? 8)/8).ToString() },

			// 			//Extract Cell Border Colors
			// 			{ "topcolor", borders?.TopBorder?.Color?.Value ?? "auto" },
			// 			{ "bottomcolor", borders?.BottomBorder?.Color?.Value ?? "auto" },
			// 			{ "leftcolor", borders?.LeftBorder?.Color?.Value ?? "auto" },
			// 			{ "rightcolor", borders?.RightBorder?.Color?.Value ?? "auto" },

			// 			//Any other border values to extract?
			// 		};

			// 		// Extract cell padding details (Not working yet)
			// 		// var cellPadding = cell.Descendants<TableCellMargin>().FirstOrDefault();
			// 		// var cellPaddingDetails = new Dictionary<string, object>
			// 		// {
			// 		// 	{ "toppadding", cellPadding?.TopMargin?.Val.ToString() },
			// 		// 	// { "bottompadding", cellPadding?.BottomMargin?.Width?.ToString() },
			// 		// 	// { "leftpadding", cellPadding?.LeftMargin?.Width?.ToString() },
			// 		// 	// { "rightpadding", cellPadding?.RightMargin?.Width?.ToString() },
			// 		// };


			// 		// // Extract cell width and height
			// 		string cellWidth = cell.Descendants<TableCellWidth>().FirstOrDefault().Width?.Value != null
			// 		? ((float.Parse(cell.Descendants<TableCellWidth>().FirstOrDefault().Width?.Value)*2.54)/1440).ToString("#.##")
			// 		: "auto";
			// 		var rowHeight = row.Descendants<TableRowHeight>().FirstOrDefault() != null //Try find a way to get value, maybe use another proprty?
			// 		? ((float.Parse(row.Descendants<TableRowHeight>().FirstOrDefault().Val)*2.54)/1440).ToString("#.##")
			// 		: "auto";

			// 		var cellSizeDetails = new Dictionary<string, object>
			// 		{
			// 			//width and height originally given in twips, to convert to cm need (twip*2.54)/1440
			// 			// { "cellwidth", ((float.Parse(cellWidth?.Width?.Value)*2.54)/1440).ToString("#.##") },
			// 			// { "cellheight", ((float.Parse(rowHeight?.Val ?? 1)*2.54)/1440).ToString("#.##")},
			// 			{ "cellwidth", cellWidth },
			// 			{ "cellheight", rowHeight }
			// 		};

			// 		// Extract cell background color
			// 		var cellShading = cell.Descendants<Shading>().FirstOrDefault();
			// 		string cellColor = cellShading?.Fill?.Value;
					

			// 		// Create the cell dictionary in the desired format. (Details of cell)
			// 		var cellDict = new Dictionary<string, object>
			// 		{
			// 			{ "type", "cell" },
			// 			{ "content", cellText },
			// 			{
			// 				"styling",
			// 				new Dictionary<string, object>
			// 				{
			// 					{ "underline", underline},
			// 					{ "bold", bold},
			// 					{ "italic", italic},
			// 					{ "fontType", fontType },
			// 					{ "fontsize", fontSize },
			// 					{ "horizontalalignment", horizontalalignment},
			// 					//GPT Start
			// 					{ "border", cellBorderDetails },
			// 					// { "padding", cellPaddingDetails },
			// 					{ "size", cellSizeDetails },
			// 					{ "backgroundcolor", cellColor },
			// 					// { "test", test },
			// 				}
			// 			},
			// 		};

			// 		cellList.Add(cellDict);
			// 	}

			// 	// // You can also adjust the row styling as needed.
			// 	// var rowStyling = new Dictionary<string, object>
			// 	// {
			// 	// 	{ "bold", false },
			// 	// 	{ "italic", true },
			// 	// 	{ "alignment", "right" },
			// 	// 	{ "fontsize", 12 },
			// 	// 	{ "fonttype", "Aptos" },
			// 	// 	{ "fontcolor", "0E2841" },
			// 	// 	{ "highlight", "none" },
			// 	// };

			// 	// Create a row dictionary matching the desired structure.
			// 	var rowDict = new Dictionary<string, object>
			// 	{
			// 		{ "type", "row" },
			// 		{ "content", "" },
			// 		{ "runs", cellList },
			// 		// { "styling", rowStyling },
			// 	};

			// 	tableRows.Add(rowDict);
			// }

			// // Create the table dictionary with type "Table" and add the row dictionaries as its "runs".
			// var tableDict = new Dictionary<string, object>
			// {
			// 	{ "type", "table" },
			// 	{ "content", "" },
			// 	{ "runs", tableRows },
			// };

			// return tableDict;
		}

	}
}
