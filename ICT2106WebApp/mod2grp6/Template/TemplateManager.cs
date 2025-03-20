using ICT2106WebApp.Utilities;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class TemplateManager : ITemplate
    {
        private Dictionary<string, List<AbstractNode>> templates = new Dictionary<string, List<AbstractNode>>();

        // Convert the document to the specified template (with 2-column layout)
        public Template ConvertToTemplate(string id)
        {
            if (templates.ContainsKey(id))
            {
                var templateContent = templates[id];
                AddTwoColumnLayout(templateContent); // Apply IEEE-style two-column layout
                return new Template(id, "IEEE Style Template", templateContent);
            }
            return null;
        }

        // Retrieve a template by its ID
        public Template GetTemplate(string id)
        {
            if (templates.ContainsKey(id))
            {
                var templateContent = templates[id];
                return new Template(id, "Custom Template", templateContent);
            }
            return null;
        }

        // Set a template by ID and content
        public void SetTemplate(string id, List<AbstractNode> content)
        {
            if (templates.ContainsKey(id))
            {
                templates[id] = content;
            }
            else
            {
                templates.Add(id, content);
            }
        }

        // Helper method to apply a two-column layout (IEEE format)
        private void AddTwoColumnLayout(List<AbstractNode> templateContent)
        {
            // Add LaTeX commands for two-column layout
            var columnLayoutNode = new SimpleNode(
                1,
                "columnFormat",
                @"\usepackage{multicol} \begin{multicols}{2}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
            );
            templateContent.Insert(0, columnLayoutNode);

            var columnEndNode = new SimpleNode(
                2,
                "columnEnd",
                @"\end{multicols}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
            );
            templateContent.Add(columnEndNode);
        }
    }
}
