// public interface IDocumentRetrieveNotify
// {
// 	Task notifyRetrievedDocument(Docx docx);
// 	Task notifyRetrievedJson();
// }

public interface ICrashRecoveryRetrieveNotify
{
	Task notifyRetrievedDocument(Docx docx);
	Task notifyRetrievedJson();
}

