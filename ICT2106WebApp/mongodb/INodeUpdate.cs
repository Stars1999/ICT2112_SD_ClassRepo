using Utilities;

public interface INodeUpdate{
	ITreeUpdateNotify treeUpdate { get; set; }
	Task saveTree(AbstractNode rootNode);
}