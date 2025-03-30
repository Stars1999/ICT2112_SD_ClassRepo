using System.Collections.Generic;
using ICT2106WebApp.Utilities;

public interface IProcessor
{
    List<AbstractNode> convertContent(ContentType type);
}
