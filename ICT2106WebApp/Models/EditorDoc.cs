using System;

public class EditorDoc
{
    private static string _latexContent = ""; // Static variable to persist across requests

    public void SetLatexContent(string latexContent)
    {
        if (string.IsNullOrEmpty(latexContent))
        {
            Console.WriteLine("[ERROR] Attempted to store empty LaTeX content in EditorDoc.");
        }
        else
        {
            _latexContent = latexContent;
            Console.WriteLine("[INFO] LaTeX content stored successfully.");
        }
    }

    public string GetLatexContent()
    {
        if (string.IsNullOrEmpty(_latexContent))
        {
            Console.WriteLine("[WARNING] No LaTeX content found in EditorDoc.");
        }
        return _latexContent;
    }
}
