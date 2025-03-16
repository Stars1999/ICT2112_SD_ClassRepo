using System;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6
{
    public interface IProcessTemplate
    {
        bool convertToLatexTemplate(string id, string templateid);
    }
}