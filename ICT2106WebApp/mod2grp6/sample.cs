using ICT2106WebApp.mod2grp6.Text;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6
{
    class Testing
    {
        List<AbstractNode> HeadingsAndParagraphs = new List<AbstractNode> { 
            new SimpleNode(1, "h1", "Header 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(2, "h2", "Header 2", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(3, "h3", "Header 3", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "434343" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
        };
        
        List<AbstractNode> LayoutContent = new List<AbstractNode> {
            new SimpleNode(1, "layout", "", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Orientation", "Portrait" }, { "PageWidth", 21 }, { "PageHeight", 29.7 }, { "ColumnNum", 1 }, { "ColumnSpacing", 1.25 }, { "Margins", new Dictionary<string, object> { { "Top", 2.54 }, { "Bottom", 2.54 }, { "Left", 2.54 }, { "Right", 2.54 }, { "Header", 1.25 }, { "Footer", 1.25 } } } } })
        };

        List<AbstractNode> TextContent = new List<AbstractNode>{
        new SimpleNode(1, "paragraph", "test text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(2, "paragraph", "Colored text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "FF0000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(3, "paragraph", "diff color and highlighted", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "0000FF" }, { "Highlight", "cyan" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(4, "paragraph", "this is a bolded text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(5, "paragraph", "this is an italic text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(6, "paragraph", "this is an indented text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(7, "paragraph", "middle align", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "center" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(8, "paragraph", "end align", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "right" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(9, "paragraph", "sample text 示例文本", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Arial Unicode MS" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        new SimpleNode(11, "paragraph", "Math Below", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Comic Sans MS" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
};

        List<AbstractNode> Metadata = new List<AbstractNode>
            {
                new SimpleNode(9, "metadata", "CreatedDate_Internal: 2025-02-21 05:57:00Z", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "CreatedDate_Internal", "2025-02-21 05:57:00Z" } } }),
                new SimpleNode(9, "metadata", "LastModified_Internal: 2025-03-17 06:25:00Z", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "LastModified_Internal", "2025-03-17 06:25:00Z" } } }),
                new SimpleNode(9, "metadata", "filename: Datarepository_zx_v2.docx", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "filename", "Datarepository_zx_v2.docx" } } }),
                new SimpleNode(9, "metadata", "size: 2148554", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "size", "2148554" } } })
            };

        List<AbstractNode> MathContent = new List<AbstractNode>
            {
                new SimpleNode(4, "math", "(1/2) × √(4)", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Italic", true } } }),
                new SimpleNode(5, "math", "∫_0^∞ e^(-x^2)dx", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "FontColor", "000000" } } })
            };

        List<AbstractNode> Lists = new List<AbstractNode>
            {
                new SimpleNode(6, "bulleted_list", "• Item 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "ListType", "bulleted" } } }),
                new SimpleNode(7, "numbered_list", "1. First item", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "ListType", "numbered" } } })
            };

        List<AbstractNode> Images = new List<AbstractNode>
            {
                new SimpleNode(10, "image", "Image_rId9.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Left Align" } } }),
                new SimpleNode(11, "image", "Image_rId10.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Center Align" } } })
            };
    }

}

