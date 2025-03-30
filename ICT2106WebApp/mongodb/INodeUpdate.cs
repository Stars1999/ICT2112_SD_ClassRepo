using Utilities;

public interface INodeUpdate{
	ITreeUpdateNotify treeUpdate { get; set; }
	// Task LoadTree();
	Task<AbstractNode> loadTree();

	Task saveTree(AbstractNode rootNode);
}