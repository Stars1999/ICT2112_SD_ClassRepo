using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ICT2106WebApp.Utilities;

public class SpecialElementProcessor : IProcessor
{
    public string Type => "special";

    public void convertContent(List<AbstractNode> nodes)
    {
        foreach (var node in nodes)
        {
            try
            {
                if (node is SimpleNode simpleNode)
                {
                    string originalContent = simpleNode.GetContent();
                    string processedContent = ProcessContent(originalContent);
                    simpleNode.SetContent(processedContent);
                }
            }
            catch (Exception ex)
            {
                if (node is SimpleNode simpleNode)
                {
                    simpleNode.SetContent($"\\text{{Error: {ex.Message}}}");
                }
            }
        }
    }

    private string ProcessContent(string input)
    {
        try
        {
            if (!input.TrimStart().StartsWith("{"))
            {
                string strContent = input.Trim();
                string lower = strContent.ToLower();

                if (lower.Contains("new page"))
                    return "\\newpage";

                if (lower.Contains("footnote"))
                    return $"\\footnote{{{strContent}}}";
                else if (lower.Contains("textbox"))
                    return $"\\fbox{{\\parbox{{\\linewidth}}{{{strContent}}}}}";
                else if (lower.Contains("endnote"))
                    return $"\\endnote{{{strContent}}}\n\\theendnotes";
                else
                    return strContent;
            }
            else
            {
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
                if (jsonObject == null || !jsonObject.ContainsKey("type") || !jsonObject.ContainsKey("content"))
                    return "Invalid JSON structure.";

                string jsonType = jsonObject["type"]?.ToString();
                var content = jsonObject["content"];
                JArray styling = jsonObject.ContainsKey("styling") ? jsonObject["styling"] as JArray : null;

                string leftContent = content switch
                {
                    string str => str.Contains("|") ? str.Split('|')[0].Trim() : str.Trim(),
                    JObject obj => obj["left"]?.ToString() ?? "",
                    _ => content?.ToString() ?? ""
                };

                leftContent = FormatContent(leftContent, styling, 0);

                string endnoteSection = (jsonType == "endnote") ? "\\theendnotes\n" : "";

                string latexOutput = jsonType switch
                {
                    "footnote" => $"\\footnote{{{leftContent}}}",
                    "textbox" => $"\\fbox{{\\parbox{{\\linewidth}}{{{leftContent}}}}}",
                    "pagebreak" => "\\newpage",
                    "endnote" => $"\\endnote{{{leftContent}}}\n{endnoteSection}",
                    _ => "Unsupported element type."
                };

                return latexOutput;
            }
        }
        catch (Exception ex)
        {
            return $"\\text{{Error: {ex.Message}}}";
        }
    }

    private string FormatContent(string text, JArray styling, int index)
    {
        if (styling != null && index < styling.Count)
        {
            var styleItem = styling[index];
            bool bold = styleItem["bold"]?.ToObject<bool>() ?? false;
            bool italic = styleItem["italic"]?.ToObject<bool>() ?? false;
            return ApplyStyling(text, bold, italic);
        }
        return text;
    }

    private string ApplyStyling(string text, bool bold, bool italic)
    {
        if (bold)
            text = $"\\textbf{{{text}}}";
        if (italic)
            text = $"\\textit{{{text}}}";
        return text;
    }
}
