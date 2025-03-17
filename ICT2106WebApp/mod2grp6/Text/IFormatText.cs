using System.Collections.Generic;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Text
{
    /// Interface for text formatting operations
    public interface IFormatText
    {
        /// Starts the text formatting process
        /// <param name="content">The content to format</param>
        /// returns true if successful, false otherwise
        bool StartTextFormatting(List<AbstractNode> content);
        
        /// Formats fonts in the document
        /// 
        bool FormatFonts();
        
        /// Formats styles in the document
        /// returns true if successful, false otherwise
        bool FormatStyles();
        
        /// Formats colors in the document
        /// returns true if successful, false otherwise
        bool FormatColors();
        
        /// Formats line spacing in the document
        /// returns true if successful, false otherwise
        bool FormatLineSpacing();
        
        /// Formats alignment in the document
        /// returns true if successful, false otherwise
        bool FormatAlignment();
        
        /// /// Applies all text formatting and returns the formatted content
        /// The list of formatted nodes
        List<AbstractNode> ApplyTextFormatting();
    }
}