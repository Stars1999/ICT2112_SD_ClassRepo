using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Format
{
    public interface IFormatDocument
    {

        bool StartFormatting(List<AbstractNode> content);
        bool FormatHeadings();
        bool FormatParagraphs();
        bool ProcessMetaData()
    }
}
