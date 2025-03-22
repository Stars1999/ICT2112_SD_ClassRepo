using Microsoft.AspNetCore.Mvc;
using ICT2106WebApp.mod2grp6;
using ICT2106WebApp.mod2grp6.Template;
using ICT2106WebApp.Utilities;
using System.Collections.Generic;

namespace ICT2106WebApp.Controllers
{
    public class YourNewController : Controller
    {
        private readonly DocumentManager _documentManager;
        private readonly TemplateManager _templateManager;

        public YourNewController()
        {
            _documentManager = new DocumentManager();
            _templateManager = new TemplateManager();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TemplateTest()
        {
            // Initialize sample data for the IEEE template
            InitializeIEEETemplate();
            
            // For initial page load, just return the view
            return View();
        }

        [HttpPost]
        public IActionResult ApplyTemplate(string documentId, string templateId, bool useTemplateData = true)
        {
            // Process the template application using TemplateSample data
            bool success = ((DocumentManager)_documentManager).convertToLatexTemplateWithTemplate(documentId, templateId, useTemplateData);
            
            // Return JSON result
            return Json(new { success = success });
        }

        private void InitializeIEEETemplate()
        {
            // Create IEEE template content
            var ieeeTemplate = new List<AbstractNode>();
            
            // IEEE document class
            ieeeTemplate.Add(new SimpleNode(
                1,
                "documentClass",
                "\\documentclass[conference]{IEEEtran}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "documentclass" } } }
            ));
            
            // Common packages for IEEE papers
            ieeeTemplate.Add(new SimpleNode(
                2,
                "package",
                "\\usepackage{cite}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
            ));
            
            ieeeTemplate.Add(new SimpleNode(
                3,
                "package",
                "\\usepackage{graphicx}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
            ));

            ieeeTemplate.Add(new SimpleNode(
                4,
                "package",
                "\\usepackage{algorithm}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
            ));

            ieeeTemplate.Add(new SimpleNode(
                5,
                "package",
                "\\usepackage{algorithmic}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
            ));
            
            ieeeTemplate.Add(new SimpleNode(
                6,
                "columnFormat",
                "% IEEE papers are typically two-column format",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "comment", true } } }
            ));
            
            // IEEE title and author format
            ieeeTemplate.Add(new SimpleNode(
                7,
                "titleFormat",
                "\\title{$title$}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "title" } } }
            ));
            
            ieeeTemplate.Add(new SimpleNode(
                8,
                "authorFormat",
                "\\author{$authors$}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "author" } } }
            ));
            
            // Store the template in the TemplateManager
            _templateManager.SetTemplate("ieee", ieeeTemplate);
            
            // You could add more templates here (ACM, Springer, etc.)
        }
    }
}