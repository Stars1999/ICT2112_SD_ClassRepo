using System.Collections.Generic;
using Utilities;

public class AllNodesList
{
    // Node Type: root
    public List<AbstractNode> root = new List<AbstractNode>
    {
        new CompositeNode(1, 0, "root", "", new List<Dictionary<string, object>> { }),
    };
    // Node Type: layouts
    public List<AbstractNode> layouts = new List<AbstractNode>
    {
        new CompositeNode(2, -1, "layout", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(57, -1, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { }),
        new CompositeNode(85, -1, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { }),
        new CompositeNode(97, -1, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { }),
        new CompositeNode(108, -1, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { }),
        new CompositeNode(127, -1, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { }),
        new CompositeNode(151, -1, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { }),
        new CompositeNode(152, -1, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { }),
        new CompositeNode(162, -1, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { }),
    };
    // Node Type: headers
    public List<AbstractNode> headers = new List<AbstractNode>
    {
        new CompositeNode(3, 1, "h1", "Header 1", new List<Dictionary<string, object>> { }),
        new CompositeNode(4, 2, "h2", "Header 2", new List<Dictionary<string, object>> { }),
        new CompositeNode(5, 3, "h3", "Header 3", new List<Dictionary<string, object>> { }),
    };
    // Node Type: paragraphs
    public List<AbstractNode> paragraphs = new List<AbstractNode>
    {
        new CompositeNode(6, 7, "paragraph", "test text", new List<Dictionary<string, object>> { }),
        new CompositeNode(7, -1, "paragraph_run?", "https://puginarug.com/", new List<Dictionary<string, object>> { }),
        new CompositeNode(8, 7, "paragraph", "Colored text", new List<Dictionary<string, object>> { }),
        new CompositeNode(9, 7, "paragraph", "diff color and highlighted", new List<Dictionary<string, object>> { }),
        new CompositeNode(10, 7, "paragraph", "this is a bolded text", new List<Dictionary<string, object>> { }),
        new CompositeNode(11, 7, "paragraph", "this is a italic text", new List<Dictionary<string, object>> { }),
        new CompositeNode(31, 7, "paragraph", "this is an indented text", new List<Dictionary<string, object>> { }),
        new CompositeNode(32, 7, "paragraph", "middle align", new List<Dictionary<string, object>> { }),
        new CompositeNode(33, 7, "paragraph", "end align", new List<Dictionary<string, object>> { }),
        new CompositeNode(36, 7, "paragraph", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic", new List<Dictionary<string, object>> { }),
        new CompositeNode(37, 7, "paragraph", "^ align to margin", new List<Dictionary<string, object>> { }),
        new CompositeNode(38, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(48, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(49, 7, "paragraph", "whit text here", new List<Dictionary<string, object>> { }),
        new CompositeNode(50, 7, "paragraph", "!@#$%^&*(){}[];’:,./ 🤪", new List<Dictionary<string, object>> { }),
        new CompositeNode(51, 7, "paragraph", "font calibri here", new List<Dictionary<string, object>> { }),
        new CompositeNode(52, 7, "paragraph", "font times new roman here", new List<Dictionary<string, object>> { }),
        new CompositeNode(53, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(54, 7, "paragraph", "diff spacing heres", new List<Dictionary<string, object>> { }),
        new CompositeNode(55, 7, "paragraph", "line spacing", new List<Dictionary<string, object>> { }),
        new CompositeNode(56, 7, "paragraph", "space after paragraph", new List<Dictionary<string, object>> { }),
        new CompositeNode(61, 7, "paragraph", "Many farmers focus on raising pigs (Cole, 1998).", new List<Dictionary<string, object>> { }),
        new CompositeNode(65, 7, "paragraph", "Germany played a crucial role in the study (Wdadwa, 1990).", new List<Dictionary<string, object>> { }),
        new CompositeNode(69, 7, "paragraph", "Some argue that love crosses all boundaries (Chan, n.d.).", new List<Dictionary<string, object>> { }),
        new CompositeNode(70, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(71, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(72, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(75, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(76, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(77, 7, "paragraph", "😉👩☺👐💅💪😋😉😔😖😡😜😏😹💩💩💩", new List<Dictionary<string, object>> { }),
        new CompositeNode(78, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(79, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(81, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(83, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(84, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(87, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(88, 7, "paragraph", "Math Below", new List<Dictionary<string, object>> { }),
        new CompositeNode(96, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(100, 7, "paragraph", "PNG with Align left alignment & In Line with Text position (no move with text or fix position on page)", new List<Dictionary<string, object>> { }),
        new CompositeNode(106, 7, "paragraph", "Figure 1: PNG", new List<Dictionary<string, object>> { }),
        new CompositeNode(107, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(109, 7, "paragraph", "JPG with Align right alignment then enable Through text wrapping", new List<Dictionary<string, object>> { }),
        new CompositeNode(114, 7, "paragraph", "Figure 3: JPG", new List<Dictionary<string, object>> { }),
        new CompositeNode(115, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(116, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(117, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(118, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(119, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(120, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(121, 7, "paragraph", "JPG with Align right alignment then enable Top & Bottom text wrapping", new List<Dictionary<string, object>> { }),
        new CompositeNode(126, 7, "paragraph", "Figure 3: JPG", new List<Dictionary<string, object>> { }),
        new CompositeNode(128, 7, "paragraph", "JPEG with Justify alignment then enable Behind Text text wrapping", new List<Dictionary<string, object>> { }),
        new CompositeNode(133, 7, "paragraph", "Figure 4: JPEG", new List<Dictionary<string, object>> { }),
        new CompositeNode(134, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(135, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(136, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(137, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(138, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(139, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(140, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(141, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(142, 7, "paragraph", "JPEG with Justify alignment then enable In Front of Text text wrapping (caption hidden underneath image -> can move image to see)", new List<Dictionary<string, object>> { }),
        new CompositeNode(147, 7, "paragraph", "Figure 4: JPEG", new List<Dictionary<string, object>> { }),
        new CompositeNode(148, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(149, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(150, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(153, 7, "paragraph", "- - This is a Page Break - -", new List<Dictionary<string, object>> { }),
        new CompositeNode(154, 7, "paragraph", "-- Next Page Section Break --", new List<Dictionary<string, object>> { }),
        new CompositeNode(155, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(160, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(161, -1, "paragraph_run?", "This is citation/references", new List<Dictionary<string, object>> { }),
        new CompositeNode(163, 7, "paragraph", "This is an Endnote", new List<Dictionary<string, object>> { }),
        new CompositeNode(164, -1, "empty_paragraph1", "", new List<Dictionary<string, object>> { }),
    };
    // Node Type: lists
    public List<AbstractNode> lists = new List<AbstractNode>
    {
        new CompositeNode(12, -1, "bulleted_list", "bulleted type 1", new List<Dictionary<string, object>> { }),
        new CompositeNode(13, -1, "bulleted_list", "bulleted type 1 again", new List<Dictionary<string, object>> { }),
        new CompositeNode(14, -1, "hollow_bulleted_list", "bulleted type 2", new List<Dictionary<string, object>> { }),
        new CompositeNode(15, -1, "square_bulleted_list", "bulleted type 3", new List<Dictionary<string, object>> { }),
        new CompositeNode(16, -1, "diamond_bulleted_list", "bulleted type 4", new List<Dictionary<string, object>> { }),
        new CompositeNode(17, -1, "arrow_bulleted_list", "bulleted type 5", new List<Dictionary<string, object>> { }),
        new CompositeNode(18, -1, "checkmark_bulleted_list", "bulleted type 6", new List<Dictionary<string, object>> { }),
        new CompositeNode(19, -1, "dash_bulleted_list", "bulleted type 7", new List<Dictionary<string, object>> { }),
        new CompositeNode(20, -1, "numbered_list", "numbered list", new List<Dictionary<string, object>> { }),
        new CompositeNode(21, -1, "numbered_parenthesis_list", "numbered list with bracket", new List<Dictionary<string, object>> { }),
        new CompositeNode(22, -1, "roman_numeral_list", "roman numeral list", new List<Dictionary<string, object>> { }),
        new CompositeNode(23, -1, "lowercase_roman_numeral_list", "lowercase roman numeral list", new List<Dictionary<string, object>> { }),
        new CompositeNode(24, -1, "uppercase_lettered_list", "uppercase lettered list", new List<Dictionary<string, object>> { }),
        new CompositeNode(25, -1, "uppercase_lettered_list", "uppercase lettered list 2", new List<Dictionary<string, object>> { }),
        new CompositeNode(26, -1, "uppercase_lettered_list", "uppercase lettered list 2.1 nested", new List<Dictionary<string, object>> { }),
        new CompositeNode(27, -1, "uppercase_lettered_list", "uppercase lettered list 2.2 nested", new List<Dictionary<string, object>> { }),
        new CompositeNode(28, -1, "uppercase_lettered_list", "uppercase lettered list 2.2.1 nested", new List<Dictionary<string, object>> { }),
        new CompositeNode(29, -1, "lowercase_lettered_list", "lowercase lettered list", new List<Dictionary<string, object>> { }),
        new CompositeNode(30, -1, "lowercase_lettered_parenthesis_list", "lowercase lettered with bracket list ", new List<Dictionary<string, object>> { }),
    };
    // Node Type: text_run
    public List<AbstractNode> text_run = new List<AbstractNode>
    {
        new CompositeNode(34, -1, "text_run", "Lorem Ipsum", new List<Dictionary<string, object>> { }),
        new CompositeNode(35, -1, "text_run", " is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic", new List<Dictionary<string, object>> { }),
        new CompositeNode(58, -1, "text_run", "Many farmers focus on raising pigs ", new List<Dictionary<string, object>> { }),
        new CompositeNode(60, -1, "text_run", ".", new List<Dictionary<string, object>> { }),
        new CompositeNode(62, -1, "text_run", "Germany played a crucial role in the study", new List<Dictionary<string, object>> { }),
        new CompositeNode(64, -1, "text_run", ".", new List<Dictionary<string, object>> { }),
        new CompositeNode(66, -1, "text_run", "Some argue that love crosses all boundaries", new List<Dictionary<string, object>> { }),
        new CompositeNode(68, -1, "text_run", ".", new List<Dictionary<string, object>> { }),
        new CompositeNode(98, -1, "text_run", "PNG with Align left alignment & In Line with Text position", new List<Dictionary<string, object>> { }),
        new CompositeNode(99, -1, "text_run", "(no move with text or fix position on page)", new List<Dictionary<string, object>> { }),
        new CompositeNode(102, -1, "text_run", "Figure ", new List<Dictionary<string, object>> { }),
        new CompositeNode(103, -1, "text_run", "1", new List<Dictionary<string, object>> { }),
        new CompositeNode(104, -1, "text_run", ": ", new List<Dictionary<string, object>> { }),
        new CompositeNode(105, -1, "text_run", "PNG", new List<Dictionary<string, object>> { }),
        new CompositeNode(111, -1, "text_run", "Figure ", new List<Dictionary<string, object>> { }),
        new CompositeNode(112, -1, "text_run", "3", new List<Dictionary<string, object>> { }),
        new CompositeNode(113, -1, "text_run", ": JPG", new List<Dictionary<string, object>> { }),
        new CompositeNode(123, -1, "text_run", "Figure ", new List<Dictionary<string, object>> { }),
        new CompositeNode(124, -1, "text_run", "3", new List<Dictionary<string, object>> { }),
        new CompositeNode(125, -1, "text_run", ": JPG", new List<Dictionary<string, object>> { }),
        new CompositeNode(130, -1, "text_run", "Figure ", new List<Dictionary<string, object>> { }),
        new CompositeNode(131, -1, "text_run", "4", new List<Dictionary<string, object>> { }),
        new CompositeNode(132, -1, "text_run", ": JPEG", new List<Dictionary<string, object>> { }),
        new CompositeNode(144, -1, "text_run", "Figure ", new List<Dictionary<string, object>> { }),
        new CompositeNode(145, -1, "text_run", "4", new List<Dictionary<string, object>> { }),
        new CompositeNode(146, -1, "text_run", ": JPEG", new List<Dictionary<string, object>> { }),
    };
    // Node Type: tables
    public List<AbstractNode> tables = new List<AbstractNode>
    {
        new CompositeNode(47, 7, "table", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(42, 8, "row", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(39, -1, "cell", "Hi", new List<Dictionary<string, object>> { }),
        new CompositeNode(40, -1, "cell", "i", new List<Dictionary<string, object>> { }),
        new CompositeNode(41, -1, "cell", "Am", new List<Dictionary<string, object>> { }),
        new CompositeNode(46, 8, "row", "", new List<Dictionary<string, object>> { }),
        new CompositeNode(43, -1, "cell", "Going", new List<Dictionary<string, object>> { }),
        new CompositeNode(44, -1, "cell", "To", new List<Dictionary<string, object>> { }),
        new CompositeNode(45, -1, "cell", "Remod", new List<Dictionary<string, object>> { }),
    };
    // Node Type: citationAndbibliographys
    public List<AbstractNode> citationAndbibliographys = new List<AbstractNode>
    {
        new CompositeNode(59, -1, "intext-citation", "(Cole, 1998)", new List<Dictionary<string, object>> { }),
        new CompositeNode(63, -1, "intext-citation", " (Wdadwa, 1990)", new List<Dictionary<string, object>> { }),
        new CompositeNode(67, -1, "intext-citation", "(Chan, n.d.)", new List<Dictionary<string, object>> { }),
        new CompositeNode(156, 1, "bibliography", "Reference (IEE)", new List<Dictionary<string, object>> { }),
        new CompositeNode(157, -1, "citation_run", "[1] J. Cole, “I love pigs,” *Dr. J.*, vol. 20, 1998.", new List<Dictionary<string, object>> { }),
        new CompositeNode(158, -1, "citation_run", "[2] Wdadwa, “I love Germany,” *Sit.*, 1990.", new List<Dictionary<string, object>> { }),
        new CompositeNode(159, -1, "citation_run", "[3] D. Chan, “I love black people,” *White Man*, vol. 20, n.d.", new List<Dictionary<string, object>> { }),
    };
    // Node Type: math
    public List<AbstractNode> math = new List<AbstractNode>
    {
        new CompositeNode(73, -1, "math", "(1/2) × √(4)  <- math", new List<Dictionary<string, object>> { }),
        new CompositeNode(74, -1, "math", "∫", new List<Dictionary<string, object>> { }),
        new CompositeNode(89, -1, "math", "(1/2) × √(4)=1", new List<Dictionary<string, object>> { }),
        new CompositeNode(90, -1, "math", "(1/2)cos 2x+ (3/8)sin 4x   =3x", new List<Dictionary<string, object>> { }),
        new CompositeNode(91, -1, "math", "log_24y ≥ (π/2)", new List<Dictionary<string, object>> { }),
        new CompositeNode(92, -1, "math", "∴ ∞ ≠ ±α", new List<Dictionary<string, object>> { }),
        new CompositeNode(93, -1, "math", "∃xPersonx∧∀yTimey→Happyx,y", new List<Dictionary<string, object>> { }),
        new CompositeNode(94, -1, "math", "F=ma", new List<Dictionary<string, object>> { }),
        new CompositeNode(95, -1, "math", "lim1+(1/n)^n= e", new List<Dictionary<string, object>> { }),
    };
    // Node Type: image
    public List<AbstractNode> image = new List<AbstractNode>
    {
        new CompositeNode(80, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId9.png", new List<Dictionary<string, object>> { }),
        new CompositeNode(82, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId10.png", new List<Dictionary<string, object>> { }),
        new CompositeNode(86, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId11.png", new List<Dictionary<string, object>> { }),
        new CompositeNode(101, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId12.png", new List<Dictionary<string, object>> { }),
        new CompositeNode(110, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId13.png", new List<Dictionary<string, object>> { }),
        new CompositeNode(122, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId13.png", new List<Dictionary<string, object>> { }),
        new CompositeNode(129, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId14.png", new List<Dictionary<string, object>> { }),
        new CompositeNode(143, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId14.png", new List<Dictionary<string, object>> { }),
    };
}
