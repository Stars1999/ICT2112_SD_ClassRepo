namespace ICT2106WebApp.mod2grp6.Format
{
    public class Layout
    {
        private string id;
        private string headers;
        private string footers;
        private float margins;
        private string orientation;
        private float pageSize;
        private int columnNum;
        private float columnSpacing;

        public string GetHeaders()
        {
            return headers;
        }

        public string GetFooters()
        {
            return footers;
        }

        public float GetMargins()
        {
            return margins;
        }

        public string GetOrientation()
        {
            return orientation;
        }

        public float GetPageSize()
        {
            return pageSize;
        }

        public int GetColumnNum()
        {
            return columnNum;
        }

        public float GetColumnSpacing()
        {
            return columnSpacing;
        }

        public void SetHeaders(string content)
        {
            this.headers = content;
        }

        public void SetFooters(string content)
        {
            this.footers = content;
        }

        public void SetMargins(float margin)
        {
            this.margins = margin;
        }

        public void SetOrientation(string orientation)
        {
            this.orientation = orientation;
        }

        public void SetPageSize(float size)
        {
            this.pageSize = size;
        }

        public void SetColumnNum(int num)
        {
            this.columnNum = num;
        }

        public void SetColumnSpacing(float space)
        {
            this.columnSpacing = space;
        }
    }
}