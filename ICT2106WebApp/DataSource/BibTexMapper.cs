using MongoDB.Driver;
using System;
using System.Text.Json;

public class BibTexMapper : IInsertBibTex
{
    private readonly MongoDbContext _context;

    public BibTexMapper(MongoDbContext context)
    {
        _context = context;
    }

    public void SetUpdatedJson(string json)
    {
        Console.WriteLine("[DEBUG] BibTexMapper.SetUpdatedJson() was called.");

        if (string.IsNullOrWhiteSpace(json))
        {
            Console.WriteLine("[ERROR] Cannot insert empty JSON.");
            return;
        }

        Console.WriteLine("[DEBUG] Raw JSON received:");
        Console.WriteLine(json);

        try
        {
            // Deserialize the incoming JSON into the Reference object
            var reference = JsonSerializer.Deserialize<Reference>(json);
            if (reference != null && reference.Documents != null && reference.Documents.Count > 0)
            {
                // Check if this reference already exists based on some unique criteria (e.g., Title + Author)
                var existingReference = _context.References
                    .Find(r => r.Documents.Any(d => reference.Documents
                        .Any(newDoc => newDoc.Title == d.Title && newDoc.Author == d.Author)))
                    .FirstOrDefault();

                if (existingReference != null)
                {
                    Console.WriteLine("[DEBUG] Found existing reference, updating.");

                    // Important: Preserve original _id
                    reference.Id = existingReference.Id;
                    reference.InsertedAt = DateTime.UtcNow;
                    reference.Source = "Converted";

                    for (int i = 0; i < reference.Documents.Count; i++)
                    {
                        var newDoc = reference.Documents[i];
                        var oldDoc = existingReference.Documents
                            .FirstOrDefault(d => d.Title == newDoc.Title && d.Author == newDoc.Author);

                        if (oldDoc != null && !string.IsNullOrWhiteSpace(oldDoc.OriginalLatexContent))
                        {
                            newDoc.OriginalLatexContent = oldDoc.OriginalLatexContent;
                        }
                    }

                    _context.References.ReplaceOne(
                        r => r.Id == existingReference.Id, reference
                    );

                    Console.WriteLine($"[INFO] Updated existing reference with {reference.Documents.Count} document(s).");
                }
                else
                {
                    // If not found, insert as a new reference
                    Console.WriteLine("[DEBUG] No existing reference found, inserting new.");
                    reference.InsertedAt = DateTime.UtcNow;
                    reference.Source = "Converted";

                    _context.References.InsertOne(reference);
                    Console.WriteLine($"[INFO] Inserted {reference.Documents.Count} document(s) into MongoDB.");
                }
            }
            else
            {
                Console.WriteLine("[ERROR] Deserialized reference is null or has no documents.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Exception during MongoDB insert or update: {ex.Message}");
        }
    }
}
