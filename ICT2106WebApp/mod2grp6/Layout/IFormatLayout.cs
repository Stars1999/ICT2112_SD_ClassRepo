namespace ICT2106WebApp.mod2grp6.Layout
{
    public interface IFormatLayout
    {
        bool StartLayoutFormatting(Layout content);
        bool FormatHeaders();
        bool FormatFooters();
        bool FormatMargins();
        bool FormatOrientation();
        bool FormatPageSize();
        bool FormatColumnNum();
        bool FormatColumnSpacing();
        //List<AbstractNode> ApplyLayoutFormatting();
    }
}