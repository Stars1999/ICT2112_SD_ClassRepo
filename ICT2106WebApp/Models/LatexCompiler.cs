using System;

public class LatexCompiler:iCompileLatex
{
    private readonly iConversionStatus _converter;

    public LatexCompiler(iConversionStatus converter)
    {
        _converter = converter;
    }

    public string Compile()
    {
        if (!_converter.fetchConversionStatus())
        {
            Console.WriteLine("[ERROR] No converted JSON available.");
            return null;
        }

        string json = _converter.GetUpdatedJson();
        // Use json to generate LaTeX or forward to generator...
        Console.WriteLine("[INFO] LaTeXCompiler is using converted JSON.");
        return json;
    }
}
