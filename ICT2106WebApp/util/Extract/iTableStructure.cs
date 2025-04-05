using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

// Interface for iTableStructure (Jonathan - COMPLETED)
public interface iTableStructure
{
	Dictionary<string, object> extractTableStructure(Table table);
}
