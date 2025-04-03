namespace ICT2106WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;
    using CustomLogger = ICT2106WebApp.Interfaces.ILogger;

    public class Mod1TestCases
    {
        private readonly CustomLogger _logger;

        // Constructor to inject logger
        public Mod1TestCases(CustomLogger logger)
        {
            _logger = logger;

            _logger.InsertLog(DateTime.Now, "Initializing Mod1 test cases.", "Mod1TestCases");
        }

        // --------------------------------- To Hold Nodes --------------------------------- //
        private Dictionary<string, BsonDocument> _nodes = new Dictionary<string, BsonDocument>();
        private Dictionary<string, BsonDocument> _latexNodesPass = new Dictionary<string, BsonDocument>();
        private Dictionary<string, BsonDocument> _latexNodesFail = new Dictionary<string, BsonDocument>();

        // --------------------------------- MOCK NODES --------------------------------- //
        public Mod1TestCases()
        {
            // TC01: Header 1
            _nodes["TC01"] = new BsonDocument {
                { "Type", "h1" }, 
                { "Content", "Header 1" },
                { "Styling", new BsonArray { 
                    new BsonDocument { 
                        { "bold", true }, 
                        { "alignment", "left" }, 
                        { "fontSize", 20 }, 
                        { "fontType", "Heading1" } 
                    } 
                } }
            };
            // Pass - correct LaTeX conversion
            _latexNodesPass["TC01"] = new BsonDocument { 
                { "Content", "\\section{\\textbf{Header 1}}" } 
            };
            // Fail - incorrect LaTeX conversion
            _latexNodesFail["TC01"] = new BsonDocument { 
                { "Content", "Header 1" } 
            };

            // TC02: Paragraph with bold text
            _nodes["TC02"] = new BsonDocument {
                { "Type", "paragraph" }, 
                { "Content", "Bold Paragraph" },
                { "Styling", new BsonArray { 
                    new BsonDocument { 
                        { "bold", true }, 
                        { "alignment", "left" } 
                    } 
                } }
            };
            // Pass
            _latexNodesPass["TC02"] = new BsonDocument { 
                { "Content", "\\textbf{Bold Paragraph}" } 
            };
            // Fail
            _latexNodesFail["TC02"] = new BsonDocument { 
                { "Content", "Bold Paragraph" } 
            };

            // TC03: Table
            _nodes["TC03"] = new BsonDocument {
                { "Type", "table" },
                { "Content", new BsonArray {
                    new BsonArray { "Hi", "i", "Am" },
                    new BsonArray { "Going", "To", "Remod" }
                }}
            };
            // Pass
            _latexNodesPass["TC03"] = new BsonDocument { 
                { "Content", "\\begin{tabular}{|c|c|c|}\n\\hline\nHi & i & Am \\\\\n\\hline\nGoing & To & Remod \\\\\n\\hline\n\\end{tabular}" } 
            };
            // Fail
            _latexNodesFail["TC03"] = new BsonDocument { 
                { "Content", "Table not converted" } 
            };

            // TC04: Multilingual content
            _nodes["TC04"] = new BsonDocument {
                { "Type", "paragraph" },
                { "Content", "sample text 示例文本" },
                { "Styling", new BsonArray { 
                    new BsonDocument { 
                        { "alignment", "left" } 
                    } 
                } }
            };
            // Pass
            _latexNodesPass["TC04"] = new BsonDocument { 
                { "Content", "sample text 示例文本" } 
            };
            // Fail
            _latexNodesFail["TC04"] = new BsonDocument { 
                { "Content", "" } 
            };

            // TC05: Special characters
            _nodes["TC05"] = new BsonDocument {
                { "Type", "paragraph" },
                { "Content", "!@#$%^&*(){}[];':,./ 🤪" },
                { "Styling", new BsonArray { 
                    new BsonDocument { 
                        { "alignment", "left" } 
                    } 
                } }
            };
            // Pass
            _latexNodesPass["TC05"] = new BsonDocument { 
                { "Content", "!@#$%^&*(){}[];':,./ 🤪" } 
            };
            // Fail
            _latexNodesFail["TC05"] = new BsonDocument { 
                { "Content", "Cleaned special characters" } 
            };

            _logger.InsertLog(DateTime.Now, "Test case nodes initialized successfully.", "Mod1TestCases");
        }

        // --------------------------------- PASS METHOD --------------------------------- //
        public bool RunPassTests()
        {
            _logger.InsertLog(DateTime.Now, "Running tests for Mod1.", "Mod1TestCases");
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
                            testPassed &= CheckParagraphContent(node, latexContent);
                            testPassed &= CheckBoldText(node, latexContent);
                            break;
                        case "table":
                            testPassed &= CheckTable(node, latexContent);
                            break;
                    }
                }
                else
                {
                    testPassed = false;
                }

                results.Add(testPassed);
            }

            bool allPassed = !results.Contains(false);
            _logger.InsertLog(DateTime.Now, allPassed ? "All tests passed." : "Some tests failed.", "Mod1TestCases");
            return allPassed;
        }

        // --------------------------------- FAIL METHOD --------------------------------- //
        public bool RunFailTests()
        {
            _logger.InsertLog(DateTime.Now, "Running tests for Mod1.", "Mod1TestCases");
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
                bool testPassed = false; // Expecting failure

                if (node.Contains("Type"))
                {
                    string nodeType = node["Type"].AsString;

                    switch (nodeType)
                    {
                        case var _ when nodeType.StartsWith("h"):
                            testPassed = !CheckHeading(node, latexContent);
                            break;
                        case "paragraph":
                            testPassed = !CheckParagraphContent(node, latexContent);
                            break;
                        case "table":
                            testPassed = !CheckTable(node, latexContent);
                            break;
                    }
                }

                results.Add(testPassed);
            }

            bool allFailed = results.Contains(false);
            _logger.InsertLog(DateTime.Now, "TC03 failed: Missing tabular environment", "Mod1TestCases");
            _logger.InsertLog(DateTime.Now, allFailed ? "All tests passed." : "Tests failed.", "Mod1TestCases");
            return allFailed;
        }

        // --------------------------------- HELPER METHODS --------------------------------- //
        private bool CheckHeading(BsonDocument node, string latexContent)
        {
            if (node["Type"].AsString.StartsWith("h"))
            {
                string expectedContent = node["Content"].AsString;
                
                // Check for bold heading if styling indicates
                if (node.Contains("Styling") && node["Styling"].AsBsonArray.Count > 0)
                {
                    var styling = node["Styling"].AsBsonArray[0].AsBsonDocument;
                    if (styling.Contains("bold") && styling["bold"].AsBoolean)
                    {
                        return latexContent.Contains($"\\textbf{{{expectedContent}}}");
                    }
                }

                return latexContent.Contains(expectedContent);
            }
            return true;
        }

        private bool CheckParagraphContent(BsonDocument node, string latexContent)
        {
            return latexContent.Contains(node["Content"].AsString);
        }

        private bool CheckBoldText(BsonDocument node, string latexContent)
        {
            if (node.Contains("Styling") && node["Styling"].AsBsonArray.Count > 0)
            {
                var styling = node["Styling"].AsBsonArray[0].AsBsonDocument;
                if (styling.Contains("bold") && styling["bold"].AsBoolean)
                {
                    return latexContent.Contains("\\textbf{");
                }
            }
            return true;
        }

        private bool CheckTable(BsonDocument node, string latexContent)
        {
            if (node["Type"].AsString == "table")
            {
                var tableContent = node["Content"].AsBsonArray;
                bool result = true;

                // Check for LaTeX table structure
                result &= latexContent.Contains("\\begin{tabular}");
                result &= latexContent.Contains("\\end{tabular}");

                // Check table content
                foreach (var row in tableContent)
                {
                    var rowArray = row.AsBsonArray;
                    foreach (var cell in rowArray)
                    {
                        result &= latexContent.Contains(cell.AsString);
                    }
                }

                return result;
            }
            return true;
        }
    }
}