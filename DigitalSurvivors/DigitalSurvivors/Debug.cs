using Spectre.Console;

namespace DigitalSurvivors;

public static class Debug
{
    public static event Action<String> OnDebug;

    public static void Log(object message)
    {
        OnDebug?.Invoke($"[LOG] {DateTime.Now:HH:mm:ss} - {message}");
    }

    public static void LogWarning(object message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        OnDebug?.Invoke($"[WARNING] {DateTime.Now:HH:mm:ss} - {message}");
        Console.ResetColor();
    }

    public static void LogError(object message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        OnDebug?.Invoke($"[ERROR] {DateTime.Now:HH:mm:ss} - {message}");
        Console.ResetColor();
    }
}