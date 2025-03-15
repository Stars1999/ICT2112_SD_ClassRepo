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
