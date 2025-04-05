using System.Collections.Generic;
using System.Threading.Tasks;
namespace ICT2106WebApp.mod1Grp3
{
	public interface ICrashRecoveryRetrieve
	{
		ICrashRecoveryRetrieveNotify docxRetrieve { get; set; }

		Task<Docx> getDocument(string id);

		Task getJsonFile();
	}
}
