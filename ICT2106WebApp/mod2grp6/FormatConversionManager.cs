using System;
using System.Collections.Generic;
using ICT2106WebApp.mod2grp6.Format;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.mod2grp6.Text;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6
{
    
    /// Manager class responsible for converting different types of content
    /// Implementation based on class diagram
    public class FormatConversionManager : IFormatter
    {
        // Private member to store content as defined in the class diagram
        private List<AbstractNode> content;

        /// Constructor for FormatConversionManager
        public FormatConversionManager()
        {
            content = new List<AbstractNode>();
        }

        
        /// Converts format-related content
        
        /// <param name="content">Format content to convert</param>
        /// <returns>True if conversion is successful</returns>
        public bool convertFormat(Format.Format content)
        {
            try
            {
                // Create a FormatManager instance to handle format operations
                FormatManager formatManager = new FormatManager();
                
                // Start the formatting process
                bool result = formatManager.StartFormatting(content);
                
                if (result)
                {
                    // Apply necessary formatting operations
                    formatManager.FormatHeadings();
                    formatManager.FormatParagraphs();
                    
                    // Get the formatted content and add it to the content list
                    List<AbstractNode_SDM> formattedContent = formatManager.ApplyBaseFormatting();
                    
                    // Convert AbstractNode_SDM to AbstractNode and add to content list
                    foreach (var node in formattedContent)
                    {
                        AbstractNode abstractNode = ConvertToAbstractNode(node);
                        this.content.Add(abstractNode);
                    }
                    
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                Console.WriteLine($"Error in convertFormat: {ex.Message}");
                return false;
            }
        }

        
        /// Converts text-related content
        
        /// <param name="content">Text content to convert</param>
        /// <returns>True if conversion is successful</returns>
        public bool convertText(Text.Text content)
        {
            try
            {
                // Create a TextManager instance to handle text operations
                TextManager textManager = new TextManager();
                
                // Start the text formatting process
                bool result = textManager.StartTextFormatting(content);
                
                if (result)
                {
                    // Apply necessary text formatting operations
                    textManager.FormatFonts();
                    textManager.FormatStyles();
                    textManager.FormatColors();
                    textManager.FormatAlignment();
                    textManager.FormatLineSpacing();
                    
                    // Get the formatted content and add it to the content list
                    List<AbstractNode_SDM> formattedContent = textManager.ApplyTextFormatting();
                    
                    // Convert AbstractNode_SDM to AbstractNode and add to content list
                    foreach (var node in formattedContent)
                    {
                        AbstractNode abstractNode = ConvertToAbstractNode(node);
                        this.content.Add(abstractNode);
                    }
                    
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                Console.WriteLine($"Error in convertText: {ex.Message}");
                return false;
            }
        }

        
        /// Converts layout-related content
        
        /// <param name="content">Layout content to convert</param>
        /// <returns>True if conversion is successful</returns>
        public bool convertLayout(Layout.Layout content)
        {
            try
            {
                // Create a LayoutManager instance to handle layout operations
                LayoutManager layoutManager = new LayoutManager();
                
                // Start the layout formatting process
                bool result = layoutManager.StartLayoutFormatting(content);
                
                if (result)
                {
                    // Apply necessary layout formatting operations
                    layoutManager.FormatHeaders();
                    layoutManager.FormatFooters();
                    layoutManager.FormatMargins();
                    layoutManager.FormatOrientation();
                    layoutManager.FormatPageSize();
                    layoutManager.FormatColumnNum();
                    layoutManager.FormatColumnSpacing();
                    
                    // Get the formatted content and add it to the content list
                    List<AbstractNode_SDM> formattedContent = layoutManager.ApplyLayoutFormatting();
                    
                    // Convert AbstractNode_SDM to AbstractNode and add to content list
                    foreach (var node in formattedContent)
                    {
                        AbstractNode abstractNode = ConvertToAbstractNode(node);
                        this.content.Add(abstractNode);
                    }
                    
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                Console.WriteLine($"Error in convertLayout: {ex.Message}");
                return false;
            }
        }

        
        /// Gets the content after conversion
        
        /// <returns>List of AbstractNode representing the converted content</returns>
        public List<AbstractNode> getContent()
        {
            return content;
        }
        
        
        /// Helper method to convert AbstractNode_SDM to AbstractNode
        
        /// <param name="node">AbstractNode_SDM to convert</param>
        /// <returns>Converted AbstractNode</returns>
        private AbstractNode ConvertToAbstractNode(AbstractNode_SDM node)
        {
            // In a real implementation, this would convert all properties
            // For this example, we'll create a simple conversion
            AbstractNode abstractNode = new AbstractNode
            {
                NodeId = node.getNodeId(),
                NodeType = node.getNodeType(),
                Content = node.getContent(),
                // Convert styling and other properties as needed
            };
            
            return abstractNode;
        }
    }

    
    /// AbstractNode class for use in this example
    /// This would be defined elsewhere in a real implementation
    
    public class AbstractNode
    {
        public int NodeId { get; set; }
        public string NodeType { get; set; }
        public string Content { get; set; }
        public bool Converted { get; set; }
        // Additional properties as needed
    }
}