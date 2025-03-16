using System;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6
{
    
    /// Interface for processing documents to LaTeX format
    
    public interface IProcessDocument
    {
        bool toLaTeX(string id);
        bool convertToLatexTemplate(string id, string templateid);
    }
}