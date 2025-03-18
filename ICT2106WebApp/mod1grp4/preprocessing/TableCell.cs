using System.Collections.Generic;

namespace ICT2106WebApp.mod1grp4
{
    public class TableCell
    {
        private string content;
        private List<string> contentStyle;
        private int rowSpan;
        private int colSpan;
        private List<string> cellStyle;





        public string GetContent() => content;
        public List<string> GetContentStyle() => contentStyle;
        public int GetRowSpan() => rowSpan;
        public int GetColSpan() => colSpan;
        public List<string> GetCellStyle() => cellStyle;

        public void SetContent(string content) => this.content = content;
        public void SetContentStyle(List<string> style) => contentStyle = style;
        public void SetRowSpan(int rowSpan) => this.rowSpan = rowSpan;
        public void SetColSpan(int colSpan) => this.colSpan = colSpan;
        public void SetCellStyle(List<string> style) => cellStyle = style;

        // private string content { get; set; }
        // private List<string> contentStyle { get; set; }
        // private int rowSpan { get; set; }
        // private int colSpan { get; set; }
        // private List<string> cellStyle { get; set; }

        // Constructor
        public TableCell(string content, List<string> contentStyle, int rowSpan, int colSpan, List<string> cellStyle)
        {
            this.content = content;
            this.contentStyle = contentStyle ?? new List<string>(); // Avoid null reference
            this.rowSpan = rowSpan;
            this.colSpan = colSpan;
            this.cellStyle = cellStyle ?? new List<string>(); // Avoid null reference
        }
    }
}