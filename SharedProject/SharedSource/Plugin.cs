using Barotrauma;
using Barotrauma.Plugins;
using Microsoft.Xna.Framework;

namespace Examples;

public partial class Plugin : IBarotraumaPlugin
{
    public static readonly IDebugConsole DebugConsole = PluginServiceProvider.GetService<IDebugConsole>();
    public static readonly ISettingsService SettingsService = PluginServiceProvider.GetService<ISettingsService>();
    public static readonly IItemComponentRegistrar ItemComponentRegistrar = PluginServiceProvider.GetService<IItemComponentRegistrar>();

    public void Init()
    {
        DebugConsole.NewMessage("Plugin loaded", Color.Lime);

        ItemComponentRegistrar.RegisterAllItemComponents();

        InitProjectSpecific();
    }

    public partial void InitProjectSpecific();

    public void Dispose() 
    { 
        DebugConsole.NewMessage("Plugin unloaded", Color.Red); 
    }

    public void OnContentLoaded()
    {
        
    }
}