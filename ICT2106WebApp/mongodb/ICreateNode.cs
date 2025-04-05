using Utilities;

namespace ICT2106WebApp.mod1Grp3
{
public interface ICreateNode
{
    public List<AbstractNode> CreateNodeList(List<object> documentContents);
    public bool ValidateContentRecursive(
        List<AbstractNode> treeNodes,
        Newtonsoft.Json.Linq.JArray documentArray,
        int startIndex
    );
}
}