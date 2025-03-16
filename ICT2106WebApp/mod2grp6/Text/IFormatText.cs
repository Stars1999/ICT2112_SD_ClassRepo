using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Text
{
    public interface IFormatText
    {
        bool StartTextFormatting(List<AbstractNode> content);
        bool FormatFonts();
        bool FormatStyles();
        bool FormatColors();
        public bool FormatLineSpacing();
        bool FormatAlignment();
        //List<AbstractNode> ApplyTextFormatting();
    }
}