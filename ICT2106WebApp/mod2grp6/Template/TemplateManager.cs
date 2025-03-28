using ICT2106WebApp.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class TemplateManager : TemplateSubject, ITemplate
    {
        private Dictionary<string, List<AbstractNode>> templates = new Dictionary<string, List<AbstractNode>>();
        private readonly TemplateRepository _templateRepository;

        public TemplateManager()
        {
            _templateRepository = new TemplateRepository();
            LoadTemplatesFromDatabase().Wait(); // Load templates on initialization
        }

        private async Task LoadTemplatesFromDatabase()
{
    try
    {
        var templateDocs = await _templateRepository.GetAllTemplatesAsync();
        templates.Clear();
        
        foreach (var doc in templateDocs)
        {
            if (doc != null && doc.Id != null && doc.Content != null)
            {
                templates[doc.Id] = doc.Content;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading templates: {ex.Message}");
        // Continue with empty templates dictionary
    }
}

        // Convert the document to the specified template (with 2-column layout)
        public Template ConvertToTemplate(string id)
        {
            // Try to load template if not already loaded
            if (!templates.ContainsKey(id))
            {
                var templateDoc = _templateRepository.GetTemplateAsync(id).Result;
                if (templateDoc != null)
                {
                    templates[id] = templateDoc.Content;
                }
            }

            if (templates.ContainsKey(id))
            {
                var templateContent = new List<AbstractNode>(templates[id]); // Create a copy to avoid modifying the original
                AddTwoColumnLayout(templateContent); // Apply IEEE-style two-column layout
                NotifyObservers(id); // Notify observers when the template is applied
                return new Template(id, GetTemplateNameById(id), templateContent);
            }
            return null;
        }

        // Retrieve a template by its ID
        public Template GetTemplate(string id)
        {
            // Try to load template if not already loaded
            if (!templates.ContainsKey(id))
            {
                var templateDoc = _templateRepository.GetTemplateAsync(id).Result;
                if (templateDoc != null)
                {
                    templates[id] = templateDoc.Content;
                }
            }

            if (templates.ContainsKey(id))
            {
                var templateContent = templates[id];
                return new Template(id, GetTemplateNameById(id), templateContent);
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

            // Save to database
            SaveTemplateToDatabase(id, content);
            
            NotifyObservers(id); // Notify observers when a template is set
        }

        // Helper method to apply a two-column layout (IEEE format)
        private void AddTwoColumnLayout(List<AbstractNode> templateContent)
        {
            // Check if the template already has a column layout node
            bool hasColumnFormat = false;
            bool hasColumnEnd = false;
            
            foreach (var node in templateContent)
            {
                if (node.GetNodeType() == "columnFormat")
                    hasColumnFormat = true;
                if (node.GetNodeType() == "columnEnd")
                    hasColumnEnd = true;
            }

            // Add LaTeX commands for two-column layout if not already present
            if (!hasColumnFormat)
            {
                var columnLayoutNode = new SimpleNode(
                    1,
                    "columnFormat",
                    @"\usepackage{multicol} \begin{multicols}{2}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
                );
                templateContent.Insert(0, columnLayoutNode);
            }

            if (!hasColumnEnd)
            {
                var columnEndNode = new SimpleNode(
                    templateContent.Count + 1,
                    "columnEnd",
                    @"\end{multicols}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
                );
                templateContent.Add(columnEndNode);
            }
        }

        // Get all available template IDs
        public async Task<List<string>> GetAllTemplateIdsAsync()
        {
            return await _templateRepository.GetTemplateIdsAsync();
        }

        // Save template to database
        private async void SaveTemplateToDatabase(string id, List<AbstractNode> content)
        {
            var templateName = GetTemplateNameById(id);
            var templateDoc = new TemplateDocument
            {
                Id = id,
                TemplateName = templateName,
                Content = content
            };

            await _templateRepository.SaveTemplateAsync(templateDoc);
        }

        // Helper method to get template name
        private string GetTemplateNameById(string id)
        {
            switch (id.ToLower())
            {
                case "ieee":
                    return "IEEE Conference Template";
                case "editorial":
                    return "Editorial Style Template";
                case "acm":
                    return "ACM Format Template";
                case "springer":
                    return "Springer Format Template";
                default:
                    return "Custom Template";
            }
        }
    }
}