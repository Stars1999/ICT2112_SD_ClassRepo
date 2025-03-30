using System.Collections.Generic;
using ICT2106WebApp.Utilities;

public class AdvancedConversionManager : IAdvancedConversion
{
    private IProcessor processor;

    public AdvancedConversionManager(IProcessor processor)
    {
        this.processor = processor;
    }

    public List<AbstractNode> getContent()
    {
        List<AbstractNode> result = new List<AbstractNode>();
        result.AddRange(processor.convertContent(ContentType.Math));
        result.AddRange(processor.convertContent(ContentType.List));
        result.AddRange(processor.convertContent(ContentType.Image));
        return result;
    }
}
