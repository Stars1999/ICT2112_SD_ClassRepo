using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using ICT2106WebApp.mod2grp6;
using System.Text;
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
            //OriginalLaTeXResult = ((DocumentManager)_documentManager).toLaTeXWithTemplate("template", true);
            
            // Step 3: Convert to template-applied LaTeX format
            //TemplateResult = ((DocumentManager)_documentManager).convertToLatexTemplateWithTemplate("template", SelectedTemplateId, true);
            
            // Step 4: Get formatted content for display
            GenerateDisplayContent();
            
            return Page();
        }
        
        private void LoadDocumentContent()
        {
            // Load all content types from the DocumentManager using TemplateSample data
            MetadataContent = _documentManager.GetContentForTemplateByType("metadata");
            FormatContent = _documentManager.GetContentForTemplateByType("format");
            TextContent = _documentManager.GetContentForTemplateByType("text");
            ParagraphContent = _documentManager.GetContentForTemplateByType("paragraph");
            LayoutContent = _documentManager.GetContentForTemplateByType("layout");
            MathContent = _documentManager.GetContentForTemplateByType("math");
            ListContent = _documentManager.GetContentForTemplateByType("lists");
            ImageContent = _documentManager.GetContentForTemplateByType("images");
            
            IsDocumentLoaded = true;
        }
        
        private void GenerateDisplayContent()
        {
            // Generate Original LaTeX Content
            OriginalLaTeXContent = GenerateOriginalLaTeXContent();
            
            // Generate Template-Applied Content
            TemplateAppliedContent = GenerateTemplateAppliedContent();
        }

        private string GenerateEditorialTemplate(string title, string authors)
        {
            var content = new System.Text.StringBuilder();

            // Editorial Template Specific Packages
            content.AppendLine("\\documentclass[twocolumn]{article}");
            content.AppendLine("\\usepackage[utf8]{inputenc}");
            content.AppendLine("\\usepackage{graphicx}");
            content.AppendLine("\\usepackage{multicol}");
            content.AppendLine("\\usepackage{geometry}");
            content.AppendLine("\\geometry{margin=1in}");
            content.AppendLine("\\usepackage{fancyhdr}");
            content.AppendLine("\\pagestyle{fancy}");
            content.AppendLine("\\fancyhf{}");
            content.AppendLine("\\renewcommand{\\headrulewidth}{0pt}");
            content.AppendLine("\\lfoot{Editorial Perspective}");
            content.AppendLine("\\rfoot{Page \\thepage}");
            content.AppendLine();

            // Title and Author Configuration
            content.AppendLine($"\\title{{{title}}}");
            content.AppendLine($"\\author{{{CleanTextForLaTeX(authors)}}}");
            content.AppendLine("\\date{}"); // Remove date
            content.AppendLine();

            content.AppendLine("\\begin{document}");

            // Switch to single column for the title and abstract
            content.AppendLine("\\onecolumn");
            content.AppendLine("\\maketitle");

            // Abstract Section
            if (ParagraphContent.Count > 0)
            {
                content.AppendLine("\\begin{abstract}");
                string abstractText = CleanTextForLaTeX(ParagraphContent[0].GetContent());
                content.AppendLine(abstractText);
                content.AppendLine("\\end{abstract}");
                content.AppendLine();
            }

            // Switch back to two-column layout for the main content
            content.AppendLine("\\twocolumn");
            content.AppendLine("\\begin{multicols}{2}");

            // Iterate through paragraphs for content
            for (int i = 1; i < ParagraphContent.Count; i++)
            {
                var paragraph = ParagraphContent[i];
                string paraText = CleanTextForLaTeX(paragraph.GetContent());
                paraText = ApplyNodeStyling(paraText, paragraph);

                // Section headers from FormatContent
                if (i < FormatContent.Count && FormatContent[i].GetNodeType() == "h2")
                {
                    content.AppendLine($"\\section*{{{CleanTextForLaTeX(FormatContent[i].GetContent())}}}");
                }

                content.AppendLine(paraText);
                content.AppendLine("\\vspace{0.5em}");
            }

            // Mathematical Content
            if (MathContent.Count > 0)
            {
                content.AppendLine("\\section*{Mathematical Insights}");
                foreach (var node in MathContent)
                {
                    string mathText = CleanTextForLaTeX(node.GetContent(), "math");
                    content.AppendLine("\\begin{equation}");
                    content.AppendLine(EnsureBalancedBraces(mathText));
                    content.AppendLine("\\end{equation}");
                }
            }

            // List Content
            if (ListContent.Count > 0)
            {
                content.AppendLine("\\section*{Key Points}");
                
                var bulletedLists = ListContent.Where(n => n.GetNodeType().Contains("bulleted")).ToList();
                var numberedLists = ListContent.Where(n => n.GetNodeType().Contains("numbered")).ToList();

                if (bulletedLists.Any())
                {
                    content.AppendLine("\\begin{itemize}");
                    foreach (var item in bulletedLists)
                    {
                        content.AppendLine($"  \\item {CleanTextForLaTeX(item.GetContent())}");
                    }
                    content.AppendLine("\\end{itemize}");
                }

                if (numberedLists.Any())
                {
                    content.AppendLine("\\begin{enumerate}");
                    foreach (var item in numberedLists)
                    {
                        content.AppendLine($"  \\item {CleanTextForLaTeX(item.GetContent())}");
                    }
                    content.AppendLine("\\end{enumerate}");
                }
            }

            // Bibliography
            if (BibliographyContent.Count > 0)
            {
                content.AppendLine("\\section*{References}");
                content.AppendLine("\\begin{thebibliography}{99}");
                foreach (var node in BibliographyContent)
                {
                    string bibText = CleanTextForLaTeX(node.GetContent());
                    content.AppendLine($"  \\bibitem{{ref{node.GetNodeId()}}} {bibText}");
                }
                content.AppendLine("\\end{thebibliography}");
            }

            content.AppendLine("\\end{multicols}");
            content.AppendLine("\\end{document}");

            return content.ToString();
        }

        private string GenerateTemplateAppliedContent()
        {
            string title = "Artificial Intelligence in Modern Education";
            string authors = "Unknown Author";

            // Update title and authors from FormatContent and MetadataContent
            if (FormatContent.Count > 0 && FormatContent[0].GetNodeType() == "h1")
            {
                title = CleanTextForLaTeX(FormatContent[0].GetContent());
            }

            if (MetadataContent.Count > 0)
            {
                var metadata = MetadataContent[0].GetStyling();
                if (metadata != null)
                {
                    foreach (var style in metadata)
                    {
                        if (style.ContainsKey("author") && style["author"] != null)
                        {
                            authors = CleanTextForLaTeX(style["author"].ToString());
                            break;
                        }
                    }
                }
            }

            // Template selection logic
            if (SelectedTemplateId == "editorial")
            {
                return GenerateEditorialTemplate(title, authors);
            }
            else
            {
                // IEEE Template (existing logic)
                var content = new System.Text.StringBuilder();

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
                content.AppendLine($"\\title{{{title}}}");
                content.AppendLine($"\\author{{{authors}}}");
                content.AppendLine();
                content.AppendLine("\\begin{document}");
                content.AppendLine("\\maketitle");
                content.AppendLine();

                // Abstract
                if (FormatContent.Count > 1 && FormatContent[1].GetNodeType() == "h2" && 
                    CleanTextForLaTeX(FormatContent[1].GetContent()).Contains("Abstract"))
                {
                    content.AppendLine("\\begin{abstract}");
                    if (ParagraphContent.Count > 0)
                    {
                        string abstractText = ParagraphContent[0].GetContent();
                        abstractText = CleanTextForLaTeX(abstractText);

                        bool isItalic = false;
                        foreach (var style in ParagraphContent[0].GetStyling())
                        {
                            if (style.ContainsKey("Italic") && (bool)style["Italic"])
                            {
                                isItalic = true;
                                break;
                            }
                        }

                        if (isItalic)
                            abstractText = $"\\textit{{{abstractText}}}";

                        content.AppendLine(abstractText);
                    }
                    content.AppendLine("\\end{abstract}");
                    content.AppendLine("\\IEEEpeerreviewmaketitle");
                    content.AppendLine();
                }

                // Main content - include ALL paragraphs with CJK support
                content.AppendLine("\\section*{Content}");
                foreach (var paragraph in ParagraphContent.Skip(1))
                {
                    string paraText = CleanTextForLaTeX(paragraph.GetContent());
                    paraText = ApplyNodeStyling(paraText, paragraph);
                    content.AppendLine(paraText);
                    content.AppendLine();
                }

                // Sections and content
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

                    if (i - 1 < ParagraphContent.Count)
                    {
                        var paragraph = ParagraphContent[i - 1];
                        string paraText = CleanTextForLaTeX(paragraph.GetContent());
                        paraText = ApplyNodeStyling(paraText, paragraph);
                        content.AppendLine(paraText);
                        content.AppendLine();
                    }
                }

                // Math
                if (MathContent.Count > 0)
                {
                    content.AppendLine("\\section*{Mathematical Formulations}");
                    foreach (var node in MathContent)
                    {
                        string mathText = CleanTextForLaTeX(node.GetContent(), "math");
                        if (!string.IsNullOrEmpty(mathText))
                        {
                            content.AppendLine("\\begin{equation}");
                            content.AppendLine(EnsureBalancedBraces(mathText));
                            content.AppendLine("\\end{equation}");
                            content.AppendLine();
                        }
                    }
                }

                // Lists
                if (ListContent.Count > 0)
                {
                    content.AppendLine("\\section{Key Findings}");

                    var bulletedLists = new List<AbstractNode>();
                    var numberedLists = new List<AbstractNode>();

                    foreach (var node in ListContent)
                    {
                        if (node.GetNodeType().Contains("numbered"))
                            numberedLists.Add(node);
                        else
                            bulletedLists.Add(node);
                    }

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

                // Bibliography
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

                content.AppendLine("\\end{document}");

                return content.ToString();
            }
        }

        private string GenerateOriginalLaTeXContent()
        {
            var content = new System.Text.StringBuilder();

            // Base LaTeX setup
            content.AppendLine("\\documentclass{article}");
            content.AppendLine("\\usepackage[utf8]{inputenc}");
            content.AppendLine("\\usepackage{graphicx}");
            content.AppendLine("\\usepackage{amsmath}");
            content.AppendLine("\\usepackage{xcolor}");
            content.AppendLine("\\usepackage{hyperref}");
            content.AppendLine("\\usepackage{amssymb}");
            content.AppendLine("\\usepackage{textcomp}");
            content.AppendLine();

            // Metadata
            string title = "Untitled Document";
            string authors = "Unknown Author";

            if (FormatContent.Count > 0 && FormatContent[0].GetNodeType() == "h1")
            {
                title = CleanTextForLaTeX(FormatContent[0].GetContent());
            }

            if (MetadataContent.Count > 0)
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
            content.AppendLine("\\date{}");
            content.AppendLine("\\begin{document}");
            content.AppendLine("\\maketitle");
            content.AppendLine();

            // Abstract
            if (ParagraphContent.Count > 0)
            {
                content.AppendLine("\\begin{abstract}");
                string abstractText = CleanTextForLaTeX(ParagraphContent[0].GetContent());
                content.AppendLine(abstractText);
                content.AppendLine("\\end{abstract}");
                content.AppendLine();
            }

            // Main content - include ALL paragraphs
            content.AppendLine("\\section*{Content}");
            foreach (var paragraph in ParagraphContent.Skip(1)) // Skip abstract paragraph
            {
                string paraText = CleanTextForLaTeX(paragraph.GetContent());
                paraText = ApplyNodeStyling(paraText, paragraph);
                content.AppendLine(paraText);
                content.AppendLine(); // Add space between paragraphs
            }

            // Main sections
            for (int i = 1; i < FormatContent.Count; i++)
            {
                string nodeType = FormatContent[i].GetNodeType();
                string nodeText = CleanTextForLaTeX(FormatContent[i].GetContent());

                if (nodeType == "h2")
                    content.AppendLine($"\\section*{{{nodeText}}}");
                else if (nodeType == "h3")
                    content.AppendLine($"\\subsection*{{{nodeText}}}");

                if (i - 1 < ParagraphContent.Count)
                {
                    string paraText = CleanTextForLaTeX(ParagraphContent[i - 1].GetContent());
                    paraText = ApplyNodeStyling(paraText, ParagraphContent[i - 1]);
                    content.AppendLine(paraText);
                    content.AppendLine();
                }
            }

            // Math Section
            if (MathContent.Count > 0)
            {
                content.AppendLine("\\section*{Mathematical Content}");
                foreach (var math in MathContent)
                {
                    content.AppendLine("\\begin{equation}");
                    content.AppendLine(EnsureBalancedBraces(CleanTextForLaTeX(math.GetContent(), "math")));
                    content.AppendLine("\\end{equation}");
                    content.AppendLine();
                }
            }

            // Lists
            if (ListContent.Count > 0)
            {
                content.AppendLine("\\section*{Lists}");

                var bulleted = ListContent.Where(x => x.GetNodeType().Contains("bulleted")).ToList();
                var numbered = ListContent.Where(x => x.GetNodeType().Contains("numbered")).ToList();

                if (bulleted.Count > 0)
                {
                    content.AppendLine("\\begin{itemize}");
                    foreach (var item in bulleted)
                    {
                        content.AppendLine($"  \\item {CleanTextForLaTeX(item.GetContent())}");
                    }
                    content.AppendLine("\\end{itemize}");
                }

                if (numbered.Count > 0)
                {
                    content.AppendLine("\\begin{enumerate}");
                    foreach (var item in numbered)
                    {
                        content.AppendLine($"  \\item {CleanTextForLaTeX(item.GetContent())}");
                    }
                    content.AppendLine("\\end{enumerate}");
                }

                content.AppendLine();
            }

            // Bibliography
            if (BibliographyContent.Count > 0)
            {
                content.AppendLine("\\section*{References}");
                content.AppendLine("\\begin{thebibliography}{99}");
                foreach (var bib in BibliographyContent)
                {
                    content.AppendLine($"  \\bibitem{{ref{bib.GetNodeId()}}} {CleanTextForLaTeX(bib.GetContent())}");
                }
                content.AppendLine("\\end{thebibliography}");
            }

            content.AppendLine("\\end{document}");

            return content.ToString();
        }


        // Helper methods
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

        private string CleanTextForLaTeX(string text, string contentType = "text")
        {
            if (string.IsNullOrEmpty(text))
                return "";

            // Special handling for math content
            if (contentType == "math")
            {
                var mathReplacements = new Dictionary<string, string>
                {
                    {"×", "\\times "}, {"≥", "\\geq "}, {"≠", "\\neq "},
                    {"±", "\\pm "}, {"∞", "\\infty "}, {"∴", "\\therefore "},
                    {"∃", "\\exists "}, {"∀", "\\forall "}, {"→", "\\rightarrow "},
                    {"π", "\\pi "}, {"α", "\\alpha "}, {"√", "\\sqrt{"},
                    {"∫", "\\int "}, {"∧", "\\wedge "}, {"∨", "\\vee "}
                };

                foreach (var (unicode, latex) in mathReplacements)
                {
                    text = text.Replace(unicode, latex);
                }
                return EnsureBalancedBraces(text);
            }

            // General text processing
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                switch (c)
                {
                    // Basic LaTeX special characters
                    case '&': sb.Append("\\&"); break;
                    case '%': sb.Append("\\%"); break;
                    case '$': sb.Append("\\$"); break;
                    case '#': sb.Append("\\#"); break;
                    case '_': sb.Append("\\_"); break;
                    case '{': sb.Append("\\{"); break;
                    case '}': sb.Append("\\}"); break;
                    case '~': sb.Append("\\textasciitilde "); break;
                    case '^': sb.Append("\\textasciicircum "); break;
                    case '\\': sb.Append("\\textbackslash "); break;
                    case '<': sb.Append("\\textless "); break;
                    case '>': sb.Append("\\textgreater "); break;
                    case '|': sb.Append("\\textbar "); break;
                    
                    default:
                        // Only keep ASCII characters
                        if (c <= 0x7F)
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }
    }
}