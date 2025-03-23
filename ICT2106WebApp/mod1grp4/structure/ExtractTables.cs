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

// //From grp3 folder, just here for completion sake, delete if needed

// namespace Utilities
// {
// 	public static partial class ExtractContent
// 	{
// 		//extract table
// 		public static Dictionary<string, object> ExtractTable(Table table)
// 		{
// 			//List of table rows
// 			var tableRows = new List<Dictionary<string, object>>();

// 			foreach (var row in table.Elements<TableRow>())
// 			{
// 				// List to hold cell dictionaries for this row. (List of cells in this row)
// 				var cellList = new List<Dictionary<string, object>>();

// 				//Extract height of row
// 				var rowHeight = row.Descendants<TableRowHeight>().FirstOrDefault();
// 				var rowheight = ((float.Parse(rowHeight?.Val)*2.54)/1440).ToString("#.##");
// 				//height originally given in twips, to convert to cm need (twip*2.54)/1440

// 				foreach (var cell in row.Elements<TableCell>())
// 				{
// 					// Extract the cell text.
// 					string cellText = string.Join("", cell.Descendants<Text>().Select(t => t.Text));

// 					// Extract basic text formatting from the first run.
// 					var firstRun = cell.Descendants<RunProperties>().FirstOrDefault();
// 					bool isBold = firstRun?.GetFirstChild<Bold>() != null;
// 					bool isItalic = firstRun?.GetFirstChild<Italic>() != null;

// 					// GPT Start
// 					// Extract cell borders details
// 					var borders = cell.Descendants<TableCellBorders>().FirstOrDefault();
// 					var cellBorderDetails = new Dictionary<string, object>
// 					{
						
// 						//Extract Cell Border Styles
// 						{ "topstyle", borders?.TopBorder?.Val?.ToString() },
// 						{ "bottomstyle", borders?.BottomBorder?.Val?.ToString() },
// 						{ "leftstyle", borders?.LeftBorder?.Val?.ToString() },
// 						{ "rightstyle", borders?.RightBorder?.Val?.ToString() },

// 						//Extract Cell Border Width
// 						{ "topwidth", borders?.TopBorder?.Size?.ToString() },
// 						{ "bottomwidth", borders?.BottomBorder?.Size?.ToString() },
// 						{ "leftwidth", borders?.LeftBorder?.Size?.ToString() },
// 						{ "rightwidth", borders?.RightBorder?.Size?.ToString() },

// 						//Extract Cell Border Colors
// 						{ "topcolor", borders?.TopBorder?.Color?.Value },
// 						{ "bottomcolor", borders?.BottomBorder?.Color?.Value },
// 						{ "leftcolor", borders?.LeftBorder?.Color?.Value },
// 						{ "rightcolor", borders?.RightBorder?.Color?.Value },

// 						//Any other border values to extract?
// 						// { "thickness", borders?.Top?.Size?.Value }, // Thickness in points
// 					};

// 					// Extract cell padding details (Not working yet)
// 					// var cellPadding = cell.Descendants<TableCellMargin>().FirstOrDefault();
// 					// var cellPaddingDetails = new Dictionary<string, object>
// 					// {
// 					// 	{ "toppadding", cellPadding?.TopMargin?.Val.ToString() },
// 					// 	// { "bottompadding", cellPadding?.BottomMargin?.Width?.ToString() },
// 					// 	// { "leftpadding", cellPadding?.LeftMargin?.Width?.ToString() },
// 					// 	// { "rightpadding", cellPadding?.RightMargin?.Width?.ToString() },
// 					// };

// 					// // Extract cell width (Height will extracted above)
// 					var cellWidth = cell.Descendants<TableCellWidth>().FirstOrDefault();
// 					var cellSizeDetails = new Dictionary<string, object>
// 					{
// 						{ "cellwidth", ((float.Parse(cellWidth?.Width?.Value)*2.54)/1440).ToString("#.##") },
// 						//width originally given in twips, to convert to cm need (twip*2.54)/1440
// 						{ "cellheight", rowheight}
// 					};

// 					// Extract cell background color
// 					var cellShading = cell.Descendants<Shading>().FirstOrDefault();
// 					string cellColor = cellShading?.Fill?.Value;
// 					//GPT section end

// 					// Create the cell dictionary in the desired format. (Details of cell)
// 					var cellDict = new Dictionary<string, object>
// 					{
// 						{ "type", "Cell" },
// 						{ "content", cellText },
// 						{
// 							"styling",
// 							new Dictionary<string, object>
// 							{
// 								{ "bold", isBold },
// 								{ "italic", isItalic },
// 								//GPT Start
// 								{ "border", cellBorderDetails },
// 								// { "padding", cellPaddingDetails },
// 								{ "size", cellSizeDetails },
// 								{ "backgroundcolor", cellColor },
// 								//GPT End
// 							}
// 						},
// 					};

// 					cellList.Add(cellDict);
// 				}

// 				// You can also adjust the row styling as needed. [Not used rn]
// 				// var rowStyling = new Dictionary<string, object>
// 				// {
// 				// 	{ "bold", false },
// 				// 	{ "italic", true },
// 				// 	{ "alignment", "right" },
// 				// 	{ "fontsize", 12 },
// 				// 	{ "fonttype", "Aptos" },
// 				// 	{ "fontcolor", "0E2841" },
// 				// 	{ "highlight", "none" },
// 				// };

// 				// Create a row dictionary matching the desired structure. (details of a row)
// 				var rowDict = new Dictionary<string, object>
// 				{
// 					{ "type", "Row" },
// 					{ "content", "" },
// 					{ "runs", cellList },
// 					// { "styling", rowStyling },
// 				};

// 				tableRows.Add(rowDict);
// 			}

// 			// Create the table dictionary with type "Table" and add the row dictionaries as its "runs".
// 			var tableDict = new Dictionary<string, object>
// 			{
// 				{ "type", "Table" },
// 				{ "content", "" },
// 				{ "runs", tableRows },
// 			};

// 			return tableDict;
// 		}

// 	}
// }
