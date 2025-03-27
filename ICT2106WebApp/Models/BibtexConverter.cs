using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

public class BibTeXConverter : iConversionStatus //Implements the interface
{
    private string _updatedJson;
    private readonly IScannerFactory _citationFactory;
    private readonly IScannerFactory _bibliographyFactory;
    private string _preferredStyle; // Default APA, can be changed
    private readonly IInsertBibTex _bibtexMapper;

    /// <summary>
    /// Constructor for BibTeXConverter
    /// </summary>
    public BibTeXConverter(IScannerFactory citationFactory, IScannerFactory bibliographyFactory, IInsertBibTex bibtexMapper, string preferredStyle = "apa")
    {
        _citationFactory = citationFactory;
        _bibliographyFactory = bibliographyFactory;
        _preferredStyle = preferredStyle.ToLower(); // Ensure lowercase for consistency
        _bibtexMapper = bibtexMapper;

    }

    private string DetectCitationStyle(string latexContent)
    {
        if (string.IsNullOrWhiteSpace(latexContent)) return "apa"; // Default fallback

        // APA Example: (Smith, 2019)
        if (Regex.IsMatch(latexContent, @"\([A-Z][a-z]+, \d{4}\)")) return "apa";

        // MLA Example: (Smith 45)
        if (Regex.IsMatch(latexContent, @"\([A-Z][a-z]+ \d+\)")) return "mla";

        return "apa"; // Fallback
    }

    /// <summary>
    /// Converts citations and bibliography based on the preferred style.
    /// </summary>
    public string ConvertCitationsAndBibliography(string jsonData, string overrideStyle = null)
    public string ConvertCitationsAndBibliography(string jsonData, string overrideStyle = null)
    {
        Console.WriteLine($"[DEBUG] Received JSON: {jsonData}");
        

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            Console.WriteLine("[ERROR] No input JSON received in BibTeXConverter.");
            return null;
        }

        try
        {
            // ✅ Deserialize directly into a list of BibliographyDocument
            var reference = JsonSerializer.Deserialize<Reference>(jsonData);
            if (reference == null || reference.Documents == null || reference.Documents.Count == 0)
            {
                Console.WriteLine("[ERROR] No documents found in JSON.");
                return null;
            }


            Console.WriteLine($"[DEBUG] Successfully deserialized {reference.Documents.Count} documents.");

            var updatedDocuments = new List<BibliographyDocument>();

            foreach (var doc in reference.Documents)
            {
        
                try
                {
                    string latexContent = doc.OriginalLatexContent?.Trim() ?? doc.LatexContent?.Trim() ?? "";

                    if (!string.IsNullOrWhiteSpace(overrideStyle))
                    {
                        _preferredStyle = overrideStyle.ToLower();
                        Console.WriteLine($"[INFO] Using override style: {_preferredStyle}");
                    }
                    else
                    {
                        _preferredStyle = DetectCitationStyle(latexContent);
                        Console.WriteLine($"[INFO] Detected citation style: {_preferredStyle}");
                    }

                    // Apply citation formatting
                    if (_preferredStyle == "apa")
                    {
                        IAPA apaScanner = _citationFactory.CreateAPA();
                        latexContent = apaScanner.FormatCitations(latexContent);

                        IAPA apaBibliographyScanner = _bibliographyFactory.CreateAPA();
                        latexContent = apaBibliographyScanner.FormatBibliographies(latexContent);

                        apaScanner.ApplyAPAFormatting();
                    }
                    else if (_preferredStyle == "mla")
                    {
                        IMLA mlaScanner = _citationFactory.CreateMLA();
                        latexContent = mlaScanner.FormatCitations(latexContent);

                        IMLA mlaBibliographyScanner = _bibliographyFactory.CreateMLA();
                        latexContent = mlaBibliographyScanner.FormatBibliographies(latexContent);

                        mlaScanner.ApplyMLAFormatting();
                    }

                    // Clean LaTeX content formatting
                    latexContent = SanitizeLatexContent(latexContent);

                    updatedDocuments.Add(new BibliographyDocument
                    {
                        Title = doc.Title,
                        Author = doc.Author,
                        Date = doc.Date,
                        LatexContent = latexContent
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process '{doc.Title}'. Exception: {ex.Message}");
                }
            }

            string updatedJson = JsonSerializer.Serialize(new Reference { Documents = updatedDocuments }, new JsonSerializerOptions { WriteIndented = true });

            if (_bibtexMapper == null)
                {
                    Console.WriteLine("[FATAL] BibTeXConverter received a NULL bibtexMapper!");
                }
                else
                {
                    Console.WriteLine("[DEBUG] Preparing to insert converted JSON into MongoDB...");
                    _bibtexMapper.SetUpdatedJson(updatedJson);
                    SetUpdatedJson(updatedJson);
                    Console.WriteLine("[DEBUG] Called SetUpdatedJson successfully.");
                }
            

            return updatedJson;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] JSON deserialization error: {ex.Message}");
            return null;
        }
    }
    
    private string SanitizeLatexContent(string content)
    {
        // Fix common broken LaTeX syntax issues
        content = content.Replace(@"{article)", "{article}");
        content = content.Replace(@"\title{", "\\title{");
        content = content.Replace(@"\author{", "\\author{");
        content = content.Replace(@"\date{", "\\date{");
        content = content.Replace(@"\begin{document)", "\\begin{document}");
        content = content.Replace(@"\end{document)", "\\end{document}");
        content = content.Replace(@"\section{References)", "\\section{References}");

        // Remove extra closing braces if too many
        content = Regex.Replace(content, @"\}{2,}", "}");

        return content;
    }



    /// <summary>
    /// ✅ Fetches the conversion status.
    /// Returns true if the converted JSON has been updated in memory.
    /// </summary>
     public void SetUpdatedJson(string convertedJson)
    {
        _updatedJson = convertedJson;
    }

    public string GetUpdatedJson()
    {
        return _updatedJson;
    }

    public bool fetchConversionStatus()
    {
        return !string.IsNullOrEmpty(_updatedJson);
    }
}