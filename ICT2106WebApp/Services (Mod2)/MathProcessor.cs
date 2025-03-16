using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AngouriMath;

public class MathProcessor
{
    public List<string> ConvertContent(string equation)
    {
        try
        {
            // ✅ Step 1: Preprocess the equation (replace `√` with `sqrt`)
            equation = PreprocessEquation(equation);

            // ✅ Step 2: Parse the equation using AngouriMath
            var parsedEquation = MathS.FromString(equation);

            // ✅ Step 3: Convert to LaTeX format
            string latexOutput = parsedEquation.Latexise();

            // ✅ Step 4: Remove all unnecessary `\left(...\right)` globally
            latexOutput = latexOutput.Replace(@"\left(", "(").Replace(@"\right)", ")");

            // ✅ Step 4: Replace placeholders with correct LaTeX commands
            latexOutput = latexOutput.Replace("THEREFORE", @"\therefore")
                                    .Replace("INFINITY", @"\infty")
                                    .Replace("NOTEQUAL", @"\neq")
                                    .Replace("PLUSMINUS", @"\pm")
                                    .Replace("ALPHA", @"\alpha");
            
            // ✅ Step 5: Replace placeholders with correct LaTeX commands
            latexOutput = latexOutput.Replace("EXISTS", @"\exists")
                                    .Replace("FORALL", @"\forall")
                                    .Replace("AND", @"\land")
                                    .Replace("IMPLIES", @"\rightarrow");

            // ✅ Step 6: Ensure correct `\log_{b} x` format
            latexOutput = Regex.Replace(latexOutput, @"\\log_{(\d+)}\(([^)]+)\)", @"\log_{$1} $2");

            // ✅ Step 7: Replace `\geqslant` with `\geq` for standard LaTeX
            latexOutput = latexOutput.Replace(@"\geqslant", @"\geq");

            // ✅ Step 8: Replace `\cdot` with `\times` for multiplication
            latexOutput = latexOutput.Replace(@"\cdot", @"\times");

            // ✅ Step 9: Wrap output with `$$ ... $$` for proper LaTeX rendering
            latexOutput = $"$ {latexOutput} $";

            return new List<string> { latexOutput };
        }
        catch (Exception ex)
        {
            return new List<string> { $"Error: {ex.Message}" };
        }
    }

    private string PreprocessEquation(string equation)
    {
        // ✅ Convert square root symbol `√(x)` → `sqrt(x)`
        equation = Regex.Replace(equation, @"√\(([^)]+)\)", @"sqrt($1)");

        // ✅ Replace '×' with '*' for multiplication
        equation = equation.Replace("×", "*");

        // ✅ Ensure trigonometric functions use parentheses
        equation = Regex.Replace(equation, @"cos(\d+[a-zA-Z]?)", @"cos($1)");
        equation = Regex.Replace(equation, @"sin(\d+[a-zA-Z]?)", @"sin($1)");
        equation = Regex.Replace(equation, @"tan(\d+[a-zA-Z]?)", @"tan($1)");

        // ✅ Replace Unicode `≥` and `≤` with `>=` and `<=`
        equation = equation.Replace("≥", ">=").Replace("≤", "<=");

        // ✅ Ensure only the first character is the log base, rest is the argument
        equation = Regex.Replace(equation, @"log_([a-zA-Z0-9])(\d*[a-zA-Z]*)", @"log($1, $2)");

        // ✅ Convert `π` to `pi` before parsing so AngouriMath recognizes it
        equation = equation.Replace("π", "pi");

        // ✅ Replace mathematical symbols with their text equivalents
        equation = equation.Replace("∴", "THEREFORE")
                        .Replace("∞", "INFINITY")
                        .Replace("≠", "NOTEQUAL")
                        .Replace("±", "PLUSMINUS")
                        .Replace("α", "ALPHA");

        // ✅ Replace logical symbols with placeholders
        equation = equation.Replace("∃", "EXISTS ")
                        .Replace("∀", "FORALL ")
                        .Replace("∧", " AND ")
                        .Replace("→", " IMPLIES ");
                        

        return equation;
    }
}
