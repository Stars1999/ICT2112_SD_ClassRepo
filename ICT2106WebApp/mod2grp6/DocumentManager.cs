using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;
using ICT2106WebApp.mod2grp6.Template;
using ICT2106WebApp.mod2grp6.Format;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.mod2grp6.Text;

namespace ICT2106WebApp.mod2grp6
{
    public class DocumentManager : IProcessDocument, IProcessTemplate
    {
        // Services and components needed for document operations
        private FormatConversionManager formatConversionManager;
        private TemplateManager templateManager;
        
        // Sample document data from sample.cs
        private Dictionary<string, List<AbstractNode>> documentContent;
        
        // Constructor for DocumentManager
        public DocumentManager()
        {
            formatConversionManager = new FormatConversionManager();
            templateManager = new TemplateManager();
            
            // Initialize content directly from sample.cs
            InitializeFromSample();
        }
        
        // Initialize document content directly from the Testing class in sample.cs
        private void InitializeFromSample()
        {
            documentContent = new Dictionary<string, List<AbstractNode>>();
            
            // These would be retrieved from the Testing class in sample.cs
            documentContent["format"] = new List<AbstractNode> { 
                new SimpleNode(1, "h1", "Header 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(2, "h2", "Header 2", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(3, "h3", "Header 3", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "434343" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
            };
            
            documentContent["layout"] = new List<AbstractNode> {
                new SimpleNode(1, "layout", "", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Orientation", "Portrait" }, { "PageWidth", 21 }, { "PageHeight", 29.7 }, { "ColumnNum", 1 }, { "ColumnSpacing", 1.25 }, { "Margins", new Dictionary<string, object> { { "Top", 2.54 }, { "Bottom", 2.54 }, { "Left", 2.54 }, { "Right", 2.54 }, { "Header", 1.25 }, { "Footer", 1.25 } } } } }),
                new SimpleNode(2, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
            };
            
            documentContent["text"] = new List<AbstractNode>{
                new SimpleNode(1, "paragraph", "test text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(2, "paragraph_run", "https://puginarug.com/", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" } } }),
                new SimpleNode(3, "paragraph", "Colored text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "FF0000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(4, "paragraph", "diff color and highlighted", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "0000FF" }, { "Highlight", "cyan" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(5, "paragraph", "this is a bolded text", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
            };
            
            documentContent["metadata"] = new List<AbstractNode>
            {
                new SimpleNode(1, "metadata", "CreatedDate_Internal: 2025-02-21 05:57:00Z", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "CreatedDate_Internal", "2025-02-21 05:57:00Z" } } }),
                new SimpleNode(2, "metadata", "LastModified_Internal: 2025-03-17 06:25:00Z", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "LastModified_Internal", "2025-03-17 06:25:00Z" } } }),
                new SimpleNode(3, "metadata", "filename: Datarepository_zx_v2.docx", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "filename", "Datarepository_zx_v2.docx" } } }),
                new SimpleNode(4, "metadata", "size: 2148554", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "size", "2148554" } } })
            };
            
            documentContent["math"] = new List<AbstractNode>
            {
                new SimpleNode(1, "math", "(1/2) × √(4)  <- math", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(2, "math", "∫", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(3, "math", "(1/2) × √(4)=1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(4, "math", "s=√(([i=1,N](x^2)/N-1))", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(5, "math", "(1/2)cos2x+ (3/8)sin4x =3x", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
            };
            
            documentContent["lists"] = new List<AbstractNode>
            {
                new SimpleNode(1, "bulleted_list", "bulleted type 1", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(2, "hollow_bulleted_list", "bulleted type 2", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(3, "square_bulleted_list", "bulleted type 3", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(4, "numbered_list", "numbered list", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
                new SimpleNode(5, "numbered_parenthesis_list", "numbered list with bracket", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Default Font" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
            };
            
            documentContent["images"] = new List<AbstractNode>
            {
                new SimpleNode(1, "image", "Image_rId9.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Left Align (Ctrl + L)" }, { "Format", "PNG" }, { "Position", "Inline (position determined by text flow)" }, { "WidthEMU", 2500550 }, { "HeightEMU", 2500550 } } }),
                new SimpleNode(2, "image", "Image_rId10.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Center Align (Ctrl + E)" }, { "Format", "PNG" }, { "Position", "Inline (position determined by text flow)" }, { "WidthEMU", 2786063 }, { "HeightEMU", 1567160 } } }),
                new SimpleNode(3, "image", "Image_rId11.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Left Align (Ctrl + L)" }, { "Format", "PNG" }, { "Position", "Inline (position determined by text flow)" }, { "WidthEMU", 3205163 }, { "HeightEMU", 2136775 } } })
            };
            
            documentContent["bibliography"] = new List<AbstractNode>
            {
                new SimpleNode(1, "bibliography", "Reference (IEE)", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 10 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
            };
        }
        
        /// <summary>
        /// Retrieve content by content type
        /// </summary>
        /// <param name="contentType">Type of content (format, layout, text, metadata, math, lists, images, bibliography)</param>
        /// <returns>List of AbstractNode objects for the specified content type</returns>
        public List<AbstractNode> GetContentByType(string contentType)
        {
            if (documentContent.ContainsKey(contentType))
            {
                return documentContent[contentType];
            }
            
            return new List<AbstractNode>();
        }
        
        /// <summary>
        /// Converts document to LaTeX format
        /// </summary>
        /// <param name="id">Document ID (unused in this implementation since we're using sample data)</param>
        /// <returns>Success flag</returns>
        public bool toLaTeX(string id)
        {
            try
            {
                // Convert format content
                List<AbstractNode> formatContent = GetContentByType("format");
                bool formatSuccess = formatConversionManager.convertFormat(formatContent);
                
                // Convert text content
                List<AbstractNode> textContent = GetContentByType("text");
                bool textSuccess = formatConversionManager.convertText(textContent);
                
                // Convert layout content
                List<AbstractNode> layoutContent = GetContentByType("layout");
                bool layoutSuccess = formatConversionManager.convertLayout(layoutContent);
                
                // We'd also handle math content, lists, images, and bibliography in a real implementation
                
                return formatSuccess && textSuccess && layoutSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in toLaTeX: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Converts a document to a LaTeX template
        /// </summary>
        /// <param name="id">Document ID (unused in this implementation)</param>
        /// <param name="templateId">Template ID</param>
        /// <returns>Success flag</returns>
        public bool convertToLatexTemplate(string id, string templateId)
        {
            try
            {
                // Retrieve all content types
                List<AbstractNode> allContent = new List<AbstractNode>();
                allContent.AddRange(GetContentByType("format"));
                allContent.AddRange(GetContentByType("text"));
                allContent.AddRange(GetContentByType("layout"));
                allContent.AddRange(GetContentByType("math"));
                allContent.AddRange(GetContentByType("lists"));
                allContent.AddRange(GetContentByType("images"));
                
                // Get the template
                Template.Template template = templateManager.ConvertToTemplate(templateId);
                if (template != null)
                {
                    // Merge with template content
                    List<AbstractNode> templateContent = template.GetContent();
                    allContent.AddRange(templateContent);
                    
                    // Apply conversions
                    bool formatSuccess = formatConversionManager.convertFormat(allContent);
                    bool textSuccess = formatConversionManager.convertText(allContent);
                    bool layoutSuccess = formatConversionManager.convertLayout(allContent);
                    
                    return formatSuccess && textSuccess && layoutSuccess;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in convertToLatexTemplate: {ex.Message}");
                return false;
            }
        }
        
        // Helper methods for retrieving content - these now simply call GetContentByType
        private List<AbstractNode> retrieveFormatContent(string id)
        {
            return GetContentByType("format");
        }
        
        private List<AbstractNode> retrieveTextContent(string id)
        {
            return GetContentByType("text");
        }
        
        private List<AbstractNode> retrieveLayoutContent(string id)
        {
            return GetContentByType("layout");
        }
    }
}