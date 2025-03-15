using System;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6
{
    
    /// Interface for processing documents to LaTeX format
    
    public interface IProcessDocument
    {
        
        /// Converts document to LaTeX format
        
        /// <param name="id">Document identifier</param>
        /// <returns>LaTeX document</returns>
        LatexDocument toLaTeX(string id);
    }

    
    /// Placeholder for LaTeX document if it's not defined elsewhere
    /// Remove this if LatexDocument is already defined in another file
    
    public class LatexDocument
    {
        // Properties and methods for LaTeX document
    }
}