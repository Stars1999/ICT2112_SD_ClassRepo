using System.Text.RegularExpressions;
using Utilities;

namespace ICT2106WebApp.mod1grp4
{
	public class TableValidationManager : iTableValidate
	{
		// Validate the content and style of the original nodes given and the processed tables (Hiew Teng - COMPLETED)
		public string validateTableLatexOutput(
			List<AbstractNode> originalNodes,
			List<Table> processedTables
		)
		{
			var currentTableIndex = 0;

			foreach (var tableNode in originalNodes)
			{
				if (
					tableNode is CompositeNode tableCompositeNode
					&& tableCompositeNode.GetNodeData("nodeinfo")["nodeType"].ToString() == "table"
				)
				{
					var processedTable = processedTables[currentTableIndex];
					var latexOutput = processedTable.latexOutput;

					// Step 1: Verify the LaTeX output starts with "\begin{tabular}"
					if (!latexOutput.StartsWith("\\begin{tabular}"))
					{
						if (!latexOutput.Contains("\\begin{tabular}"))
						{
							return $"LaTeX output does not start with \\begin{{tabular}} for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
						}
					}

					// Step 2: Verify the LaTeX output ends with "\end{tabular}"
					if (!latexOutput.EndsWith("\\end{tabular}"))
					{
						return $"LaTeX output does not end with \\end{{tabular}} for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
					}

					// Step 3: Verify the number of \hlines matches the number of rows + 1
					var numberOfRows = tableCompositeNode.GetChildren().Count;
					var hlineCount = Regex.Matches(latexOutput, @"\\hline").Count;

					if ((numberOfRows + 1) != hlineCount)
					{
						return $"Mismatch in \\hline count. Expected {numberOfRows}, found {hlineCount} for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
					}

					// Step 4: Verify the number of cells in each row
					var latexCellCount = Regex.Matches(latexOutput, @"&").Count;
					var totalCellCount = 0;

					foreach (var rowNode in tableCompositeNode.GetChildren())
					{
						if (
							rowNode is CompositeNode rowCompositeNode
							&& rowCompositeNode.GetNodeData("nodeinfo")["nodeType"].ToString()
								== "row"
						)
						{
							var cellNodes = rowCompositeNode
								.GetChildren()
								.Where(child =>
									child.GetNodeData("nodeinfo")["nodeType"].ToString() == "cell"
								)
								.ToList();
							totalCellCount += cellNodes.Count - 1;
						}
					}

					if (latexCellCount != totalCellCount)
					{
						return $"Mismatch in cell count. Expected {totalCellCount}, found {latexCellCount} for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
					}

					// Step 5: Verify cell content and styling
					foreach (var rowNode in tableCompositeNode.GetChildren())
					{
						if (
							rowNode is CompositeNode rowCompositeNode
							&& rowCompositeNode.GetNodeData("nodeinfo")["nodeType"].ToString()
								== "row"
						)
						{
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

									Dictionary<string, string> latexEscapes = new Dictionary<
										string,
										string
									>
									{
										{ "&", "\\&" },
										{ "%", "\\%" },
										{ "$", "\\$" },
										{ "#", "\\#" },
										{ "_", "\\_" },
										{ "{", "\\{" },
										{ "}", "\\}" },
										{ "~", "\\~" },
										{ "^", "\\^" },
										{ "\\", "\\\\" },
									};

									foreach (var pair in latexEscapes)
									{
										cellContent = cellContent.Replace(pair.Key, pair.Value); // Escape special characters
									}
									string latexCell = cellContent; //Save content after escaping special char

									if (!latexOutput.Contains(latexCell))
									{
										return $"Cell content '{latexCell}' not found in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
									}

									// Verify styling
									var cellStyling =
										cellAbstractNode.GetNodeData("nodeinfo")["styling"]
										as List<Dictionary<string, object>>;
									;

									if (cellStyling != null)
									{
										if (
											cellStyling.Any(dict =>
												dict.TryGetValue("bold", out var isBold)
												&& isBold is bool boldValue
												&& boldValue
											)
										)
										{
											latexCell = $"\\textbf{{{latexCell}}}";
											if (!latexOutput.Contains(latexCell))
											{
												return $"Bold styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}

										if (
											cellStyling.Any(dict =>
												dict.TryGetValue("italic", out var isItalic)
												&& isItalic is bool italicValue
												&& italicValue
											)
										)
										{
											latexCell = $"\\textit{{{latexCell}}}";
											if (!latexOutput.Contains(latexCell))
											{
												return $"Italic styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}
										if (
											cellStyling.Any(dict =>
												dict.TryGetValue(
													"highlightcolor",
													out var highlightValue
												)
												&& highlightValue is string highlight
												&& !string.IsNullOrEmpty(highlight)
												&& highlight != "none"
											)
										)
										{
											latexCell = $"\\hl{{{latexCell}}}";

											if (!latexOutput.Contains(latexCell))
											{
												return $"Highlight styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}
										if (
											cellStyling.Any(dict =>
												dict.TryGetValue("underline", out var isUnderline)
												&& isUnderline is bool underlineValue
												&& underlineValue
											)
										)
										{
											latexCell = $"\\underline{{{latexCell}}}";
											if (!latexOutput.Contains(latexCell))
											{
												return $"Underline styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}

										var textcolorEntry =
											cellStyling
												.SelectMany(dict => dict) // Flatten the list of dictionaries into a sequence of key-value pairs
												.FirstOrDefault(kv =>
													kv.Key == "textcolor" && kv.Value is string
												) // Find the first key-value pair where the key is "rowheight" and the value is a string
												.Value as string;

										if (
											cellStyling.Any(dict =>
												dict.TryGetValue(
													"textcolor",
													out var textColorValue
												)
												&& textColorValue is string
												&& !string.IsNullOrEmpty(textColorValue.ToString())
												&& textColorValue.ToString() != "auto"
											)
										)
										{
											latexCell =
												$"\\textcolor{{{textcolorEntry}}}{{{latexCell}}}";

											if (!latexOutput.Contains(latexCell))
											{
												return $"Text color styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}

										var fontsizeValue = cellStyling
											.SelectMany(dict => dict) // Flatten the list of dictionaries into a sequence of key-value pairs
											.FirstOrDefault(kv =>
												kv.Key == "fontsize" && kv.Value is float
											) // Find the first key-value pair where the key is "horizontalalignment" and the value is a string
											.Value; // Extract the value and cast it to a string

										if (fontsizeValue is float fontSize && fontSize != 0)
										{
											latexCell =
												$"{{\\fontsize{{{fontsizeValue}}}{{\\baselineskip}}\\selectfont {latexCell}}}";

											if (!latexOutput.Contains(latexCell))
											{
												return $"Font size styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}

										var cellcolorEntry =
											cellStyling
												.SelectMany(dict => dict) // Flatten the list of dictionaries into a sequence of key-value pairs
												.FirstOrDefault(kv =>
													kv.Key == "backgroundcolor"
													&& kv.Value is string
												) // Find the first key-value pair where the key is "rowheight" and the value is a string
												.Value as string;

										if (
											cellStyling.Any(dict =>
												dict.TryGetValue(
													"backgroundcolor",
													out var textColorValue
												)
												&& textColorValue is string
												&& !string.IsNullOrEmpty(textColorValue.ToString())
												&& textColorValue.ToString() != "auto"
											)
										)
										{
											latexCell =
												$"\\cellcolor{{{cellcolorEntry}}}{{{latexCell}}}";

											if (!latexOutput.Contains(latexCell))
											{
												return $"Cell color styling for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}

										// Check if verticalalignment and rowHeight are present in cellStyling
										if (
											cellStyling.Any(dict =>
												dict.TryGetValue(
													"verticalalignment",
													out var verticalAlignmentValue
												)
												&& verticalAlignmentValue
													is string verticalAlignment
												&& !string.IsNullOrEmpty(verticalAlignment)
												&& dict.TryGetValue(
													"rowHeight",
													out var rowHeightValue
												)
												&& rowHeightValue is string rowHeight
												&& rowHeight != "auto"
											)
										)
										{
											var rowHeightEntry =
												cellStyling
													.SelectMany(dict => dict) // Flatten the list of dictionaries into a sequence of key-value pairs
													.FirstOrDefault(kv =>
														kv.Key == "rowHeight" && kv.Value is string
													) // Find the first key-value pair where the key is "rowheight" and the value is a string
													.Value as string;

											var verticalAlignmentEntry =
												cellStyling
													.SelectMany(dict => dict) // Flatten the list of dictionaries into a sequence of key-value pairs
													.FirstOrDefault(kv =>
														kv.Key == "verticalalignment"
														&& kv.Value is string
													) // Find the first key-value pair where the key is "rowheight" and the value is a string
													.Value as string;

											// Parse rowHeight
											double rowHeight = 0;
											if (
												double.TryParse(
													rowHeightEntry,
													out double parsedRowHeight
												)
											)
											{
												rowHeight = parsedRowHeight;
											}

											double topSpace = 0;
											double bottomSpace = 0;

											// Handle vertical alignment based on the parsed rowHeight
											switch (verticalAlignmentEntry.ToLower())
											{
												case "top":
													topSpace = 0;
													bottomSpace = rowHeight;
													break;

												case "bottom":
													topSpace = rowHeight;
													bottomSpace = 0;
													break;

												case "center":
													topSpace = rowHeight / 2;
													bottomSpace = rowHeight / 2;
													break;

												default:
													topSpace = 0;
													bottomSpace = rowHeight;
													break;
											}

											// Apply LaTeX formatting with the calculated topSpace and bottomSpace
											latexCell =
												$"{{ \\rule{{0pt}}{{{topSpace}cm}} \\vspace{{{bottomSpace}cm}} {latexCell}}}";

											// Check if latexOutput contains latexCell
											if (!latexOutput.Contains(latexCell))
											{
												return $"Vertical alignment for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}
										var alignmentValue =
											cellStyling
												.SelectMany(dict => dict) // Flatten the list of dictionaries into a sequence of key-value pairs
												.FirstOrDefault(kv =>
													kv.Key == "horizontalalignment"
													&& kv.Value is string
												) // Find the first key-value pair where the key is "horizontalalignment" and the value is a string
												.Value as string; // Extract the value and cast it to a string

										var cellWidthValue =
											cellStyling
												.SelectMany(dict => dict) // Flatten the list of dictionaries into a sequence of key-value pairs
												.FirstOrDefault(kv =>
													kv.Key == "cellWidth" && kv.Value is string
												) // Find the first key-value pair where the key is "horizontalalignment" and the value is a string
												.Value as string; // Extract the value and cast it to a string
										if (!string.IsNullOrEmpty(alignmentValue))
										{
											string alignmentChar =
												alignmentValue == "right" ? "r"
												: alignmentValue == "left" ? "l"
												: "c";
											string alignmentRagged =
												alignmentValue == "right" ? "raggedleft"
												: alignmentValue == "left" ? "raggedright"
												: alignmentValue == "both" ? "justifying"
												: "centering";

											latexCell =
												$"\n\\multicolumn{{1}}{{|{alignmentChar}|}}{{\\parbox{{{cellWidthValue}cm}}{{\\{alignmentRagged} {latexCell}}}}}";

											if (!latexOutput.Contains(latexCell))
											{
												return $"Horizontal alignment for cell '{cellContent}' is missing in LaTeX output for table {tableCompositeNode.GetNodeData("nodeinfo")["nodeId"]}.";
											}
										}
									}
								}
							}
						}
					}

					currentTableIndex++;
				}
			}

			return "Validation passed for all tables. Sending status for logging.";
		}
	}
}
