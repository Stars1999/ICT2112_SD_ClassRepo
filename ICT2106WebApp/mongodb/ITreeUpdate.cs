using Utilities;

public interface ITreeUpdate
{
	ITreeUpdateNotify treeUpdate { get; set; }

	// Task LoadTree();
	Task<AbstractNode> getTree();

	Task saveTree(AbstractNode rootNode);
}
