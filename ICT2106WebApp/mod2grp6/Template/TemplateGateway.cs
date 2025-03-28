using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod2grp6.Template
{
    public class TemplateGateway : ITemplateObserver
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<TemplateDocument> _templates;

        public TemplateGateway()
        {
            _context = new MongoDbContext();
            _templates = _context.Templates;
        }

        public async Task<TemplateDocument> GetTemplateAsync(string id)
        {
            try
            {
                var filter = Builders<TemplateDocument>.Filter.Eq(t => t.Id, id);
                return await _templates.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting template {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<TemplateDocument>> GetAllTemplatesAsync()
        {
            try
            {
                return await _templates.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all templates: {ex.Message}");
                return new List<TemplateDocument>();
            }
        }

        public async Task<List<string>> GetTemplateIdsAsync()
        {
            try
            {
                var projection = Builders<TemplateDocument>.Projection.Include(t => t.Id);
                var templates = await _templates.Find(_ => true).Project(projection).ToListAsync();
                var ids = new List<string>();
                
                foreach (var template in templates)
                {
                    // Directly access the "Id" field, not "_id"
                    if (template.Contains("Id"))
                    {
                        ids.Add(template["Id"].AsString);
                    }
                }
                
                return ids;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting template IDs: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<bool> SaveTemplateAsync(TemplateDocument template)
        {
            try
            {
                var filter = Builders<TemplateDocument>.Filter.Eq(t => t.Id, template.Id);
                var existingTemplate = await _templates.Find(filter).FirstOrDefaultAsync();

                if (existingTemplate != null)
                {
                    // Update existing template
                    var result = await _templates.ReplaceOneAsync(filter, template);
                    return result.IsAcknowledged && result.ModifiedCount > 0;
                }
                else
                {
                    // Insert new template
                    await _templates.InsertOneAsync(template);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving template: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTemplateAsync(string id)
        {
            try
            {
                var filter = Builders<TemplateDocument>.Filter.Eq(t => t.Id, id);
                var result = await _templates.DeleteOneAsync(filter);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting template {id}: {ex.Message}");
                return false;
            }
        }

        // Helper method to convert TemplateDocument to Template
        public Template ConvertToTemplate(TemplateDocument document)
        {
            if (document == null)
                return null;
                
            return new Template(document.Id, document.TemplateName, document.AbstractContent);
        }

        public void UpdateTemplate(string templateId)
        {
            // Handle the template update notification
            Console.WriteLine($"Template {templateId} has been updated.");
        }
    }
}