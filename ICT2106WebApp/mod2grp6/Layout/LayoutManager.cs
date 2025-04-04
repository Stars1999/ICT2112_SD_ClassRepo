using ICT2106WebApp.Utilities;
using System;
using System.Collections.Generic;

namespace ICT2106WebApp.mod2grp6.Layout
{
    public class LayoutManager : IFormatLayout
    {
        private List<AbstractNode> content;

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
                
                // Also check for header margin in layout node
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
                                    if (margins.ContainsKey("Header"))
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
                
                // Also check for footer margin in layout node
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
                                    if (margins.ContainsKey("Footer"))
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

        // Get the orientation setting from content (landscape or portrait)
        public string GetOrientationSetting()
        {
            try
            {
                if (content == null || content.Count == 0)
                    return "Portrait"; // Default to portrait

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
                                    return style["Orientation"].ToString();
                                }
                            }
                        }
                    }
                }
                return "Portrait"; // Default to portrait if not specified
            }
            catch (Exception)
            {
                return "Portrait"; // Default to portrait on error
            }
        }

        // Improved to check actual orientation value
        public bool FormatOrientation()
        {
            try
            {
                if (content == null || content.Count == 0)
                    return false;

                // Check for layout node with orientation information
                string orientation = GetOrientationSetting();
                return orientation.Equals("Landscape", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get the page dimensions from the content
        public (double Width, double Height) GetPageDimensions()
        {
            double defaultWidth = 21.0; // Default A4 width in cm
            double defaultHeight = 29.7; // Default A4 height in cm

            try
            {
                if (content == null || content.Count == 0)
                    return (defaultWidth, defaultHeight);

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
                                    double width = Convert.ToDouble(style["PageWidth"]);
                                    double height = Convert.ToDouble(style["PageHeight"]);
                                    return (width, height);
                                }
                            }
                        }
                    }
                }
                return (defaultWidth, defaultHeight);
            }
            catch (Exception)
            {
                return (defaultWidth, defaultHeight);
            }
        }

        public bool FormatPageSize()
        {
            try
            {
                // Always format page size if content exists
                return content != null && content.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get the column number setting from the content
        public int GetColumnCount()
        {
            try
            {
                if (content == null || content.Count == 0)
                    return 1; // Default to 1 column

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
                                    int columnCount = Convert.ToInt32(style["ColumnNum"]);
                                    return columnCount > 0 ? columnCount : 1;
                                }
                            }
                        }
                    }
                }
                return 1; // Default to 1 column if not specified
            }
            catch (Exception)
            {
                return 1; // Default to 1 column on error
            }
        }

        // Get the column spacing setting from the content
        public double GetColumnSpacing()
        {
            try
            {
                if (content == null || content.Count == 0)
                    return 0.5; // Default to 0.5 cm

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
                                    return Convert.ToDouble(style["ColumnSpacing"]);
                                }
                            }
                        }
                    }
                }
                return 0.5; // Default to 0.5 cm if not specified
            }
            catch (Exception)
            {
                return 0.5; // Default to 0.5 cm on error
            }
        }

        public bool FormatColumnNum()
        {
            try
            {
                // Always format column settings if content exists
                return content != null && content.Count > 0;
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
                // Format column spacing if column count > 1
                return FormatColumnNum() && GetColumnCount() > 1;
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
                    "\\usepackage{fancyhdr}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
                ));
            }

            int columnCount = GetColumnCount();
            if (columnCount > 1)
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
            
            // Handle orientation - add appropriate orientation setting
            string orientation = GetOrientationSetting();
            
            if (orientation.Equals("Landscape", StringComparison.OrdinalIgnoreCase))
            {
                // Add pdflscape package for landscape support
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "package",
                    "\\usepackage{pdflscape}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "package" } } }
                ));
                
                // Use landscape environment - note: typically this would go around content
                // but for this test we're just adding it as a command
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "orientation",
                    "\\begin{landscape}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "pdflscape" } } }
                ));
            }
            else
            {
                // Add explicit portrait orientation to the geometry options
                geometryOptions.Add("portrait");
            }

            // Add page dimensions
            if (FormatPageSize())
            {
                var (width, height) = GetPageDimensions();
                
                // Always include explicit dimensions
                geometryOptions.Add($"paperwidth={width}cm");
                geometryOptions.Add($"paperheight={height}cm");
                
                // Also add a4paper if dimensions match A4
                if (Math.Abs(width - 21) < 0.5 && Math.Abs(height - 29.7) < 0.5)
                {
                    geometryOptions.Add("a4paper");
                }
            }
            
            // Process margins
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
                string headerAlignment = "C"; // Default to Center
                Dictionary<string, object> headerStyles = null;
                
                foreach (var node in content)
                {
                    if (node.GetNodeType().Contains("header") || node.GetNodeType().StartsWith("h"))
                    {
                        headerContent = node.GetContent();
                        
                        // Extract styling information if available
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            headerStyles = styling[0];
                            
                            // Get alignment if specified
                            if (headerStyles.ContainsKey("Alignment"))
                            {
                                string alignment = headerStyles["Alignment"].ToString().ToLower();
                                if (alignment.Contains("left"))
                                    headerAlignment = "L";
                                else if (alignment.Contains("right"))
                                    headerAlignment = "R";
                                // C is already the default for center/other
                            }
                        }
                        break;
                    }
                }

                // Add header commands
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "header_clear",
                    "\\fancyhead{}",  // Clear all header fields first
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "fancyhdr" } } }
                ));

                // Add formatting to header content if needed
                string formattedHeaderContent = headerContent;
                if (headerStyles != null)
                {
                    if (headerStyles.ContainsKey("Bold") && Convert.ToBoolean(headerStyles["Bold"]))
                        formattedHeaderContent = $"\\textbf{{{formattedHeaderContent}}}";
                    if (headerStyles.ContainsKey("Italic") && Convert.ToBoolean(headerStyles["Italic"]))
                        formattedHeaderContent = $"\\textit{{{formattedHeaderContent}}}";
                    // Additional formatting could be added here (color, size, etc.)
                }

                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "header",
                    $"\\fancyhead[{headerAlignment}]{{{formattedHeaderContent}}}",  // Use detected alignment
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "fancyhdr" } } }
                ));
            }

            if (FormatFooters())
            {
                // Get the first footer node content or use default
                string footerContent = "Footer with page number: \\thepage";
                string footerAlignment = "C"; // Default to Center
                Dictionary<string, object> footerStyles = null;
                
                foreach (var node in content)
                {
                    if (node.GetNodeType().Contains("footer"))
                    {
                        footerContent = node.GetContent();
                        
                        // Extract styling information if available
                        var styling = node.GetStyling();
                        if (styling != null && styling.Count > 0)
                        {
                            footerStyles = styling[0];
                            
                            // Get alignment if specified
                            if (footerStyles.ContainsKey("Alignment"))
                            {
                                string alignment = footerStyles["Alignment"].ToString().ToLower();
                                if (alignment.Contains("left"))
                                    footerAlignment = "L";
                                else if (alignment.Contains("right"))
                                    footerAlignment = "R";
                                // C is already the default for center/other
                            }
                        }
                        break;
                    }
                }

                // Add footer commands
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "footer_clear",
                    "\\fancyfoot{}",  // Clear all footer fields first
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "fancyhdr" } } }
                ));

                // Add formatting to footer content if needed
                string formattedFooterContent = footerContent;
                if (footerStyles != null)
                {
                    if (footerStyles.ContainsKey("Bold") && Convert.ToBoolean(footerStyles["Bold"]))
                        formattedFooterContent = $"\\textbf{{{formattedFooterContent}}}";
                    if (footerStyles.ContainsKey("Italic") && Convert.ToBoolean(footerStyles["Italic"]))
                        formattedFooterContent = $"\\textit{{{formattedFooterContent}}}";
                    // Additional formatting could be added here (color, size, etc.)
                }

                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "footer",
                    $"\\fancyfoot[{footerAlignment}]{{{formattedFooterContent}}}",  // Use detected alignment
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "fancyhdr" } } }
                ));
                
                // Add page style application
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "pagestyle",
                    "\\pagestyle{fancy}",  // Apply the fancy page style
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "fancyhdr" } } }
                ));
            }

            // Handle column layout if more than 1 column
            if (columnCount > 1)
            {
                double columnSpacing = GetColumnSpacing();
                
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "columnspacing",
                    $"\\setlength{{\\columnsep}}{{{columnSpacing}cm}}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "multicol" } } }
                ));

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

            // Close landscape environment if it was opened
            if (FormatOrientation())
            {
                layoutNodes.Add(new SimpleNode(
                    nodeId++,
                    "orientationEnd",
                    "\\end{landscape}",
                    new List<Dictionary<string, object>> { new Dictionary<string, object> { { "command", "pdflscape" } } }
                ));
            }

            return layoutNodes;
        }
    }
}