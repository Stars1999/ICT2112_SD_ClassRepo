public interface IInsertBibTex
{
    /// <summary>
    /// Accepts a JSON string representing converted BibTeX data
    /// and stores it to the target system (e.g., MongoDB).
    /// </summary>
    /// <param name="json">Serialized BibTeX JSON string</param>
    void SetUpdatedJson(string json);
}
