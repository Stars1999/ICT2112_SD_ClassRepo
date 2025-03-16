using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Layout
{

    public class LayoutManager : IFormatLayout
    {
        private Layout content;

        public bool StartLayoutFormatting(List<AbstractNode> content)
        {
            return false;
        }

        public bool FormatHeaders()
        {
            return false;
        }

        public bool FormatFooters()
        {
            return false;
        }

        public bool FormatMargins()
        {
            return false;
        }

        public bool FormatOrientation()
        {
            return false;
        }

        public bool FormatPageSize()
        {
            return false;
        }

        public bool FormatColumnNum()
        {
            return false;
        }

        public bool FormatColumnSpacing()
        {
            return false;
        }
        
        public List<AbstractNode> ApplyLayoutFormatting()
        {
            return null;
        }

    }
}