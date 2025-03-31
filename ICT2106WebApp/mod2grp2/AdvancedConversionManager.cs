using System;
using System.Collections.Generic;
using ICT2106WebApp.Utilities;

public class AdvancedConversionManager : IAdvancedConversion
{
    // === FACADE: Holds content data grouped by type (e.g., "math", "image") ===
    private readonly Dictionary<string, List<AbstractNode>> contentMap;

    // === STRATEGY: Maps content types to corresponding processing strategies ===
    private readonly Dictionary<string, IProcessor> processorMap;

    // === FACADE: Constructor wires up internal subsystems (content + strategy handlers) ===
    public AdvancedConversionManager(
        Dictionary<string, List<AbstractNode>> contentMap,
        Dictionary<string, IProcessor> processorMap)
    {
        this.contentMap = contentMap ?? throw new ArgumentNullException(nameof(contentMap));
        this.processorMap = processorMap ?? throw new ArgumentNullException(nameof(processorMap));
    }

    // === FACADE: Unified entry point to process all content types ===
    public List<AbstractNode> getContent()
    {
        // === FACADE: Delegates conversion for each content type ===
        foreach (var entry in contentMap)
        {
            var type = entry.Key.ToLower();
            var nodes = entry.Value;

            if (nodes == null || nodes.Count == 0) continue;

            // === STRATEGY: Selects the appropriate processor at runtime ===
            if (processorMap.TryGetValue(type, out IProcessor processor))
            {
                // === STRATEGY: Executes the strategy (processor) on the given content ===
                processor.convertContent(nodes);
            }
            else
            {
                // === STRATEGY (Fallback): If no strategy exists for this type, skip or warn ===
                Console.WriteLine($"No processor found for type: {type}");
            }
        }

        // === FACADE: Returns combined result in a unified format ===
        return Flatten(contentMap.Values);
    }

    // === UTILITY: Combines all node lists into one list ===
    private List<AbstractNode> Flatten(IEnumerable<List<AbstractNode>> lists)
    {
        var all = new List<AbstractNode>();
        foreach (var list in lists)
            all.AddRange(list);
        return all;
    }
}
