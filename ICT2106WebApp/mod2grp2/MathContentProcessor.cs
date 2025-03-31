using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AngouriMath;
using ICT2106WebApp.Utilities;

public class MathContentProcessor : IProcessor
{
    public string Type => "math";

    public void convertContent(List<AbstractNode> nodes)
    {
        foreach (var node in nodes)
        {
            try
            {
                if (node is SimpleNode simpleNode)
                {
                    string raw = PreprocessEquation(simpleNode.GetContent());
                    var parsed = MathS.FromString(raw);
                    string latex = parsed.Latexise();

                    // Clean-up and replace
                    latex = latex.Replace(@"\left(", "(").Replace(@"\right)", ")");
                    latex = latex.Replace("THEREFORE", @"\therefore")
                                 .Replace("INFINITY", @"\infty")
                                 .Replace("NOTEQUAL", @"\neq")
                                 .Replace("PLUSMINUS", @"\pm")
                                 .Replace("ALPHA", @"\alpha")
                                 .Replace("EXISTS", @"\exists")
                                 .Replace("FORALL", @"\forall")
                                 .Replace("AND", @"\land")
                                 .Replace("IMPLIES", @"\rightarrow")
                                 .Replace(@"\geqslant", @"\geq")
                                 .Replace(@"\cdot", @"\times");

                    latex = Regex.Replace(latex, @"\\log_{(\d+)}\(([^)]+)\)", @"\log_{$1} $2");

                    simpleNode.SetContent($"${latex}$"); // ✅ Set converted LaTeX
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

    private string PreprocessEquation(string equation)
    {
        equation = Regex.Replace(equation, @"√\(([^)]+)\)", @"sqrt($1)");
        equation = equation.Replace("×", "*");
        equation = Regex.Replace(equation, @"cos(\d+[a-zA-Z]?)", @"cos($1)");
        equation = Regex.Replace(equation, @"sin(\d+[a-zA-Z]?)", @"sin($1)");
        equation = Regex.Replace(equation, @"tan(\d+[a-zA-Z]?)", @"tan($1)");
        equation = equation.Replace("≥", ">=").Replace("≤", "<=");
        equation = Regex.Replace(equation, @"log_([a-zA-Z0-9])(\d*[a-zA-Z]*)", @"log($1, $2)");
        equation = equation.Replace("π", "pi");
        equation = equation.Replace("∴", "THEREFORE")
                           .Replace("∞", "INFINITY")
                           .Replace("≠", "NOTEQUAL")
                           .Replace("±", "PLUSMINUS")
                           .Replace("α", "ALPHA")
                           .Replace("∃", "EXISTS ")
                           .Replace("∀", "FORALL ")
                           .Replace("∧", " AND ")
                           .Replace("→", " IMPLIES ");
        return equation;
    }
}
