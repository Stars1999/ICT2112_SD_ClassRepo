using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class PDFGenerator
{
    private readonly string outputDirectory;
    private readonly EditorDocumentMapper _editorDocMapper;

    public PDFGenerator(EditorDocumentMapper editorDocMapper)
    {
        _editorDocMapper = editorDocMapper;

        outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }
    }

    public async Task<bool> GeneratePDF()
    {
        try
        {
            //Get the latest document from MongoDB
            var latestDoc = await _editorDocMapper.GetLatestAsync();

            if (latestDoc == null || string.IsNullOrWhiteSpace(latestDoc.LatexContent))
            {
                Console.WriteLine("[ERROR] No LaTeX content found in the latest EditorDocument.");
                return false;
            }

            string latexContent = latestDoc.LatexContent;
            Console.WriteLine("[INFO] Using latest EditorDocument LaTeX content.");

            // Write to .tex file
            string latexFilePath = Path.Combine(outputDirectory, "document.tex");
            await File.WriteAllTextAsync(latexFilePath, latexContent);
            Console.WriteLine("[INFO] LaTeX file saved at: " + latexFilePath);

            // Compile it
            return CompileWithPdfLaTeX(latexFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] PDF Generation Exception: {ex.Message}");
            return false;
        }
    }

    private bool CompileWithPdfLaTeX(string latexFilePath)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "pdflatex",
                Arguments = $"-interaction=nonstopmode -output-directory \"{outputDirectory}\" \"{latexFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine("[LATEX OUTPUT] " + output);
            if (!string.IsNullOrEmpty(error)) Console.WriteLine("[LATEX ERROR] " + error);

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"[ERROR] LaTeX failed with exit code: {process.ExitCode}");
                return false;
            }

            Console.WriteLine("[INFO] PDF compilation successful.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Compilation failed: {ex.Message}");
            return false;
        }
    }

    public string GetGeneratedPDFUrl()
    {
        return "/pdfs/document.pdf";
    }
}