using System;
using System.Collections.Generic;
using ICT2106WebApp.mod2grp6.Format;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.mod2grp6.Text;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6
{
    
    // Manager class responsible for converting different types of content
    // Implementation based on class diagram
    public class FormatConversionManager : IFormatter
    {
        // Private member to store content as defined in the class diagram
        private List<AbstractNode> content;
        private LayoutManager layoutManager;
        private FormatManager formatManager;
        private TextManager textManager;


        // Constructor for FormatConversionManager
        public FormatConversionManager()
        {
            content = new List<AbstractNode>();
            layoutManager = new LayoutManager();
            formatManager = new FormatManager();
            textManager = new TextManager();
        }


        // Converts format-related content
        public bool convertFormat(List<AbstractNode> content)
        {
            try
            {
                // Start the formatting process
                bool result = formatManager.StartFormatting(content);
                
                if (result)
                {
                    // Apply necessary formatting operations
                    formatManager.FormatHeadings();
                    formatManager.FormatParagraphs();
                    
                    // Get the formatted content and add it to the content list
                    List<AbstractNode> formattedContent = formatManager.ApplyBaseFormatting();
                    

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

        
        // Converts text-related content
        public bool convertText(List<AbstractNode> content)
        {
            try
            {
                
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
                    List<AbstractNode> formattedContent = textManager.ApplyTextFormatting();
                    

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

        
        // Converts layout-related content
        public bool convertLayout(List<AbstractNode> content)
        {
            try
            {
                
                // Start the layout formatting process
                bool result = layoutManager.StartLayoutFormatting(content);
                
                if (result)
                {
                    // Apply layout formatting operations
                    layoutManager.FormatHeaders();
                    layoutManager.FormatFooters();
                    layoutManager.FormatMargins();
                    layoutManager.FormatOrientation();
                    layoutManager.FormatPageSize();
                    layoutManager.FormatColumnNum();
                    layoutManager.FormatColumnSpacing();
                    
                    // Get the formatted content and REPLACE the original content
                    List<AbstractNode> formattedContent = layoutManager.ApplyLayoutFormatting();
                    content.Clear();
                    content.AddRange(formattedContent);  // Add this line
                    
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in convertLayout: {ex.Message}");
                return false;
            }
        }
        
        // Gets the content after conversion
        public List<AbstractNode> getContent()
        {
            return content;
        }
    }
    
}