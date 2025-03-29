using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ICT2106WebApp.Utilities;

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

        // Changed to match ITemplate interface (capital C)
        public Template convertToTemplate(TemplateDocument document)
        {
            if (document == null)
                return null;

            return new Template(document.Id, document.TemplateName, document.AbstractContent);
        }

        // Changed to match ITemplate interface (capital G)
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

        // Keep lowercase getTemplate for backward compatibility 
        public async Task<Template> getTemplate(string id)
        {
            return await GetTemplate(id);
        }



        // Added to match class diagram
        public Template applyTemplateConversion(string id)
        {
            try
            {
                // Check if the template exists in memory
                if (!templates.ContainsKey(id))
                {
                    var templateTask = _templateRepository.GetTemplateAsync(id);
                    templateTask.Wait();
                    var templateDoc = templateTask.Result;
                    
                    if (templateDoc != null)
                    {
                        templates[id] = templateDoc.AbstractContent;
                    }
                    else
                    {
                        return null;
                    }
                }

                // Get the template content
                var content = templates[id];
                
                // Apply IEEE format if it's an IEEE template
                if (id.ToLower() == "ieee")
                {
                }
                
                return new Template(id, GetTemplateNameById(id), content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying template conversion: {ex.Message}");
                return null;
            }
        }

        // Set a template by ID and content
        public async Task setTemplate(string id, List<AbstractNode> content)
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

            // Notify observers when a template is set
            notifyObservers(id);
        }

        // Get all available template IDs
        public async Task<List<string>> GetAllTemplateIdsAsync()
        {
            return await _templateRepository.GetTemplateIdsAsync();
        }

        // Save template to database
        private async Task SaveTemplateToDatabaseAsync(string id, List<AbstractNode> content)
        {
            try
            {
                var templateName = GetTemplateNameById(id);
                var template = new Template(id, templateName, content);
                
                // Convert to TemplateDocument
                var templateDoc = TemplateDocument.FromTemplate(template);

                // Save to database
                await _templateRepository.UpdateTemplate(templateDoc);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving template: {ex.Message}");
            }
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

        // Fixed to match the base class method in TemplateSubject
        protected override async Task<TemplateDocument> GetTemplateByIdAsync(string id)
        {
            return await _templateRepository.GetTemplateAsync(id);
        }
    }
}