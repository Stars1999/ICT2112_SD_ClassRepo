using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using ICT2106WebApp.mod2grp6;
using ICT2106WebApp.Utilities;
using ICT2106WebApp.mod2grp6.Template;

namespace ICT2106WebApp.Pages
{
    public class TemplateTestModel : PageModel
    {
        private readonly DocumentManager _documentManager;
        private readonly TemplateManager _templateManager;
        
        // Document content properties
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
        
        // Conversion results
        public bool OriginalLaTeXResult { get; private set; }
        public bool TemplateResult { get; private set; }
        
        // Original and template-applied content for display
        public string OriginalLaTeXContent { get; private set; }
        public string TemplateAppliedContent { get; private set; }
        
        // Selected template
        [BindProperty]
        public string SelectedTemplateId { get; set; } = "ieee"; // Default to IEEE template
        
        public TemplateTestModel()
        {
            _documentManager = new DocumentManager();
            _templateManager = new TemplateManager();
            
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
            
            IsDocumentLoaded = false;
            OriginalLaTeXContent = string.Empty;
            TemplateAppliedContent = string.Empty;
        }
        
        public void OnGet()
        {
            // Initial page load - do nothing
        }
        
        public IActionResult OnPost()
        {
            // Step 1: Load document content
            LoadDocumentContent();
            
            // Step 2: Convert to original LaTeX format
            OriginalLaTeXResult = _documentManager.toLaTeX("sample");
            
            // Step 3: Convert to template-applied LaTeX format
            TemplateResult = _documentManager.convertToLatexTemplate("sample", SelectedTemplateId);
            
            // Step 4: Get formatted content for display
            GenerateDisplayContent();
            
            return Page();
        }
        
        private void LoadDocumentContent()
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
            BibliographyContent = _documentManager.GetContentByType("bibliography");
            
            IsDocumentLoaded = true;
        }
        
        private void GenerateDisplayContent()
        {
            // For demonstration, we'll create representative LaTeX output
            // In a real scenario, this would be generated from the converted nodes
            
            // Generate Original LaTeX Content
            OriginalLaTeXContent = GenerateOriginalLaTeXContent();
            
            // Generate Template-Applied Content
            TemplateAppliedContent = GenerateTemplateAppliedContent();
        }
        
        private string GenerateOriginalLaTeXContent()
        {
            // Create a string representation of the LaTeX content based on the converted nodes
            var content = new System.Text.StringBuilder();
            
            // Document class and basic packages
            content.AppendLine("\\documentclass{article}");
            content.AppendLine("\\usepackage[utf8]{inputenc}");
            content.AppendLine("\\usepackage{graphicx}");
            content.AppendLine("\\usepackage{amsmath}");
            content.AppendLine("\\usepackage{xcolor}");
            content.AppendLine();
            
            // Add metadata if available
            if (MetadataContent.Count > 0)
            {
                foreach (var node in MetadataContent)
                {
                    content.AppendLine(node.GetContent());
                }
                content.AppendLine();
            }
            
            content.AppendLine("\\begin{document}");
            content.AppendLine();
            
            // Add headings and paragraphs
            if (FormatContent.Count > 0)
            {
                foreach (var node in FormatContent)
                {
                    content.AppendLine(node.GetContent());
                    content.AppendLine();
                }
            }
            
            // Add paragraph content
            if (ParagraphContent.Count > 0)
            {
                foreach (var node in ParagraphContent)
                {
                    content.AppendLine(node.GetContent());
                    content.AppendLine();
                }
            }
            
            // Add math content
            if (MathContent.Count > 0)
            {
                content.AppendLine("% Mathematical content");
                foreach (var node in MathContent)
                {
                    content.AppendLine("$" + node.GetContent() + "$");
                }
                content.AppendLine();
            }
            
            // Add lists
            if (ListContent.Count > 0)
            {
                content.AppendLine("\\begin{itemize}");
                foreach (var node in ListContent)
                {
                    content.AppendLine("  \\item " + node.GetContent());
                }
                content.AppendLine("\\end{itemize}");
                content.AppendLine();
            }
            
            // End document
            content.AppendLine("\\end{document}");
            
            return content.ToString();
        }
        
