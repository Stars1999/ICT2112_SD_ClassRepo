using System;

public class LatexCompiler
{
    private string _updatedJson;

    public void UpdateJson(string convertedJson)
    {
        _updatedJson = convertedJson;
        Console.WriteLine("[INFO] JSON updated in-memory.");
    }

    public string GetUpdatedJson()
    {
        return _updatedJson;
    }
}
