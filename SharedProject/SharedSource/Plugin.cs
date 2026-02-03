using Barotrauma;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace Examples;

public partial class Plugin : IBarotraumaPlugin
{
    public static readonly IDebugConsole DebugConsole = PluginServiceProvider.GetService<IDebugConsole>();
    public static readonly ISettingsService SettingsService = PluginServiceProvider.GetService<ISettingsService>();
    public static readonly IItemComponentRegistrar ItemComponentRegistrar = PluginServiceProvider.GetService<IItemComponentRegistrar>();
    public static readonly ISimpleHookService HookService = PluginServiceProvider.GetService<ISimpleHookService>();
    public static readonly IHarmonyProvider HarmonyProvider = PluginServiceProvider.GetService<IHarmonyProvider>();
    public static readonly IContentFileRegistrar ContentFileRegistrar = PluginServiceProvider.GetService<IContentFileRegistrar>();
    public static readonly IGameNetwork GameNetwork = PluginServiceProvider.GetService<IGameNetwork>();

    public void Init()
    {
        DebugConsole.NewMessage("Plugin example loaded", Color.Lime);

        InitProjectSpecific();

        ContentFileRegistrar.RegisterContentFile<ExampleFile>();
        ItemComponentRegistrar.RegisterItemComponent<ExampleItemComponent>();

        HarmonyProvider.PatchAll();

        HookExamples.Register();
        ExampleCustomSetting.CreateSettings();
        ExampleNetworking.Register();

        DebugConsole.RegisterCommand("forceunloadfail", "forceunloadfail: Starts a thread that is never cleaned up that causes the examples plugin to never unload.", 
            CommandFlags.DoNotRelayToServer, (string[] args) =>
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1);
                }
            });

            thread.Start();

            DebugConsole.NewMessage("OK", Color.Aqua);
        });
    }

    public partial void InitProjectSpecific();

    public void OnContentLoaded()
    {
        
    }
    public void Dispose()
    {
        DebugConsole.NewMessage("Plugin example unloaded", Color.Red);
    }
}