using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6
{
    
    /// Interface for formatting content
    
    public interface IFormatter
    {
        
        bool convertFormat(List<AbstractNode> content);
        bool convertText(List<AbstractNode> content);
        bool convertLayout(List<AbstractNode> content);
        List<AbstractNode> getContent();
    }
}