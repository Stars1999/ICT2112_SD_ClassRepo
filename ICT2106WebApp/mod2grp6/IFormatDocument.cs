namespace ICT2106WebApp.mod2grp6
{
    public interface IFormatDocument
    {

        bool StartFormatting(Format content);


        bool FormatHeadings();


        bool FormatParagraphs();


        MetaData ProcessMetaData();
    }
}
