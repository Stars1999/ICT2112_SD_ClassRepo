using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ICT2106WebApp.mod1grp4
{
    public class TableCell
    {
        [BsonElement("content")]
        private string content;
        
        [BsonElement("contentStyle")]
        private Dictionary<string, bool> contentStyle;
        
        [BsonElement("cellStyle")]
        private List<string> cellStyle;

        public string GetContent() => content;
        public Dictionary<string, bool> GetContentStyle() => contentStyle;
        public List<string> GetCellStyle() => cellStyle;

        public void SetContent(string content) => this.content = content;
        public void SetContentStyle(Dictionary<string, bool> style) => contentStyle = style;
        public void SetCellStyle(List<string> style) => cellStyle = style;

        // Constructor
        public TableCell(string content, Dictionary<string, bool> contentStyle, List<string> cellStyle)
        {
            this.content = content;
            this.contentStyle = contentStyle ?? new Dictionary<string, bool>(); // Avoid null reference
            this.cellStyle = cellStyle ?? new List<string>(); // Avoid null reference
        }
    }
}