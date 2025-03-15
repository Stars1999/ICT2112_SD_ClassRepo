using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class PDFGenerator
{
    private readonly string outputDirectory;

    public PDFGenerator()
    {
        // Define the directory where PDFs will be stored
        outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");

        // Ensure the directory exists
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }
    }

    public async Task<bool> GeneratePDF(string latexContent)
    {
        try
        {
            Console.WriteLine("[INFO] Starting LaTeX to PDF compilation...");

            // Define LaTeX file path
            string latexFilePath = Path.Combine(outputDirectory, "document.tex");

            // Validate LaTeX content
            if (string.IsNullOrWhiteSpace(latexContent) || !latexContent.Contains("\\begin{document}"))
            {
                Console.WriteLine("[ERROR] Invalid LaTeX content.");
                return false;
            }

            // Write LaTeX content to the .tex file
            await File.WriteAllTextAsync(latexFilePath, latexContent);
            Console.WriteLine("[INFO] LaTeX file saved at: " + latexFilePath);

            // Compile LaTeX to PDF
            return CompileWithPdfLaTeX(latexFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            return false;
        }
    }

    private bool CompileWithPdfLaTeX(string latexFilePath)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "pdflatex",
                Arguments = $"-interaction=nonstopmode -output-directory \"{outputDirectory}\" \"{latexFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            Console.WriteLine("[INFO] LaTeX Output:\n" + output);
            Console.WriteLine("[ERROR] LaTeX Errors:\n" + error);

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"[ERROR] LaTeX compilation failed. Exit code: {process.ExitCode}");
                return false;
            }

            Console.WriteLine("[INFO] LaTeX compilation successful.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] LaTeX compilation error: {ex.Message}");
            return false;
        }
    }

    public string GetGeneratedPDFUrl()
    {
        return "/pdfs/document.pdf";
    }
}
