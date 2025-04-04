using System.Collections.Generic;
using Utilities;

public class AllNodesList
{
	// Node Type: root
	public List<AbstractNode> root = new List<AbstractNode>
	{
		//  new CompositeNode(1, 0, "root", "", "[]"),
	};

	// Node Type: layouts
	public List<AbstractNode> layouts = new List<AbstractNode>
	{
		//  new CompositeNode(2, -1, "layout", "", "[{"orientation":"Portrait","pageWidth":21.0,"pageHeight":29.7,"columnNum":1,"columnSpacing":1.25,"margins":{"top":2.54,"bottom":2.54,"left":2.54,"right":2.54,"header":1.25,"footer":1.25}}]"),
		//  new CompositeNode(58, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(90, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(102, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(113, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(132, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(162, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(163, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(173, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
	};

	// Node Type: headers
	public List<AbstractNode> headers = new List<AbstractNode>
	{
		//  new CompositeNode(3, 1, "h1", "Header 1", "[{"bold":true,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(4, 2, "h2", "Header 2", "[{"bold":true,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(5, 3, "h3", "Header 3", "[{"bold":true,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"434343","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
	};

	// Node Type: paragraphs
	public List<AbstractNode> paragraphs = new List<AbstractNode>
	{
		//  new CompositeNode(6, 7, "paragraph", "test text", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(7, -1, "paragraph_run?", "https://puginarug.com/", "[]"),
		//  new CompositeNode(8, 7, "paragraph", "Colored text", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"FF0000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(9, 7, "paragraph", "diff color and highlighted", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"0000FF","highlight":"cyan","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(12, 7, "paragraph", "this is a bolded text", "[]"),
		//  new CompositeNode(16, 7, "paragraph", "this is a italic text", "[]"),
		//  new CompositeNode(36, 7, "paragraph", "this is an indented text", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(37, 7, "paragraph", "middle align", "[{"bold":false,"italic":false,"alignment":"center","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(38, 7, "paragraph", "end align", "[{"bold":false,"italic":false,"alignment":"right","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(41, 7, "paragraph", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic", "[]"),
		//  new CompositeNode(42, 7, "paragraph", "^ align to margin", "[{"bold":false,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Default Font","fontcolor":"000000","highlight":"white","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(43, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(44, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(45, 7, "paragraph", "whit text here", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"FFE599","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(46, 7, "paragraph", "!@#$%^&*(){}[];’:,./ 🤪", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(50, 7, "paragraph", "font calibri here", "[]"),
		//  new CompositeNode(51, 7, "paragraph", "font times new roman here", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(52, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(55, 7, "paragraph", "diff spacing heres", "[]"),
		//  new CompositeNode(56, 7, "paragraph", "line spacing", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(57, 7, "paragraph", "space after paragraph", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(62, 7, "paragraph", "Many farmers focus on raising pigs (Cole, 1998).", "[]"),
		//  new CompositeNode(68, 7, "paragraph", "Germany played a crucial role in the study (Wdadwa, 1990).", "[]"),
		//  new CompositeNode(72, 7, "paragraph", "Some argue that love crosses all boundaries (Chan, n.d.).", "[]"),
		//  new CompositeNode(73, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(74, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(75, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(76, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(77, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(80, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(81, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(82, 7, "paragraph", "😉👩☺👐💅💪😋😉😔😖😡😜😏😹💩💩💩", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(83, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(84, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(86, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(88, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"center","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(89, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(92, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(93, 7, "paragraph", "Math Below", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(101, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(105, 7, "paragraph", "PNG with Align left alignment & In Line with Text position (no move with text or fix position on page)", "[]"),
		//  new CompositeNode(111, 7, "paragraph", "Figure 1: PNG", "[]"),
		//  new CompositeNode(112, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":9,"fonttype":"Caption","fontcolor":"1F497D","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(114, 7, "paragraph", "JPG with Align right alignment then enable Through text wrapping", "[{"bold":false,"italic":false,"alignment":"left","fontsize":16,"fonttype":"Aptos Display","fontcolor":"0F4761","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(119, 7, "paragraph", "Figure 2: JPG", "[]"),
		//  new CompositeNode(120, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(121, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(122, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(123, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(124, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(125, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(126, 7, "paragraph", "JPG with Align right alignment then enable Top & Bottom text wrapping", "[{"bold":false,"italic":false,"alignment":"left","fontsize":16,"fonttype":"Aptos Display","fontcolor":"0F4761","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(131, 7, "paragraph", "Figure 3: JPG", "[]"),
		//  new CompositeNode(136, 7, "paragraph", "JPEG with Justify alignment then enable Behind Text text wrapping", "[]"),
		//  new CompositeNode(141, 7, "paragraph", "Figure 4: JPEG", "[]"),
		//  new CompositeNode(142, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(143, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(144, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(145, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(146, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(147, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(148, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(149, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(153, 7, "paragraph", "JPEG with Justify alignment then enable In Front of Text text wrapping (caption hidden underneath image -> can move image to see)", "[]"),
		//  new CompositeNode(158, 7, "paragraph", "Figure 5: JPEG", "[]"),
		//  new CompositeNode(159, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(160, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(161, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(164, 7, "paragraph", "- - This is a Page Break - -", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Aptos","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(165, 7, "paragraph", "-- Next Page Section Break --", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Aptos","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(166, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":20,"fonttype":"Heading1","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(171, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(172, -1, "paragraph_run?", "This is citation/references", "[]"),
		//  new CompositeNode(174, 7, "paragraph", "This is an Endnote", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Aptos","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(175, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
	};

