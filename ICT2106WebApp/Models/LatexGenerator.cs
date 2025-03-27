using System;
using System.Text.Json;

public class LatexGenerator : iGetGeneratedLatex
{
    private string _latexContent = "";
    private readonly iCompileLatex _compiler;

    public LatexGenerator(iCompileLatex compiler)
    {
        _compiler = compiler;
    }

    public void GenerateLatex()
    {
        string jsonData = _compiler.Compile();
        if (string.IsNullOrEmpty(jsonData))
        {
            Console.WriteLine("[ERROR] No JSON received from compiler.");
            return;
        }

        Console.WriteLine($"[DEBUG] LatexGenerator received JSON: {jsonData}");

        try
        {
            var reference = JsonSerializer.Deserialize<Reference>(jsonData);
            if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
            {
                Console.WriteLine("[ERROR] No valid LaTeX documents found.");
                return;
            }

            string compiledLatex = "";
            foreach (var doc in reference.Documents)
            {
                compiledLatex += doc.LatexContent + "\n%% --------------------------------------------\n";
            }

            _latexContent = compiledLatex;
            Console.WriteLine("[INFO] LaTeX compiled successfully.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to process JSON: {ex.Message}");
        }
    }

    public string GetLatexContent()
    {
        return _latexContent;
    }
}
