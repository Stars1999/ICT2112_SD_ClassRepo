// using Microsoft.AspNetCore.Mvc.RazorPages;
// using ICT2106WebApp.mod2grp6.Format;
// using ICT2106WebApp.Utilities;
// using System.Collections.Generic;

// namespace ICT2106WebApp.Pages
// {
//    public class TestFormatModel : PageModel
//    {
//        public bool? Result { get; set; }
//        public List<AbstractNode> Content { get; set; }

//        public void OnPost()
//        {
//            // Create example nodes
//            Content = new List<AbstractNode>
//            {
//                new SimpleNode(1, "h1", "Heading 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true } } }),
//                new SimpleNode(2, "h2", "Heading 2", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Italic", true } } }),
//                new CompositeNode(3, "h3", "Heading 3", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "FontColor", "434343" } } })
//            };

//            // Create and test FormatManager
//            var formatManager = new FormatManager();
//            formatManager.StartFormatting(Content);
//            Result = formatManager.FormatHeadings();
//        }
//    }
// }

using Microsoft.AspNetCore.Mvc.RazorPages;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.Utilities;
using System.Collections.Generic;

namespace ICT2106WebApp.Pages
{
    public class TestFormatModel : PageModel
    {
        public bool? Result { get; set; }
        public List<AbstractNode> Content { get; set; }

        public void OnPost()
        {
            // Create example nodes based on output.json examples
            Content = new List<AbstractNode>
            {
                new SimpleNode(1, "header", "Header from output.json", 
                    new List<Dictionary<string, object>> { 
                        new Dictionary<string, object> { { "alignment", "center" } } 
                    }),
                new SimpleNode(2, "footer", "Footer with page number: \\thepage", 
                    new List<Dictionary<string, object>> { 
                        new Dictionary<string, object> { { "alignment", "center" } } 
                    }),
                new SimpleNode(3, "h1", "Heading 1", 
                    new List<Dictionary<string, object>> { 
                        new Dictionary<string, object> { { "bold", true }, { "alignment", "left" } } 
                    }),
                new SimpleNode(4, "paragraph", "This is a paragraph with left alignment.", 
                    new List<Dictionary<string, object>> { 
                        new Dictionary<string, object> { { "alignment", "left" } } 
                    }),
                new SimpleNode(5, "paragraph", "This is a paragraph with center alignment.", 
                    new List<Dictionary<string, object>> { 
                        new Dictionary<string, object> { { "alignment", "center" } } 
                    }),
                new SimpleNode(6, "paragraph", "This is a paragraph with right alignment.", 
                    new List<Dictionary<string, object>> { 
                        new Dictionary<string, object> { { "alignment", "right" } } 
                    })
            };

            // Create and test LayoutManager with the List<AbstractNode>
            var layoutManager = new LayoutManager();
            bool success = layoutManager.StartLayoutFormatting(Content);

            if (success)
            {
                // Apply layout formatting
                Content = layoutManager.ApplyLayoutFormatting();
                Result = true;
            }
            else
            {
                Result = false;
                Content = new List<AbstractNode>();
            }
        }
    }
}