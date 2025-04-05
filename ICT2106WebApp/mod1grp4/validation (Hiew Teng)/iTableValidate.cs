using System.Collections.Generic;
using ICT2106WebApp.mod1Grp3;

namespace ICT2106WebApp.mod1grp4
{
	public interface iTableValidate
	{
		public string validateTableLatexOutput(
			List<AbstractNode> originalNodes,
			List<Table> processedTables
		);
	}
}
