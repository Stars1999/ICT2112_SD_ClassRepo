using ICT2106WebApp.Utilities;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class TemplateDocument
    {
        public string Id { get; set; }
        public string TemplateName { get; set; }
        public List<AbstractNode> Content { get; set; }
    }
}
