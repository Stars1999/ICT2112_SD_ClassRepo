using System.Collections.Generic;

namespace ICT2106WebApp.mod1grp4
{
    public interface iTableValidate
    {
        public string validateTableLatexOutput(List<AbstractNode> originalNodes, List<Table> processedTables);
    }
}