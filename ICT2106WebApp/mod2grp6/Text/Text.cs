using System;

namespace ICT2106WebApp.mod2grp6.Text
{
    /// Text DTO class for transferring text formatting data
    public class Text
    {
        // Properties
        private string id;
        private string font;
        private string styles;
        private string color;
        private float textSpacing;
        private string alignment;

        /// Constructor for Text DTO
        public Text()
        {
            id = Guid.NewGuid().ToString();
            font = "Times New Roman";
            styles = "";
            color = "black";
            textSpacing = 1.0f;
            alignment = "left";
        }

        /// Parameterized constructor for Text DTO
        public Text(string id, string font, string styles, string color, float textSpacing, string alignment)
        {
            this.id = id;
            this.font = font;
            this.styles = styles;
            this.color = color;
            this.textSpacing = textSpacing;
            this.alignment = alignment;
        }

        // Getters
        public string GetId() { return id; }
        public string GetFont() { return font; }
        public string GetStyles() { return styles; }
        public string GetColor() { return color; }
        public float GetTextSpacing() { return textSpacing; }
        public string GetAlignment() { return alignment; }

        // Setters
        public void SetId(string id) { this.id = id; }
        public void SetFont(string font) { this.font = font; }
        public void SetStyles(string styles) { this.styles = styles; }
        public void SetColor(string color) { this.color = color; }
        public void SetTextSpacing(float textSpacing) { this.textSpacing = textSpacing; }
        public void SetAlignment(string alignment) { this.alignment = alignment; }
    }
}