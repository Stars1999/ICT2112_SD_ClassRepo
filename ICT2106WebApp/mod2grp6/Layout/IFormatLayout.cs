using System.Collections.Generic;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Layout
{
    public interface IFormatLayout
    {
        // Change this method to accept List<AbstractNode>
        bool StartLayoutFormatting(List<AbstractNode> content);
        bool FormatHeaders();
        bool FormatFooters();
        bool FormatMargins();
        bool FormatOrientation();
        bool FormatPageSize();
        bool FormatColumnNum();
        bool FormatColumnSpacing();
        List<AbstractNode> ApplyLayoutFormatting();
    }
}