	// Node Type: text_run
	public List<AbstractNode> text_run = new List<AbstractNode>
	{
		//  new CompositeNode(10, -1, "text_run", "this", "[{}]"),
		//  new CompositeNode(11, -1, "text_run", " is a bolded text", "[{}]"),
		//  new CompositeNode(13, -1, "text_run", "this is ", "[{}]"),
		//  new CompositeNode(14, -1, "text_run", "a", "[{}]"),
		//  new CompositeNode(15, -1, "text_run", " italic text", "[{}]"),
		//  new CompositeNode(39, -1, "text_run", "Lorem Ipsum", "[{}]"),
		//  new CompositeNode(40, -1, "text_run", " is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic", "[{}]"),
		//  new CompositeNode(47, -1, "text_run", "font ", "[{}]"),
		//  new CompositeNode(48, -1, "text_run", "calibri", "[{}]"),
		//  new CompositeNode(49, -1, "text_run", " here", "[{}]"),
		//  new CompositeNode(53, -1, "text_run", "diff spacing ", "[{}]"),
		//  new CompositeNode(54, -1, "text_run", "heres", "[{}]"),
		//  new CompositeNode(59, -1, "text_run", "Many farmers focus on raising pigs ", "[{}]"),
		//  new CompositeNode(61, -1, "text_run", ".", "[{}]"),
		//  new CompositeNode(63, -1, "text_run", "Germany played a crucial role in the study", "[{}]"),
		//  new CompositeNode(67, -1, "text_run", ".", "[{}]"),
		//  new CompositeNode(69, -1, "text_run", "Some argue that love crosses all boundaries", "[{}]"),
		//  new CompositeNode(71, -1, "text_run", ".", "[{}]"),
		//  new CompositeNode(103, -1, "text_run", "PNG with Align left alignment & In Line with Text position", "[{}]"),
		//  new CompositeNode(104, -1, "text_run", "(no move with text or fix position on page)", "[{}]"),
		//  new CompositeNode(107, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(108, -1, "text_run", "1", "[{}]"),
		//  new CompositeNode(109, -1, "text_run", ": ", "[{}]"),
		//  new CompositeNode(110, -1, "text_run", "PNG", "[{}]"),
		//  new CompositeNode(116, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(117, -1, "text_run", "2", "[{}]"),
		//  new CompositeNode(118, -1, "text_run", ": JPG", "[{}]"),
		//  new CompositeNode(128, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(129, -1, "text_run", "3", "[{}]"),
		//  new CompositeNode(130, -1, "text_run", ": JPG", "[{}]"),
		//  new CompositeNode(133, -1, "text_run", "JPEG with Justify alignment then enable Behind Text ", "[{}]"),
		//  new CompositeNode(134, -1, "text_run", "text", "[{}]"),
		//  new CompositeNode(135, -1, "text_run", " wrapping", "[{}]"),
		//  new CompositeNode(138, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(139, -1, "text_run", "4", "[{}]"),
		//  new CompositeNode(140, -1, "text_run", ": JPEG", "[{}]"),
		//  new CompositeNode(150, -1, "text_run", "JPEG with Justify alignment then enable In Front of Text ", "[{}]"),
		//  new CompositeNode(151, -1, "text_run", "text", "[{}]"),
		//  new CompositeNode(152, -1, "text_run", " wrapping (caption hidden underneath image -> can move image to see)", "[{}]"),
		//  new CompositeNode(155, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(156, -1, "text_run", "5", "[{}]"),
		//  new CompositeNode(157, -1, "text_run", ": JPEG", "[{}]"),
	};

