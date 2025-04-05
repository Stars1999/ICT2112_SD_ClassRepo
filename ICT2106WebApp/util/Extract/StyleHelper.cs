using System.Collections.Generic;
using System.Text.Json;

namespace ICT2106WebApp.mod1Grp3
{
	public static class StyleHelper
	{
		// Converts values of type JsonElement within the given dictionary to native .NET types (string, number, boolean) and returns the updated dictionary.
		public static Dictionary<string, object> ConvertJsonElements(
			Dictionary<string, object> input
		)
		{
			var result = new Dictionary<string, object>();

			foreach (var kvp in input)
			{
				if (kvp.Value is JsonElement jsonElement)
				{
					switch (jsonElement.ValueKind)
					{
						case JsonValueKind.String:
							result[kvp.Key] = jsonElement.GetString();
							break;
						case JsonValueKind.Number:
							result[kvp.Key] = jsonElement.GetDouble(); // or GetInt32() if needed
							break;
						case JsonValueKind.True:
						case JsonValueKind.False:
							result[kvp.Key] = jsonElement.GetBoolean();
							break;
						case JsonValueKind.Object:
						case JsonValueKind.Array:
							result[kvp.Key] = jsonElement.ToString(); // fallback as string
							break;
						default:
							result[kvp.Key] = null;
							break;
					}
				}
				else
				{
					result[kvp.Key] = kvp.Value;
				}
			}

			return result;
		}
	}
}
