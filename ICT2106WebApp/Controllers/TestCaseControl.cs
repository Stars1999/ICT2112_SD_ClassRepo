using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;
using System.Text;
using CustomLogger = ICT2106WebApp.Interfaces.ILogger;

namespace ICT2106WebApp.Controllers
{
	public class TestCaseControl : IDocumentTestCase
	{
		private readonly CustomLogger _logger;

		public TestCaseControl(CustomLogger logger)
		{
			_logger = logger;
		}
		
		public bool runModTestCases(int modNumber)
		{
			bool result = false;
			switch (modNumber)
			{
				case 1:
					var mod1 = new Mod1TestCases(_logger);
					result = mod1.RunPassTests();
					break;

				case 2:
					var mod2 = new mod2testcases();
					result = mod2.RunPassTests();
					break;

				case 3:
					// Handle citation format specifically for Mod3
					string citationFormat = DetermineCitationFormat();
					if (string.IsNullOrEmpty(citationFormat))
					{
						return false;
					}

					var mod3 = new Mod3TestCases(_logger);
					result = mod3.RunPassTests(citationFormat).Result;
					break;

				default:
					result = false;
					break;
			}

			return result;
		}

		// Helper method to determine the citation format for Mod3
		private string DetermineCitationFormat()
		{
			// Logic to determine citation format (e.g., APA, MLA, etc.)
			string fileName = "apa_test.json";
			if (fileName.Contains("apa"))
			{
				return "APA";
			}
			else if (fileName.Contains("mla"))
			{
				return "MLA";
			}

			return null;
		}

		public string getTestStatus(int modNumber)
		{
			bool result = runModTestCases(modNumber);
			return result ? $"Module {modNumber}: Passed" : $"Module {modNumber}: Failed";
		}
	}
}
