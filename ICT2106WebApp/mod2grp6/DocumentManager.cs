using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;
using ICT2106WebApp.mod2grp6.Template;
using ICT2106WebApp.mod2grp6.Format;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.mod2grp6.Text;

namespace ICT2106WebApp.mod2grp6
{
    
    /// Manager class responsible for document processing operations
    /// Implementation based on class diagram
    
    public class DocumentManager : IProcessDocument, IProcessTemplate
    {
        // Services and components needed for document operations
        private FormatConversionManager formatConversionManager;
        private TemplateManager templateManager;
        private iNodeTraversal nodeTraversal;

        
        /// Constructor for DocumentManager
        
        public DocumentManager()
        {
            formatConversionManager = new FormatConversionManager();
            templateManager = new TemplateManager();
            // In a real implementation, nodeTraversal would be instantiated or injected
        }

        
        /// Converts document to LaTeX format
        /// Implementation of IProcessDocument interface
        
        /// <param name="id">Document identifier</param>
        /// <returns>LaTeX document</returns>
        public LatexDocument toLaTeX(string id)
        {
            try
            {
                // Retrieve the document data based on id
                // This would involve fetching format, text, and layout data
                
                // Convert format
                Format.Format formatContent = retrieveFormatContent(id);
                bool formatSuccess = formatConversionManager.convertFormat(formatContent);
                
                // Convert text
                Text.Text textContent = retrieveTextContent(id);
                bool textSuccess = formatConversionManager.convertText(textContent);
                
                // Convert layout
                Layout.Layout layoutContent = retrieveLayoutContent(id);
                bool layoutSuccess = formatConversionManager.convertLayout(layoutContent);
                
                if (formatSuccess && textSuccess && layoutSuccess)
                {
                    // Get the converted content
                    List<AbstractNode> convertedContent = formatConversionManager.getContent();
                    
                    // Use nodeTraversal to process the content for LaTeX
                    List<AbstractNode_SDM> processedNodes = nodeTraversal.traverseNode(convertedContent);
                    
                    // Create a LaTeX document
                    LatexDocument latexDocument = new LatexDocument();
                    
                    // Update the LaTeX document with processed nodes
                    bool updateSuccess = nodeTraversal.updateLatexDoument(processedNodes);
                    
                    if (updateSuccess)
                    {
                        return latexDocument;
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                Console.WriteLine($"Error in toLaTeX: {ex.Message}");
                return null;
            }
        }

        
        /// Converts a document to a LaTeX template
        /// Implementation of IProcessTemplate interface
        
        /// <param name="id">Document identifier</param>
        /// <param name="templateid">Template identifier</param>
        /// <returns>LaTeX document based on the template</returns>
        public LaTeXDocument convertToLatexTemplate(string id, string templateid)
        {
            try
            {
                // Get the template
                Template_RDM template = templateManager.getTemplate(templateid);
                
                if (template == null)
                {
                    return null;
                }
                
                // Convert the document to LaTeX
                LatexDocument baseDocument = toLaTeX(id);
                
                if (baseDocument == null)
                {
                    return null;
                }
                
                // Apply template to the LaTeX document
                // In a real implementation, this would involve merging the document with template
                LaTeXDocument templateDocument = new LaTeXDocument();
                
                // Apply template content from Template_RDM to templateDocument
                List<AbstractNode> templateContent = template.getContent();
                // Process template content and apply to document
                
                // Notify observers about the template usage
                templateManager.notifyObservers(templateid);
                
                return templateDocument;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                Console.WriteLine($"Error in convertToLatexTemplate: {ex.Message}");
                return null;
            }
        }

        
        /// Helper method to retrieve format content
        
        /// <param name="id">Document identifier</param>
        /// <returns>Format content</returns>
        private Format.Format retrieveFormatContent(string id)
        {
            // In a real implementation, this would retrieve format content from a data source
            // For now, return a new Format instance
            return new Format.Format();
        }

        
        /// Helper method to retrieve text content
        
        /// <param name="id">Document identifier</param>
        /// <returns>Text content</returns>
        private Text.Text retrieveTextContent(string id)
        {
            // In a real implementation, this would retrieve text content from a data source
            // For now, return a new Text instance
            return new Text.Text();
        }

        
        /// Helper method to retrieve layout content
        
        /// <param name="id">Document identifier</param>
        /// <returns>Layout content</returns>
        private Layout.Layout retrieveLayoutContent(string id)
        {
            // In a real implementation, this would retrieve layout content from a data source
            // For now, return a new Layout instance
            return new Layout.Layout();
        }
    }
}