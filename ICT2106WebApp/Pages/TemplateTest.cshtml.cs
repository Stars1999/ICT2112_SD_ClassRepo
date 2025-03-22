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
            // Step 1: Load document content using TemplateSample data
            LoadDocumentContent(true);
            
            // Step 2: Convert to original LaTeX format
            OriginalLaTeXResult = ((DocumentManager)_documentManager).toLaTeXWithTemplate("template", true);
            
            // Step 3: Convert to template-applied LaTeX format
            TemplateResult = ((DocumentManager)_documentManager).convertToLatexTemplateWithTemplate("template", SelectedTemplateId, true);
            
            // Step 4: Get formatted content for display
            GenerateDisplayContent();
            
            return Page();
        }
        
        private void LoadDocumentContent(bool useTemplateData = true)
        {
            // Load all content types from the DocumentManager using TemplateSample data
            MetadataContent = _documentManager.GetContentByType("metadata", useTemplateData);
            FormatContent = _documentManager.GetContentByType("format", useTemplateData);
            TextContent = _documentManager.GetContentByType("text", useTemplateData);
            ParagraphContent = _documentManager.GetContentByType("paragraph", useTemplateData);
            LayoutContent = _documentManager.GetContentByType("layout", useTemplateData);
            MathContent = _documentManager.GetContentByType("math", useTemplateData);
            ListContent = _documentManager.GetContentByType("lists", useTemplateData);
            ImageContent = _documentManager.GetContentByType("images", useTemplateData);
            BibliographyContent = _documentManager.GetContentByType("bibliography", useTemplateData);
            
            IsDocumentLoaded = true;
        }
        
        private void GenerateDisplayContent()
        {
            // Generate Original LaTeX Content
            OriginalLaTeXContent = GenerateOriginalLaTeXContent();
            
            // Generate Template-Applied Content
            TemplateAppliedContent = GenerateTemplateAppliedContent();
        }
        
        private string GenerateOriginalLaTeXContent()
        {
            // Create a string representation of the LaTeX content
            var content = new System.Text.StringBuilder();
            
            // Document class and basic packages
            content.AppendLine("\\documentclass{article}");
            content.AppendLine("\\usepackage[utf8]{inputenc}");
            content.AppendLine("\\usepackage{graphicx}");
            content.AppendLine("\\usepackage{amsmath}");
            content.AppendLine("\\usepackage{xcolor}");
            content.AppendLine("\\usepackage{hyperref}");
            content.AppendLine("\\usepackage{amsfonts}");
            content.AppendLine("\\usepackage{amssymb}");
            
            // Set proper paragraph formatting
            content.AppendLine("\\setlength{\\parindent}{0em}");
            content.AppendLine("\\setlength{\\parskip}{1em}");
            content.AppendLine();
            
            // Get title from first heading
            string title = "Artificial Intelligence in Modern Education";
            if (FormatContent.Count > 0 && FormatContent[0].GetNodeType() == "h1")
            {
                title = CleanTextForLaTeX(FormatContent[0].GetContent());
            }
            
            // Get author from metadata if available
            string authors = "J. Smith, K. Johnson, L. Chen";
            if (MetadataContent.Count > 0 && MetadataContent[0].GetStyling() != null)
            {
                foreach (var style in MetadataContent[0].GetStyling())
                {
                    if (style.ContainsKey("author"))
                    {
                        authors = style["author"].ToString();
                        break;
                    }
                }
            }
            
            content.AppendLine($"\\title{{{title}}}");
            content.AppendLine($"\\author{{{authors}}}");
            content.AppendLine("\\date{\\today}");
            content.AppendLine();
            
            content.AppendLine("\\begin{document}");
            content.AppendLine("\\maketitle");
            content.AppendLine();
            
            // Add abstract section
            if (FormatContent.Count > 1 && FormatContent[1].GetNodeType() == "h2" && 
                CleanTextForLaTeX(FormatContent[1].GetContent()).Contains("Abstract"))
            {
                content.AppendLine("\\begin{abstract}");
                if (ParagraphContent.Count > 0)
                {
                    // Get the abstract text
                    string abstractText = ParagraphContent[0].GetContent();
                    
                    // Check if the abstract should be italic based on node styling
                    bool isItalic = false;
                    foreach (var style in ParagraphContent[0].GetStyling())
                    {
                        if (style.ContainsKey("Italic") && (bool)style["Italic"])
                        {
                            isItalic = true;
                            break;
                        }
                    }
                    
                    // Clean up the text
                    abstractText = CleanTextForLaTeX(abstractText);
                    
                    // Apply italic formatting if needed
                    if (isItalic)
                    {
                        abstractText = $"\\textit{{{abstractText}}}";
                    }
                    
                    content.AppendLine(abstractText);
                }
                else
                {
                    // Default abstract with italic formatting
                    content.AppendLine("\\textit{This paper examines the role of artificial intelligence in modern educational systems. We explore how AI-driven tools can enhance learning experiences, personalize education, and support both educators and students. Key findings indicate that while AI offers significant benefits, challenges remain regarding implementation, ethics, and accessibility.}");
                }
                content.AppendLine("\\end{abstract}");
                content.AppendLine();
            }
            
            // Add sections and content
            for (int i = 2; i < FormatContent.Count; i++)
            {
                string nodeType = FormatContent[i].GetNodeType();
                string nodeContent = CleanTextForLaTeX(FormatContent[i].GetContent());
                
                if (nodeType == "h2")
                {
                    content.AppendLine($"\\section{{{nodeContent}}}");
                }
                else if (nodeType == "h3")
                {
                    content.AppendLine($"\\subsection{{{nodeContent}}}");
                }
                else
                {
                    content.AppendLine(nodeContent);
                }
                content.AppendLine();
                
                // Add corresponding paragraphs after each heading
                if (i - 1 < ParagraphContent.Count)
                {
                    var paragraph = ParagraphContent[i - 1];
                    string paraText = CleanTextForLaTeX(paragraph.GetContent());
                    
                    // Apply formatting based on paragraph styling
                    paraText = ApplyNodeStyling(paraText, paragraph);
                    
                    content.AppendLine(paraText);
                    content.AppendLine();
                }
            }
            
            // Add math content using equation environment
            if (MathContent.Count > 0)
            {
                content.AppendLine("\\section*{Mathematical Formulations}");
                
                foreach (var node in MathContent)
                {
                    string mathText = CleanTextForLaTeX(node.GetContent(), "math");
                    if (!string.IsNullOrEmpty(mathText))
                    {
                        // Use proper equation environment with balanced braces
                        content.AppendLine("\\begin{equation}");
                        content.AppendLine(EnsureBalancedBraces(mathText));
                        content.AppendLine("\\end{equation}");
                        content.AppendLine();
                    }
                }
            }
            
            // Add lists with proper items
            if (ListContent.Count > 0)
            {
                content.AppendLine("\\section*{Key Points}");
                
                // Determine if we have numbered lists
                bool hasNumberedLists = false;
                foreach (var node in ListContent)
                {
                    if (node.GetNodeType().Contains("numbered"))
                    {
                        hasNumberedLists = true;
                        break;
                    }
                }
                
                // Add bulleted lists
                if (!hasNumberedLists)
                {
                    content.AppendLine("\\begin{itemize}");
                    foreach (var node in ListContent)
                    {
                        if (node.GetNodeType().Contains("bulleted"))
                        {
                            string itemText = CleanTextForLaTeX(node.GetContent());
                            content.AppendLine($"  \\item {EnsureBalancedBraces(itemText)}");
                        }
                    }
                    content.AppendLine("\\end{itemize}");
                }
                else
                {
                    // Add numbered lists
                    content.AppendLine("\\begin{enumerate}");
                    foreach (var node in ListContent)
                    {
                        if (node.GetNodeType().Contains("numbered"))
                        {
                            string itemText = CleanTextForLaTeX(node.GetContent());
                            content.AppendLine($"  \\item {EnsureBalancedBraces(itemText)}");
                        }
                    }
                    content.AppendLine("\\end{enumerate}");
                }
                content.AppendLine();
            }
            
            // Add bibliography
            if (BibliographyContent.Count > 0)
            {
                content.AppendLine("\\begin{thebibliography}{99}");
                
                foreach (var node in BibliographyContent)
                {
                    string bibText = CleanTextForLaTeX(node.GetContent());
                    content.AppendLine($"  \\bibitem{{ref{node.GetNodeId()}}} {bibText}");
                }
                
                content.AppendLine("\\end{thebibliography}");
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
            content.AppendLine("\\usepackage{hyperref}");
            content.AppendLine("\\usepackage{textgreek}");
            content.AppendLine("\\usepackage{amsfonts}");
            content.AppendLine("\\usepackage{amssymb}");
            content.AppendLine();
            
            // Get title and author information
            string title = "Artificial Intelligence in Modern Education";
            string authors = "J. Smith, K. Johnson, L. Chen";
            
            if (FormatContent.Count > 0 && FormatContent[0].GetNodeType() == "h1")
            {
                title = CleanTextForLaTeX(FormatContent[0].GetContent());
            }
            
            if (MetadataContent.Count > 0 && MetadataContent[0].GetStyling() != null)
            {
                foreach (var style in MetadataContent[0].GetStyling())
                {
                    if (style.ContainsKey("author"))
                    {
                        authors = style["author"].ToString();
                        break;
                    }
                }
            }
            
            // Add title and author commands
            content.AppendLine($"\\title{{{title}}}");
            content.AppendLine($"\\author{{{authors}}}");
            content.AppendLine();
            
            content.AppendLine("\\begin{document}");
            content.AppendLine("\\maketitle");
            content.AppendLine();
            
            // Add abstract section
            if (FormatContent.Count > 1 && FormatContent[1].GetNodeType() == "h2" && 
                CleanTextForLaTeX(FormatContent[1].GetContent()).Contains("Abstract"))
            {
                content.AppendLine("\\begin{abstract}");
                if (ParagraphContent.Count > 0)
                {
                    // Get the abstract text
                    string abstractText = ParagraphContent[0].GetContent();
                    
                    // Clean up the text
                    abstractText = CleanTextForLaTeX(abstractText);
                    
                    // Check if the abstract should be italic based on node styling
                    bool isItalic = false;
                    foreach (var style in ParagraphContent[0].GetStyling())
                    {
                        if (style.ContainsKey("Italic") && (bool)style["Italic"])
                        {
                            isItalic = true;
                            break;
                        }
                    }
                    
                    // Apply italic formatting if needed
                    if (isItalic)
                    {
                        abstractText = $"\\textit{{{abstractText}}}";
                    }
                    
                    content.AppendLine(abstractText);
                }
                content.AppendLine("\\end{abstract}");
                content.AppendLine("\\IEEEpeerreviewmaketitle");
                content.AppendLine();
            }
            
            // Add sections and content
            for (int i = 2; i < FormatContent.Count; i++)
            {
                string nodeType = FormatContent[i].GetNodeType();
                string nodeContent = CleanTextForLaTeX(FormatContent[i].GetContent());
                
                if (nodeType == "h2")
                {
                    content.AppendLine($"\\section{{{nodeContent}}}");
                }
                else if (nodeType == "h3")
                {
                    content.AppendLine($"\\subsection{{{nodeContent}}}");
                }
                else
                {
                    content.AppendLine(nodeContent);
                }
                content.AppendLine();
                
                // Add corresponding paragraphs after each heading
                if (i - 1 < ParagraphContent.Count)
                {
                    var paragraph = ParagraphContent[i - 1];
                    string paraText = CleanTextForLaTeX(paragraph.GetContent());
                    
                    // Apply formatting based on paragraph styling
                    paraText = ApplyNodeStyling(paraText, paragraph);
                    
                    content.AppendLine(paraText);
                    content.AppendLine();
                }
            }
            
            // Add math content with proper handling
            if (MathContent.Count > 0)
            {
                content.AppendLine("\\section*{Mathematical Formulations}");
                
                foreach (var node in MathContent)
                {
                    string mathText = CleanTextForLaTeX(node.GetContent(), "math");
                    if (!string.IsNullOrEmpty(mathText))
                    {
                        // Use proper equation environment
                        content.AppendLine("\\begin{equation}");
                        content.AppendLine(EnsureBalancedBraces(mathText));
                        content.AppendLine("\\end{equation}");
                        content.AppendLine();
                    }
                }
            }
            
            // Add lists with proper items using IEEE style
            if (ListContent.Count > 0)
            {
                content.AppendLine("\\section{Key Findings}");
                
                // Separate bulleted and numbered lists
                var bulletedLists = new List<AbstractNode>();
                var numberedLists = new List<AbstractNode>();
                
                foreach (var node in ListContent)
                {
                    if (node.GetNodeType().Contains("numbered"))
                    {
                        numberedLists.Add(node);
                    }
                    else
                    {
                        bulletedLists.Add(node);
                    }
                }
                
                // Add bulleted lists
                if (bulletedLists.Count > 0)
                {
                    content.AppendLine("\\begin{itemize}");
                    foreach (var node in bulletedLists)
                    {
                        string itemText = CleanTextForLaTeX(node.GetContent());
                        content.AppendLine($"  \\item {EnsureBalancedBraces(itemText)}");
                    }
                    content.AppendLine("\\end{itemize}");
                    content.AppendLine();
                }
                
                // Add numbered lists
                if (numberedLists.Count > 0)
                {
                    content.AppendLine("\\begin{enumerate}");
                    foreach (var node in numberedLists)
                    {
                        string itemText = CleanTextForLaTeX(node.GetContent());
                        content.AppendLine($"  \\item {EnsureBalancedBraces(itemText)}");
                    }
                    content.AppendLine("\\end{enumerate}");
                    content.AppendLine();
                }
            }
            
            // Bibliography section in IEEE format
            if (BibliographyContent.Count > 0)
            {
                content.AppendLine("\\section{References}");
                content.AppendLine("\\begin{thebibliography}{99}");
                
                foreach (var node in BibliographyContent)
                {
                    string bibText = CleanTextForLaTeX(node.GetContent());
                    content.AppendLine($"  \\bibitem{{ref{node.GetNodeId()}}} {bibText}");
                }
                
                content.AppendLine("\\end{thebibliography}");
                content.AppendLine();
            }
            
            // End document
            content.AppendLine("\\end{document}");
            
            return content.ToString();
        }

        private void AppendMathContent(System.Text.StringBuilder content, List<AbstractNode> mathNodes)
        {
            if (mathNodes == null || mathNodes.Count == 0)
                return;
            
            // Add necessary LaTeX packages for math
            content.AppendLine("% Required math packages for advanced formulas");
            content.AppendLine("\\usepackage{amsmath}");
            content.AppendLine("\\usepackage{amssymb}");
            content.AppendLine();
            
            content.AppendLine("\\section*{Mathematical Formulations}");
            
            foreach (var node in mathNodes)
            {
                string mathText = node.GetContent();
                
                if (!string.IsNullOrEmpty(mathText))
                {
                    // Check if the formula contains specific environments
                    if (mathText.Contains("\\begin{"))
                    {
                        // If it already has an environment, use it as is
                        content.AppendLine(mathText);
                    }
                    else
                    {
                        // Use equation environment for displayed equations
                        content.AppendLine("\\begin{equation}");
                        content.AppendLine(mathText);
                        content.AppendLine("\\end{equation}");
                    }
                    content.AppendLine();
                }
            }
        }
        

        // Helper method to apply node styling (bold, italic, etc.) based on node properties
        private string ApplyNodeStyling(string text, AbstractNode node)
        {
            if (string.IsNullOrEmpty(text) || node == null || node.GetStyling() == null)
                return text;
            
            foreach (var style in node.GetStyling())
            {
                // Apply bold formatting
                if (style.ContainsKey("Bold") && (bool)style["Bold"])
                {
                    text = $"\\textbf{{{text}}}";
                }
                
                // Apply italic formatting
                if (style.ContainsKey("Italic") && (bool)style["Italic"])
                {
                    text = $"\\textit{{{text}}}";
                }
                
                // Apply color formatting if not default black
                if (style.ContainsKey("FontColor") && !string.IsNullOrEmpty((string)style["FontColor"]) && (string)style["FontColor"] != "000000")
                {
                    text = $"\\textcolor[HTML]{{{(string)style["FontColor"]}}}{{{text}}}";
                }
                
                // Apply alignment if specified
                if (style.ContainsKey("Alignment") && !string.IsNullOrEmpty((string)style["Alignment"]))
                {
                    string alignment = ((string)style["Alignment"]).ToLower();
                    if (alignment == "center")
                    {
                        text = $"\\begin{{center}}\n{text}\n\\end{{center}}";
                    }
                    else if (alignment == "right")
                    {
                        text = $"\\begin{{flushright}}\n{text}\n\\end{{flushright}}";
                    }
                    else if (alignment == "justified")
                    {
                        // No specific command needed for justified text in LaTeX (it's the default)
                    }
                }
            }
            
            return text;
        }
        
        // Helper method to ensure balanced braces in LaTeX content
        private string EnsureBalancedBraces(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            
            // Count opening and closing braces
            int openBraces = 0;
            int closeBraces = 0;
            
            foreach (char c in text)
            {
                if (c == '{') openBraces++;
                if (c == '}') closeBraces++;
            }
            
            // Add missing closing braces
            string result = text;
            for (int i = 0; i < openBraces - closeBraces; i++)
            {
                result += "}";
            }
            
            // Add missing opening braces
            for (int i = 0; i < closeBraces - openBraces; i++)
            {
                result = "{" + result;
            }
            
            return result;
        }

        // Consolidated text cleaning method
        private string CleanTextForLaTeX(string text, string contentType = "text")
        {
            if (string.IsNullOrEmpty(text))
                return "";
            
            // Fix any incorrectly double-escaped ampersands
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\\\&", "\\&");
            
            // Remove problematic command patterns
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\paragraph\\\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textit\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textbf\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\selectfont\b", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\fontsize\{[^}]*\}\{[^}]*\}", "");
            
            // Remove trailing backslashes
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\\\$", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\\\(\s)", "$1");
            
            // Remove any \textbackslash{} followed by ampersand
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textbackslash\{\s*\}&", "\\&");
            
            // Extract content from section commands if needed
            if (text.Contains("\\section{") || text.Contains("\\subsection{") || text.Contains("\\subsubsection{"))
            {
                int startIndex = text.IndexOf('{');
                int endIndex = text.LastIndexOf('}');
                
                if (startIndex >= 0 && endIndex > startIndex)
                {
                    return text.Substring(startIndex + 1, endIndex - startIndex - 1);
                }
            }
            
            // Different handling for math content
            if (contentType == "math")
            {
                // Convert Unicode math symbols to LaTeX commands
                text = text.Replace("Σ", "\\Sigma ");
                text = text.Replace("×", "\\times ");
                text = text.Replace("≥", "\\geq ");
                text = text.Replace("≠", "\\neq ");
                text = text.Replace("±", "\\pm ");
                text = text.Replace("∞", "\\infty ");
                text = text.Replace("∴", "\\therefore ");
                text = text.Replace("∃", "\\exists ");
                text = text.Replace("∀", "\\forall ");
                text = text.Replace("→", "\\rightarrow ");
                text = text.Replace("π", "\\pi ");
                text = text.Replace("α", "\\alpha ");
                text = text.Replace("Σ", "\\sum ");

                
                // Fix unnecessary backslashes for math mode
                text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textbackslash\{\}", "");
                text = System.Text.RegularExpressions.Regex.Replace(text, @"_\\", "_");
                
                return text;
            }
            
            // Handle regular text content
            // Fix any double-brace patterns
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\{\{", "{");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\}\}", "}");
            
            // Handle special LaTeX characters
            text = System.Text.RegularExpressions.Regex.Replace(text, @"(?<!\\)&", "\\&");
            text = text.Replace("%", "\\%");
            text = text.Replace("$", "\\$");
            text = text.Replace("#", "\\#");
            text = text.Replace("_", "\\_");
            text = text.Replace("×", "$\\times$");
            
            return text;
        }
    }
}