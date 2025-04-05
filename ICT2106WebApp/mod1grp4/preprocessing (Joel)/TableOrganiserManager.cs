using System.Text.Json;
using DocumentFormat.OpenXml.EMMA;
using ICT2106WebApp.mod1Grp3;

namespace ICT2106WebApp.mod1grp4
{
	public class TableOrganiserManager : iTableOrganise<AbstractNode>
	{
		// Convert the abstract nodes given into custom table entity (Joel - COMPLETED)
		public List<Table> organiseTables(List<AbstractNode> abstractNodes)
		{
			Console.WriteLine("MODULE 1 GROUP 4: START");
			var tables = new Dictionary<int, Table>(); // Use a dictionary to group tables by their ID

			foreach (var node in abstractNodes)
			{
				if (
					node is CompositeNode tableCompositeNode
					&& tableCompositeNode.GetNodeData("nodeinfo")["nodeType"].ToString() == "table"
				)
				{
					var tableId = tableCompositeNode.GetNodeData("nodeinfo")["nodeId"].ToString();

					// Check if the table already exists in the dictionary
					if (!tables.TryGetValue(int.Parse(tableId), out var existingTable))
					{
						// Create a new table if it doesn't already exist
						existingTable = new Table(
							int.Parse(tableId),
							new List<TableRow>(),
							false,
							tableCompositeNode.GetNodeData("nodeinfo")["content"].ToString()
						);
						existingTable.type = "table";
						tables[int.Parse(tableId)] = existingTable;
					}

					// Process rows and add them to the existing table
					foreach (var rowNode in tableCompositeNode.GetChildren())
					{
						if (
							rowNode is CompositeNode rowCompositeNode
							&& rowCompositeNode.GetNodeData("nodeinfo")["nodeType"].ToString()
								== "row"
						)
						{
							var cells = new List<TableCell>();

							// Process cells within the row
							foreach (var cellNode in rowCompositeNode.GetChildren())
							{
								if (
									cellNode is AbstractNode cellAbstractNode
									&& cellAbstractNode
										.GetNodeData("nodeinfo")["nodeType"]
										.ToString() == "cell"
								)
								{
									var cellContent = cellAbstractNode
										.GetNodeData("nodeinfo")["content"]
										.ToString();
									var cellStylingRaw =
										cellAbstractNode.GetNodeData("nodeinfo")["styling"]
										as List<Dictionary<string, object>>;
									;
									;

									// Inline conversion logic
									var cellStyling = new CellStyling();
									foreach (var styleDict in cellStylingRaw)
									{
										foreach (var kvp in styleDict)
										{
											switch (kvp.Key.ToLower())
											{
												case "bold":

													cellStyling.bold = Convert.ToBoolean(kvp.Value);
													break;
												case "italic":
													cellStyling.italic = Convert.ToBoolean(
														kvp.Value
													);
													break;
												case "underline":
													cellStyling.underline = Convert.ToBoolean(
														kvp.Value
													);
													break;
												case "highlightcolor":
													cellStyling.highlight = kvp.Value?.ToString();
													break;
												case "textcolor":
													cellStyling.textcolor = kvp.Value?.ToString();
													break;
												case "fontsize":
													cellStyling.fontsize = Convert.ToInt32(
														kvp.Value
													);
													break;
												case "horizontalalignment":
													cellStyling.horizontalalignment =
														kvp.Value?.ToString();
													break;
												case "verticalalignment":
													cellStyling.verticalalignment =
														kvp.Value?.ToString();
													break;
												case "topstyle":
													cellStyling.topstyle = kvp.Value?.ToString();
													break;
												case "bottomstyle":
													cellStyling.bottomstyle = kvp.Value?.ToString();
													break;
												case "leftstyle":
													cellStyling.leftstyle = kvp.Value?.ToString();
													break;
												case "rightstyle":
													cellStyling.rightstyle = kvp.Value?.ToString();
													break;
												case "topwidth":
													cellStyling.topwidth = kvp.Value?.ToString();
													break;
												case "bottomwidth":
													cellStyling.bottomwidth = kvp.Value?.ToString();
													break;
												case "leftwidth":
													cellStyling.leftwidth = kvp.Value?.ToString();
													break;
												case "rightwidth":
													cellStyling.rightwidth = kvp.Value?.ToString();
													break;
												case "topcolor":
													cellStyling.topcolor = kvp.Value?.ToString();
													break;
												case "bottomcolor":
													cellStyling.bottomcolor = kvp.Value?.ToString();
													break;
												case "leftcolor":
													cellStyling.leftcolor = kvp.Value?.ToString();
													break;
												case "rightcolor":
													cellStyling.rightcolor = kvp.Value?.ToString();
													break;
												case "bordertopstyle":
													cellStyling.bordertopstyle =
														kvp.Value?.ToString();
													break;
												case "borderbottomstyle":
													cellStyling.borderbottomstyle =
														kvp.Value?.ToString();
													break;
												case "borderleftstyle":
													cellStyling.borderleftstyle =
														kvp.Value?.ToString();
													break;
												case "borderrightstyle":
													cellStyling.borderrightstyle =
														kvp.Value?.ToString();
													break;
												case "bordertopwidth":
													cellStyling.bordertopwidth =
														kvp.Value?.ToString();
													break;
												case "borderbottomwidth":
													cellStyling.borderbottomwidth =
														kvp.Value?.ToString();
													break;
												case "cellwidth":
													cellStyling.cellWidth = kvp.Value?.ToString();
													break;
												case "rowheight":
													cellStyling.rowHeight = kvp.Value?.ToString();
													break;
												case "backgroundcolor":
													cellStyling.backgroundcolor =
														kvp.Value?.ToString();
													break;
												default:
													break;
											}
										}
									}

									cells.Add(
										new TableCell(cellContent, cellStyling) { type = "cell" }
									);
								}
							}

							// Add the row to the table
							existingTable.rows.Add(
								new TableRow(string.Empty, cells) { type = "row" }
							);
						}
					}
				}
			}

			// Return the list of tables
			return tables.Values.ToList();
		}
	}
}
