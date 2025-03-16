using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Text
{
    public class TextManager : IFormatText
    {
        private List<AbstractNode> content;

        public bool StartTextFormatting(List<AbstractNode> content)
        {
            return false;
        }

        public bool FormatFonts()
        {
            return false;
        }

        public bool FormatStyles()
        {
            return false;
        }

        public bool FormatColors()
        {
            return false;
        }

        public bool FormatAlignment()
        {
            return false;
        }

        public bool FormatLineSpacing()
        {
            return false;
        }

        public List<AbstractNode> ApplyTextFormatting()
        {
            return null;
        } 
    }
}