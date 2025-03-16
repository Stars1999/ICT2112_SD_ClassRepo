using System.Globalization;
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
                    String command;
                    switch (child.GetNodeType())
                    {
                        case "h1":
                            command = @"\section*";
                            break;
                        case "h2":
                            command = @"\subsection*";
                            break;
                        case "h3":
                            command = @"\subsubsection*";
                            break;
                        case "h4":
                            command = @"\paragraph*";
                            break;
                        case "h5":
                            command = @"\subparagraph*";
                            break;
                        default:
                            continue; // Skip non-heading nodes
                    }

                    // Get the text content
                    string formattedText = child.GetContent();

                    // Check for bold or italic styling
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
            //somehow got here means thrs an issue
            return false;
        }


        public bool FormatParagraphs()
        {
            try
            {
                foreach (AbstractNode child in content)
                {
                    String command = @"\paragraph*";

                    // Get the text content
                    string formattedText = child.GetContent();

                    // Check for bold or italic styling
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
            //somehow got here means thrs an issue
            return false;
        }


        public bool ProcessMetaData() { return true; }
        /*public bool ProcessMetaData()
        {
            try
            {
                foreach (AbstractNode child: content){
                    string filename = child.getFilename();
                    string createdDate = child.getCreatedDate();
                    string lastModified = child.GetLastModified();
                    string fileSize = child.getFileSize();

                    string latexMetaData = $@"
                                    \begin{{itemize}}
                                        \item \textbf{{File Name:}} {filename}
                                        \item \textbf{{Created Date:}} {createdDate}
                                        \item \textbf{{Last Modified:}} {lastModified}
                                        \item \textbf{{Size:}} {fileSize}
                                    \end{{itemize}}
                                    ";
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }
        */


        public List<AbstractNode> ApplyBaseFormatting()
        {
            return content;
        }


    }

}

