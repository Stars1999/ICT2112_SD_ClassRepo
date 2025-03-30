using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ICT2106WebApp.Utilities;

public class ImageProcessor : IProcessor
{
    public string Type => "images";

    public void convertContent(List<AbstractNode> nodes)
    {
        if (nodes == null || nodes.Count == 0)
            return;

        foreach (var node in nodes)
        {
            // Only process nodes that are images
            if (node is SimpleNode simpleNode && node.GetNodeType().Equals("image", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    string latexCommand = ConvertImageToLaTeX(simpleNode);
                    simpleNode.SetContent(latexCommand);
                    simpleNode.SetConverted(true);
                }
                catch (Exception ex)
                {
                    simpleNode.SetContent($"\\text{{Error: {ex.Message}}}");
                }
            }
        }
    }

    private string ConvertImageToLaTeX(SimpleNode node)
    {
        // Get the image filename from the node's content
        string imageFileName = node.GetContent();
        List<Dictionary<string, object>> styling = node.GetStyling();
        string alignment = GetImageAlignment(styling);

        // Retrieve dimensions from styling; use defaults if missing
        int widthEMU = styling.FirstOrDefault()?.ContainsKey("WidthEMU") == true 
            ? Convert.ToInt32(styling.First()["WidthEMU"]) 
            : 12700;
        int heightEMU = styling.FirstOrDefault()?.ContainsKey("HeightEMU") == true 
            ? Convert.ToInt32(styling.First()["HeightEMU"]) 
            : 12700;

        // Convert EMU to pixels (1 EMU = 96 / 12700 pixels)
        float widthInPixels = widthEMU * (96f / 12700f);
        float heightInPixels = heightEMU * (96f / 12700f);

        // Handle horizontal and vertical offsets
        string horizontalOffset = GetHorizontalOffset(styling);
        string verticalOffset = GetVerticalOffset(styling);

        // Start building the LaTeX command for the figure
        string latexCommand = $@"\begin{{figure}}";

        // Add horizontal and vertical offsets before the image if they exist
        if (!string.IsNullOrEmpty(horizontalOffset))
        {
           latexCommand += $@"\hspace*{{{horizontalOffset}}}";
        }
        if (!string.IsNullOrEmpty(verticalOffset))
        {
            latexCommand += $@"\vspace*{{{verticalOffset}}}";
        }

        latexCommand += $@"\{alignment}";

        // Add the image itself
        latexCommand += $@"\includegraphics[width={widthInPixels}px,height={heightInPixels}px]{{{imageFileName}}}";

        // Close the LaTeX figure environment
        latexCommand += @"\end{figure}";

        return latexCommand;
    }

    private string GetImageAlignment(List<Dictionary<string, object>> styling)
    {
        foreach (var style in styling)
        {
            if (style.TryGetValue("Alignment", out var alignmentObj))
            {
                string alignment = alignmentObj.ToString();
                if (alignment.Contains("Left"))
                    return "raggedright";
                else if (alignment.Contains("Right"))
                    return "raggedleft";
                else
                    return "centering";
            }
        }
        return "centering"; // Default to center alignment
    }

   private string GetHorizontalOffset(List<Dictionary<string, object>> styling)
    {
        foreach (var style in styling)
        {
            if (style.TryGetValue("Position", out var positionObj))
            {
                // Extract the Horizontal Offset from the Position string using regex
                string position = positionObj.ToString();
                var match = Regex.Match(position, @"Horizontal Offset:\s*(\d+)");
                if (match.Success)
                {
                    int offset = Convert.ToInt32(match.Groups[1].Value);
                    return $"{offset * 0.007559}cm"; // Convert EMU to cm
                }
            }
        }
        return ""; // No horizontal offset by default
    }

    private string GetVerticalOffset(List<Dictionary<string, object>> styling)
    {
        foreach (var style in styling)
        {
            if (style.TryGetValue("Position", out var positionObj))
            {
                // Extract the Vertical Offset from the Position string using regex
                string position = positionObj.ToString();
                var match = Regex.Match(position, @"Vertical Offset:\s*(\d+)");
                if (match.Success)
                {
                    int offset = Convert.ToInt32(match.Groups[1].Value);
                    return $"{offset * 0.007559}cm"; // Convert EMU to cm
                }
            }
        }
        return ""; // No vertical offset by default
    }
}
