using Utilities;
using Newtonsoft.Json;

namespace ICT2106WebApp.mod1Grp3
{
    public class NodeTraverser : INodeTraverser, IQueryUpdateNotify
    {
        private  CompositeNode _rootNode;
        private readonly IQueryUpdate _QueryUpdate;
        
        public NodeTraverser(CompositeNode rootNode) 
        {
            _rootNode = rootNode;
            _QueryUpdate = (IQueryUpdate) new DocumentGateway_RDG(); //idk
            _QueryUpdate.queryUpdate = this; //idk
        }

        // Define grouped node types
        private static Dictionary<string, HashSet<string>> nodeTypeGroups = new Dictionary<string, HashSet<string>>
        {
            { "headers", new HashSet<string> { "h1", "h2", "h3" } },
            { "layouts", new HashSet<string> { "layout", "page_break" } },
            { "lists", new HashSet<string> { "bulleted_list", "hollow_bulleted_list", "square_bulleted_list", "diamond_bulleted_list", "arrow_bulleted_list", "checkmark_bulleted_list", "dash_bulleted_list", "numbered_list", "numbered_parenthesis_list", "roman_numeral_list", "lowercase_roman_numeral_list", "uppercase_lettered_list", "lowercase_lettered_list", "lowercase_lettered_parenthesis_list" } },
            { "paragraphs", new HashSet<string> { "paragraph", "paragraph_run?", "empty_paragraph1" } },
            { "tables", new HashSet<string> { "table", "cell", "row" } },
            { "citationAndbibliographys", new HashSet<string> { "bibliography", "citation_run", "intext-citation" } },
            { "allNodes", new HashSet<string> { "headers", "layouts", "lists", "paragraphs", "tables", "citationAndbibliographys" } }
        };

        // NODETYPE LIST (for reference)
        // metadata (not avaliable)
        // headers: h1, h2, h3
        // layouts: layout, page_break
        // lists: bulleted_list, hollow_bulleted_list, square_bulleted_list, diamond_bulleted_list, arrow_bulleted_list, checkmark_bulleted_list, dash_bulleted_list, numbered_list, numbered_parenthesis_list, roman_numeral_list, lowercase_roman_numeral_list, uppercase_lettered_list, lowercase_lettered_list, lowercase_lettered_parenthesis_list
        // paragraphs: paragraph, paragraph_run?, empty_paragraph1
        // table : table, cell, row
        // citationAndbibliographys: bibliography, citation_run, intext-citation
        // text_run
        // Image
        // math

        public List<AbstractNode> TraverseNode(string nodeType)
        {
            if (string.IsNullOrWhiteSpace(nodeType))
                throw new ArgumentException("Node type cannot be null or empty", nameof(nodeType));

            List<AbstractNode> matchingNodesList = new List<AbstractNode>();

            void TraverseWithIterator(AbstractNode node, string nodeType, List<AbstractNode> matchingNodes)
            {
                // Check if current node matches the type
                if (node.GetNodeType() == nodeType || (nodeTypeGroups.ContainsKey(nodeType) && nodeTypeGroups[nodeType].Contains(node.GetNodeType())))
                {
                    matchingNodes.Add(node);
                }
                
                // If node can provide an iterator, process its children
                if (node is INodeCollection collection)
                {
                    INodeIterator iterator = collection.CreateIterator();
                    
                    while (!iterator.isDone())
                    {
                        AbstractNode childNode = iterator.next();
                        if (childNode != null)
                        {
                            TraverseWithIterator(childNode, nodeType, matchingNodes);
                        }
                    }
                }
            }
            
            // Process all nodes starting from root
            TraverseWithIterator(_rootNode, nodeType, matchingNodesList);

            foreach (var node in matchingNodesList)
            {
                Console.WriteLine($"Matching Node: ID={node.GetNodeId()}, Type={node.GetNodeType()}, Content={node.GetContent()}, styling={JsonConvert.SerializeObject(node.GetStyling())}, converted={node.IsConverted()}");
            }
            Console.WriteLine(matchingNodesList.Count + " nodes found of type " + nodeType);
            
            return matchingNodesList;
        }

