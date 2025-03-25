namespace ICT2106WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;

    public class mod2testcases
    {
        // --------------------------------- To Hold Nodes --------------------------------- //
        private Dictionary<string, BsonDocument> _nodes = new Dictionary<string, BsonDocument>();
        private Dictionary<string, BsonDocument> _latexNodesPass = new Dictionary<string, BsonDocument>();
        private Dictionary<string, BsonDocument> _latexNodesFail = new Dictionary<string, BsonDocument>();

        // --------------------------------- MOCK NODES --------------------------------- //
        // DISCLAIMER: I am simply following what I see in the online database
        // This is under the assumption that each LINE in doc/letax is saved as a different node
        public mod2testcases()
        {
            // TC01: Heading
            _nodes["TC01"] = new BsonDocument {
            { "Type", "h1" }, { "Content", "Header 1" },
            { "Styling", new BsonArray { new BsonDocument { { "bold", true } } } }
        };
            // Pass
            _latexNodesPass["TC01"] = new BsonDocument { { "Content", "\\section{\\textbf{Header 1}}" } };
            // Fail - set to fail
            _latexNodesFail["TC01"] = new BsonDocument { { "Content", "\\section{\\textb{Header 1}}" } };

            // TC02: Bold text
            _nodes["TC02"] = new BsonDocument {
            { "Type", "paragraph" }, { "Content", "Bold Text" },
            { "Styling", new BsonArray { new BsonDocument { { "bold", true } } } }
        };
            // Pass
            _latexNodesPass["TC02"] = new BsonDocument { { "Content", "\\textbf{Bold Text}" } };
            // Fail - set to pass
            _latexNodesFail["TC02"] = new BsonDocument { { "Content", "\\textbf{Bold Text}" } };

            // TC03: Italic text
            _nodes["TC03"] = new BsonDocument {
            { "Type", "paragraph" }, { "Content", "Italic Text" },
            { "Styling", new BsonArray { new BsonDocument { { "italic", true } } } }
        };
            // Pass
            _latexNodesPass["TC03"] = new BsonDocument { { "Content", "\\textit{Italic Text}" } };
            // Fail - set to fail
            _latexNodesFail["TC03"] = new BsonDocument { { "Content", "\\text" } };

            // TC06: Bulleted list
            _nodes["TC06"] = new BsonDocument {
            { "Type", "list" }, { "Style", "bullet" },
            { "Items", new BsonArray { "Item1", "Item2" } }
        };
            // Pass
            _latexNodesPass["TC06"] = new BsonDocument { { "Content", "\\begin{itemize}\\item Item1 \\item Item2\\end{itemize}" } };
            // Fail - set to pass
            _latexNodesFail["TC06"] = new BsonDocument { { "Content", "\\begin{itemize}\\item Item1 \\item Item2\\end{itemize}" } };

            // TC07: Numbered list
            _nodes["TC07"] = new BsonDocument {
            { "Type", "list" }, { "Style", "number" },
            { "Items", new BsonArray { "Item1", "Item2" } }
        };
            // Pass
            _latexNodesPass["TC07"] = new BsonDocument { { "Content", "\\begin{enumerate}\\item Item1 \\item Item2\\end{enumerate}" } };
            // Fail - set to pass
            _latexNodesFail["TC07"] = new BsonDocument { { "Content", "\\begin{enumerate}\\item Item1 \\item Item2\\end{enumerate}" } };

            // TC12: Paragraph spacing
            _nodes["TC12"] = new BsonDocument {
            { "Type", "paragraph" }, { "Content", "Paragraph with space" },
            { "Spacing", "double" }
        };
            // Pass
            _latexNodesPass["TC12"] = new BsonDocument { { "Content", "\\doublespacing Paragraph with space" } };
            // Fail - set to pass
            _latexNodesFail["TC12"] = new BsonDocument { { "Content", "\\doublespacing Paragraph with space" } };

            // TC15: Hyperlink
            _nodes["TC15"] = new BsonDocument {
            { "Type", "link" }, { "Content", "LinkText" },
            { "URL", "http://example.com" }
        };
            // Pass
            _latexNodesPass["TC15"] = new BsonDocument { { "Content", "\\href{http://example.com}{LinkText}" } };
            // Fail - set to pass
            _latexNodesFail["TC15"] = new BsonDocument { { "Content", "\\href{http://example.com}{LinkText}" } };

            // TC18: Image
            _nodes["TC18"] = new BsonDocument {
            { "Type", "image" }, { "Content", "image1.png" }, { "AltText", "Sample Image" }
        };
            // Pass
            _latexNodesPass["TC18"] = new BsonDocument { { "Content", "\\includegraphics{image1.png}" } };
            // Fail - set to pass
            _latexNodesFail["TC18"] = new BsonDocument { { "Content", "\\includegraphics{image1.png}" } };
        }

        // --------------------------------- PASS METHOD --------------------------------- //
        public List<bool> RunPassTests()
        {
            List<bool> results = new List<bool>();

            foreach (var nodePair in _nodes)
            {
                var node = nodePair.Value;
                string key = nodePair.Key;

                if (!_latexNodesPass.TryGetValue(key, out var latexNode))
                {
                    results.Add(false);
                    continue;
                }

                string latexContent = latexNode.Contains("Content") ? latexNode["Content"].AsString : "";
                bool testPassed = true;

                if (node.Contains("Type"))
                {
                    string nodeType = node["Type"].AsString;

                    switch (nodeType)
                    {
                        case var _ when nodeType.StartsWith("h"):
                            testPassed &= CheckHeading(node, latexContent);
                            break;
                        case "paragraph":
                            testPassed &= CheckBoldText(node, latexContent);
                            testPassed &= CheckParagraphSpacing(node, latexContent);
                            break;
                        case "list":
                            testPassed &= CheckList(node, latexContent);
                            break;
                        case "image":
                            testPassed &= CheckImage(node, latexContent);
                            break;
                        case "link":
                            testPassed &= CheckHyperlink(node, latexContent);
                            break;
                    }
                }
                else
                {
                    testPassed = false;
                }

                results.Add(testPassed);
            }

            Console.WriteLine(string.Join(", ", results));
            return results;
        }

        // --------------------------------- FAIL METHOD --------------------------------- //
        public List<bool> RunFailTests()
        {
            List<bool> results = new List<bool>();

            foreach (var nodePair in _nodes)
            {
                var node = nodePair.Value;
                string key = nodePair.Key;

                if (!_latexNodesFail.TryGetValue(key, out var latexNode))
                {
                    results.Add(false);
                    continue;
                }

                string latexContent = latexNode.Contains("Content") ? latexNode["Content"].AsString : "";
                bool testPassed = true;

                if (node.Contains("Type"))
                {
                    string nodeType = node["Type"].AsString;

                    switch (nodeType)
                    {
                        case var _ when nodeType.StartsWith("h"):
                            testPassed &= CheckHeading(node, latexContent);
                            break;
                        case "paragraph":
                            testPassed &= CheckBoldText(node, latexContent);
                            testPassed &= CheckParagraphSpacing(node, latexContent);
                            break;
                        case "list":
                            testPassed &= CheckList(node, latexContent);
                            break;
                        case "image":
                            testPassed &= CheckImage(node, latexContent);
                            break;
                        case "link":
                            testPassed &= CheckHyperlink(node, latexContent);
                            break;
                    }
                }
                else
                {
                    testPassed = false;
                }

                results.Add(testPassed);
            }

            Console.WriteLine(string.Join(", ", results));
            return results;
        }

        // --------------------------------- HELPER METHODS --------------------------------- //
        private bool CheckHeading(BsonDocument node, string latexContent)
        {
            if (node["Type"].AsString.StartsWith("h"))
            {
                string latexHeading = $"\\section{{{node["Content"].AsString}}}";
                if (node.Contains("Styling") && node["Styling"].IsBsonArray)
                {
                    foreach (var style in node["Styling"].AsBsonArray)
                    {
                        if (style.AsBsonDocument.Contains("bold") && style["bold"].AsBoolean)
                        {
                            latexHeading = $"\\section{{\\textbf{{{node["Content"].AsString}}}}}";
                        }
                    }
                }
                return latexContent.Contains(latexHeading);
            }
            return true;
        }

        private bool CheckBoldText(BsonDocument node, string latexContent)
        {
            if (node.Contains("Styling") && node["Styling"].AsBsonArray.Count > 0)
            {
                foreach (var style in node["Styling"].AsBsonArray)
                {
                    if (style.AsBsonDocument.Contains("bold") && style["bold"].AsBoolean)
                    {
                        return latexContent.Contains("\\textbf{");
                    }
                }
            }
            return true;
        }

        private bool CheckList(BsonDocument node, string latexContent)
        {
            if (node["Type"].AsString == "list")
            {
                bool result = true;

                if (node.Contains("Style") && node["Style"].AsString == "bullet")
                {
                    result &= latexContent.Contains("\\begin{itemize}");
                    result &= latexContent.Contains("\\end{itemize}");
                }
                else if (node.Contains("Style") && node["Style"].AsString == "number")
                {
                    result &= latexContent.Contains("\\begin{enumerate}");
                    result &= latexContent.Contains("\\end{enumerate}");
                }

                if (node.Contains("Items") && node["Items"].IsBsonArray)
                {
                    foreach (var item in node["Items"].AsBsonArray)
                    {
                        result &= latexContent.Contains($"\\item {item.AsString}");
                    }
                }
                return result;
            }
            return true;
        }

        private bool CheckImage(BsonDocument node, string latexContent)
        {
            if (node["Type"].AsString == "image")
            {
                return latexContent.Contains("\\includegraphics{");
            }
            return true;
        }

        private bool CheckHyperlink(BsonDocument node, string latexContent)
        {
            if (node["Type"].AsString == "link")
            {
                return latexContent.Contains("\\href{");
            }
            return true;
        }

        private bool CheckParagraphSpacing(BsonDocument node, string latexContent)
        {
            if (node.Contains("Spacing") && node["Spacing"].AsString == "double")
            {
                return latexContent.Contains("\\doublespacing");
            }
            return true;
        }
    }

    // ------------------------------------ used for testing ------------------------------------ //
    //class Program
    //{
    //    static void Main()
    //    {
    //        // Example of use (i.e: called by scheduler etc)
    //        var test = new mod2testcases();
    //        test.RunPassTests();
    //        test.RunFailTests();
    //    }
    //}
}

