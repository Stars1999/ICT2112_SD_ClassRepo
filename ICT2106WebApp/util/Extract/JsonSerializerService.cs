using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

public class JsonSerializerService : IJsonSerializer
{
	public string SerializeToFile(object data, string path)
	{
		var jsonOutput = JsonSerializer.Serialize(
			data,
			new JsonSerializerOptions
			{
				WriteIndented = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
			}
		);

		File.WriteAllText(path, jsonOutput);
		return jsonOutput;
	}
}
