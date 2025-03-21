using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;
using ICT2106WebApp.mod2grp6.Template;
using ICT2106WebApp.mod2grp6.Format;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.mod2grp6.Text;
// Import the TestCase namespace with an alias to avoid ambiguity
using TestCaseNamespace = ICT2106WebApp.mod2grp6.TestCase;

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
            
            // Initialize content from TestCase in sample.cs
            InitializeFromSample();
        }
        
        // Initialize document content from the TestCase class in sample.cs
        private void InitializeFromSample()
        {
            documentContent = new Dictionary<string, List<AbstractNode>>();
            
            // Create an instance of the TestCase class to access its data
            TestCaseNamespace.TestCase testCase = new TestCaseNamespace.TestCase();
            
            // Map the TestCase properties to our document content dictionary
            documentContent["format"] = testCase.HeadingsAndParagraphs;
            documentContent["layout"] = testCase.LayoutContent;
            documentContent["paragraph"] = testCase.ParagraphContent;
            documentContent["text"] = testCase.TextContent;
            
            // Metadata is likely private in TestCase class, so use original metadata
            documentContent["metadata"] = new List<AbstractNode>
            {
                new SimpleNode(1, "metadata", "CreatedDate_Internal: 2025-02-21 05:57:00Z", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "CreatedDate_Internal", "2025-02-21 05:57:00Z" } } }),
                new SimpleNode(2, "metadata", "LastModified_Internal: 2025-03-17 06:25:00Z", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "LastModified_Internal", "2025-03-17 06:25:00Z" } } }),
                new SimpleNode(3, "metadata", "filename: Datarepository_zx_v2.docx", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "filename", "Datarepository_zx_v2.docx" } } }),
                new SimpleNode(4, "metadata", "size: 2148554", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "size", "2148554" } } })
            };
            
            documentContent["math"] = testCase.MathContent;
            documentContent["lists"] = testCase.Lists;
            documentContent["images"] = testCase.Images;
            documentContent["bibliography"] = testCase.Bibliography;
        }
        
        /// <summary>
        /// Retrieve content by content type
        /// </summary>
        /// <param name="contentType">Type of content (format, layout, text, paragraph, metadata, math, lists, images, bibliography)</param>
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
                
                // Convert paragraph content
                List<AbstractNode> paragraphContent = GetContentByType("paragraph");
                bool paragraphSuccess = formatConversionManager.convertFormat(paragraphContent);
                
                // Convert layout content
                List<AbstractNode> layoutContent = GetContentByType("layout");
                bool layoutSuccess = formatConversionManager.convertLayout(layoutContent);
                
                // We'd also handle math content, lists, images, and bibliography in a real implementation
                
                return formatSuccess && textSuccess && paragraphSuccess && layoutSuccess;
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
                allContent.AddRange(GetContentByType("paragraph"));
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
        
        private List<AbstractNode> retrieveParagraphContent(string id)
        {
            return GetContentByType("paragraph");
        }
        
        private List<AbstractNode> retrieveLayoutContent(string id)
        {
            return GetContentByType("layout");
        }
    }
}