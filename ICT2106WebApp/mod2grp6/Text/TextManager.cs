using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Text
{
    /// <summary>
    /// TextManager class responsible for handling text formatting operations.
    /// Implements the IFormatText interface.
    /// </summary>
    public class TextManager : IFormatText
    {
        // Private member to store text content
        private List<AbstractNode> content;
        private bool isInitialized;

        /// Constructor for TextManager
        public TextManager()
        {
            content = new List<AbstractNode>();
            isInitialized = false;
        }

        /// Starts the text formatting process by initializing the content
        /// <param name="content">The document content to format</param>
        /// returns true if initialization was successful, false otherwise
        public bool StartTextFormatting(List<AbstractNode> content)
        {
            try
            {
                if (content == null || content.Count == 0)
                {
                    Console.WriteLine("Error: Content is null or empty");
                    return false;
                }

                this.content = content;
                isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StartTextFormatting: {ex.Message}");
                return false;
            }
        }

        /// Formats the fonts in the document
        /// returns true if formatting was successful, false otherwise
        public bool FormatFonts()
        {
            if (!isInitialized)
            {
                Console.WriteLine("Error: TextManager not initialized");
                return false;
            }

            try
            {
                foreach (AbstractNode node in content)
                {
                    ApplyFontFormatting(node);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatFonts: {ex.Message}");
                return false;
            }
        }

        // Helper method to recursively apply font formatting to nodes
        private void ApplyFontFormatting(AbstractNode node)
        {
            // Apply font formatting to the current node
            List<Dictionary<string, object>> stylings = node.GetStyling();
            
            foreach (Dictionary<string, object> styling in stylings)
            {
                if (styling.ContainsKey("fontFamily"))
                {
                    string fontFamily = styling["fontFamily"].ToString();
                    string latexFont = ConvertFontToLatex(fontFamily);
                    
                    // Update the node's content with LaTeX font commands
                    string originalContent = node.GetContent();
                    string newContent = ApplyLatexFontCommand(originalContent, latexFont);
                    node.SetContent(newContent);
                }
            }
            
            // If it's a composite node, apply formatting to children
            if (node is CompositeNode compositeNode)
            {
                foreach (AbstractNode child in compositeNode.GetChildren())
                {
                    ApplyFontFormatting(child);
                }
            }
        }

        /// Formats text styles (bold, italic, underline) in the document
        /// returns true if formatting was successful, false otherwise
        public bool FormatStyles()
        {
            if (!isInitialized)
            {
                Console.WriteLine("Error: TextManager not initialized");
                return false;
            }

            try
            {
                foreach (AbstractNode node in content)
                {
                    ApplyStyleFormatting(node);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatStyles: {ex.Message}");
                return false;
            }
        }
        
        // Helper method to recursively apply style formatting to nodes
        private void ApplyStyleFormatting(AbstractNode node)
        {
            // Apply style formatting to the current node
            List<Dictionary<string, object>> stylings = node.GetStyling();
            
            foreach (Dictionary<string, object> styling in stylings)
            {
                bool isBold = styling.ContainsKey("bold") && (bool)styling["bold"];
                bool isItalic = styling.ContainsKey("italic") && (bool)styling["italic"];
                bool isUnderline = styling.ContainsKey("underline") && (bool)styling["underline"];
                
                // Apply LaTeX style commands
                string originalContent = node.GetContent();
                string newContent = originalContent;

                if (isBold)
                {
                    newContent = $"\\textbf{{{newContent}}}";
                }

                if (isItalic)
                {
                    newContent = $"\\textit{{{newContent}}}";
                }

                if (isUnderline)
                {
                    newContent = $"\\underline{{{newContent}}}";
                }

                node.SetContent(newContent);
            }
            
            // If it's a composite node, apply formatting to children
            if (node is CompositeNode compositeNode)
            {
                foreach (AbstractNode child in compositeNode.GetChildren())
                {
                    ApplyStyleFormatting(child);
                }
            }
        }
        
        /// Formats colors in the document
        /// returns true if formatting was successful, false otherwise
        public bool FormatColors()
        {
            if (!isInitialized)
            {
                Console.WriteLine("Error: TextManager not initialized");
                return false;
            }

            try
            {
                foreach (AbstractNode node in content)
                {
                    ApplyColorFormatting(node);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatColors: {ex.Message}");
                return false;
            }
        }
        
        // Helper method to recursively apply color formatting to nodes
        private void ApplyColorFormatting(AbstractNode node)
        {
            // Apply color formatting to the current node
            List<Dictionary<string, object>> stylings = node.GetStyling();
            
            foreach (Dictionary<string, object> styling in stylings)
            {
                if (styling.ContainsKey("color"))
                {
                    string color = styling["color"].ToString();
                    string htmlColorCode = ConvertColorToLatex(color);
                    
                    // Update the node's content with LaTeX color commands using HTML format
                    string originalContent = node.GetContent();
                    string newContent = $"\\textcolor[HTML]{{{htmlColorCode}}}{{{originalContent}}}";
                    node.SetContent(newContent);
                }
            }
            
            // If it's a composite node, apply formatting to children
            if (node is CompositeNode compositeNode)
            {
                foreach (AbstractNode child in compositeNode.GetChildren())
                {
                    ApplyColorFormatting(child);
                }
            }
        }

        /// Formats line spacing in the document
        /// returns true if formatting was successful, false otherwise
        public bool FormatLineSpacing()
        {
            if (!isInitialized)
            {
                Console.WriteLine("Error: TextManager not initialized");
                return false;
            }

            try
            {
                foreach (AbstractNode node in content)
                {
                    ApplyLineSpacingFormatting(node);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatLineSpacing: {ex.Message}");
                return false;
            }
        }
        
        // Helper method to recursively apply line spacing formatting to nodes
        private void ApplyLineSpacingFormatting(AbstractNode node)
        {
            // Apply line spacing formatting to the current node
            List<Dictionary<string, object>> stylings = node.GetStyling();
            
            foreach (Dictionary<string, object> styling in stylings)
            {
                if (styling.ContainsKey("lineSpacing"))
                {
                    // Convert the line spacing to a float
                    float lineSpacing = 0;
                    if (styling["lineSpacing"] is float)
                    {
                        lineSpacing = (float)styling["lineSpacing"];
                    }
                    else if (styling["lineSpacing"] is int)
                    {
                        lineSpacing = (int)styling["lineSpacing"];
                    }
                    else if (styling["lineSpacing"] is string)
                    {
                        float.TryParse(styling["lineSpacing"].ToString(), out lineSpacing);
                    }
                    
                    // Apply line spacing to the node based on its type
                    if (node.GetNodeType().Equals("paragraph", StringComparison.OrdinalIgnoreCase))
                    {
                        // For paragraphs, apply line spacing using LaTeX commands
                        string originalContent = node.GetContent();
                        
                        // Convert the line spacing value to a LaTeX-compatible format
                        // LaTeX typically uses either direct multipliers (like 1.5) or specific measurements
                        string latexSpacing;
                        if (lineSpacing <= 0)
                        {
                            latexSpacing = "1.0"; // Default single spacing
                        }
                        else if (lineSpacing <= 1.0)
                        {
                            latexSpacing = lineSpacing.ToString("0.0");
                        }
                        else if (lineSpacing <= 2.0)
                        {
                            latexSpacing = lineSpacing.ToString("0.0");
                        }
                        else
                        {
                            // Cap at a reasonable maximum for very large values
                            latexSpacing = "2.0";
                        }
                        
                        // Apply the line spacing command to the content
                        // Using the \linespread command which affects the baselineskip
                        string newContent = $"{{\\linespread{{{latexSpacing}}}\\selectfont {originalContent}}}";
                        
                        node.SetContent(newContent);
                    }
                    else if (node.GetNodeType().Equals("section", StringComparison.OrdinalIgnoreCase) || 
                            node.GetNodeType().Equals("document", StringComparison.OrdinalIgnoreCase))
                    {
                        // For section or document level nodes, we might need a different approach
                        // Add the line spacing command at the beginning of the content
                        string originalContent = node.GetContent();
                        
                        // For these container nodes, we typically want the command to affect everything inside
                        string latexSpacing = lineSpacing.ToString("0.0");
                        string spacingCommand = $"\\linespread{{{latexSpacing}}}\\selectfont";
                        
                        string newContent = spacingCommand + "\n" + originalContent;
                        node.SetContent(newContent);
                    }
                }
            }
            
            // If it's a composite node, apply formatting to children
            if (node is CompositeNode compositeNode)
            {
                foreach (AbstractNode child in compositeNode.GetChildren())
                {
                    ApplyLineSpacingFormatting(child);
                }
            }
        }

        /// Formats alignment in the document
        /// <returns>True if formatting was successful, false otherwise</returns>
        public bool FormatAlignment()
        {
            if (!isInitialized)
            {
                Console.WriteLine("Error: TextManager not initialized");
                return false;
            }

            try
            {
                foreach (AbstractNode node in content)
                {
                    ApplyAlignmentFormatting(node);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatAlignment: {ex.Message}");
                return false;
            }
        }
        
        // Helper method to recursively apply alignment formatting to nodes
        private void ApplyAlignmentFormatting(AbstractNode node)
        {
            // Apply alignment formatting to the current node
            List<Dictionary<string, object>> stylings = node.GetStyling();
            
            foreach (Dictionary<string, object> styling in stylings)
            {
                if (styling.ContainsKey("alignment"))
                {
                    string alignment = styling["alignment"].ToString().ToLower();
                    string latexAlignment = GetLatexAlignmentEnvironment(alignment);
                    
                    // Update the node's content with LaTeX alignment environment
                    string originalContent = node.GetContent();
                    string newContent = $"\\begin{{{latexAlignment}}}\n{originalContent}\n\\end{{{latexAlignment}}}";
                    node.SetContent(newContent);
                }
            }
            
            // If it's a composite node, apply formatting to children
            if (node is CompositeNode compositeNode)
            {
                foreach (AbstractNode child in compositeNode.GetChildren())
                {
                    ApplyAlignmentFormatting(child);
                }
            }
        }

        /// Applies all text formatting and returns the formatted content
        /// returns list of formatted nodes
        public List<AbstractNode> ApplyTextFormatting()
        {
            if (!isInitialized)
            {
                Console.WriteLine("Error: TextManager not initialized");
                return null;
            }

            try
            {
                // Mark all nodes as converted
                foreach (AbstractNode node in content)
                {
                    MarkNodeAsConverted(node);
                }
                
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ApplyTextFormatting: {ex.Message}");
                return null;
            }
        }
        
        // Helper method to recursively mark nodes as converted
        private void MarkNodeAsConverted(AbstractNode node)
        {
            // Mark the current node as converted
            node.SetConverted(true);
            
            // If it's a composite node, mark all children as converted
            if (node is CompositeNode compositeNode)
            {
                foreach (AbstractNode child in compositeNode.GetChildren())
                {
                    MarkNodeAsConverted(child);
                }
            }
        }
        
        /// Helper method to convert font names to LaTeX font commands
        /// <param name="fontFamily">The font family name</param>
        /// returns the corresponding LaTeX font command
        private string ConvertFontToLatex(string fontFamily)
        {
            // Map common font families to LaTeX font commands
            switch (fontFamily.ToLower())
            {
                case "times new roman":
                    return "rmfamily";
                case "arial":
                case "helvetica":
                    return "sffamily";
                case "calibri":
                    return "sffamily"; 
                default:
                    return "rmfamily"; 
            }
        }

        /// Helper method to apply LaTeX font commands
        /// <param name="content">The original content</param>
        /// <param name="fontCommand">The LaTeX font command</param>
        /// returns the content with LaTeX font command applied
        private string ApplyLatexFontCommand(string content, string fontCommand)
        {
            return $"{{\\{fontCommand} {content}}}";
        }

        /// Helper method to convert colors to LaTeX color commands
        /// <param name="color">The color name or hex value</param>
        /// returns the corresponding LaTeX color name
        private string ConvertColorToLatex(string color)
        {
            // For hex colors, return in HTML format
            if (color.StartsWith("#") && (color.Length == 7 || color.Length == 9))
            {
                // Remove the # prefix and return
                return color.Substring(1).ToUpper();
            }
            
            // For named colors, convert to hex format
            switch (color.ToLower())
            {
                case "black":
                    return "000000";
                case "red":
                    return "FF0000";
                case "green":
                    return "008000";
                case "blue":
                    return "0000FF";
                case "yellow":
                    return "FFFF00";
                case "magenta":
                    return "FF00FF";
                case "cyan":
                    return "00FFFF";
                case "white":
                    return "FFFFFF";
                default:
                    return "000000"; // Default to black
            }
        }

        /// Helper method to get LaTeX alignment environment
        /// <param name="alignment">The alignment direction</param>
        /// returns the corresponding LaTeX alignment environment</returns>
        private string GetLatexAlignmentEnvironment(string alignment)
        {
            switch (alignment)
            {
                case "left":
                    return "flushleft";
                case "center":
                    return "center";
                case "right":
                    return "flushright";
                case "justify":
                    return "justify";
                default:
                    return "flushleft"; // Default to left alignment
            }
        }
    }
}