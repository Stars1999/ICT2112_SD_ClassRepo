using System;
using System.Collections.Generic;

public class ErrorPresenter
{
    private static List<string> errorMessages = new List<string>();

    // ✅ Log an error message
    public static void LogError(string errorMessage)
    {
        Console.WriteLine($"[ERROR] {errorMessage}");
        errorMessages.Add(errorMessage);
    }

    // ✅ Retrieve all logged errors and clear the list
    public static List<string> GetErrors()
    {
        List<string> errorsToReturn = new List<string>(errorMessages);
        errorMessages.Clear();
        return errorsToReturn;
    }
}
