using ICT2106WebApp.Utilities;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class Template
    {
        private string id;
        private string templateName;
        private List<AbstractNode> content;

        // Default constructor for deserialization
        public Template()
        {
            this.content = new List<AbstractNode>();
        }

        public Template(string id, string templateName, List<AbstractNode> content)
        {
            this.id = id;
            this.templateName = templateName;
            this.content = content ?? new List<AbstractNode>();
        }

        // Changed to match class diagram
        public string getId()
        {
            return id;
        }

        // Changed to match class diagram
        public string getTemplateName()
        {
            return templateName;
        }

        // Changed to match class diagram
        public List<AbstractNode> getContent()
        {
            return content;
        }

        // Changed to match class diagram
        public void setId(string id)
        {
            this.id = id;
        }

        // Changed to match class diagram
        public void setTemplateName(string name)
        {
            this.templateName = name;
        }

        // Changed to match class diagram
        public void setContent(List<AbstractNode> content)
        {
            this.content = content ?? new List<AbstractNode>();
        }
    }
}