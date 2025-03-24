// public static Dictionary<string, object> ExtractBibliography3(WordprocessingDocument doc)
// {
// 	var body = doc.MainDocumentPart.Document.Body;
// 	var bibliographyText = "";
// 	bool foundBibliography = false;

// 	// Look for SimpleField elements with BIBLIOGRAPHY instruction
// 	// foreach (SimpleField field in body.Descendants<SimpleField>())
// 	// {
// 	// 	string instruction = field.Instruction?.Value?.Trim() ?? "";

// 	// 	if (instruction.StartsWith("BIBLIOGRAPHY"))
// 	// 	{

// 	// 		foundBibliography = true;
// 	// 		bibliographyText = field.InnerText;
// 	// 		Console.WriteLine($"found 1 bibliography: {bibliographyText}");
// 	// 		break;
// 	// 	}
// 	// }

// 	// If no SimpleField with BIBLIOGRAPHY found, check for paragraphs that might contain bibliography
// 	if (!foundBibliography)
// 	{
// 		foreach (var paragraph in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
// 		{
// 			string paraText = paragraph.InnerText.Trim();

// 			// Check if it's a References or Bibliography section
// 			if (paraText.StartsWith("References", StringComparison.OrdinalIgnoreCase) ||
// 				paraText.StartsWith("Bibliography", StringComparison.OrdinalIgnoreCase))
// 			{
// 				Console.WriteLine("found 2\n");
// 				// Found the bibliography heading, now collect all subsequent paragraphs
// 				var currentPara = paragraph.NextSibling();
// 				while (currentPara != null && currentPara is DocumentFormat.OpenXml.Wordprocessing.Paragraph)
// 				{
// 					bibliographyText += ((DocumentFormat.OpenXml.Wordprocessing.Paragraph)currentPara).InnerText + "\n";
// 					currentPara = currentPara.NextSibling();
// 				}

// 				foundBibliography = true;
// 				break;
// 			}
// 		}
// 	}

// 	return new Dictionary<string, object>
// 	{
// 		{ "type", "bibliography" },
// 		{ "content", bibliographyText },
// 		{ "found", foundBibliography }
// 	};
// }


// // test2
// public static List<Dictionary<string, string>> ExtractBibliography2(WordprocessingDocument doc)
// {

// 	var bibliographyEntries = new List<Dictionary<string, string>>();

// 	// ✅ Step 1: Get CustomXmlParts
// 	var customXmlParts = doc.MainDocumentPart?.CustomXmlParts;
// 	if (customXmlParts == null)
// 	{
// 		Console.WriteLine("⚠️ No CustomXmlParts found in the document.");
// 		return bibliographyEntries;
// 	}

// 	Console.WriteLine("found something here for bib2\n");
// 	// ✅ Step 2: Search for Bibliography XML
// 	foreach (var xmlPart in customXmlParts)
// 	{
// 		try
// 		{
// 			XDocument xmlDoc = XDocument.Load(xmlPart.GetStream());

// 			// ✅ Step 3: Find the <Sources> tag
// 			var sources = xmlDoc.Descendants()
// 								.Where(e => e.Name.LocalName == "Source");

// 			foreach (var source in sources)
// 			{
// 				var entry = new Dictionary<string, string>
// 				{
// 					["Tag"] = source.Element(source.Name.Namespace + "Tag")?.Value ?? "Unknown",
// 					["Title"] = source.Element(source.Name.Namespace + "Title")?.Value ?? "Unknown",
// 					["Author"] = source.Element(source.Name.Namespace + "Author")?.Value ?? "Unknown",
// 					["Year"] = source.Element(source.Name.Namespace + "Year")?.Value ?? "Unknown",
// 					["SourceType"] = source.Element(source.Name.Namespace + "SourceType")?.Value ?? "Unknown"
// 				};
// 				bibliographyEntries.Add(entry);
// 			}
// 		}
// 		catch (Exception ex)
// 		{
// 			Console.WriteLine($"⚠️ Error processing bibliography XML: {ex.Message}");
// 		}
// 	}

// 	return bibliographyEntries;
// }


// public static List<Dictionary<string, string>> ExtractBibliography(WordprocessingDocument doc)
// {
// 	var bibliographyEntries = new List<Dictionary<string, string>>();

// 	var customXmlParts = doc.MainDocumentPart?.CustomXmlParts;
// 	if (customXmlParts == null)
// 	{
// 		return bibliographyEntries; // No bibliography found
// 	}

// 	foreach (var xmlPart in customXmlParts)
// 	{
// 		try
// 		{
// 			XDocument xmlDoc = XDocument.Load(xmlPart.GetStream());

