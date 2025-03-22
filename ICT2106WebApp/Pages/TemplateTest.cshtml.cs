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
            // Create a string representation of the LaTeX content based on the converted nodes
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
            
            // Set up title and author
            string title = "Artificial Intelligence in Modern Education";
            if (FormatContent.Count > 0 && FormatContent[0].GetNodeType() == "h1")
            {
                title = CleanText(FormatContent[0].GetContent());
            }
            
            content.AppendLine($"\\title{{{title}}}");
            content.AppendLine("\\author{J. Smith, K. Johnson, L. Chen}");
            content.AppendLine("\\date{\\today}");
            content.AppendLine();
            
            content.AppendLine("\\begin{document}");
            content.AppendLine("\\maketitle");
            content.AppendLine();
            
            // Add abstract section - ensure there's content in the abstract
            if (FormatContent.Count > 1 && FormatContent[1].GetNodeType() == "h2" && 
                CleanText(FormatContent[1].GetContent()).Contains("Abstract"))
            {
                content.AppendLine("\\begin{abstract}");
                if (ParagraphContent.Count > 0)
                {
                    string abstractText = CleanText(ParagraphContent[0].GetContent());
                    // Make sure the abstract text doesn't have any unbalanced braces
                    abstractText = EnsureBalancedBraces(abstractText);
                    content.AppendLine(abstractText);
                }
                else
                {
                    // Always include some content in abstract to avoid LaTeX errors
                    content.AppendLine("This paper examines the role of artificial intelligence in modern educational systems.");
                }
                content.AppendLine("\\end{abstract}");
                content.AppendLine();
            }
            
            // Add sections and content
            for (int i = 2; i < FormatContent.Count; i++)
            {
                string nodeType = FormatContent[i].GetNodeType();
                string nodeContent = CleanText(FormatContent[i].GetContent());
                
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
                    string paraText = CleanText(ParagraphContent[i - 1].GetContent());
                    // Make sure paragraph text doesn't have any unbalanced braces
                    paraText = EnsureBalancedBraces(paraText);
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
                    string mathText = CleanMathContent(node.GetContent());
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
                AppendListItems(content, ListContent);
            }
            
            // Add bibliography
            if (BibliographyContent.Count > 0)
            {
                AppendBibliography(content, BibliographyContent);
            }
            
            // End document
            content.AppendLine("\\end{document}");
            
            return content.ToString();
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
            
            // Add missing opening braces (should be at the start to avoid messing up the structure)
            for (int i = 0; i < closeBraces - openBraces; i++)
            {
                result = "{" + result;
            }
            
            return result;
        }

        // Simpler helper to clean text for LaTeX
        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            
            // First, fix any incorrectly double-escaped ampersands (this is crucial)
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
            
            // Fix any double-brace patterns {{ or }} that aren't meant to be escaped
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\{\{", "{");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\}\}", "}");
            
            // NOW properly handle ampersands AFTER fixing double-escaping
            // Only replace & with \& if it's not already escaped
            text = System.Text.RegularExpressions.Regex.Replace(text, @"(?<!\\)&", "\\&");
            
            // Other character replacements
            text = text.Replace("%", "\\%");
            text = text.Replace("$", "\\$");
            text = text.Replace("#", "\\#");
            text = text.Replace("_", "\\_");
            text = text.Replace("×", "$\\times$");
            
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
            
            return text;
        }

        // Safe math content processor
        private string CleanMathContent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            
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
            
            // Remove any \textbackslash{} in math mode
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textbackslash\{\}", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"_\\", "_");
            
            // Remove problematic command patterns
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\paragraph\\\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textit\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textbf\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\selectfont\b", "");
            
            return text;
        }

        // Helper for safe list generation
        private void AppendListItems(System.Text.StringBuilder content, List<AbstractNode> items, string listType = "itemize")
        {
            if (items == null || items.Count == 0)
                return;
            
            content.AppendLine($"\\begin{{{listType}}}");
            
            bool hasValidItems = false;
            
            foreach (var node in items)
            {
                string itemText = CleanText(node.GetContent());
                if (!string.IsNullOrWhiteSpace(itemText))
                {
                    // Ensure each item has balanced braces
                    content.AppendLine($"  \\item {EnsureBalancedBraces(itemText)}");
                    hasValidItems = true;
                }
            }
            
            // Always ensure there's at least one item
            if (!hasValidItems)
            {
                content.AppendLine("  \\item No items available");
            }
            
            content.AppendLine($"\\end{{{listType}}}");
            content.AppendLine();
        }

        private void AppendBibliography(System.Text.StringBuilder content, List<AbstractNode> bibNodes, bool isIEEE = false)
        {
            if (bibNodes == null || bibNodes.Count == 0)
                return;
            
            if (isIEEE)
            {
                content.AppendLine("\\section{References}");
                content.AppendLine("\\bibliographystyle{IEEEtran}");
            }
            
            content.AppendLine("\\begin{thebibliography}{99}");
            
            bool hasValidItems = false;
            foreach (var node in bibNodes)
            {
                // Get the raw content first
                string bibText = node.GetContent();
                
                // CRITICAL: Fix any double-escaped ampersands first (\\& → \&)
                bibText = System.Text.RegularExpressions.Regex.Replace(bibText, @"\\\\&", "\\&");
                
                // Now apply general cleanup
                bibText = CleanText(bibText);
                
                // Final sanity check for any remaining problematic patterns
                bibText = System.Text.RegularExpressions.Regex.Replace(bibText, @"\\textbackslash\{\s*\}&", "\\&");
                bibText = System.Text.RegularExpressions.Regex.Replace(bibText, @"\\\\&", "\\&");
                
                if (!string.IsNullOrEmpty(bibText))
                {
                    content.AppendLine($"  \\bibitem{{ref{node.GetNodeId()}}} {bibText}");
                    hasValidItems = true;
                }
            }
            
            // Always ensure there's at least one bibliography item
            if (!hasValidItems)
            {
                content.AppendLine("  \\bibitem{dummy} No references available");
            }
            
            content.AppendLine("\\end{thebibliography}");
            content.AppendLine();
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
            
            // Add Unicode support
            content.AppendLine("\\usepackage{textgreek}");
            content.AppendLine("\\usepackage{amsfonts}");
            content.AppendLine("\\usepackage{amssymb}");
            content.AppendLine();
            
            // Get title and author information
            string title = "Artificial Intelligence in Modern Education";
            string authors = "J. Smith, K. Johnson, L. Chen";
            
            if (FormatContent.Count > 0 && FormatContent[0].GetNodeType() == "h1")
            {
                title = StripLaTeXCommands(FormatContent[0].GetContent());
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
                StripLaTeXCommands(FormatContent[1].GetContent()).Contains("Abstract"))
            {
                content.AppendLine("\\begin{abstract}");
                if (ParagraphContent.Count > 0)
                {
                    string abstractText = StripLaTeXCommands(ParagraphContent[0].GetContent());
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
                string nodeContent = StripLaTeXCommands(FormatContent[i].GetContent());
                
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
                    string paraText = StripLaTeXCommands(ParagraphContent[i - 1].GetContent());
                    content.AppendLine(paraText);
                    content.AppendLine();
                }
            }
            
            // Add math content with proper handling
            if (MathContent.Count > 0)
            {
                AppendMathContent(content, MathContent);
            }
            
            // Add lists with proper items
            if (ListContent.Count > 0)
            {
                content.AppendLine("\\section{Key Findings}");
                AppendListItems(content, ListContent);
            }
            
            // Bibliography section in IEEE format with proper handling
            if (BibliographyContent.Count > 0)
            {
                AppendBibliography(content, BibliographyContent, true);
            }
            
            // End document
            content.AppendLine("\\end{document}");
            
            return content.ToString();
        }
        
        // Helper method to properly clean LaTeX content
        private string StripLaTeXCommands(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            
            // Remove problematic command artifacts first
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\paragraph\\\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textit\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textbf\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\selectfont\b", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\fontsize\{[^}]*\}\{[^}]*\}", "");
            
            // Remove trailing double backslashes
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\\\$", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\\\(\s)", "$1");
            
            // Check if this is section content
            if (text.Contains("\\section{") || text.Contains("\\subsection{") || text.Contains("\\subsubsection{"))
            {
                int startIndex = text.IndexOf('{');
                int endIndex = text.LastIndexOf('}');
                
                if (startIndex >= 0 && endIndex > startIndex)
                {
                    return text.Substring(startIndex + 1, endIndex - startIndex - 1);
                }
            }
            
            // Handle mathematical content differently
            if (text.Contains("$") || text.Contains("\\begin{equation}"))
            {
                // Don't apply text-mode escaping to math content
                return text;
            }
            
            // Fix Greek letters - replace Unicode with LaTeX commands
            text = System.Text.RegularExpressions.Regex.Replace(text, "Σ", "\\\\Sigma");
            
            // Replace problematic ampersands correctly
            text = text.Replace("&", "\\&");
            
            // Replace non-breaking spaces
            text = text.Replace("\u00A0", "~");
            
            // Replace times symbol with proper LaTeX command
            text = text.Replace("×", "$\\times$");
            
            // Handle special LaTeX characters only for regular text
            if (!text.Contains("\\begin{") && !text.Contains("\\end{") && !text.StartsWith("\\"))
            {
                // Handle additional special characters
                var specialChars = new Dictionary<string, string>
                {
                    { "%", "\\%" },
                    { "$", "\\$" },
                    { "#", "\\#" },
                    { "_", "\\_" },
                    { "{", "\\{" },
                    { "}", "\\}" }
                };
                
                foreach (var pair in specialChars)
                {
                    // Only replace if not already escaped
                    if (!text.Contains("\\" + pair.Key))
                        text = text.Replace(pair.Key, pair.Value);
                }
            }
            
            return text;
        }
        
        // Special method for handling math content
        private string ProcessMathContent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            
            // Convert problematic command artifacts first
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\paragraph\\\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textit\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textbf\\", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\selectfont\b", "");
            
            // Fix Unicode characters with LaTeX math equivalents
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
            
            // Fix unnecessary backslashes
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\\textbackslash{}", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"_\\", "_");
            
            return text;
        }
        
        // Helper method to generate proper math content in LaTeX
        private void AppendMathContent(System.Text.StringBuilder content, List<AbstractNode> mathNodes)
        {
            if (mathNodes == null || mathNodes.Count == 0)
                return;
            
            content.AppendLine("\\section*{Mathematical Formulations}");
            
            foreach (var node in mathNodes)
            {
                string mathText = ProcessMathContent(node.GetContent());
                if (!string.IsNullOrEmpty(mathText))
                {
                    // Use proper equation environment
                    content.AppendLine("\\begin{equation}");
                    content.AppendLine(mathText);
                    content.AppendLine("\\end{equation}");
                    content.AppendLine();
                }
            }
        }

}}