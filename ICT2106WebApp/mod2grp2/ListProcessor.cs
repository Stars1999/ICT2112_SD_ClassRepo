using System.Text;

namespace ICT2106WebApp.Utilities
{
    public class ListProcessor : IProcessor
    {
        public string Type => "list";

        // Method to convert the content of a node to LaTeX and set it
        public void convertContent(List<AbstractNode> nodes)
        {
            if (nodes == null || nodes.Count == 0)
            {
                return;
            }

            // Iterate through each node and convert it to LaTeX
            foreach (var node in nodes)
            {
                if (node == null)
                {
                    continue;
                }

                // Retrieve the node's content, type, and sub-nodes from the AbstractNode
                string nodeType = node.GetNodeType();
                string content = node.GetContent();
                var subNodes = new List<AbstractNode>();  // Initialize empty list of sub-nodes

                // If the node is a CompositeNode, retrieve its children
                if (node is CompositeNode compositeNode)
                {
                    subNodes = compositeNode.GetChildren();  // For CompositeNode, access its children
                }

                // Check if this node type is 'list' and it has sub-nodes
                if (nodeType != "list" || subNodes.Count == 0)
                {
                    continue;
                }

                // Convert the sub-nodes (children) to LaTeX format
                List<Dictionary<string, object>> itemsList = new List<Dictionary<string, object>>();
                foreach (var child in subNodes)
                {
                    if (child is CompositeNode childCompositeNode)
                    {
                        // Add child node content to LaTeX format
                        itemsList.Add(new Dictionary<string, object>
                        {
                            { "content", childCompositeNode.GetContent() },
                            { "list_style", childCompositeNode.GetNodeType() },
                            { "sub_items", ConvertSubItemsToLatex(childCompositeNode) }
                        });
                    }
                }

                // Generate LaTeX code from the constructed list and set the content
                string latexContent = GenerateLatexList(itemsList, 0);
                
                // Log the converted LaTeX content to the console
                Console.WriteLine("Converted LaTeX Content:");
                Console.WriteLine(latexContent);
                
                // Set the content of the node
                node.SetContent(latexContent);
            }
        }

        // Convert sub-items (child nodes) into LaTeX format recursively
        private string ConvertSubItemsToLatex(CompositeNode node)
        {
            if (node.GetChildren().Count == 0)
            {
                return string.Empty;
            }

            List<Dictionary<string, object>> subItemsList = new List<Dictionary<string, object>>();
            foreach (var child in node.GetChildren())
            {
                if (child is CompositeNode compositeChild)
                {
                    subItemsList.Add(new Dictionary<string, object>
                    {
                        { "content", compositeChild.GetContent() },
                        { "list_style", compositeChild.GetNodeType() },
                        { "sub_items", ConvertSubItemsToLatex(compositeChild) } // Recurse for nested items (children of a child)
                    });
                }
            }
            return GenerateLatexList(subItemsList, 1); // Indentation level 1 for nested items
        }

        // Method to generate LaTeX formatted list from items
        private string GenerateLatexList(List<Dictionary<string, object>> items, int indentLevel)
        {
            if (items == null || items.Count == 0) return "";

            StringBuilder latex = new StringBuilder();
            string listType = GetLatexListType(items[0]["list_style"].ToString());
            string specialFormatting = GetSpecialListFormatting(items[0]["list_style"].ToString());
            string bulletCommand = GetBulletCommand(items[0]["list_style"].ToString());
            string indentation = new string(' ', indentLevel * 4);  // Adjust indentation level

            latex.AppendLine($"{indentation}\\begin{{{listType}}}");
            if (!string.IsNullOrEmpty(specialFormatting))
            {
                latex.AppendLine($"{indentation}    {specialFormatting}");
            }

            foreach (var item in items)
            {
                if (!item.ContainsKey("content") || !item.ContainsKey("list_style"))
                    continue;

                string content = item["content"].ToString();
                if (!string.IsNullOrEmpty(bulletCommand))
                {
                    latex.AppendLine($"{indentation} {bulletCommand} \\item {content}");
                }
                else
                {
                    latex.AppendLine($"{indentation}    \\item {content}");
                }

                // Recursively add sub-items (nested lists) and their children
                if (item.ContainsKey("sub_items") && !string.IsNullOrEmpty(item["sub_items"].ToString()))
                {
                    latex.AppendLine(item["sub_items"].ToString());
                }
            }

            latex.AppendLine($"{indentation}\\end{{{listType}}}");
            return latex.ToString();
        }

        // Get the LaTeX list type based on the list style
        private string GetLatexListType(string listStyle)
        {
            return listStyle switch
            {
                "bulleted_list" or "hollow_bulleted_list" or "square_bulleted_list" or
                "diamond_bulleted_list" or "arrow_bulleted_list" or "checkmark_bulleted_list" or
                "dash_bulleted_list" => "itemize",

                "numbered_list" or "numbered_parenthesis_list" or "roman_numeral_list" or
                "lowercase_roman_numeral_list" or "uppercase_lettered_list" or "lowercase_lettered_list" or
                "lowercase_lettered_parenthesis_list" => "enumerate",
            };
        }

        // Get special formatting for the LaTeX list based on the list style
        private string GetSpecialListFormatting(string listStyle)
        {
            return listStyle switch
            {
                "roman_numeral_list" => @"\renewcommand{\labelenumi}{\Roman{enumi}.}",
                "lowercase_roman_numeral_list" => @"\renewcommand{\labelenumi}{\roman{enumi}.}",
                "uppercase_lettered_list" => @"\renewcommand{\labelenumi}{\Alph{enumi}.}",
                "lowercase_lettered_list" => @"\renewcommand{\labelenumi}{\alph{enumi}.}",
                "lowercase_lettered_parenthesis_list" => @"\renewcommand{\labelenumi}{\alph{enumi})}",
                "numbered_parenthesis_list" => @"\renewcommand{\labelenumi}{\arabic{enumi})}",
                _ => ""
            };
        }

        // Get the bullet command for the LaTeX list based on the list style
        private string GetBulletCommand(string listStyle)
        {
            return listStyle switch
            {
                "hollow_bulleted_list" => @"\renewcommand{\labelitemi}{\ensuremath{\circ}}",   // Hollow circle
                "square_bulleted_list" => @"\renewcommand{\labelitemi}{$\blacksquare$}",               // Square bullet
                "diamond_bulleted_list" => @"\renewcommand{\labelitemi}{$\lozenge$}",             // Diamond bullet
                "arrow_bulleted_list" => @"\renewcommand{\labelitemi}{$\rightarrow$}",             // Arrow bullet
                "checkmark_bulleted_list" => @"\renewcommand{\labelitemi}{$\checkmark$}",         // Checkmark bullet (requires \usepackage{amssymb})
                "dash_bulleted_list" => @"\renewcommand{\labelitemi}{$\textendash$}",              // Dash bullet
                _ => ""
            };
        }
    }
}
