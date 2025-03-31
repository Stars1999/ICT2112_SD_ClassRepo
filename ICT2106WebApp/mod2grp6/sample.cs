using ICT2106WebApp.mod2grp6.Text;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.Utilities;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6.TestCase
{
    class TestCases
    {
        public List<AbstractNode> HeadingsAndParagraphs = new List<AbstractNode> {
            new SimpleNode(1, "h1", "Header 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(2, "h2", "Header 2", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(3, "h3", "Header 3", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "434343" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
        };

        public List<AbstractNode> LayoutContent = new List<AbstractNode> {
            new SimpleNode(1, "layout", "", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Orientation", "Portrait" }, { "PageWidth", 21 }, { "PageHeight", 29.7 }, { "ColumnNum", 1 }, { "ColumnSpacing", 1.25 }, { "Margins", new Dictionary<string, object> { { "Top", 2.54 }, { "Bottom", 2.54 }, { "Left", 2.54 }, { "Right", 2.54 }, { "Header", 1.25 }, { "Footer", 1.25 } } } } }),
            new SimpleNode(2, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
        };

        public List<AbstractNode> ParagraphContent = new List<AbstractNode>{
            new SimpleNode(1, "paragraph", "test text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(2, "paragraph_run?", "https://puginarug.com/", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" } } }),
            new SimpleNode(3, "paragraph", "Colored text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "FF0000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(4, "paragraph", "diff color and highlighted", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "0000FF" }, { "Highlight", "cyan" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(5, "paragraph", "this is a bolded text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(6, "paragraph", "this is an italic text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(7, "paragraph", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "both" }, { "FontSize", 10 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "white" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(8, "paragraph", "whit text here", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "FFE599" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(9, "paragraph", "font calibri here", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Calibri" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(10, "paragraph", "font times new roman here", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(11, "paragraph", "diff spacing heres", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(12, "paragraph", "line spacing", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(13, "paragraph", "space after paragraph", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(14, "paragraph", "- - This is a Page Break - -", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Aptos" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.1x)" }, { "LineSpacingValue", 12.95 } } }),
            new SimpleNode(15, "paragraph", "-- Next Page Section Break --", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Aptos" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.1x)" }, { "LineSpacingValue", 12.95 } } }),
            new SimpleNode(16, "empty_paragraph1", "", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(17, "paragraph", "This is an Endnote", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Aptos" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.1x)" }, { "LineSpacingValue", 12.95 } } }),
            new SimpleNode(18, "paragraph", "😉👩☺👐💅💪😋😉😔😖😡😜😏😹💩💩💩", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(19, "paragraph", "Math Below", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Comic Sans MS" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
};

        public List<AbstractNode> TextContent = new List<AbstractNode>{
            new SimpleNode(1, "text", "Lorem Ipsum", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "both" }, { "FontSize", 10 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "white" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(2, "text", "font calibri", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Calibri" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(3, "text", "diff spacing", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(4, "text_run", "Lorem Ipsum", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "both" }, { "FontSize", 10 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "white" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(5, "text_run", "this", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(6, "text_run", " is a bolded text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(7, "text_run", "this is ", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(8, "text_run", "a", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(9, "text_run", " italic text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(10, "text_run", "this is an indented text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(11, "text_run", "middle align", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "center" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(12, "text_run", "end align", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "right" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(13, "paragraph", "PNG with Align left alignment & In Line with Text position (no move with text or fix position on page)", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 16 }, { "FontType", "Aptos Display" }, { "FontColor", "0F4761" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.2x)" }, { "LineSpacingValue", 13.9 } } }),
            new SimpleNode(14, "paragraph", "Figure 1: PNG", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 9 }, { "FontType", "Aptos" }, { "FontColor", "0E2841" }, { "Highlight", "none" }, { "LineSpacingType", "Single" }, { "LineSpacingValue", 12 } } }),
            new SimpleNode(15, "paragraph", "JPG with Align right alignment then enable Through text wrapping", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 16 }, { "FontType", "Aptos Display" }, { "FontColor", "0F4761" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.2x)" }, { "LineSpacingValue", 13.9 } } }),
            new SimpleNode(16, "paragraph", "Figure 3: JPG", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "right" }, { "FontSize", 9 }, { "FontType", "Aptos" }, { "FontColor", "0E2841" }, { "Highlight", "none" }, { "LineSpacingType", "Single" }, { "LineSpacingValue", 12 } } }),
            new SimpleNode(17, "paragraph", "JPG with Align right alignment then enable Top & Bottom text wrapping", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 16 }, { "FontType", "Aptos Display" }, { "FontColor", "0F4761" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.2x)" }, { "LineSpacingValue", 13.9 } } }),
            new SimpleNode(18, "paragraph", "JPEG with Justify alignment then enable Behind Text text wrapping", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 16 }, { "FontType", "Aptos Display" }, { "FontColor", "0F4761" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.2x)" }, { "LineSpacingValue", 13.9 } } }),
            new SimpleNode(19, "paragraph", "Figure 4: JPEG", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "both" }, { "FontSize", 9 }, { "FontType", "Aptos" }, { "FontColor", "0E2841" }, { "Highlight", "yellow" }, { "LineSpacingType", "Single" }, { "LineSpacingValue", 12 } } }),
            new SimpleNode(20, "paragraph", "JPEG with Justify alignment then enable In Front of Text text wrapping (caption hidden underneath image -> can move image to see)", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 16 }, { "FontType", "Aptos Display" }, { "FontColor", "0F4761" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.2x)" }, { "LineSpacingValue", 13.9 } } })

        };

        public List<AbstractNode> MetadataContent = new List<AbstractNode>
        {
            new SimpleNode(1, "metadata", "aa", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "CreatedDate_Internal", "2025-02-21 05:57:00Z" }, { "LastModified_Internal", "2025-03-30 00:03:00Z" }, { "filename", "Datarepository_zx_v3.docx" }, { "size", "2149420" } } }),
        };

        public List<AbstractNode> MathContent = new List<AbstractNode>
        {
            new SimpleNode(1, "math", "(1/2) × √(4)=1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(2, "math", "(1/2)cos2x+ (3/8)sin4x =3x", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(3, "math", "log_24y ≥ (π/2)", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "center" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.1x)" }, { "LineSpacingValue", 12.95 } } }),
            new SimpleNode(4, "math", "∴ ∞ ≠ ±α", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(5, "math", "∃xPersonx∧∀yTimey→Happyxy", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.1x)" }, { "LineSpacingValue", 12.95 } } }),
            new SimpleNode(6, "math", "F=ma", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.1x)" }, { "LineSpacingValue", 12.95 } } }),
            new SimpleNode(7, "math", "lim(1+(1/n)^n)=e", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.1x)" }, { "LineSpacingValue", 12.95 } } }),
        };

        public List<AbstractNode> Lists = new List<AbstractNode>();

        public TestCases()
        {
            // numbered list to demo nested lists
            CompositeNode listNode = new CompositeNode(0, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // Numbered list with nested items
            CompositeNode numberedList = new CompositeNode(1, "numbered_list", "First item in the list", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // Nested item 1 for numbered list
            CompositeNode nestedItem1 = new CompositeNode(2, "lowercase_lettered_list", "Nested item 1", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // Nested item 1 for numbered list
            CompositeNode nestedItem2 = new CompositeNode(3, "lowercase_lettered_list", "Nested item 2", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // Nested item 1.1 for nested item 1
            CompositeNode nestedItem1_1 = new CompositeNode(4, "bulleted_list", "Nested item 1.1", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            nestedItem1.AddChild(nestedItem1_1);  // Add nested item 1.1 to nested item 1
            numberedList.AddChild(nestedItem1);   // Add nested item 1 to the numbered list
            numberedList.AddChild(nestedItem2);   // Add nested item 1 to the numbered list


            // Add the numbered list to listNode
            listNode.AddChild(numberedList);

            // Second numbered list item (without nested items)
            CompositeNode numberedList2 = new CompositeNode(5, "numbered_list", "Second item in the list", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // Add the second numbered list to listNode
            listNode.AddChild(numberedList2);

            // Add the entire listNode (parent node) to the global Lists collection
            Lists.Add(listNode);

            // bulleted list, bulleted type 1
            CompositeNode listNode1 = new CompositeNode(1, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode bulletedList = new CompositeNode(7, "bulleted_list", "bulleted type 1", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode bulletedList2 = new CompositeNode(8, "bulleted_list", "bulleted type 1 again", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode1.AddChild(bulletedList);
            listNode1.AddChild(bulletedList2);
            Lists.Add(listNode1);

            // hollow bulleted, bulleted type 2
            CompositeNode listNode2 = new CompositeNode(2, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode hollowBulletedList = new CompositeNode(10, "hollow_bulleted_list", "bulleted type 2", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode2.AddChild(hollowBulletedList);
            Lists.Add(listNode2);

            // square bulleted, bulleted type 3
            CompositeNode listNode3 = new CompositeNode(3, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode squareBulletedList = new CompositeNode(12, "square_bulleted_list", "bulleted type 3", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode3.AddChild(squareBulletedList);
            Lists.Add(listNode3);

            // diamond bulleted, bulleted type 4
            CompositeNode listNode4 = new CompositeNode(4, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode diamondBulletedList = new CompositeNode(14, "diamond_bulleted_list", "bulleted type 4", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode4.AddChild(diamondBulletedList);
            Lists.Add(listNode4);

            // arrow bulleted, bulleted type 5
            CompositeNode listNode5 = new CompositeNode(5, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode arrowBulletedList = new CompositeNode(16, "arrow_bulleted_list", "bulleted type 5", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode5.AddChild(arrowBulletedList);
            Lists.Add(listNode5);

            // checkmark bulleted, bulleted type 6
            CompositeNode listNode6 = new CompositeNode(6, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode checkMarkBulletedList = new CompositeNode(16, "checkmark_bulleted_list", "bulleted type 6", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode6.AddChild(checkMarkBulletedList);
            Lists.Add(listNode6);


            // dash bulleted, bulleted type 7
            CompositeNode listNode7 = new CompositeNode(7, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode dashBulletedList = new CompositeNode(16, "dash_bulleted_list", "bulleted type 7", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode7.AddChild(dashBulletedList);
            Lists.Add(listNode7);

            // numbered list, numbered list
            CompositeNode listNode8 = new CompositeNode(8, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode numberedListSolo = new CompositeNode(16, "numbered_list", "numbered list", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode8.AddChild(numberedListSolo);
            Lists.Add(listNode8);

            // numbered parenthesis list, numbered list with bracket
            CompositeNode listNode9 = new CompositeNode(9, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode numberedParenthesisList = new CompositeNode(16, "numbered_parenthesis_list", "numbered list with bracket", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode9.AddChild(numberedParenthesisList);
            Lists.Add(listNode9);

            // roman numeral list, roman numeral list
            CompositeNode listNode10 = new CompositeNode(10, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode romanNumeralList = new CompositeNode(16, "roman_numeral_list", "roman numeral list", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode10.AddChild(romanNumeralList);
            Lists.Add(listNode10);

            // lowercase roman numeral list, lowercase roman numeral list
            CompositeNode listNode11 = new CompositeNode(11, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode lowercaseRomanNumeralList = new CompositeNode(16, "lowercase_roman_numeral_list", "lowercase roman numeral list", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode11.AddChild(lowercaseRomanNumeralList);
            Lists.Add(listNode11);

            // Demo nested lists
            CompositeNode listNode12 = new CompositeNode(12, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // uppercase lettered list
            CompositeNode uppercaseLetteredList1 = new CompositeNode(1, "uppercase_lettered_list", "uppercase lettered list", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // uppercase lettered list 2
            CompositeNode uppercaseLetteredList2 = new CompositeNode(1, "uppercase_lettered_list", "uppercase lettered list 2", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // uppercase lettered list 2.1 nested
            CompositeNode uppercaseNested1 = new CompositeNode(2, "lowercase_lettered_list", "uppercase lettered list 2.1 nested", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            // uppercase lettered list 2.2 nested
            CompositeNode uppercaseNested2 = new CompositeNode(2, "lowercase_lettered_list", "uppercase lettered list 2.2 nested", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

             // uppercase lettered list 2.2.1 nested
            CompositeNode uppercaseNested2_2 = new CompositeNode(2, "lowercase_roman_numeral_list", "uppercase lettered list 2.2.1 nested", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });


            uppercaseNested2.AddChild(uppercaseNested2_2);  // Add "uppercase lettered list 2.2.1 nested" to "uppercase lettered list 2.2 nested"
            uppercaseLetteredList2.AddChild(uppercaseNested1);  
            uppercaseLetteredList2.AddChild(uppercaseNested2);   

            // Add the numbered list to listNode
            listNode12.AddChild(uppercaseLetteredList1);
            listNode12.AddChild(uppercaseLetteredList2);

            // Add the entire listNode (parent node) to the global Lists collection
            Lists.Add(listNode12);

            // lowercase lettered list
            CompositeNode listNode13 = new CompositeNode(13, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode lowercaseLetteredList = new CompositeNode(16, "lowercase_lettered_list", "lowercase lettered list", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode13.AddChild(lowercaseLetteredList);
            Lists.Add(listNode13);

            // lowercase lettered list with bracket
            CompositeNode listNode14 = new CompositeNode(14, "list", "", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            CompositeNode lowercaseLetteredParenthesisList = new CompositeNode(16, "lowercase_lettered_parenthesis_list", "lowercase lettered with bracket list ", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "bold", false },
                    { "italic", false },
                    { "fontsize", 12 },
                    { "fonttype", "Default Font" },
                    { "fontcolor", "000000" },
                    { "highlight", "none" }
                }
            });

            listNode14.AddChild(lowercaseLetteredParenthesisList);
            Lists.Add(listNode14);
        }

        public List<AbstractNode> Images = new List<AbstractNode>
        {
            new SimpleNode(1, "image", "Image_rId9.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Left Align (Ctrl + L)" }, { "Format", "PNG" }, { "Position", "Inline (position determined by text flow)" }, { "WidthEMU", 25005 }, { "HeightEMU", 25005 } } }),
            new SimpleNode(2, "image", "Image_rId10.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Center Align (Ctrl + E)" }, { "Format", "PNG" }, { "Position", "Inline (position determined by text flow)" }, { "WidthEMU", 27860 }, { "HeightEMU", 15671 } } }),
            new SimpleNode(3, "image", "Image_rId11.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Left Align (Ctrl + L)" }, { "Format", "PNG" }, { "Position", "Inline (position determined by text flow)" }, { "WidthEMU", 32051 }, { "HeightEMU", 21367 } } }),
            new SimpleNode(4, "image", "Image_rId12.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Left Align (Ctrl + L)" }, { "Format", "PNG" }, { "Position", "Inline (position determined by text flow)" }, { "WidthEMU", 42597 }, { "HeightEMU", 31950 } } }),
            new SimpleNode(5, "image", "Image_rId13.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Right Align (Ctrl + R)" }, { "Format", "JPG" }, { "Position", "Horizontal Offset: 2264, Vertical Offset: 1046" }, { "WidthEMU", 34695 }, { "HeightEMU", 23128 } } }),
            new SimpleNode(6, "image", "Image_rId14.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Justify (Ctrl + J)" }, { "Format", "JPG" }, { "Position", "Horizontal Offset: 0, Vertical Offset: 0" }, { "WidthEMU", 38404 }, { "HeightEMU", 25603 } } })
        };

        public List<AbstractNode> SpecialContent = new List<AbstractNode>
        {
            // Header node
            new SimpleNode(101, "header", "This is a header Text",
                new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "Bold", true },
                        { "Italic", false },
                        { "Alignment", "left" },
                        { "FontSize", 12 },
                        { "FontType", "Default Font" },
                        { "FontColor", "000000" },
                        { "Highlight", "none" }
                    }
                }
            ),
            // Footer node
            new SimpleNode(102, "footer", "This is a footer text",
                new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "Bold", false },
                        { "Italic", false },
                        { "Alignment", "left" },
                        { "FontSize", 12 },
                        { "FontType", "Default Font" },
                        { "FontColor", "000000" },
                        { "Highlight", "none" }
                    }
                }
            ),
            // Footnote node
            new SimpleNode(103, "footnote", "This is the footnote text.",
                new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "Bold", false },
                        { "Italic", false },
                        { "Alignment", "left" },
                        { "FontSize", 10 },
                        { "FontType", "Default Font" },
                        { "FontColor", "000000" },
                        { "Highlight", "none" }
                    }
                }
            ),
            // Textbox node
            new SimpleNode(104, "textbox", "This is a simple textbox. You can adjust the width and content as needed. This textbox can contain multiple lines of text and will automatically wrap.",
                new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "Bold", false },
                        { "Italic", false },
                        { "Alignment", "left" },
                        { "FontSize", 12 },
                        { "FontType", "Default Font" },
                        { "FontColor", "000000" },
                        { "Highlight", "none" },
                        { "Border", "solid" },
                        { "BorderColor", "000000" }
                    }
                }
            ),
            // Pagebreak node
            new SimpleNode(105, "pagebreak", "New page",
                new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "Bold", false },
                        { "Italic", false },
                        { "Alignment", "center" },
                        { "FontSize", 12 },
                        { "FontType", "Default Font" },
                        { "FontColor", "000000" },
                        { "Highlight", "none" }
                    }
                }
            ),
            // Endnote node
            new SimpleNode(106, "endnote", "This is the endnote text.",
                new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "Bold", false },
                        { "Italic", false },
                        { "Alignment", "left" },
                        { "FontSize", 10 },
                        { "FontType", "Default Font" },
                        { "FontColor", "000000" },
                        { "Highlight", "none" }
                    }
                }
            )};
    }
}