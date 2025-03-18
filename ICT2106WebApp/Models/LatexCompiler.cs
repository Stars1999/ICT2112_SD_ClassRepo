using System;

public class LatexCompiler : iConversionStatus
{
    private string _updatedJson;

    public void SetUpdatedJson(string convertedJson)
    {
        _updatedJson = convertedJson;
        Console.WriteLine("[INFO] JSON updated in-memory.");
    }

    public string GetUpdatedJson()
    {
        return _updatedJson;
    }

    public bool fetchConversionStatus()
    {
        bool status = !string.IsNullOrEmpty(_updatedJson);
        Console.WriteLine($"[INFO] LatexCompiler Conversion Status: {status}");
        return status;
    }
}