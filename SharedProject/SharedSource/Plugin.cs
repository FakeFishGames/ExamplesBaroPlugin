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

    public void Init()
    {
        DebugConsole.NewMessage("Plugin example loaded", Color.Lime);

        InitProjectSpecific();

        ItemComponentRegistrar.RegisterAllItemComponents();
        HarmonyProvider.PatchAll();

        HookExamples.RegisterHooks();
    }


    public partial void InitProjectSpecific();

    public void Dispose() 
    { 
        DebugConsole.NewMessage("Plugin example unloaded", Color.Red); 
    }

    public void OnContentLoaded()
    {
        
    }
}