using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Format
{
    public class FormatManager : IFormatDocument
    {
        private List<AbstractNode> content;

        public bool StartFormatting(List<AbstractNode> content)
        {
            try
            {
                this.content = content;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

                public bool FormatHeadings()
        {
            try
            {
                foreach (AbstractNode child in content)
                {
                    if (child.GetNodeType() == "metadata") return false;

                    String command;
                    switch (child.GetNodeType())
                    {
                        case "h1":
                            command = @"\section";
                            break;
                        case "h2":
                            command = @"\subsection";
                            break;
                        case "h3":
                            command = @"\subsubsection";
                            break;
                        case "h4":
                            command = @"\paragraph";
                            break;
                        case "h5":
                            command = @"\subparagraph";
                            break;
                        default:
                            continue; // Skip non-heading nodes
                    }

                    // Get the text content
                    string formattedText = child.GetContent();
        
                    // Check for styling
                    if (child.GetStyling() != null)
                    {
                        foreach (var style in child.GetStyling())
                        {
                            if (style.ContainsKey("Bold") && (bool)style["Bold"])
                                formattedText = $@"\textbf{{{formattedText}}}";
                            if (style.ContainsKey("Italic") && (bool)style["Italic"])
                                formattedText = $@"\textit{{{formattedText}}}";
                            if (style.ContainsKey("FontColor") && !string.IsNullOrEmpty((string)style["FontColor"]) && (string)style["FontColor"] != "000000")
                                formattedText = $@"\textcolor[HTML]{{{(string)style["FontColor"]}}}{{{formattedText}}}";
                            // Add FontSize checking
                            if (style.ContainsKey("FontSize") && style["FontSize"] != null)
                            {
                                int fontSize = Convert.ToInt32(style["FontSize"]);
                                formattedText = $@"{{\fontsize{{{fontSize}}}{{auto}}\selectfont {formattedText}}}";
                            }
                        }
                    }
                    // Apply the command to the formatted text
                    formattedText = $"{command}{{{formattedText}}}";
        
                    // Set the formatted text back to the child node
                    child.SetContent(formattedText);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public bool FormatParagraphs()
        {
            try
            {
                foreach (AbstractNode child in content)
                {
                    if (!child.GetNodeType().Contains("paragraph")) return false;
                    
                    String command = @"\paragraph";
        
                    // Get the text content
                    string formattedText = child.GetContent();
        
                    // Check for styling
                    if (child.GetStyling() != null)
                    {
                        foreach (var style in child.GetStyling())
                        {
                            if (style.ContainsKey("Bold") && (bool)style["Bold"])
                                formattedText = $@"\textbf{{{formattedText}}}";
                            if (style.ContainsKey("Italic") && (bool)style["Italic"])
                                formattedText = $@"\textit{{{formattedText}}}";
                            if (style.ContainsKey("FontColor") && !string.IsNullOrEmpty((string)style["FontColor"]) && (string)style["FontColor"] != "000000")
                                formattedText = $@"\textcolor[HTML]{{{(string)style["FontColor"]}}}{{{formattedText}}}";
                            // Add FontSize checking
                            if (style.ContainsKey("FontSize") && style["FontSize"] != null)
                            {
                                int fontSize = Convert.ToInt32(style["FontSize"]);
                                formattedText = $@"{{\fontsize{{{fontSize}}}{{auto}}\selectfont {formattedText}}}";
                            }
                        }
                    }
                    // Apply the command to the formatted text
                    formattedText = $"{command}{{{formattedText}}}";
        
                    // Set the formatted text back to the child node
                    child.SetContent(formattedText);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool ProcessMetaData()
        {
            try
            {
                foreach (AbstractNode child in content)
                {
                    // Check if the node type is "metadata"
                    if (child.GetNodeType() == "metadata" && child.GetStyling() != null)
                    {
                        // Initialize variables with default values
                        string filename = string.Empty;
                        string createdDate = string.Empty;
                        string lastModified = string.Empty;
                        string fileSize = string.Empty;
        
                        // Extract metadata from the styling dictionary
                        foreach (var metadata in child.GetStyling())
                        {
                            if (metadata.ContainsKey("filename") && metadata["filename"] != null)
                                filename = metadata["filename"].ToString();
                            if (metadata.ContainsKey("CreatedDate_Internal") && metadata["CreatedDate_Internal"] != null)
                                createdDate = metadata["CreatedDate_Internal"].ToString();
                            if (metadata.ContainsKey("LastModified_Internal") && metadata["LastModified_Internal"] != null)
                                lastModified = metadata["LastModified_Internal"].ToString();
                            if (metadata.ContainsKey("size") && metadata["size"] != null)
                                fileSize = metadata["size"].ToString();
                        }
        
                        // Generate LaTeX metadata commands
                        string latexMetaData = $@"
        \newcommand{{\docFileName}}{{{filename}}}
        \newcommand{{\docCreatedDate}}{{{createdDate}}}
        \newcommand{{\docLastModified}}{{{lastModified}}}
        \newcommand{{\docFileSize}}{{{fileSize}}}";
        
                        // Set the metadata commands to the child node
                        child.SetContent(latexMetaData);
                    }
                }
                return true; // Indicate successful processing
            }
            catch (Exception e)
            {
                // Log the exception if necessary
                Console.WriteLine($"Error in ProcessMetaData: {e.Message}");
                return false; // Indicate failure
            }
        }
        


        public List<AbstractNode> ApplyBaseFormatting()
        {
            return content;
        }


    }

}

