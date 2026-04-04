namespace PluginToolbox;

public static class Logger
{
    public static void Log(string message)
    {
        Console.WriteLine(message);
    }

    public static void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}