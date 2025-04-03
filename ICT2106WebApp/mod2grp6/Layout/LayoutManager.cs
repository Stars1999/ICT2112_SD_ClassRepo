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
                if (content == null || content.Count == 0)
                    return false;

                // Check for layout node with orientation information
                foreach (var node in content)
                {
                    if (node.GetNodeType().Equals("layout"))
                    {
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            foreach (var style in styling)
                            {
                                if (style.ContainsKey("Orientation"))
                                {
                                    // Return true regardless of orientation to apply orientation formatting
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
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

                // First check for explicit column settings in layout nodes
                foreach (var node in content)
                {
                    if (node.GetNodeType().Equals("layout"))
                    {
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            foreach (var style in styling)
                            {
                                if (style.ContainsKey("ColumnNum"))
                                {
                                    // If ColumnNum is greater than 1, we need column formatting
                                    if (Convert.ToInt32(style["ColumnNum"]) > 1)
                                        return true;
                                }
                            }
                        }
                    }
                }

                // Fallback to checking alignment as before
                bool hasCenterAlign = false;
                bool hasRightAlign = false;

                foreach (var node in content)
                {
                    var styling = node.GetStyling();
                    if (styling == null || styling.Count == 0)
                        continue;

                    foreach (var style in styling)
                    {
                        if (style.ContainsKey("Alignment"))
                        {
                            string alignment = style["Alignment"].ToString().ToLower();
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

            // Combine geometry options
            List<string> geometryOptions = new List<string>();
            
            // Handle orientation settings
            if (FormatOrientation())
            {
                // Determine orientation type
                string orientationType = "portrait"; // Default
                
                foreach (var node in content)
                {
                    if (node.GetNodeType().Equals("layout"))
                    {
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            foreach (var style in styling)
                            {
                                if (style.ContainsKey("Orientation"))
                                {
                                    string orientation = style["Orientation"].ToString();
                                    if (orientation.Equals("Landscape", StringComparison.OrdinalIgnoreCase))
                                    {
                                        orientationType = "landscape";
                                        
                                        // Add pdflscape package for landscape support
                                        layoutNodes.Add(new SimpleNode(
                                            nodeId++,
                                            "package",
                                            "\\usepackage{pdflscape}",
                                            new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
                                        ));
                                        
                                        // Use landscape environment
                                        layoutNodes.Add(new SimpleNode(
                                            nodeId++,
                                            "orientation",
                                            "\\begin{landscape}",
                                            new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "pdflscape" } } }
                                        ));
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                
                // Add orientation to geometry options
                geometryOptions.Add(orientationType);
            }
            
            // Format margins if needed
            if (FormatMargins())
            {
                // Check for explicit margin settings in the content
                bool foundMargins = false;
                foreach (var node in content)
                {
                    if (node.GetNodeType().Equals("layout"))
                    {
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            foreach (var style in styling)
                            {
                                if (style.ContainsKey("Margins") && style["Margins"] is Dictionary<string, object> margins)
                                {
                                    // Extract margin values
                                    if (margins.ContainsKey("Top"))
                                        geometryOptions.Add($"top={margins["Top"]}cm");
                                    if (margins.ContainsKey("Bottom"))
                                        geometryOptions.Add($"bottom={margins["Bottom"]}cm");
                                    if (margins.ContainsKey("Left"))
                                        geometryOptions.Add($"left={margins["Left"]}cm");
                                    if (margins.ContainsKey("Right"))
                                        geometryOptions.Add($"right={margins["Right"]}cm");
                                    if (margins.ContainsKey("Header"))
                                        geometryOptions.Add($"headheight={margins["Header"]}cm");
                                    if (margins.ContainsKey("Footer"))
                                        geometryOptions.Add($"footskip={margins["Footer"]}cm");
                                    
                                    foundMargins = true;
                                    break;
                                }
                            }
                            if (foundMargins) break;
                        }
                    }
                }
                
                // Default to 1 inch margins if no specific margins found
                if (!foundMargins)
                {
                    geometryOptions.Add("margin=1in");
                }
            }
            
            // Format page size if needed
            if (FormatPageSize())
            {
                // Check for explicit page size in content
                bool foundPageSize = false;
                foreach (var node in content)
                {
                    if (node.GetNodeType().Equals("layout"))
                    {
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            foreach (var style in styling)
                            {
                                if (style.ContainsKey("PageWidth") && style.ContainsKey("PageHeight"))
                                {
                                    // If dimensions are approximately A4 (21 x 29.7 cm)
                                    double width = Convert.ToDouble(style["PageWidth"]);
                                    double height = Convert.ToDouble(style["PageHeight"]);
                                    
                                    if (Math.Abs(width - 21) < 0.5 && Math.Abs(height - 29.7) < 0.5)
                                    {
                                        geometryOptions.Add("a4paper");
                                    }
                                    else
                                    {
                                        // Custom paper size in centimeters
                                        geometryOptions.Add($"paperwidth={width}cm,paperheight={height}cm");
                                    }
                                    foundPageSize = true;
                                    break;
                                }
                            }
                            if (foundPageSize) break;
                        }
                    }
                }
                
                // Default to a4paper if no specific size found
                if (!foundPageSize)
                {
                    geometryOptions.Add("a4paper");
                }
            }
            
            // If we have any geometry options, add the combined node
            if (geometryOptions.Count > 0)
            {
                string combinedOptions = string.Join(",", geometryOptions);
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "geometry",
                    $"\\geometry{{{combinedOptions}}}",
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
                // Get the column count from the content if available
                int columnCount = 2; // Default to 2 columns
                foreach (var node in content)
                {
                    if (node.GetNodeType().Equals("layout"))
                    {
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            foreach (var style in styling)
                            {
                                if (style.ContainsKey("ColumnNum"))
                                {
                                    columnCount = Convert.ToInt32(style["ColumnNum"]);
                                    // Ensure at least 1 column
                                    if (columnCount < 1) columnCount = 1;
                                    break;
                                }
                            }
                        }
                    }
                }

                // Only add multicol commands if we have more than 1 column
                if (columnCount > 1)
                {
                    if (FormatColumnSpacing())
                    {
                        // Get column spacing if available
                        double columnSpacing = 0.5; // Default spacing in cm
                        foreach (var node in content)
                        {
                            if (node.GetNodeType().Equals("layout"))
                            {
                                var styling = node.GetStyling();
                                if (styling != null && styling.Count > 0)
                                {
                                    foreach (var style in styling)
                                    {
                                        if (style.ContainsKey("ColumnSpacing"))
                                        {
                                            columnSpacing = Convert.ToDouble(style["ColumnSpacing"]);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        
                        layoutNodes.Add(new SimpleNode(
                            nodeId++,
                            "columnspacing",
                            $"\\setlength{{\\columnsep}}{{{columnSpacing}cm}}",
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
            }

            // Close landscape environment if it was opened
            if (FormatOrientation())
            {
                // Check if we're using landscape orientation
                foreach (var node in content)
                {
                    if (node.GetNodeType().Equals("layout"))
                    {
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            foreach (var style in styling)
                            {
                                if (style.ContainsKey("Orientation"))
                                {
                                    string orientation = style["Orientation"].ToString();
                                    if (orientation.Equals("Landscape", StringComparison.OrdinalIgnoreCase))
                                    {
                                        // Close landscape environment
                                        layoutNodes.Add(new SimpleNode(
                                            nodeId++,
                                            "orientationEnd",
                                            "\\end{landscape}",
                                            new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "pdflscape" } } }
                                        ));
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return layoutNodes;
        }
    }
}