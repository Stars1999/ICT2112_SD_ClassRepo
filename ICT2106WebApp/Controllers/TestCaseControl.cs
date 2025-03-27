using ICT2106WebApp.Interfaces;
using ICT2106WebApp.Models;
using System.Text;

namespace ICT2106WebApp.Controllers
{
	public class TestCaseControl : IDocumentTestCase
	{
		public bool runModTestCases(int modNumber)
		{
			bool result = false;

			switch (modNumber)
			{
				case 1:
					// Implement mod1 test cases
					break;

				case 2:
					var mod2 = new mod2testcases();
					result = mod2.RunPassTests();
					break;

				case 3:
					// Implement mod3 test cases
					break;

				default:
					result = false;
					break;
			}

			return result;
		}

		public string getTestStatus(int modNumber)
		{
			bool result = RunModTestCases(modNumber);
			return result ? $"Module {modNumber}: Passed" : $"Module {modNumber}: Failed";
		}
	}
}
