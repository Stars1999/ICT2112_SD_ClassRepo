using Utilities;

public interface ITreeUpdate
{
	ITreeUpdateNotify treeUpdate { get; set; }

	// Task LoadTree();
	Task<AbstractNode> getTree(string collectionName);

	Task saveTree(AbstractNode rootNode, string collectionName);
}
