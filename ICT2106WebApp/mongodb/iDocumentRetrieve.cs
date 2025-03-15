using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDocumentRetrieve
{
	Task<Docx> getDocument(string id);
}
