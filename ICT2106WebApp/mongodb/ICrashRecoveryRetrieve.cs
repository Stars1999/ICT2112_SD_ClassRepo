using System.Collections.Generic;
using System.Threading.Tasks;

// public interface IDocumentRetrieve
// {
// 	IDocumentRetrieveNotify docxRetrieve { get; set; }

// 	Task<Docx> getDocument(string id);

// 	Task getJsonFile();
// }
public interface ICrashRecoveryRetrieve
{
	ICrashRecoveryRetrieveNotify docxRetrieve { get; set; }

	Task<Docx> getDocument(string id);

	Task getJsonFile();
}
