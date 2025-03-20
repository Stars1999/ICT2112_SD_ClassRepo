using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using ICT2106WebApp.mod2grp6.Text;
using ICT2106WebApp.mod2grp6.Template;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.Pages
{
    public class TemplateTestModel : PageModel
    {
        private readonly IFormatText _textManager;
        private readonly TemplateManager _templateManager;

        // Input properties
        [BindProperty]
        public string Content { get; set; } = "Sample content for preview.";
        
        [BindProperty]
        public string Font { get; set; } = "Times New Roman";
        
        [BindProperty]
        public bool BoldStyle { get; set; } = false;
        
        [BindProperty]
        public bool ItalicStyle { get; set; } = false;
        
        [BindProperty]
        public bool UnderlineStyle { get; set; } = false;
        
        [BindProperty]
        public string Color { get; set; } = "black";
        
        [BindProperty]
        public float LineSpacing { get; set; } = 1.0f;
        
        [BindProperty]
        public string Alignment { get; set; } = "left";
        
        // Output properties
        public bool ConversionSuccess { get; set; } = false;
        public List<AbstractNode> ConvertedNodes { get; set; } = new List<AbstractNode>();

        public TemplateTestModel(IFormatText textManager, TemplateManager templateManager)
        {
            _textManager = textManager;
            _templateManager = templateManager;
        }

        public void OnGet()
        {
            // Default values already set in properties
        }

        public void OnPost()
        {
            // Create a node with the input text and styling
            var stylingDict = new Dictionary<string, object>
            {
                { "fontFamily", Font },
                { "bold", BoldStyle },
                { "italic", ItalicStyle },
                { "underline", UnderlineStyle },
                { "color", Color },
                { "lineSpacing", LineSpacing },
                { "alignment", Alignment }
            };

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

            // Apply the template to the content
            Template template = _templateManager.ConvertToTemplate("defaultTemplateId"); // For now, using a default template ID
            if (template != null)
            {
                // Merge the template with the content
                List<AbstractNode> templateContent = template.GetContent();
                ConvertedNodes.AddRange(templateContent); // Add template content to the converted nodes
            }
        }
    }
}
