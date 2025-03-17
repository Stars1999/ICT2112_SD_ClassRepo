using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Text
{
    /// TextManager class responsible for handling text formatting operations.
    /// Implements the IFormatText interface.
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
        /// returns True if initialization was successful, false otherwise
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
        /// returns True if formatting was successful, false otherwise
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
                    // Find font information in styling
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
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatFonts: {ex.Message}");
                return false;
            }
        }

        /// Formats text styles (bold, italic, underline) in the document
        /// returns True if formatting was successful, false otherwise
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
                    // Find style information in styling
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
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatStyles: {ex.Message}");
                return false;
            }
        }

        /// Formats colors in the document
        /// returns True if formatting was successful, false otherwise
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
                    // Find color information in styling
                    List<Dictionary<string, object>> stylings = node.GetStyling();
                    
                    foreach (Dictionary<string, object> styling in stylings)
                    {
                        if (styling.ContainsKey("color"))
                        {
                            string color = styling["color"].ToString();
                            string latexColor = ConvertColorToLatex(color);
                            
                            // Update the node's content with LaTeX color commands
                            string originalContent = node.GetContent();
                            string newContent = $"\\textcolor{{{latexColor}}}{{{originalContent}}}";
                            node.SetContent(newContent);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatColors: {ex.Message}");
                return false;
            }
        }

        /// Formats line spacing in the document
        /// returns True if formatting was successful, false otherwise
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
                    // Find line spacing information in styling
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
                            
                            // Apply line spacing to the node's content
                            // This is a composite operation that may need to be applied at a higher level
                            // depending on the node type (paragraph, section, etc.)
                            if (node.GetNodeType().Equals("paragraph", StringComparison.OrdinalIgnoreCase))
                            {
                                // Set a flag or property indicating this node needs line spacing adjustment
                                // In an actual implementation, you might add a custom property to the node
                                // or maintain a separate dictionary to track this information
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatLineSpacing: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Formats alignment in the document
        /// </summary>
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
                    // Find alignment information in styling
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
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FormatAlignment: {ex.Message}");
                return false;
            }
        }

        /// Applies all text formatting and returns the formatted content
        /// returns the list of formatted nodes
        public List<AbstractNode> ApplyTextFormatting()
        {
            if (!isInitialized)
            {
                Console.WriteLine("Error: TextManager not initialized");
                return null;
            }

            try
            {
                // Apply formatting to composite nodes recursively
                ProcessCompositeNodes();
                
                // Mark all nodes as converted
                foreach (AbstractNode node in content)
                {
                    node.SetConverted(true);
                }
                
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ApplyTextFormatting: {ex.Message}");
                return null;
            }
        }

        /// Helper method to process composite nodes recursively
        private void ProcessCompositeNodes()
        {
            for (int i = 0; i < content.Count; i++)
            {
                if (content[i] is CompositeNode compositeNode)
                {
                    List<AbstractNode> children = compositeNode.GetChildren();
                    
                    // Create a new TextManager to process the children
                    TextManager childTextManager = new TextManager();
                    if (childTextManager.StartTextFormatting(children))
                    {
                        childTextManager.FormatFonts();
                        childTextManager.FormatStyles();
                        childTextManager.FormatColors();
                        childTextManager.FormatLineSpacing();
                        childTextManager.FormatAlignment();
                        List<AbstractNode> processedChildren = childTextManager.ApplyTextFormatting();
                        
                        // Update the composite node with processed children
                        // This would require an additional method in CompositeNode to set children
                        // For now, we'll assume the children are updated in place
                    }
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
                case "courier":
                case "courier new":
                    return "ttfamily";
                default:
                    return "rmfamily"; // Default to Roman font
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
            // Map common color names to LaTeX color names
            switch (color.ToLower())
            {
                case "black":
                    return "black";
                case "red":
                    return "red";
                case "green":
                    return "green";
                case "blue":
                    return "blue";
                case "yellow":
                    return "yellow";
                case "magenta":
                    return "magenta";
                case "cyan":
                    return "cyan";
                case "white":
                    return "white";
                default:
                    // If it's a hex color, convert to RGB values for LaTeX
                    if (color.StartsWith("#") && (color.Length == 7 || color.Length == 9))
                    {
                        try
                        {
                            // Parse hex color to RGB
                            int r = Convert.ToInt32(color.Substring(1, 2), 16);
                            int g = Convert.ToInt32(color.Substring(3, 2), 16);
                            int b = Convert.ToInt32(color.Substring(5, 2), 16);
                            
                            // Normalize RGB values to [0,1] for LaTeX
                            double rNorm = r / 255.0;
                            double gNorm = g / 255.0;
                            double bNorm = b / 255.0;
                            
                            return $"[RGB]{{{r},{g},{b}}}";
                        }
                        catch
                        {
                            return "black"; // Default to black on error
                        }
                    }
                    return "black"; // Default to black
            }
        }

        /// Helper method to get LaTeX alignment environment
        /// <param name="alignment">The alignment direction</param>
        /// returns the corresponding LaTeX alignment environment
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