using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;
using ICT2106WebApp.mod2grp6.Template;
using ICT2106WebApp.mod2grp6.TestCase;
using System.Diagnostics;


namespace ICT2106WebApp.mod2grp6
{
    public class DocumentManager : IProcessDocument, IProcessTemplate
    {
        // Services and components needed for document operations
        private FormatConversionManager formatConversionManager;
        private TemplateManager templateManager;

        // Sample document data from sample.cs
        //remove when submitting
        private Dictionary<string, List<AbstractNode>> documentContent;
        private Dictionary<string, List<AbstractNode>> templateDocumentContent;

        // Constructor for DocumentManager
        public DocumentManager()
        {
            formatConversionManager = new FormatConversionManager();


            // all these to be removed when submitting as a zip
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
            documentContent["special"] = sample.SpecialContent;

            TestCases templateSample = new TestCases();
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
            templateDocumentContent["special"] = templateSample.SpecialContent;
        }

        /// <summary>
        /// Retrieve content by content type
        //this should be removed for zip submission
        public List<AbstractNode> GetContentByType(string contentType)
        {
            if (documentContent.ContainsKey(contentType))
            {
                return documentContent[contentType];
            }

            return new List<AbstractNode>();
        }

        //this should be removed for zip submission
        public List<AbstractNode> GetContentForTemplateByType(string contentType)
        {
            if (documentContent.ContainsKey(contentType))
            {
                return templateDocumentContent[contentType];
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
                List<AbstractNode> formatContent = documentContent["format"];
                bool formatSuccess = formatConversionManager.convertFormat(formatContent);

                //convert metadata
                List<AbstractNode> metadataContent = documentContent["metadata"];
                bool metadataSuccess = formatConversionManager.convertFormat(metadataContent);

                // Convert text content
                List<AbstractNode> textContent = documentContent["text"];
                bool textSuccess = formatConversionManager.convertText(textContent);

                // Convert paragraph content
                List<AbstractNode> paragraphContent = documentContent["paragraph"];
                bool paragraphSuccess = formatConversionManager.convertFormat(paragraphContent);


                // Convert layout content
                List<AbstractNode> layoutContent = documentContent["layout"];
                bool layoutSuccess = formatConversionManager.convertLayout(layoutContent);

                // We'd also handle math content, lists, images, and bibliography in a real implementation

                // MOD2GRP2 ADVANCED CONTENT (Math, List, Image)
                
            var processorRegistry = new Dictionary<string, IProcessor>
            {
                { "math", new MathContentProcessor() },
                { "special", new SpecialElementProcessor() },
                // Add more without changing the manager!
            };
                var advancedManager = new AdvancedConversionManager(documentContent, processorRegistry);
                advancedManager.getContent();

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
            if(templateManager == null)
                templateManager = new TemplateManager();
        
            try
            {
                // Step 1: Retrieve document content
                List<AbstractNode> allContent = new List<AbstractNode>();
                
                // Retrieve all document content types (headings, paragraphs, etc.)
                allContent.AddRange(GetContentByType("format"));
                allContent.AddRange(GetContentByType("text"));
                allContent.AddRange(GetContentByType("paragraph"));
                allContent.AddRange(GetContentByType("layout"));
                allContent.AddRange(GetContentByType("math"));
                allContent.AddRange(GetContentByType("lists"));
                allContent.AddRange(GetContentByType("images"));
                allContent.AddRange(GetContentByType("special"));

                // Step 2: Retrieve the template by templateId
                var templateTask = templateManager.getTemplate(templateId);
                templateTask.Wait(); // Wait for the async task to complete
                Template.Template template = templateTask.Result;
                if (template != null)
                {
                    // Step 3: Merge template content with the document content
                    List<AbstractNode> templateContent = template.getContent();
                    allContent.AddRange(templateContent);
                    
                    // Step 4: Apply conversions for the format, text, and layout
                    bool formatSuccess = formatConversionManager.convertFormat(allContent);
                    bool textSuccess = formatConversionManager.convertText(allContent);
                    bool layoutSuccess = formatConversionManager.convertLayout(allContent);
                    
                    return formatSuccess && textSuccess && layoutSuccess;
                }

                // If template is not found, return false
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