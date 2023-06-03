using System.IO;
using UnityEngine;

public class ErrorLogger : MonoBehaviour
{
    private const string LogFileName = "errorlog.txt";
    private static string logFilePath;

    private void Awake()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, LogFileName);
        ClearLog();
        DontDestroyOnLoad(gameObject);
    }

    public static void LogError(string errorMessage)
    {
        string logMessage = "[ERROR] " + errorMessage;
        LogToFile(logMessage);
        Debug.LogError(logMessage);
    }

    public static void LogWarning(string warningMessage)
    {
        string logMessage = "[WARNING] " + warningMessage;
        LogToFile(logMessage);
        Debug.LogWarning(logMessage);
    }

    private static void LogToFile(string logMessage)
    {
        using (StreamWriter writer = File.AppendText(logFilePath))
        {
            writer.WriteLine(logMessage);
        }
    }

    public static void ClearLog()
    {
        File.WriteAllText(logFilePath, string.Empty);
    }
}
