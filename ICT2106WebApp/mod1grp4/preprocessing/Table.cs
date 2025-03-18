using System.Collections.Generic;

namespace ICT2106WebApp.mod1grp4
{
    public class Table
    {
        private int tableId;
        private int rows;
        private int columns;
        private List<TableCell> cells;
        private List<string> style;
        private int lastProcessedNode;
        private bool tableCompletionState;
        private Table table;
        private string latexOutput;


        public int GetTableId() => tableId;
        public int GetRows() => rows;
        public int GetColumns() => columns;
        public List<TableCell> GetCells() => cells;
        public List<string> GetStyle() => style;
        public int GetLastProcessedNode() => lastProcessedNode;
        public bool GetTableCompletionState() => tableCompletionState;
        public Table GetTable() => table;
        public string GetLatexOutput() => latexOutput;

        public void SetTableId(int id) => tableId = id;
        public void SetRows(int rows) => this.rows = rows;
        public void SetColumns(int columns) => this.columns = columns;
        public void SetCells(List<TableCell> cell) => cells = cell;
        public void SetStyle(List<string> style) => this.style = style;
        public void SetLastProcessedNode(int node) => lastProcessedNode = node;
        public void SetTableCompletionState(bool state) => tableCompletionState = state;
        public void SetTable(Table table) => this.table = table;
        public void SetLatexOutput(string latexOutput) => this.latexOutput = latexOutput;

        // private int tableId { get; set; }
        // private int rows { get; set; }
        // private int columns { get; set; }
        // private List<TableCell> cells { get; set; }
        // private List<string> style { get; set; }
        // private int lastProcessedNode { get; set; }
        // private bool tableCompletionState { get; set; }
        // private Table table { get; set; }
        // private string latexOutput { get; set; }

        // Constructor
        public Table(int tableId, int rows, int columns, List<TableCell> cells, List<string> style, int lastProcessedNode, bool tableCompletionState, Table table, string latexOutput)
        {
            this.tableId = tableId;
            this.rows = rows;
            this.columns = columns;
            this.cells = cells ?? new List<TableCell>();  // Avoid null reference
            this.style = style ?? new List<string>();  // Avoid null reference
            this.lastProcessedNode = lastProcessedNode;
            this.tableCompletionState = tableCompletionState;
            this.table = table;
            this.latexOutput = latexOutput;
        }
    }
}