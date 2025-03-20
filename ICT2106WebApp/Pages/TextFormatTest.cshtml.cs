using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.mod2grp6.Text;
using ICT2106WebApp.Utilities;
using System;
using System.Collections.Generic;

namespace ICT2106WebApp.Pages
{
    public class TextFormatTestModel : PageModel
    {
        private readonly IFormatText _textManager;
        
        // Input properties
        [BindProperty]
        public string Content { get; set; } = "Sample text for formatting demonstration.";
        
        [BindProperty]
        public string Font { get; set; } = "Times New Roman";

        [BindProperty]
        public int FontSize { get; set; } = 12; 
        
        [BindProperty]
        public bool BoldStyle { get; set; } = false;
        
        [BindProperty]
        public bool ItalicStyle { get; set; } = false;
        
        [BindProperty]
        public bool UnderlineStyle { get; set; } = false;
        
        [BindProperty]
        public string Color { get; set; } = "black";

        [BindProperty]
        public string HighlightColor { get; set; } = "none"; //
        
        [BindProperty]
        public float LineSpacing { get; set; } = 1.0f;
        
        [BindProperty]
        public string Alignment { get; set; } = "left";
        
        // Output properties
        public bool ConversionSuccess { get; set; } = false;
        public List<AbstractNode> ConvertedNodes { get; set; } = new List<AbstractNode>();
        
        public TextFormatTestModel(IFormatText textManager)
        {
            _textManager = textManager;
        }
        
        public void OnGet()
        {
            // Default values already set in properties
        }
        
        public void OnPost()
        {
            // Debug the form values
            Console.WriteLine($"Form values - Bold: {BoldStyle}, Italic: {ItalicStyle}, Underline: {UnderlineStyle}");
            
            // Override values for testing
            bool testBold = true;
            bool testItalic = true;
            bool testUnderline = true;
            
            Console.WriteLine($"Test values - Bold: {testBold}, Italic: {testItalic}, Underline: {testUnderline}");
            
            // Create a node with the input text and styling using our test values 
            var stylingDict = new Dictionary<string, object>
            {
                { "fontFamily", Font },
                { "fontSize", FontSize },
                { "bold", BoldStyle },
                { "italic", ItalicStyle },
                { "underline", UnderlineStyle },
                { "color", Color },
                { "lineSpacing", LineSpacing },
                { "alignment", Alignment }
            };

            // Only add highlight color if not "none"
            if (HighlightColor != "none")
            {
                stylingDict.Add("highlightColor", HighlightColor);
            }
            
            var stylingList = new List<Dictionary<string, object>> { stylingDict };
            
            // Create test nodes
            var testNode = new SimpleNode(1, "paragraph", Content, stylingList);
            var nodes = new List<AbstractNode> { testNode };
            
            // Apply text formatting
            bool success = _textManager.StartTextFormatting(nodes);
            
            if (success)
            {
                _textManager.FormatFonts();
                _textManager.FormatStyles();
                _textManager.FormatColors();
                _textManager.FormatLineSpacing();
                _textManager.FormatAlignment();
                
                ConvertedNodes = _textManager.ApplyTextFormatting();
                ConversionSuccess = true;
            }
            else
            {
                ConversionSuccess = false;
                ConvertedNodes = new List<AbstractNode>();
            }
        }
    }
}