	// Node Type: lists
	public List<AbstractNode> lists = new List<AbstractNode>
	{
		//  new CompositeNode(17, -1, "bulleted_list", "bulleted type 1", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(18, -1, "bulleted_list", "bulleted type 1 again", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(19, -1, "hollow_bulleted_list", "bulleted type 2", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(20, -1, "square_bulleted_list", "bulleted type 3", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(21, -1, "diamond_bulleted_list", "bulleted type 4", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(22, -1, "arrow_bulleted_list", "bulleted type 5", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(23, -1, "checkmark_bulleted_list", "bulleted type 6", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(24, -1, "dash_bulleted_list", "bulleted type 7", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"ListParagraph","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(25, -1, "numbered_list", "numbered list", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(26, -1, "numbered_parenthesis_list", "numbered list with bracket", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(27, -1, "roman_numeral_list", "roman numeral list", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(28, -1, "lowercase_roman_numeral_list", "lowercase roman numeral list", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"ListParagraph","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(29, -1, "uppercase_lettered_list", "uppercase lettered list", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(30, -1, "uppercase_lettered_list", "uppercase lettered list 2", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(31, -1, "uppercase_lettered_list", "uppercase lettered list 2.1 nested", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(32, -1, "uppercase_lettered_list", "uppercase lettered list 2.2 nested", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(33, -1, "uppercase_lettered_list", "uppercase lettered list 2.2.1 nested", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(34, -1, "lowercase_lettered_list", "lowercase lettered list", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"ListParagraph","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(35, -1, "lowercase_lettered_parenthesis_list", "lowercase lettered with bracket list ", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"ListParagraph","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
	};

	// Node Type: citationAndbibliographys
	public List<AbstractNode> citationAndbibliographys = new List<AbstractNode>
	{
		//  new CompositeNode(60, -1, "intext-citation", "(Cole, 1998)", "[{}]"),
		//  new CompositeNode(64, -1, "intext-citation", " (", "[{}]"),
		//  new CompositeNode(65, -1, "intext-citation", "Wdadwa", "[{}]"),
		//  new CompositeNode(66, -1, "intext-citation", ", 1990)", "[{}]"),
		//  new CompositeNode(70, -1, "intext-citation", "(Chan, n.d.)", "[{}]"),
		//  new CompositeNode(167, 1, "bibliography", "Reference (IEE)", "[{"bold":true,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(168, -1, "citation_run", "[1] J. Cole, “I love pigs,” *Dr. J.*, vol. 20, 1998.", "[{"bold":false,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(169, -1, "citation_run", "[2] Wdadwa, “I love Germany,” *Sit.*, 1990.", "[{"bold":false,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(170, -1, "citation_run", "[3] D. Chan, “I love black people,” *White Man*, vol. 20, n.d.", "[{"bold":false,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
	};

	// Node Type: math
	public List<AbstractNode> math = new List<AbstractNode>
	{
		//  new CompositeNode(78, -1, "math", "(1/2) × √(4)  <- math", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(79, -1, "math", "∫", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(94, -1, "math", "(1/2) × √(4)=1", "[{"bold":false,"italic":false,"alignment":"center","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(95, -1, "math", "(1/2)cos 2x+ (3/8)sin 4x   =3x", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(96, -1, "math", "log_24y ≥ (π/2)", "[{"bold":false,"italic":true,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(97, -1, "math", "∴ ∞ ≠ ±α", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(98, -1, "math", "∃xPersonx∧∀yTimey→Happyx,y", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(99, -1, "math", "F=ma", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(100, -1, "math", "lim1+(1/n)^n= e", "[{"bold":false,"italic":true,"alignment":"center","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
	};

	// Node Type: image
	public List<AbstractNode> image = new List<AbstractNode>
	{
		//  new CompositeNode(85, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId9.png", []),
		//  new CompositeNode(87, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId10.png", []),
		//  new CompositeNode(91, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId11.png", []),
		//  new CompositeNode(106, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId12.png", []),
		//  new CompositeNode(115, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId13.png", []),
		//  new CompositeNode(127, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId13.png", []),
		//  new CompositeNode(137, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId14.png", []),
		//  new CompositeNode(154, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId14.png", []),
	};
}
