using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICT2106WebApp.mod2grp6.Template
{
    // Changed to inherit from TemplateSubject
    public class TemplateGateway : TemplateSubject
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<TemplateDocument> _templates;

        public TemplateGateway()
        {
            _context = new MongoDbContext();
            _templates = _context.Templates;
        }

        #region MongoDB Operations

        public async Task<TemplateDocument> GetTemplateAsync(string id)
        {
            try
            {
                var filter = Builders<TemplateDocument>.Filter.Eq(t => t.Id, id);
                var template = await _templates.Find(filter).FirstOrDefaultAsync();
                if (template != null)
                {
                    // Notify observers that a template has been retrieved
                    notifyTemplateLoaded(template);
                }
                
                return template;
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
                var templates = await _templates.Find(_ => true).ToListAsync();
                
                // Notify observers that templates have been loaded
                if (templates != null && templates.Count > 0)
                {
                    notifyTemplatesLoaded(templates);
                }
                
                return templates;
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

        #endregion
        // Helper method to convert TemplateDocument to Template
        public Template ConvertToTemplate(TemplateDocument document)
        {
            if (document == null)
                return null;
                
            return new Template(document.Id, document.TemplateName, document.AbstractContent);
        }
    }
}