        public async Task UpdateLatexDocument(AbstractNode rootNode)
        {
            Console.WriteLine("LATEX DOCUMENT UPDATING...");
			// TODO: Save tree to database
			await _QueryUpdate.saveTree(rootNode);
        }

        public void notifyUpdatedTree(){
            //TODO
        }



/*============================ NOT PART OF METHODS ==========================================================*/
        // Method to traverse all node types (F)
        public List<AbstractNode> TraverseAllNodeTypes()
        {
            List<AbstractNode> allNodesList = new List<AbstractNode>();

            void TraverseWithIteratorForAll(AbstractNode node, List<AbstractNode> matchingNodes)
            {
                foreach (var group in nodeTypeGroups)
                {
                    // If the node type matches any of the types in the group, add it to the list
                    if (group.Value.Contains(node.GetNodeType()) || group.Key == "allNodes")
                    {
                        matchingNodes.Add(node);
                        break; 
                    }
                }

                if (node is INodeCollection collection)
                {
                    INodeIterator iterator = collection.CreateIterator();
                    
                    while (!iterator.isDone())
                    {
                        AbstractNode childNode = iterator.next();
                        if (childNode != null)
                        {
                            TraverseWithIteratorForAll(childNode, matchingNodes);
                        }
                    }
                }
            }

            // Start traversing the tree from the root node using the iterator method
            TraverseWithIteratorForAll(_rootNode, allNodesList);

            WriteToFile("traverseNodes.cs", allNodesList);

            return allNodesList;
        }
    
        public static void WriteToFile(string filePath, List<AbstractNode> nodes)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using Utilities;\n");
                writer.WriteLine("public class AllNodesList");
                writer.WriteLine("{");

                Dictionary<string, List<AbstractNode>> groupedNodes = new Dictionary<string, List<AbstractNode>>();

                // Group nodes by their corresponding group name
                foreach (var node in nodes)
                {
                    string nodeType = node.GetNodeType();
                    string groupKey = nodeTypeGroups.FirstOrDefault(g => g.Value.Contains(nodeType)).Key;

                    if (groupKey == null)
                    {
                        groupKey = nodeType; // If the node type doesn't belong to any group, use the node type as the key
                    }

                    if (!groupedNodes.ContainsKey(groupKey))
                    {
                        groupedNodes[groupKey] = new List<AbstractNode>();
                    }

                    groupedNodes[groupKey].Add(node);
                }

                // Write grouped nodes
                foreach (var entry in groupedNodes)
                {
                    string groupName = entry.Key;
                    List<AbstractNode> nodeList = entry.Value;

                    writer.WriteLine($"    // Node Type: {groupName}");
                    writer.WriteLine($"    public List<AbstractNode> {groupName} = new List<AbstractNode>");
                    writer.WriteLine("    {");

                    foreach (var node in nodeList)
                    {
                        var nodeId = node.GetNodeId();
                        var nodeLevel = node.GetNodeLevel();
                        var nodeType = node.GetNodeType();
                        var nodeContent = node.GetContent();
                        var nodeStyling = JsonConvert.SerializeObject(node.GetStyling());

                        if (nodeType == "image")
                        {
                            writer.WriteLine($"      //  new CompositeNode({nodeId}, {nodeLevel}, \"{nodeType}\", @\"{nodeContent}\", {nodeStyling}),");
                        } else {
                            writer.WriteLine($"      //  new CompositeNode({nodeId}, {nodeLevel}, \"{nodeType}\", \"{nodeContent}\", \"{nodeStyling}\"),");
                        }
                    }

                    writer.WriteLine("    };");
                }
                writer.WriteLine("}");
            }
        }
    }
}