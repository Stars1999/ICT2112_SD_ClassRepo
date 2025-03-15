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

            // ✅ Step 4: Replace `\cdot` with `\times` for multiplication
            latexOutput = latexOutput.Replace(@"\cdot", @"\times");

            // ✅ Step 5: Wrap output with `$$ ... $$` for proper LaTeX rendering
            latexOutput = $"$$ {latexOutput} $$";

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

        return equation;
    }
}
