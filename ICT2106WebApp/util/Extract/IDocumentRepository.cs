using System.Threading.Tasks;
using ICT2106WebApp.mod1Grp3;

public interface IDocumentRepository
{
	Task SaveDocument(Docx docx);
	Task SaveJson(string jsonFilePath);
}
