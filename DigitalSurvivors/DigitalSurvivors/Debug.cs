using Spectre.Console;

namespace DigitalSurvivors;

// Custom debug class that triggers log events and printing them to my custom logger
public static class Debug
{
    // Event for handling debug messages externally
    public static event Action<String> OnDebug;

    // Logs a standard message
    public static void Log(object message)
    {
        OnDebug?.Invoke($"[LOG] {DateTime.Now:HH:mm:ss} - {message}");
    }

    // Logs a warning message
    public static void LogWarning(object message)
    {
        OnDebug?.Invoke($"[WARNING] {DateTime.Now:HH:mm:ss} - {message}");
    }

    // Logs an error message
    public static void LogError(object message)
    {
        OnDebug?.Invoke($"[ERROR] {DateTime.Now:HH:mm:ss} - {message}");
    }
}