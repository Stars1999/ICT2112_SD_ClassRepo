using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;
using ICT2106WebApp.mod2grp6.Template;
using ICT2106WebApp.mod2grp6.TestCase;

namespace ICT2106WebApp.mod2grp6
{
    public class DocumentManager : IProcessDocument, IProcessTemplate
    {
        // Services and components needed for document operations
        private FormatConversionManager formatConversionManager;
        private TemplateManager templateManager;
        
        // Sample document data from sample.cs or templateSample.cs
        private Dictionary<string, List<AbstractNode>> documentContent;
        private Dictionary<string, List<AbstractNode>> templateDocumentContent;
        
        // Constructor for DocumentManager
        public DocumentManager()
        {
            formatConversionManager = new FormatConversionManager();
            templateManager = new TemplateManager();

            //get nodes from sample.cs in class integration it would be pulled from mod1 
            TestCases sample = new TestCases();
            documentContent = new Dictionary<string, List<AbstractNode>>();

            // These would be retrieved from the Testing class in sample.cs
            documentContent["format"] = sample.HeadingsAndParagraphs;
            documentContent["layout"] = sample.LayoutContent;
            documentContent["paragraph"] = sample.ParagraphContent;
            documentContent["text"] = sample.TextContent;
            documentContent["metadata"] = sample.MetadataContent;
            documentContent["math"] = sample.MathContent;
            documentContent["lists"] = sample.Lists;
            documentContent["images"] = sample.Images;
            documentContent["bibliography"] = sample.BibliographyContent;

            // Get nodes from TemplateSample.cs for template testing
            TemplateSample templateSample = new TemplateSample();
            templateDocumentContent = new Dictionary<string, List<AbstractNode>>();

            // Retrieve content from TemplateSample
            templateDocumentContent["format"] = templateSample.HeadingsAndParagraphs;
            templateDocumentContent["layout"] = templateSample.LayoutContent;
            templateDocumentContent["paragraph"] = templateSample.ParagraphContent;
            templateDocumentContent["text"] = templateSample.TextContent;
            templateDocumentContent["metadata"] = templateSample.MetadataContent;
            templateDocumentContent["math"] = templateSample.MathContent;
            templateDocumentContent["lists"] = templateSample.Lists;
            templateDocumentContent["images"] = templateSample.Images;
            templateDocumentContent["bibliography"] = templateSample.BibliographyContent;
        }
        
        /// <summary>
        /// Retrieve content by content type
        /// </summary>
        public List<AbstractNode> GetContentByType(string contentType, bool useTemplateData = false)
        {
            Dictionary<string, List<AbstractNode>> source = useTemplateData ? templateDocumentContent : documentContent;
            
            if (source.ContainsKey(contentType))
            {
                return source[contentType];
            }
            
            return new List<AbstractNode>();
        }
        
        /// <summary>
        /// Implements IProcessDocument.toLaTeX
        /// </summary>
        public bool toLaTeX(string id)
        {
            // Call the extended version with default useTemplateData = false
            return toLaTeXInternal(id, false);
        }
        
        /// <summary>
        /// Internal implementation with template data flag
        /// </summary>
        private bool toLaTeXInternal(string id, bool useTemplateData)
        {
            try
            {
                // Convert format content
                List<AbstractNode> formatContent = GetContentByType("format", useTemplateData);
                bool formatSuccess = formatConversionManager.convertFormat(formatContent);

                //convert metadata
                List<AbstractNode> metadataContent = GetContentByType("metadata", useTemplateData);
                bool metadataSuccess = formatConversionManager.convertFormat(metadataContent);
                
                // Convert text content
                List<AbstractNode> textContent = GetContentByType("text", useTemplateData);
                bool textSuccess = formatConversionManager.convertText(textContent);
                
                // Convert paragraph content
                List<AbstractNode> paragraphContent = GetContentByType("paragraph", useTemplateData);
                bool paragraphSuccess = formatConversionManager.convertFormat(paragraphContent);
                
                
                // Convert layout content
                List<AbstractNode> layoutContent = GetContentByType("layout", useTemplateData);
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
        /// Implements both IProcessDocument.convertToLatexTemplate and IProcessTemplate.convertToLatexTemplate
        /// </summary>
        public bool convertToLatexTemplate(string id, string templateId)
        {
            // Call the extended version with default useTemplateData = false
            return convertToLatexTemplateInternal(id, templateId, false);
        }
        
        /// <summary>
        /// Internal implementation with template data flag
        /// </summary>
        private bool convertToLatexTemplateInternal(string id, string templateId, bool useTemplateData)
        {
            try
            {
                // Retrieve all content types
                List<AbstractNode> allContent = new List<AbstractNode>();
                allContent.AddRange(GetContentByType("format", useTemplateData));
                allContent.AddRange(GetContentByType("text", useTemplateData));
                allContent.AddRange(GetContentByType("paragraph", useTemplateData));
                allContent.AddRange(GetContentByType("layout", useTemplateData));
                allContent.AddRange(GetContentByType("math", useTemplateData));
                allContent.AddRange(GetContentByType("lists", useTemplateData));
                allContent.AddRange(GetContentByType("images", useTemplateData));
                
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
        
        /// <summary>
        /// Public method that allows using template data for conversion
        /// </summary>
        public bool convertToLatexTemplateWithTemplate(string id, string templateId, bool useTemplateData)
        {
            return convertToLatexTemplateInternal(id, templateId, useTemplateData);
        }
        
        /// <summary>
        /// Public method that allows using template data for LaTeX conversion
        /// </summary>
        public bool toLaTeXWithTemplate(string id, bool useTemplateData)
        {
            return toLaTeXInternal(id, useTemplateData);
        }
        
        // Helper methods for retrieving content - these now simply call GetContentByType
        private List<AbstractNode> retrieveFormatContent(string id, bool useTemplateData = false)
        {
            return GetContentByType("format", useTemplateData);
        }
        
        private List<AbstractNode> retrieveTextContent(string id, bool useTemplateData = false)
        {
            return GetContentByType("text", useTemplateData);
        }
        
        private List<AbstractNode> retrieveParagraphContent(string id, bool useTemplateData = false)
        {
            return GetContentByType("paragraph", useTemplateData);
        }
        
        private List<AbstractNode> retrieveLayoutContent(string id, bool useTemplateData = false)
        {
            return GetContentByType("layout", useTemplateData);
        }
    }
}