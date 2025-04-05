namespace ICT2106WebApp.mod1Grp3
{
	public interface ICrashRecoveryRetrieveNotify
	{
		Task notifyRetrievedDocument(Docx docx);
		Task notifyRetrievedJson();
	}
}
