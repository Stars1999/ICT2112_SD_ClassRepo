using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;

public class AdvancedConversionManager : IAdvancedConversion
{
    private readonly Dictionary<string, IProcessor> processorMap;
    private readonly Dictionary<string, List<AbstractNode>> contentMap;

    public AdvancedConversionManager(
        Dictionary<string, List<AbstractNode>> contentMap,
        Dictionary<string, IProcessor> processorMap)
    {
        this.contentMap = contentMap ?? throw new ArgumentNullException(nameof(contentMap));
        this.processorMap = processorMap ?? throw new ArgumentNullException(nameof(processorMap));
    }

    public List<AbstractNode> getContent()
    {
        foreach (var entry in contentMap)
        {
            var type = entry.Key.ToLower();
            var nodes = entry.Value;

            if (nodes == null || nodes.Count == 0) continue;

            if (processorMap.TryGetValue(type, out IProcessor processor))
            {
                processor.convertContent(nodes);
            }
            else
            {
                // Optional: log or throw for unsupported types
                Console.WriteLine($"No processor found for type: {type}");
            }
        }

        return Flatten(contentMap.Values);
    }

    private List<AbstractNode> Flatten(IEnumerable<List<AbstractNode>> lists)
    {
        var all = new List<AbstractNode>();
        foreach (var list in lists)
            all.AddRange(list);
        return all;
    }
}