        private string GenerateTemplateAppliedContent()
        {
            // Create a string representation of the IEEE template-applied LaTeX content
            var content = new System.Text.StringBuilder();
            
            // IEEE-specific document class and packages
            content.AppendLine("\\documentclass[conference]{IEEEtran}");
            content.AppendLine("\\usepackage[utf8]{inputenc}");
            content.AppendLine("\\usepackage{graphicx}");
            content.AppendLine("\\usepackage{amsmath}");
            content.AppendLine("\\usepackage{xcolor}");
            content.AppendLine("\\usepackage{cite}");
            content.AppendLine("\\usepackage{algorithm}");
            content.AppendLine("\\usepackage{algorithmic}");
            content.AppendLine();
            
            // Add metadata if available
            if (MetadataContent.Count > 0)
            {
                foreach (var node in MetadataContent)
                {
                    content.AppendLine(node.GetContent());
                }
                content.AppendLine();
            }
            
            // IEEE template usually requires title, author, and abstract
            content.AppendLine("\\title{Document Title}");
            content.AppendLine("\\author{Author Name}");
            content.AppendLine();
            
            content.AppendLine("\\begin{document}");
            content.AppendLine("\\maketitle");
            content.AppendLine();
            
            content.AppendLine("\\begin{abstract}");
            content.AppendLine("This is the abstract of the document.");
            content.AppendLine("\\end{abstract}");
            content.AppendLine();
            
            // IEEE documents typically use sections instead of custom formatting
            content.AppendLine("\\section{Introduction}");
            
            // Add headings and paragraphs
            if (FormatContent.Count > 0)
            {
                foreach (var node in FormatContent)
                {
                    // Format as IEEE section/subsection instead of original formatting
                    string nodeType = node.GetNodeType();
                    string nodeContent = node.GetContent();
                    
                    if (nodeType.StartsWith("h"))
                    {
                        // Convert heading levels to IEEE format
                        if (nodeType == "h1")
                            content.AppendLine("\\section{" + ExtractTextFromLaTeX(nodeContent) + "}");
                        else if (nodeType == "h2")
                            content.AppendLine("\\subsection{" + ExtractTextFromLaTeX(nodeContent) + "}");
                        else if (nodeType == "h3")
                            content.AppendLine("\\subsubsection{" + ExtractTextFromLaTeX(nodeContent) + "}");
                        else
                            content.AppendLine(nodeContent);
                    }
                    else
                    {
                        content.AppendLine(nodeContent);
                    }
                    content.AppendLine();
                }
            }
            
            // Add paragraph content - IEEE typically has compact paragraphs
            if (ParagraphContent.Count > 0)
            {
                foreach (var node in ParagraphContent)
                {
                    content.AppendLine(node.GetContent());
                    content.AppendLine();
                }
            }
            
            // Add math content
            if (MathContent.Count > 0)
            {
                content.AppendLine("\\section{Mathematical Content}");
                foreach (var node in MathContent)
                {
                    content.AppendLine("$" + node.GetContent() + "$");
                }
                content.AppendLine();
            }
            
            // Add lists
            if (ListContent.Count > 0)
            {
                content.AppendLine("\\section{List Items}");
                content.AppendLine("\\begin{itemize}");
                foreach (var node in ListContent)
                {
                    content.AppendLine("  \\item " + node.GetContent());
                }
                content.AppendLine("\\end{itemize}");
                content.AppendLine();
            }
            
            // Bibliography section
            if (BibliographyContent.Count > 0)
            {
                content.AppendLine("\\bibliographystyle{IEEEtran}");
                content.AppendLine("\\bibliography{references}");
            }
            
            // End document
            content.AppendLine("\\end{document}");
            
            return content.ToString();
        }
        
        private string ExtractTextFromLaTeX(string latexContent)
        {
            // Simple method to extract text from LaTeX commands like \section{text}
            // This is a basic implementation and might need enhancement for complex LaTeX
            
            // If the content contains LaTeX formatting, try to extract just the text
            if (latexContent.Contains("\\"))
            {
                // Find content between braces
                int startIndex = latexContent.IndexOf('{');
                int endIndex = latexContent.LastIndexOf('}');
                
                if (startIndex >= 0 && endIndex > startIndex)
                {
                    return latexContent.Substring(startIndex + 1, endIndex - startIndex - 1);
                }
            }
            
            return latexContent;
        }
    }
}