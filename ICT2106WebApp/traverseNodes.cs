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
		//  new CompositeNode(75, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(134, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(146, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(157, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(176, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(206, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(207, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(217, -1, "page_break", "[PAGE BREAK]", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
	};

	// Node Type: headers
	public List<AbstractNode> headers = new List<AbstractNode>
	{
		//  new CompositeNode(3, 1, "h1", "Header 1", "[{"bold":true,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(4, 2, "h2", "Header 2", "[{"bold":true,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(5, 3, "h3", "Header 3", "[{"bold":true,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"434343","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
	};

	// Node Type: paragraphs
	public List<AbstractNode> paragraphs = new List<AbstractNode>
	{
		//  new CompositeNode(6, 7, "paragraph", "test text", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(7, -1, "paragraph_run?", "https://puginarug.com/", "[]"),
		//  new CompositeNode(8, 7, "paragraph", "Colored text", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"FF0000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(9, 7, "paragraph", "diff color and highlighted", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"0000FF","highlight":"cyan","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(12, 7, "paragraph", "this is a bolded text", "[]"),
		//  new CompositeNode(16, 7, "paragraph", "this is a italic text", "[]"),
		//  new CompositeNode(38, 7, "paragraph", "this is an indented text", "[]"),
		//  new CompositeNode(41, 7, "paragraph", "middle align", "[]"),
		//  new CompositeNode(44, 7, "paragraph", "end align", "[]"),
		//  new CompositeNode(49, 7, "paragraph", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic", "[]"),
		//  new CompositeNode(50, 7, "paragraph", "^ align to margin", "[{"bold":false,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Default Font","fontcolor":"000000","highlight":"white","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(51, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(61, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(62, 7, "paragraph", "whit text here", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"FFE599","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(63, 7, "paragraph", "!@#$%^&*(){}[];’:,./ 🤪", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(67, 7, "paragraph", "font calibri here", "[]"),
		//  new CompositeNode(68, 7, "paragraph", "font times new roman here", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(69, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(72, 7, "paragraph", "diff spacing heres", "[]"),
		//  new CompositeNode(73, 7, "paragraph", "line spacing", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(74, 7, "paragraph", "space after paragraph", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(79, 7, "paragraph", "Many farmers focus on raising pigs (Cole, 1998).", "[]"),
		//  new CompositeNode(85, 7, "paragraph", "Germany played a crucial role in the study (Wdadwa, 1990).", "[]"),
		//  new CompositeNode(89, 7, "paragraph", "Some argue that love crosses all boundaries (Chan, n.d.).", "[]"),
		//  new CompositeNode(99, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(109, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(119, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(120, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(121, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(124, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(125, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(126, 7, "paragraph", "😉👩☺👐💅💪😋😉😔😖😡😜😏😹💩💩💩", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(127, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(128, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(130, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(132, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"center","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(133, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(136, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(137, 7, "paragraph", "Math Below", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(145, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(149, 7, "paragraph", "PNG with Align left alignment & In Line with Text position (no move with text or fix position on page)", "[]"),
		//  new CompositeNode(155, 7, "paragraph", "Figure 1: PNG", "[]"),
		//  new CompositeNode(156, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":9,"fonttype":"Caption","fontcolor":"1F497D","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(158, 7, "paragraph", "JPG with Align right alignment then enable Through text wrapping", "[{"bold":false,"italic":false,"alignment":"left","fontsize":16,"fonttype":"Aptos Display","fontcolor":"0F4761","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(163, 7, "paragraph", "Figure 2: JPG", "[]"),
		//  new CompositeNode(164, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(165, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(166, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(167, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(168, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(169, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.2x)","lineSpacingValue":13.9}]"),
		//  new CompositeNode(170, 7, "paragraph", "JPG with Align right alignment then enable Top & Bottom text wrapping", "[{"bold":false,"italic":false,"alignment":"left","fontsize":16,"fonttype":"Aptos Display","fontcolor":"0F4761","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(175, 7, "paragraph", "Figure 3: JPG", "[]"),
		//  new CompositeNode(180, 7, "paragraph", "JPEG with Justify alignment then enable Behind Text text wrapping", "[]"),
		//  new CompositeNode(185, 7, "paragraph", "Figure 4: JPEG", "[]"),
		//  new CompositeNode(186, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(187, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(188, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(189, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(190, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(191, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(192, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(193, -1, "empty_paragraph1", "", "[{"bold":false,"italic":true,"alignment":"both","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Single","lineSpacingValue":12.0}]"),
		//  new CompositeNode(197, 7, "paragraph", "JPEG with Justify alignment then enable In Front of Text text wrapping (caption hidden underneath image -> can move image to see)", "[]"),
		//  new CompositeNode(202, 7, "paragraph", "Figure 5: JPEG", "[]"),
		//  new CompositeNode(203, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(204, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(205, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(208, 7, "paragraph", "- - This is a Page Break - -", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Aptos","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(209, 7, "paragraph", "-- Next Page Section Break --", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Aptos","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(210, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":20,"fonttype":"Heading1","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(215, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(216, -1, "paragraph_run?", "This is citation/references", "[]"),
		//  new CompositeNode(218, 7, "paragraph", "This is an Endnote", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Aptos","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(219, -1, "empty_paragraph1", "", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
	};

	// Node Type: text_run
	public List<AbstractNode> text_run = new List<AbstractNode>
	{
		//  new CompositeNode(10, -1, "text_run", "this", "[{}]"),
		//  new CompositeNode(11, -1, "text_run", " is a bolded text", "[{}]"),
		//  new CompositeNode(13, -1, "text_run", "this is ", "[{}]"),
		//  new CompositeNode(14, -1, "text_run", "a", "[{}]"),
		//  new CompositeNode(15, -1, "text_run", " italic text", "[{}]"),
		//  new CompositeNode(36, -1, "text_run", "this", "[{}]"),
		//  new CompositeNode(37, -1, "text_run", " is an indented text", "[{}]"),
		//  new CompositeNode(39, -1, "text_run", "middle ", "[{}]"),
		//  new CompositeNode(40, -1, "text_run", "align", "[{}]"),
		//  new CompositeNode(42, -1, "text_run", "end", "[{}]"),
		//  new CompositeNode(43, -1, "text_run", " align", "[{}]"),
		//  new CompositeNode(45, -1, "text_run", "Lorem Ipsum", "[{}]"),
		//  new CompositeNode(46, -1, "text_run", " is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not ", "[{}]"),
		//  new CompositeNode(47, -1, "text_run", "only", "[{}]"),
		//  new CompositeNode(48, -1, "text_run", " five centuries, but also the leap into electronic", "[{}]"),
		//  new CompositeNode(64, -1, "text_run", "font ", "[{}]"),
		//  new CompositeNode(65, -1, "text_run", "calibri", "[{}]"),
		//  new CompositeNode(66, -1, "text_run", " here", "[{}]"),
		//  new CompositeNode(70, -1, "text_run", "diff spacing ", "[{}]"),
		//  new CompositeNode(71, -1, "text_run", "heres", "[{}]"),
		//  new CompositeNode(76, -1, "text_run", "Many farmers focus on raising pigs ", "[{}]"),
		//  new CompositeNode(78, -1, "text_run", ".", "[{}]"),
		//  new CompositeNode(80, -1, "text_run", "Germany played a crucial role in the study", "[{}]"),
		//  new CompositeNode(84, -1, "text_run", ".", "[{}]"),
		//  new CompositeNode(86, -1, "text_run", "Some argue that love crosses all boundaries", "[{}]"),
		//  new CompositeNode(88, -1, "text_run", ".", "[{}]"),
		//  new CompositeNode(147, -1, "text_run", "PNG with Align left alignment & In Line with Text position", "[{}]"),
		//  new CompositeNode(148, -1, "text_run", "(no move with text or fix position on page)", "[{}]"),
		//  new CompositeNode(151, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(152, -1, "text_run", "1", "[{}]"),
		//  new CompositeNode(153, -1, "text_run", ": ", "[{}]"),
		//  new CompositeNode(154, -1, "text_run", "PNG", "[{}]"),
		//  new CompositeNode(160, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(161, -1, "text_run", "2", "[{}]"),
		//  new CompositeNode(162, -1, "text_run", ": JPG", "[{}]"),
		//  new CompositeNode(172, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(173, -1, "text_run", "3", "[{}]"),
		//  new CompositeNode(174, -1, "text_run", ": JPG", "[{}]"),
		//  new CompositeNode(177, -1, "text_run", "JPEG with Justify alignment then enable Behind Text ", "[{}]"),
		//  new CompositeNode(178, -1, "text_run", "text", "[{}]"),
		//  new CompositeNode(179, -1, "text_run", " wrapping", "[{}]"),
		//  new CompositeNode(182, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(183, -1, "text_run", "4", "[{}]"),
		//  new CompositeNode(184, -1, "text_run", ": JPEG", "[{}]"),
		//  new CompositeNode(194, -1, "text_run", "JPEG with Justify alignment then enable In Front of Text ", "[{}]"),
		//  new CompositeNode(195, -1, "text_run", "text", "[{}]"),
		//  new CompositeNode(196, -1, "text_run", " wrapping (caption hidden underneath image -> can move image to see)", "[{}]"),
		//  new CompositeNode(199, -1, "text_run", "Figure ", "[{}]"),
		//  new CompositeNode(200, -1, "text_run", "5", "[{}]"),
		//  new CompositeNode(201, -1, "text_run", ": JPEG", "[{}]"),
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

	// Node Type: tables
	public List<AbstractNode> tables = new List<AbstractNode>
	{
		//  new CompositeNode(60, 7, "table", "", "[]"),
		//  new CompositeNode(55, 8, "row", "", "[{}]"),
		//  new CompositeNode(52, -1, "cell", "Hi", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"5.5","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(53, -1, "cell", "i", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"6.91","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(54, -1, "cell", "Am", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"4.1","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(59, 8, "row", "", "[{}]"),
		//  new CompositeNode(56, -1, "cell", "Going", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"5.5","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(57, -1, "cell", "To", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"6.91","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(58, -1, "cell", "Remod", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"4.1","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(98, 7, "table", "", "[]"),
		//  new CompositeNode(93, 8, "row", "", "[{}]"),
		//  new CompositeNode(90, -1, "cell", "I love", "[{"underline":true,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3.23","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(91, -1, "cell", "flying", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"center","textcolor":"FF0000","highlightcolor":"none","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"4.75","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(92, -1, "cell", "in", "[{"underline":false,"bold":true,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"right","textcolor":"auto","highlightcolor":"none","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(97, 8, "row", "", "[{}]"),
		//  new CompositeNode(94, -1, "cell", "the", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"both","textcolor":"auto","highlightcolor":"none","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3.23","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(95, -1, "cell", "blue", "[{"underline":false,"bold":false,"italic":false,"fontType":"Arial","fontsize":20.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"4.75","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(96, -1, "cell", "sky", "[{"underline":false,"bold":false,"italic":true,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"left","textcolor":"auto","highlightcolor":"none","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3","rowHeight":"auto","backgroundcolor":"00B050"}]"),
		//  new CompositeNode(108, 7, "table", "", "[]"),
		//  new CompositeNode(103, 8, "row", "", "[{}]"),
		//  new CompositeNode(100, -1, "cell", "the", "[{"underline":true,"bold":true,"italic":true,"fontType":"Arial","fontsize":16.0,"horizontalalignment":"left","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"2.48","rowHeight":"auto","backgroundcolor":"0070C0"}]"),
		//  new CompositeNode(101, -1, "cell", "toilet", "[{"underline":true,"bold":true,"italic":true,"fontType":"Arial","fontsize":16.0,"horizontalalignment":"left","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"10.75","rowHeight":"auto","backgroundcolor":"0070C0"}]"),
		//  new CompositeNode(102, -1, "cell", "is", "[{"underline":true,"bold":true,"italic":true,"fontType":"Arial","fontsize":16.0,"horizontalalignment":"left","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3.28","rowHeight":"auto","backgroundcolor":"0070C0"}]"),
		//  new CompositeNode(107, 8, "row", "", "[{}]"),
		//  new CompositeNode(104, -1, "cell", "very", "[{"underline":true,"bold":true,"italic":true,"fontType":"Arial","fontsize":16.0,"horizontalalignment":"left","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"2.48","rowHeight":"auto","backgroundcolor":"0070C0"}]"),
		//  new CompositeNode(105, -1, "cell", "smelly", "[{"underline":true,"bold":true,"italic":true,"fontType":"Arial","fontsize":16.0,"horizontalalignment":"left","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"10.75","rowHeight":"auto","backgroundcolor":"0070C0"}]"),
		//  new CompositeNode(106, -1, "cell", "sia", "[{"underline":true,"bold":true,"italic":true,"fontType":"Arial","fontsize":16.0,"horizontalalignment":"left","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"default","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"1","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3.28","rowHeight":"auto","backgroundcolor":"0070C0"}]"),
		//  new CompositeNode(118, 7, "table", "", "[]"),
		//  new CompositeNode(113, 8, "row", "", "[{}]"),
		//  new CompositeNode(110, -1, "cell", "i", "[{"underline":true,"bold":true,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"center","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"6.48","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(111, -1, "cell", "cant", "[{"underline":true,"bold":true,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"center","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(112, -1, "cell", "wait", "[{"underline":true,"bold":true,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"center","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"single","borderbottomstyle":"single","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"0.25","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3.5","rowHeight":"auto","backgroundcolor":"auto"}]"),
		//  new CompositeNode(117, 8, "row", "", "[{}]"),
		//  new CompositeNode(114, -1, "cell", "to", "[{"underline":true,"bold":true,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"center","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"single","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"6.48","rowHeight":"2.76","backgroundcolor":"auto"}]"),
		//  new CompositeNode(115, -1, "cell", "go", "[{"underline":true,"bold":true,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"center","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"single","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3","rowHeight":"2.76","backgroundcolor":"auto"}]"),
		//  new CompositeNode(116, -1, "cell", "holiday", "[{"underline":true,"bold":true,"italic":false,"fontType":"Arial","fontsize":11.0,"horizontalalignment":"center","textcolor":"FF0000","highlightcolor":"yellow","bordertopstyle":"single","borderbottomstyle":"default","borderleftstyle":"default","borderrightstyle":"default","bordertopwidth":"0.25","borderbottomwidth":"1","borderleftwidth":"1","borderrightwidth":"1","bordertopcolor":"auto","borderbottomcolor":"auto","borderleftcolor":"auto","borderrightcolor":"auto","cellWidth":"3.5","rowHeight":"2.76","backgroundcolor":"auto"}]"),
	};

	// Node Type: citationAndbibliographys
	public List<AbstractNode> citationAndbibliographys = new List<AbstractNode>
	{
		//  new CompositeNode(77, -1, "intext-citation", "(Cole, 1998)", "[{}]"),
		//  new CompositeNode(81, -1, "intext-citation", " (", "[{}]"),
		//  new CompositeNode(82, -1, "intext-citation", "Wdadwa", "[{}]"),
		//  new CompositeNode(83, -1, "intext-citation", ", 1990)", "[{}]"),
		//  new CompositeNode(87, -1, "intext-citation", "(Chan, n.d.)", "[{}]"),
		//  new CompositeNode(211, 1, "bibliography", "Reference (IEE)", "[{"bold":true,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(212, -1, "citation_run", "[1] J. Cole, “I love pigs,” *Dr. J.*, vol. 20, 1998.", "[{"bold":false,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(213, -1, "citation_run", "[2] Wdadwa, “I love Germany,” *Sit.*, 1990.", "[{"bold":false,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(214, -1, "citation_run", "[3] D. Chan, “I love black people,” *White Man*, vol. 20, n.d.", "[{"bold":false,"italic":false,"alignment":"left","fontsize":10,"fonttype":"Times New Roman","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
	};

	// Node Type: math
	public List<AbstractNode> math = new List<AbstractNode>
	{
		//  new CompositeNode(122, -1, "math", "(1/2) × √(4)  <- math", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":{"ValueKind":3},"lineSpacingValue":{"ValueKind":4}}]"),
		//  new CompositeNode(123, -1, "math", "∫", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.15x)","lineSpacingValue":13.8}]"),
		//  new CompositeNode(138, -1, "math", "(1/2) × √(4)=1", "[{"bold":false,"italic":false,"alignment":"center","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(139, -1, "math", "(1/2)cos 2x+ (3/8)sin 4x   =3x", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(140, -1, "math", "log_24y ≥ (π/2)", "[{"bold":false,"italic":true,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(141, -1, "math", "∴ ∞ ≠ ±α", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(142, -1, "math", "∃xPersonx∧∀yTimey→Happyx,y", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(143, -1, "math", "F=ma", "[{"bold":false,"italic":false,"alignment":"left","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
		//  new CompositeNode(144, -1, "math", "lim1+(1/n)^n= e", "[{"bold":false,"italic":true,"alignment":"center","fontsize":12,"fonttype":"Default Font","fontcolor":"000000","highlight":"none","lineSpacingType":"Multiple (1.1x)","lineSpacingValue":12.95}]"),
	};

	// Node Type: image
	public List<AbstractNode> image = new List<AbstractNode>
	{
		//  new CompositeNode(129, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId9.png", []),
		//  new CompositeNode(131, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId10.png", []),
		//  new CompositeNode(135, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId11.png", []),
		//  new CompositeNode(150, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId12.png", []),
		//  new CompositeNode(159, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId13.png", []),
		//  new CompositeNode(171, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId13.png", []),
		//  new CompositeNode(181, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId14.png", []),
		//  new CompositeNode(198, -1, "image", @"C:\Users\stupi\OneDrive\Documents\GitHub\ICT2112_SD_ClassRepo\ICT2106WebApp\Images\Image_rId14.png", []),
	};
}
