namespace ICT2106WebApp.mod2grp6.Text
{
    public interface IFormatText
    {
        bool StartTextFormatting(Text content);
        bool FormatFonts();
        bool FormatStyles();
        bool FormatColors();
        public bool FormatLineSpacing();
        bool FormatAlignment();
        //List<AbstractNode> ApplyTextFormatting();
    }
}