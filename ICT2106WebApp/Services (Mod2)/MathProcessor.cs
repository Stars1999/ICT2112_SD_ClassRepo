using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MathNet.Symbolics;

public class MathProcessor
{
    public List<string> ConvertContent(string equation)
    {
        try
        {
            // ✅ Preprocess the equation before parsing
            equation = PreprocessEquation(equation);

            // ✅ Parse the modified equation
            var parsedEquation = Infix.ParseOrThrow(equation);

            // ✅ Convert to LaTeX format
            return new List<string> { LaTeX.Format(parsedEquation) };
        }
        catch (Exception ex)
        {
            return new List<string> { $"Error: {ex.Message}" };
        }
    }

     private string PreprocessEquation(string equation)
    {
        // ✅ Convert square root notation `√(x)` → `sqrt(x)`
        equation = Regex.Replace(equation, @"√\(([^)]+)\)", @"sqrt($1)");

        // ✅ Convert fractions `(a/b)` → `\frac{a}{b}`
        equation = Regex.Replace(equation, @"\((\d+)/(\d+)\)", @"\frac{$1}{$2}");

        // ✅ Ensure multiplication between `\frac{}` and functions like `sqrt(x)`
        equation = Regex.Replace(equation, @"(\\frac{\d+}{\d+})\s*(sqrt|sin|cos|tan|log|ln)", @"$1 * $2");

        // ✅ Ensure explicit multiplication for numbers before `sqrt(x)`
        equation = Regex.Replace(equation, @"(\d)\s*(sqrt|sin|cos|tan|log|ln)", @"$1 * $2");

        // ✅ Ensure explicit multiplication for variables before `sqrt(x)`
        equation = Regex.Replace(equation, @"([a-zA-Z])\s*(sqrt|sin|cos|tan|log|ln)", @"$1 * $2");

        // ✅ Convert implicit multiplication (e.g., `5x` → `5*x`)
        equation = Regex.Replace(equation, @"(\d)([a-zA-Z])", @"$1*$2");



        return equation;
    }
}
