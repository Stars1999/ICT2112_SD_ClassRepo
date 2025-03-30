using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using ICT2106WebApp.mod2grp6;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.Pages
{
    public class DocumentTestModel : PageModel
    {
        private readonly DocumentManager _documentManager;
        
        public bool IsDocumentLoaded { get; private set; }
        public List<AbstractNode> FormatContent { get; private set; }
        public List<AbstractNode> TextContent { get; private set; }
        public List<AbstractNode> ParagraphContent { get; private set; }
        public List<AbstractNode> LayoutContent { get; private set; }
        public List<AbstractNode> MetadataContent { get; private set; }
        public List<AbstractNode> MathContent { get; private set; }
        public List<AbstractNode> ListContent { get; private set; }
        public List<AbstractNode> ImageContent { get; private set; }
        public List<AbstractNode> BibliographyContent { get; private set; }
        
        public List<AbstractNode> SpecialElementContent { get; private set; }
        
        public bool LaTeXResult { get; private set; }
        public bool TemplateResult { get; private set; }
        
        public DocumentTestModel()
        {
            _documentManager = new DocumentManager();
            
            // Initialize all content lists
            FormatContent = new List<AbstractNode>();
            TextContent = new List<AbstractNode>();
            ParagraphContent = new List<AbstractNode>();
            LayoutContent = new List<AbstractNode>();
            MetadataContent = new List<AbstractNode>();
            MathContent = new List<AbstractNode>();
            ListContent = new List<AbstractNode>();
            ImageContent = new List<AbstractNode>();
            BibliographyContent = new List<AbstractNode>();
            SpecialElementContent = new List<AbstractNode>(); 
            
            IsDocumentLoaded = false;
        }
        
        public void OnGet()
        {
            // Initial page load - do nothing
        }
        
        public IActionResult OnPost()
        {
            // Load all content types from the DocumentManager
            MetadataContent = _documentManager.GetContentByType("metadata");
            FormatContent = _documentManager.GetContentByType("format");
            TextContent = _documentManager.GetContentByType("text");
            ParagraphContent = _documentManager.GetContentByType("paragraph");
            LayoutContent = _documentManager.GetContentByType("layout");
            MathContent = _documentManager.GetContentByType("math");
            ListContent = _documentManager.GetContentByType("lists");
            ImageContent = _documentManager.GetContentByType("images");
            
            SpecialElementContent = _documentManager.GetContentByType("special");
            
            IsDocumentLoaded = true;
            
            // Test LaTeX conversion
            LaTeXResult = _documentManager.toLaTeX("sample");
            
            // Test template conversion with a placeholder template ID
            //TemplateResult = _documentManager.convertToLatexTemplate("sample", "template1");
            
            return Page();
        }
    }
}
