using System.Text.Json;
using Utilities;

namespace ICT2106WebApp.mod1grp4
{
	// ProcessedTableManager (Siti - COMPLETED)
	public class ProcessedTableManager : iBackupTabularSubject
	{
		public ProcessedTableManager() { }

		// Use module 3 logger to log latex validation status (Siti - COMPLETED)
		public bool logProcessingStatus(string description)
		{
			// Log to module 3
			var logID = Guid.NewGuid().ToString();
			var logTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			var logLocation = "Table";
			Console.WriteLine(
				$"insertLog({logID}, {logTimeStamp}, {description}, {logLocation}) (HIEW TENG - VALIDATION, SITI - LOGGING)"
			);
			return true;
		}

		// Use group 3 function to slot processed tables to tree (Siti - COMPLETED)
		public async Task<bool> slotProcessedTableToTree(
			List<Table> processedTables,
			List<AbstractNode> abstractNodes
		)
		{
			// Iterate through the processed tables
			foreach (var processedTable in processedTables)
			{
				// Find the corresponding table node in the abstract nodes list
				var tableNode = abstractNodes
					.OfType<CompositeNode>()
					.FirstOrDefault(node =>
						node.GetNodeType() == "table" && node.GetNodeId() == processedTable.tableId
					);

				if (tableNode != null)
				{
					// Set the LaTeX content for the table node
					tableNode.SetContent(processedTable.latexOutput);
					tableNode.SetConverted(true); // Mark the table as converted
					Console.WriteLine(
						$"Updated group 3 table node {tableNode.GetNodeId()} with updated LaTeX content. (SITI)"
					);

					// Traverse rows and cells to mark them as converted
					foreach (var rowNode in tableNode.GetChildren())
					{
						if (
							rowNode is CompositeNode rowCompositeNode
							&& rowCompositeNode.GetNodeType() == "row"
						)
						{
							rowCompositeNode.SetConverted(true); // Mark the row as converted

							foreach (var cellNode in rowCompositeNode.GetChildren())
							{
								if (
									cellNode is AbstractNode cellAbstractNode
									&& cellAbstractNode.GetNodeType() == "cell"
								)
								{
									cellAbstractNode.SetConverted(true); // Mark the cell as converted
								}
							}
						}
					}
				}
				else
				{
					Console.WriteLine(
						$"Warning: No matching table node found for table ID {processedTable.tableId}. (SITI)"
					);
				}
			}

			Console.WriteLine("Processed tables have been slotted back into the tree. (SITI)");

			// Delete tables that are no longer needed
			await deleteTables(processedTables);
			Console.WriteLine("MODULE 1 GROUP 4: END");
			return true;
		}

		// Delete tables that are no longer needed (Siti - COMPLETED)
		public async Task<bool> deleteTables(List<Table> tables)
		{
			foreach (var table in tables)
			{
				await notify<bool>(
					OperationType.DELETE,
					"Deleting tables since no longer needed. (SITI)",
					table
				);
			}
			return true;
		}
	}
}
