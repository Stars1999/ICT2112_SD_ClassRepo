using ICT2106WebApp.mod1Grp3;

public interface ITreeUpdate
{
	ITreeUpdateNotify treeUpdate { get; set; }

	// Task LoadTree();
	Task<AbstractNode> getTree(string collectionName);

	Task saveTree(AbstractNode rootNode, string collectionName);
}
