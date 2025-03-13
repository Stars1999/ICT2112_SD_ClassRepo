using System;
using System.Text.Json;

public class LatexGenerator
{
    private string _latexContent;

    public void GenerateLatex(string jsonData)
    {
        using var document = JsonDocument.Parse(jsonData);
        var root = document.RootElement;

        if (!root.TryGetProperty("documents", out JsonElement documents) || documents.GetArrayLength() == 0)
        {
            Console.WriteLine("[ERROR] No documents found.");
            return;
        }

        string compiledLatex = "";

        foreach (var doc in documents.EnumerateArray())
        {
            compiledLatex += doc.GetProperty("LatexContent").GetString() ?? "";
            compiledLatex += "\n%% --------------------------------------------\n";
        }

        _latexContent = compiledLatex;
        Console.WriteLine("[INFO] LaTeX compiled in-memory.");
    }

    public string GetLatexContent()
    {
        return _latexContent;
    }
}
