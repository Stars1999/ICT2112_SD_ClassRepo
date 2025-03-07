
public interface IFormatDocument
{
    // <summary>
    // Start formatting the content
    // </summary>
    // <param name="content">The format content</param>
    // <returns>Boolean indicating success</returns>
    bool StartFormatting(Format content);

    // <summary>
    // Format the headings
    // </summary>
    /// <returns>Boolean indicating success</returns>
    bool FormatHeadings();

    // <summary>
    // Format the paragraphs
    // </summary>
    // <returns>Boolean indicating success</returns>
    bool FormatParagraphs();

    // <summary>
    // Apply base formatting
    // </summary>
    // <returns>List of nodes</returns>
    ///List<Node> ApplyBaseFormatting();

    // <summary>
    // Process metadata
    // </summary>
    // <returns>Metadata</returns>
    MetaData ProcessMetaData();
}