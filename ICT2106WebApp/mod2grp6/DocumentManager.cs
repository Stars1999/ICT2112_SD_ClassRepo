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
        
        //will be the class by mod1 called NodeTraverser
        //private NodeTraverser nodeTraversal;

        
        // Constructor for DocumentManager
        public DocumentManager()
        {
            formatConversionManager = new FormatConversionManager();
            templateManager = new TemplateManager();
            // In a real implementation, nodeTraversal would be instantiated or injected
        }

        
        /// Converts document to LaTeX format
        public bool toLaTeX(string id)
        {
            try
            {
                // Retrieve the document data based on id
                // This would involve fetching format, text, and layout data

                // Convert format
                List<AbstractNode> formatContent = retrieveFormatContent(id);
                bool formatSuccess = formatConversionManager.convertFormat(formatContent);
                formatContent = formatConversionManager.getContent();

                // Convert text
                List<AbstractNode> textContent = retrieveTextContent(id);
                bool textSuccess = formatConversionManager.convertText(textContent);
                textContent = formatConversionManager.getContent();

                // Convert layout
                List<AbstractNode> layoutContent = retrieveLayoutContent(id);
                bool layoutSuccess = formatConversionManager.convertLayout(layoutContent);
                layoutContent = formatConversionManager.getContent();

                if(formatSuccess){
                    //save the updated content
                    //nodeTraversal.updateLatexContent(formatContent)
                }

                if (textSuccess)
                {
                    //save the updated content
                    //nodeTraversal.updateLatexContent(textContent)
                }

                if (layoutSuccess)
                {
                    //save the updated content
                    //nodeTraversal.updateLatexContent(layoutContent)
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                Console.WriteLine($"Error in toLaTeX: {ex.Message}");
                return false;
            }
        }


         /// Converts a document to a LaTeX template (applying a specific template, e.g., two-column layout)
        public bool convertToLatexTemplate(string id, string templateId)
        {
            try
            {
                // Retrieve the document content (format, text, and layout)
                List<AbstractNode> documentContent = retrieveFormatContent(id);
                documentContent.AddRange(retrieveTextContent(id));
                documentContent.AddRange(retrieveLayoutContent(id));

                // Get the template by its ID using TemplateManager
                Template.Template template = templateManager.ConvertToTemplate(templateId);  // This applies the template, including two-column layout
                if (template != null)
                {
                    // Merge the template content with the document content
                    List<AbstractNode> templateContent = template.GetContent();
                    documentContent.AddRange(templateContent); // Combine document content with template content

                    // Process the LaTeX content (applying format, text, and layout)
                    bool formatSuccess = formatConversionManager.convertFormat(documentContent);
                    bool textSuccess = formatConversionManager.convertText(documentContent);
                    bool layoutSuccess = formatConversionManager.convertLayout(documentContent);

                    // Return success if all steps succeed
                    return formatSuccess && textSuccess && layoutSuccess;
                }

                return false;  // If the template was not found
            }
            catch (Exception ex)
            {
                // Log exception if needed
                Console.WriteLine($"Error in convertToLatexTemplate: {ex.Message}");
                return false;
            }
        }


        /// Helper method to retrieve format content
        private List<AbstractNode> retrieveFormatContent(string id)
        {
            // In a real implementation, this would retrieve format content from a data source
            return null;
        }

        
        /// Helper method to retrieve text content
        private List<AbstractNode> retrieveTextContent(string id)
        {
            // In a real implementation, this would retrieve text content from a data source
            return null;
        }

        
        /// Helper method to retrieve layout content
        private List<AbstractNode> retrieveLayoutContent(string id)
        {
            // In a real implementation, this would retrieve layout content from a data source
            return null;
        }
    }
}