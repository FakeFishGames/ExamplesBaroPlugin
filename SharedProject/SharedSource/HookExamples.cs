using Barotrauma;
using Barotrauma.Plugins;
using Microsoft.Xna.Framework;

namespace Examples;

public static class HookExamples
{
    public static void Register()
    {
        Plugin.HookService.RegisterHook<PluginRoundStartedDelegate>(OnRoundStarted);
        Plugin.HookService.RegisterHook<PluginRoundEndedDelegate>(OnRoundEnded);
    }

    private static void OnRoundStarted(GameSession gameSession)
    {
        Plugin.DebugConsole.NewMessage("Round started!", Color.Red);
    }

    private static void OnRoundEnded(GameSession gameSession)
    {
        Plugin.DebugConsole.NewMessage("Round ended!", Color.Red);
    }
}