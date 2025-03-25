public interface iRetrieveBibTex
{
    /// <summary>
    /// Retrieves the most recently updated BibTeX JSON string.
    /// </summary>
    /// <returns>Serialized BibTeX JSON string</returns>
    string GetUpdatedJson();
}
