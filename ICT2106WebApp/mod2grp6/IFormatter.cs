using System;
using System.Collections.Generic;
using ICT2106WebApp.mod2grp6.Format;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.mod2grp6.Text;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6
{
    
    /// Interface for formatting content
    
    public interface IFormatter
    {
        
        /// Converts format-related content
        
        /// <param name="content">Format content to convert</param>
        /// <returns>True if conversion is successful</returns>
        bool convertFormat(Format.Format content);

        
        /// Converts text-related content
        
        /// <param name="content">Text content to convert</param>
        /// <returns>True if conversion is successful</returns>
        bool convertText(Text.Text content);

        
        /// Converts layout-related content
        
        /// <param name="content">Layout content to convert</param>
        /// <returns>True if conversion is successful</returns>
        bool convertLayout(Layout.Layout content);

        
        /// Gets the content after conversion
        
        /// <returns>List of AbstractNode representing the converted content</returns>
        List<AbstractNode> getContent();
    }
}