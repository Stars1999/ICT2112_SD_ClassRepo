using Microsoft.AspNetCore.Mvc;
using ICT2106WebApp.mod2grp6.Format;
using ICT2106WebApp.Utilities;
using System.Collections.Generic;

namespace ICT2106WebApp.Controllers
{
    public class TestFormatController : Controller
    {
        public IActionResult Index()
        {
            // Create example nodes
            var content = new List<AbstractNode>
            {
                new SimpleNode(1, "h1", "Heading 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true } } }),
                new SimpleNode(2, "h2", "Heading 2", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Italic", true } } }),
                new CompositeNode(3, "h3", "Heading 3", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "FontColor", "434343" } } })
            };

            // Create and test FormatManager
            var formatManager = new FormatManager();
            formatManager.StartFormatting(content);
            var result = formatManager.FormatHeadings();

            // Return the result to the view
            ViewBag.Result = result;
            ViewBag.Content = content;
            return View();
        }
    }
}