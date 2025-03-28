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

        public string GetId()
        {
            return id;
        }

        public string GetTemplateName()
        {
            return templateName;
        }

        public List<AbstractNode> GetContent()
        {
            return content;
        }

        public void SetId(string id)
        {
            this.id = id;
        }

        public void SetTemplateName(string name)
        {
            this.templateName = name;
        }

        public void SetContent(List<AbstractNode> content)
        {
            this.content = content ?? new List<AbstractNode>();
        }
    }
}