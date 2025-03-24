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
        
        // Sample document data from sample.cs
        private Dictionary<string, List<AbstractNode>> documentContent;
        
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

        }
        
        /// <summary>
        /// Retrieve content by content type
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
        public bool toLaTeX(string id)
        {
            try
            {
                // Convert format content
                List<AbstractNode> formatContent = GetContentByType("format");
                bool formatSuccess = formatConversionManager.convertFormat(formatContent);

                //convert metadata
                List<AbstractNode> metadataContent = GetContentByType("metadata");
                bool metadataSuccess = formatConversionManager.convertFormat(metadataContent);
                
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
        
        
    }
}