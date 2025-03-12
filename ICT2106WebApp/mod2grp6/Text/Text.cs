namespace ICT2106WebApp.mod2grp6.Text
{
    public class Text
    {
        private string id;
        private string fontType;
        private int fontSize;
        private bool bolded;
        private bool italic;
        private bool underline;
        private string color;
        private string highlight;
        private string alignment;
        private string lineSpacing;

        public string GetFontType()
        {
            return fontType;
        }

        public int GetFontSize()
        {
            return fontSize;
        }

        public bool GetBolded()
        {
            return bolded;
        }

        public bool GetItalic()
        {
            return italic;
        }

        public bool GetUnderline()
        {
            return underline;
        }

        public string GetColor()
        {
            return color;
        }

        public string GetHighlight()
        {
            return highlight;
        }

        public string GetAlignment()
        {
            return alignment;
        }

        public string GetLineSpacing()
        {
            return lineSpacing;
        }

        public void SetFontType(string font)
        {
            this.fontType = font;
        }

        public void SetFontSize(int size)
        {
            this.fontSize = size;
        }

        public void SetBolded(bool bolded)
        {
            this.bolded = bolded;
        }

        public void SetItalic(bool italic)
        {
            this.italic = italic;
        }

        public void SetUnderline(bool underline)
        {
            this.underline = underline;
        }

        public void SetColor(string color)
        {
            this.color = color;
        }

        public void SetHighlight(string color)
        {
            this.highlight = color;
        }

        public void SetAlignment(string alignment)
        {
            this.alignment = alignment;
        }

        public void SetLineSpacing(string space)
        {
            this.lineSpacing = space;
        }
    }
}