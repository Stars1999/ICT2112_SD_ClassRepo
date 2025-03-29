using ICT2106WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class TemplateManager : TemplateSubject, ITemplate
    {
        private Dictionary<string, List<AbstractNode>> templates = new Dictionary<string, List<AbstractNode>>();
        private readonly TemplateGateway _templateRepository;

        public TemplateManager()
        {
            _templateRepository = new TemplateGateway();
            LoadTemplatesFromDatabase().Wait(); // Load templates on initialization
        }

        private async Task LoadTemplatesFromDatabase()
        {
            try
            {
                var templateDocs = await _templateRepository.GetAllTemplatesAsync();
                Console.WriteLine($"LoadTemplatesFromDatabase: Found {templateDocs.Count} templates");
                templates.Clear();

                foreach (var doc in templateDocs)
                {
                    if (doc != null && doc.Id != null && doc.Content != null)
                    {
                        templates[doc.Id] = doc.AbstractContent;
                        Console.WriteLine($"Loaded template: {doc.Id} - {doc.TemplateName ?? "Unnamed"} with {doc.Content.Count} nodes");
                    }
                    else
                    {
                        Console.WriteLine($"Skipped invalid template document: {doc?.Id ?? "null"}");
                    }
                }

                // Print out all loaded templates
                Console.WriteLine("All loaded templates:");
                foreach (var template in templates)
                {
                    Console.WriteLine($"Template ID: {template.Key}, Nodes: {template.Value.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading templates: {ex.Message}");
                Console.WriteLine($"Exception details: {ex}");
                // Continue with empty templates dictionary
            }
        }
        // Async version of ConvertToTemplate
        public Template ConvertToTemplate(TemplateDocument document)
        {
            if (document == null)
                return null;

            return new Template(document.Id, document.TemplateName, document.AbstractContent);
        }

        // Retrieve a template by its ID
        public async Task<Template> GetTemplate(string id)
        {
            try
            {
                // Try to load template if not already loaded
                if (!templates.ContainsKey(id))
                {
                    var templateDoc = await _templateRepository.GetTemplateAsync(id);
                    if (templateDoc != null)
                    {
                        templates[id] = templateDoc.AbstractContent;
                    }
                }

                if (templates.ContainsKey(id))
                {
                    var templateContent = templates[id];
                    return new Template(id, GetTemplateNameById(id), templateContent);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting template: {ex.Message}");
                return null;
            }
        }

        // Set a template by ID and content
        public async Task SetTemplate(string id, List<AbstractNode> content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (templates.ContainsKey(id))
            {
                templates[id] = content;
            }
            else
            {
                templates.Add(id, content);
            }

            // Save to database
            await SaveTemplateToDatabaseAsync(id, content);

            //NotifyObservers(id); // Notify observers when a template is set
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
        private async Task SaveTemplateToDatabaseAsync(string id, List<AbstractNode> content)
        {
            var templateName = GetTemplateNameById(id);
            var template = new Template(id, templateName, content);
            //var templateDoc = TemplateDocument.FromTemplate(template);

            //await _templateRepository.SaveTemplateAsync(templateDoc);
        }

        // Helper method to get template name
        private string GetTemplateNameById(string id)
        {
            switch (id?.ToLower() ?? "")
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