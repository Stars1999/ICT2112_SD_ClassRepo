using System.Collections.Generic;
using ICT2106WebApp.Utilities;

public interface IProcessor
{
    string Type { get; } // The type it handles, e.g., "math", "text"
    void convertContent(List<AbstractNode> nodes);
}