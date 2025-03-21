using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.mod2grp6.Format;
using ICT2106WebApp.Utilities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ICT2106WebApp.mod2grp6;
using ICT2106WebApp.mod2grp6.TestCase;

namespace ICT2106WebApp.Pages
{
    public class TestFormatModel : PageModel
    {
        public bool? Result { get; set; }
        public List<AbstractNode> Content { get; set; }
        public string TestType { get; set; } = "None";

        public void OnGet()
        {
            // Initialize empty content
            Content = new List<AbstractNode>();
        }

        public IActionResult OnPostTestLayout()
        {
            TestType = "Layout";
            
            // Create example nodes based on output.json examples - keep original code
            //Content = new List<AbstractNode> { new SimpleNode(1, "header", "Header from output.json", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "alignment", "center" } } }), new SimpleNode(2, "footer", "Footer with page number: \\thepage", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "alignment", "center" } } }), new SimpleNode(3, "h1", "Heading 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "bold", true }, { "alignment", "left" } } }), new SimpleNode(4, "paragraph", "This is a paragraph with left alignment.", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "alignment", "left" } } }), new SimpleNode(5, "paragraph", "This is a paragraph with center alignment.", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "alignment", "center" } } }), new SimpleNode(6, "paragraph", "This is a paragraph with right alignment.", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "alignment", "right" } } }) };

            Content = new TestCases().LayoutContent;
            // Create and test LayoutManager with the List<AbstractNode> - keep original code
            var layoutManager = new LayoutManager();
            bool success = layoutManager.StartLayoutFormatting(Content);

            if (success)
            {
                // Apply layout formatting - keep original code
                Content = layoutManager.ApplyLayoutFormatting();
                Result = true;
            }
            else
            {
                Result = false;
                Content = new List<AbstractNode>();
            }
            
            return Page();
        }

        public IActionResult OnPostTestFormat()
        {
            TestType = "Format";
            
            // Create example nodes for FormatManager testing
            //Content = new List<AbstractNode> { new SimpleNode(1, "h1", "Heading 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true } } }), new SimpleNode(2, "h2", "Heading 2", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Italic", true } } }), new SimpleNode(3, "h3", "Heading 3", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "FontColor", "434343" } } }) };
            Content = new TestCases().HeadingsAndParagraphs;
            // Create and test FormatManager
            var formatManager = new FormatManager();
            formatManager.StartFormatting(Content);
            Result = formatManager.FormatHeadings();
            
            return Page();
        }
    }
}