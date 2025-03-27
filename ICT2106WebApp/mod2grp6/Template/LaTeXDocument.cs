using System.Collections.Generic;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class LaTeXDocument
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Dictionary<string, List<AbstractNode>> ContentSections { get; set; }
        public string OriginalLaTeXContent { get; set; }
        public string TemplateAppliedContent { get; set; }
        public bool IsConvertedToLaTeX { get; set; }
        public bool IsTemplateApplied { get; set; }
        public string AppliedTemplateId { get; set; }

        public LaTeXDocument()
        {
            ContentSections = new Dictionary<string, List<AbstractNode>>();
            OriginalLaTeXContent = string.Empty;
            TemplateAppliedContent = string.Empty;
            IsConvertedToLaTeX = false;
            IsTemplateApplied = false;
            AppliedTemplateId = string.Empty;
        }
    }
}