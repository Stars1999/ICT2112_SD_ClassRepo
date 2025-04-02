using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

public interface iTableStructure
{
	Dictionary<string, object> extractTableStructure(Table table);
}
