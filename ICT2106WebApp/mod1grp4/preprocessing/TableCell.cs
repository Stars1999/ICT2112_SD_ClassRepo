namespace ICT2106WebApp.mod1grp4
{
    public class TableCell
    {
        private string content { get; set; }
        private List<string> contentStyle { get; set; }
        private int rowSpan { get; set; }
        private int colSpan { get; set; }
        private List<string> cellStyle { get; set; }

        // Constructor
        public TableCell(string content, List<string> contentStyle, int rowSpan, int colSpan, List<string> cellStyle)
        {
            this.content = content;
            this.contentStyle = contentStyle ?? new List<string>(); // Avoid null reference
            this.rowSpan = rowSpan;
            this.colSpan = colSpan;
            this.cellStyle = cellStyle ?? new List<string>(); // Avoid null reference
        }



        // public string GetContent() => content;
        // public List<string> GetContentStyle() => contentStyle;
        // private int GetRowSpan() => rowSpan;
        // private int GetColSpan() => colSpan;
        // private List<string> GetCellStyle() => cellStyle;

        // public void SetContent(string content) => this.content = content;
        // public void SetContentStyle(List<string> style) => contentStyle = style;
        // private void SetRowSpan(int rowSpan) => this.rowSpan = rowSpan;
        // private void SetColSpan(int colSpan) => this.colSpan = colSpan;
        // private void SetCellStyle(List<string> style) => cellStyle = style;
    }
}