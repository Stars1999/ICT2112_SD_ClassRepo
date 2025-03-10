namespace Utilities
{
        public class TreeProcessor
    {
        private List<string> nodeOrder = new List<string> { "root", "section", "h1", "h2", "h3", "h4", "h5", "h6", "table", "row", "runsParagraph" };
        
        public AbstractNode CreateTree(List<AbstractNode> sequentialList)
        {
            Stack<AbstractNode> nodeStack = new Stack<AbstractNode>();
            AbstractNode rootNode = sequentialList[0];
            nodeStack.Push(rootNode);

            foreach (AbstractNode node in sequentialList)
            {
                if (node.GetNodeType() == "root") {
                    continue;
                }

                int currentNodeLevel = nodeOrder.IndexOf(node.GetNodeType());
                int currentCompositeNodeLevel = nodeOrder.IndexOf(((CompositeNode)nodeStack.Peek()).GetNodeType());

                // Set level of runs to be +1 of runsParagraph
                if (node.GetNodeType() == "text_run") {
                    currentNodeLevel = nodeOrder.IndexOf("runsParagraph") + 1;
                }

                if (node.GetNodeType() == "cell") {
                    currentNodeLevel = nodeOrder.IndexOf("row") + 1;
                }

                if (currentNodeLevel > currentCompositeNodeLevel || currentNodeLevel == -1)
                {
                    ((CompositeNode)nodeStack.Peek()).AddChild(node);
                    nodeStack.Push(node);
                }
                else
                {
                    while (currentNodeLevel <= currentCompositeNodeLevel)
                    {
                        nodeStack.Pop();
                        currentCompositeNodeLevel = nodeOrder.IndexOf(((CompositeNode)nodeStack.Peek()).GetNodeType());
                    }
                    ((CompositeNode)nodeStack.Peek()).AddChild(node);
                    nodeStack.Push(node);
                }
            }
            return rootNode;
        }

        public AbstractNode CreateNode(int id, string nodeType, string content, List<Dictionary<string, object>> styling)
        {
            if (nodeOrder.Contains(nodeType))
            {
                return new CompositeNode(id, nodeType, content, styling);
            } else {
                return new SimpleNode(id, nodeType, content, styling);
            }
        }

        // Recursive method to print the tree hierarchy
        private void PrintTree(AbstractNode node, int level)
        {
            // Print the node's content (could be its type or content)
            Console.WriteLine(new string(' ', level * 2) + node.GetNodeType() + ": " + node.GetContent());

            if (node is CompositeNode compositeNode)
            {
                // Recursively print children of composite nodes
                foreach (var child in compositeNode.GetChildren())
                {
                    PrintTree(child, level + 1);
                }
            }
        }
    }
}