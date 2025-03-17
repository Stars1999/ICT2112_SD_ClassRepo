using ICT2106WebApp.Utilities;
using System;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6.Layout
{
    public class LayoutManager : IFormatLayout
    {
        // Change this to store List<AbstractNode>
        private List<AbstractNode> content;

        // Update this method to accept List<AbstractNode>
        public bool StartLayoutFormatting(List<AbstractNode> content)
        {
            try
            {
                this.content = content;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FormatHeaders()
        {
            try
            {
                if (content == null || content.Count == 0)
                    return false;

                // Check if there are any header nodes to format
                foreach (var node in content)
                {
                    if (node.GetNodeType().Contains("header") || node.GetNodeType().StartsWith("h"))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FormatFooters()
        {
            try
            {
                if (content == null || content.Count == 0)
                    return false;

                // Check if there are any footer nodes to format
                foreach (var node in content)
                {
                    if (node.GetNodeType().Contains("footer"))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FormatMargins()
        {
            try
            {
                // For simplicity, always return true if content exists
                return content != null && content.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FormatOrientation()
        {
            try
            {
                // For simplicity, always return true if content exists
                return content != null && content.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FormatPageSize()
        {
            try
            {
                // For simplicity, always return true if content exists
                return content != null && content.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FormatColumnNum()
        {
            try
            {
                if (content == null || content.Count == 0)
                    return false;

                // Check for alignment variations that might indicate columns
                bool hasCenterAlign = false;
                bool hasRightAlign = false;

                foreach (var node in content)
                {
                    var styling = node.GetStyling();
                    if (styling == null || styling.Count == 0)
                        continue;

                    foreach (var style in styling)
                    {
                        if (style.ContainsKey("alignment"))
                        {
                            string alignment = style["alignment"].ToString().ToLower();
                            if (alignment == "center")
                                hasCenterAlign = true;
                            else if (alignment == "right")
                                hasRightAlign = true;
                        }
                    }
                }

                // If we have different alignments, we might need columns
                return hasCenterAlign || hasRightAlign;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FormatColumnSpacing()
        {
            try
            {
                // Only apply column spacing if we're using columns
                return FormatColumnNum();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<AbstractNode> ApplyLayoutFormatting()
        {
            // Create nodes for layout elements
            List<AbstractNode> layoutNodes = new List<AbstractNode>();

            // Add package nodes
            layoutNodes.Add(new SimpleNode(
                1,
                "package",
                "\\usepackage{geometry}",
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
            ));

            if (FormatHeaders() || FormatFooters())
            {
                layoutNodes.Add(new SimpleNode(
                    2,
                    "package",
                    "\\usepackage{fancyhdr}\n\\pagestyle{fancy}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
                ));
            }

            if (FormatColumnNum())
            {
                layoutNodes.Add(new SimpleNode(
                    3,
                    "package",
                    "\\usepackage{multicol}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
                ));
            }

            // Add layout formatting nodes
            int nodeId = 4;

            if (FormatMargins())
            {
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "margin",
                    "\\usepackage[margin=1in]{geometry}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "geometry" } } }
                ));
            }

            if (FormatOrientation())
            {
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "orientation",
                    "\\usepackage[portrait]{geometry}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "geometry" } } }
                ));
            }

            if (FormatPageSize())
            {
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "pagesize",
                    "\\usepackage[a4paper]{geometry}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "geometry" } } }
                ));
            }

            if (FormatHeaders())
            {
                // Get the first header node content or use default
                string headerContent = "Header";
                foreach (var node in content)
                {
                    if (node.GetNodeType().Contains("header") || node.GetNodeType().StartsWith("h"))
                    {
                        headerContent = node.GetContent();
                        break;
                    }
                }

                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "header",
                    $"\\fancyhead[C]{{{headerContent}}}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "fancyhdr" } } }
                ));
            }

            if (FormatFooters())
            {
                // Get the first footer node content or use default
                string footerContent = "Footer with page number: \\thepage";
                foreach (var node in content)
                {
                    if (node.GetNodeType().Contains("footer"))
                    {
                        footerContent = node.GetContent();
                        break;
                    }
                }

                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "footer",
                    $"\\fancyfoot[C]{{{footerContent}}}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "fancyhdr" } } }
                ));
            }

            if (FormatColumnNum())
            {
                int columnCount = 2; // Default to 2 columns

                if (FormatColumnSpacing())
                {
                    layoutNodes.Add(new SimpleNode(
                        nodeId++,
                        "columnspacing",
                        $"\\setlength{{\\columnsep}}{{0.5cm}}",
                        new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "multicol" } } }
                    ));
                }

                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "columnbegin",
                    $"\\begin{{multicols}}{{{columnCount}}}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "multicol" } } }
                ));

                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "columnend",
                    "\\end{multicols}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "multicol" } } }
                ));
            }

            return layoutNodes;
        }
    }
}