using System;
using System.Collections.Generic;

public class ListProcessor{
    public string ConvertToLatex(Dictionary<string, object> json)
    {
        // Validate JSON structure
        if (!json.ContainsKey("type") || !json.ContainsKey("content"))
        {
            return "Invalid JSON structure.";
        }

        string type = json["type"]?.ToString();
        string content = json["content"]?.ToString()?.Trim();  // Ensure content is a string

        if (string.IsNullOrEmpty(content))
        {
            return "Content cannot be empty.";
        }

        // Determine the appropriate LaTeX list type and bullet type
        string listEnvironment = type switch
        {
            // Itemize types (bulleted lists)
            "bulleted_list" => "itemize",                    // Default bullet
            "hollow_bulleted_list" => "itemize",             // Hollow bullet
            "square_bulleted_list" => "itemize",             // Square bullet
            "diamond_bulleted_list" => "itemize",            // Diamond bullet
            "arrow_bulleted_list" => "itemize",              // Arrow bullet
            "checkmark_bulleted_list" => "itemize",          // Checkmark bullet
            "dash_bulleted_list" => "itemize",               // Dash bullet

            // Enumerate types (numbered lists)
            "numbered_list" or "numbered_parenthesis_list" or "roman_numeral_list" or
            "lowercase_roman_numeral_list" or "uppercase_lettered_list" or "lowercase_lettered_list" or
            "lowercase_lettered_parenthesis_list" => "enumerate",

            _ => "unknown"
        };

        if (listEnvironment == "unknown")
        {
            return "Unsupported list type.";
        }

        // Set up LaTeX bullet point style for itemize lists
        string bulletStyle = type switch
        {
            "hollow_bulleted_list" => @"\renewcommand{\labelitemi}{\ensuremath{\circ}}",   // Hollow circle
            "square_bulleted_list" => @"\renewcommand{\labelitemi}{$\square$}",               // Square bullet
            "diamond_bulleted_list" => @"\renewcommand{\labelitemi}{\ding{110}}",             // Diamond bullet (requires \usepackage{pifont})
            "arrow_bulleted_list" => @"\renewcommand{\labelitemi}{$\rightarrow$}",             // Arrow bullet
            "checkmark_bulleted_list" => @"\renewcommand{\labelitemi}{$\checkmark$}",         // Checkmark bullet (requires \usepackage{amssymb})
            "dash_bulleted_list" => @"\renewcommand{\labelitemi}{$\textendash$}",              // Dash bullet
            _ => ""
        };

        // Special numbering format for specific enumerate list types
        string specialFormat = type switch
        {
            "roman_numeral_list" => @"    \renewcommand{\labelenumi}{\roman{enumi}.}",
            "lowercase_roman_numeral_list" => @"    \renewcommand{\labelenumi}{\roman{enumi}.}",
            "uppercase_lettered_list" => @"    \renewcommand{\labelenumi}{\Alph{enumi}.}",
            "lowercase_lettered_list" => @"    \renewcommand{\labelenumi}{\alph{enumi}.}",
            "lowercase_lettered_parenthesis_list" => @"    \renewcommand{\labelenumi}{\alph{enumi})}",
            "numbered_parenthesis_list" => @"    \renewcommand{\labelenumi}{\arabic{enumi})}",
            _ => ""
        };

        // If it's an itemize type (bulleted list), use the custom bullet style
        if (listEnvironment == "itemize")
        {
            return $@"\begin{{itemize}}
    {bulletStyle}
        \item {content}
    \end{{itemize}}";
        }

        // For enumerate (numbered lists)
        return specialFormat == ""
            ? $@"\begin{{{listEnvironment}}}
        \item {content}
    \end{{{listEnvironment}}}"
            : $@"\begin{{enumerate}}
    {specialFormat}
        \item {content}
    \end{{enumerate}}";
    }
}