// 			// Search for bibliography sources (usually in `<Sources>`)
// 			var sources = xmlDoc.Descendants().Where(e => e.Name.LocalName == "Source");

// 			foreach (var source in sources)
// 			{
// 				var entry = new Dictionary<string, string>();

// 				// Extract bibliography fields (depends on structure)
// 				entry["Tag"] = source.Element(source.Name.Namespace + "Tag")?.Value ?? "Unknown";
// 				entry["Title"] = source.Element(source.Name.Namespace + "Title")?.Value ?? "Unknown";
// 				entry["Author"] = source.Element(source.Name.Namespace + "Author")?.Value ?? "Unknown";
// 				entry["Year"] = source.Element(source.Name.Namespace + "Year")?.Value ?? "Unknown";

// 				bibliographyEntries.Add(entry);
// 			}
// 		}
// 		catch (Exception ex)
// 		{
// 			Console.WriteLine($"⚠️ Error reading XML bibliography: {ex.Message}");
// 		}
// 	}

// 	return bibliographyEntries;
// }


// after the foreach element in body.Elements<openXMLelements()>

// Now, process the bibliography part if it exists
// using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filepath, false))
// {
// 	// var bibPart = doc.MainDocumentPart.BibliographyPart;
// 	// if (bibPart != null)
// 	// {
// 	// 	// Load the bibliography XML
// 	// 	XDocument bibXml = XDocument.Load(bibPart.GetStream());
// 	// 	// Process the bibliography XML as needed (e.g., extract sources)
// 	// }

// 	// Attempt to find a BibliographyPart in the main document part
// 	var bibPart = wordDoc?.MainDocumentPart?
// 						 .GetPartsOfType<BibliographyPart>()
// 						 .FirstOrDefault();

// 	if (bibPart != null)
// 	{
// 		// The bibliography part exists; process it as needed
// 		using (var stream = bibPart.GetStream())
// 		{
// 			// e.g. Load the bibliography XML
// 			XDocument bibXml = XDocument.Load(stream);

// 			// Extract sources or process further
// 		}
// 	}
// }

// for v1
// var bibliography = ExtractBibliography(doc);
// if (bibliography.Count > 0)
// {
// 	elements.Add(new { Bibliography = bibliography });
// }


// this works
// using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filepath, false))
// {
// 	// Find all CustomXmlParts (contains bibliography and other metadata)
// 	var customXmlParts = wordDoc.MainDocumentPart?.CustomXmlParts;

// 	if (customXmlParts != null)
// 	{
// 		foreach (var xmlPart in customXmlParts)
// 		{
// 			try
// 			{
// 				XDocument xmlDoc = XDocument.Load(xmlPart.GetStream());

// 				// Search for bibliography data (usually in `<Sources>`)
// 				var sources = xmlDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == "Sources");

// 				if (sources != null)
// 				{
// 					Console.WriteLine("✅ Bibliography Found!");
// 					Console.WriteLine(xmlDoc.ToString()); // Print XML content

// 					// TODO: Process extracted bibliography
// 				}
// 			}
// 			catch (Exception ex)
// 			{
// 				Console.WriteLine($"⚠️ Error reading XML part: {ex.Message}");
// 			}
// 		}
// 	}
// 	else
// 	{
// 		Console.WriteLine("⚠️ No Bibliography Data Found.");
// 	}
// }



// static bool IsBibliography(Paragraph paragraph)
// {
// 	// Convert paragraph text to lowercase for comparison
// 	string paraText = paragraph.InnerText.Trim().ToLower();
// 	Console.WriteLine("paraText:");
// 	Console.WriteLine(paraText);


// 	// 1️⃣ Check if it contains keywords like "References" or "Bibliography"
// 	if (paraText.Equals("references") || paraText.Equals("bibliography"))
// 	{
// 		return true;
// 	}

// 	// 2️⃣ Check if it contains a SimpleField with a bibliography or citation
// 	bool hasBibliographyField = paragraph
// 		.Descendants<SimpleField>()
// 		.Any(f => f.GetFirstChild<Run>()?.Elements<FieldCode>().Any(fc =>
// 		fc.Text.Contains("BIBLIOGRAPHY") || fc.Text.Contains("CITATION")) ?? false);
// 	if (hasBibliographyField)
// 	{
// 		return true;
// 	}

// 	// 3️⃣ Check if the paragraph contains a structured document tag (Zotero, EndNote)
// 	bool hasCitationManagerField = paragraph
// 		.Descendants<SdtBlock>()
// 		.Any();

// 	if (hasCitationManagerField)
// 	{
// 		return true;
// 	}

// 	return false;
// }p
// 	}
