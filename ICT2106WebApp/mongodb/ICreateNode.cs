using Utilities;

public interface ICreateNode
{

    public List<AbstractNode> CreateNodeList(List<object> documentContents);
    public bool ValidateContentRecursive(
        List<AbstractNode> treeNodes,
        Newtonsoft.Json.Linq.JArray documentArray,
        int startIndex
    );
}