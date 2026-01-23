using Barotrauma;
using Barotrauma.Plugins;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Xml.Linq;

namespace Examples;

public class MyCustomSetting(Identifier identifier, byte defaultValue) : BaseSetting<byte>(identifier, defaultValue)
{
    public override void Load(XElement element)
    {
        throw new NotImplementedException();
    }

    public override void Save(XElement element)
    {
        throw new NotImplementedException();
    }

#if CLIENT
    public override void CreateUI()
    {
        MainUI = new GUIFrame(new RectTransform((1.0f, 0.10f), null), style: null);
        var label = new GUITextBlock(new RectTransform((0.28f, 1.0f), MainUI.RectTransform), "Test setting", wrap: false, textAlignment: Alignment.Center);

        new GUIImage(new RectTransform(new Point(256, 256), MainUI.RectTransform), ItemPrefab.Prefabs["screwdriver"].InventoryIcon);
    }
#endif
}

public class SettingTestMod : IBarotraumaPlugin
{
    public void Init()
    {
        var settingsService = Plugin.SettingsService;
        var debugConsole = Plugin.DebugConsole;

        settingsService.RegisterSetting(new FloatSetting($"foobar".ToIdentifier(), 5.0f, label: $"Foobar")
        {
            ShowInUI = true,
            ToolTip = "This is foobar",
            LabelFunc = (x) => $"{MathF.Round(x)} foobar(s)",
            StepValue = 1f,
            Range = (0f, 100f),
            SyncMode = SettingSyncMode.ServerAuthority,
            IsAllowedToSet = c => c.Permissions.HasFlag(Barotrauma.Networking.ClientPermissions.ManageRound)
        });

        settingsService.RegisterSetting(new FloatSetting($"fooslider".ToIdentifier(), 5.0f, label: $"Fooslider")
        {
            ShowInUI = true,
            UseSlider = true,
            ToolTip = "This is foobar but slider",
            LabelFunc = (x) => $"{MathF.Round(x)} foobar(s)",
            StepValue = 1f,
            Range = (0f, 100f),
            SyncMode = SettingSyncMode.ServerAuthority,
            IsAllowedToSet = c => c.Permissions.HasFlag(Barotrauma.Networking.ClientPermissions.ManageRound)
        });

        settingsService.RegisterSetting(new BooleanSetting($"foobool".ToIdentifier(), true, label: $"Foobool active")
        {
            ShowInUI = true,
            ToolTip = "This is foobool",
            SyncMode = SettingSyncMode.ClientPermissiveDesync
        });

        settingsService.RegisterSetting(new StringSetting($"foostring".ToIdentifier(), "Bar!", label: $"Foostring")
        {
            ShowInUI = true,
            ToolTip = "This is foostring",
            MaxSize = 32,
            SyncMode = SettingSyncMode.NoSync
        });

        settingsService.RegisterSetting(new IntSetting($"fooint".ToIdentifier(), 42, label: $"Footint")
        {
            ShowInUI = true,
            ToolTip = "This is fooint",
            Range = (-10, 1000),
            SyncMode = SettingSyncMode.NoSync
        });

        settingsService.RegisterSetting(new MyCustomSetting($"customsetting".ToIdentifier(), 7)
        {
            ShowInUI = true,
            SyncMode = SettingSyncMode.NoSync
        });

        debugConsole.RegisterCommand(
            command: "settingfoobar",
            helpMessage: "",
            flags: CommandFlags.DoNotRelayToServer,
            onCommandExecuted: (string[] args) =>
            {
                if (args.Length == 0)
                {
                    debugConsole.NewMessage($"Setting value is {settingsService?.RetrieveSetting<FloatSetting>("foobar".ToIdentifier())?.Value}.");
                    return;
                }

                float.TryParse(args[0], out float value);

                settingsService?.RetrieveSetting<FloatSetting>("foobar".ToIdentifier())?.Set(value);
            });

        debugConsole.RegisterCommand(
            command: "settingfoobool",
            helpMessage: "",
            flags: CommandFlags.DoNotRelayToServer,
            onCommandExecuted: (string[] args) =>
            {
                if (args.Length == 0)
                {
                    debugConsole.NewMessage($"Setting value is {settingsService?.RetrieveSetting<BooleanSetting>("foobool".ToIdentifier())?.Value}.");
                    return;
                }

                bool.TryParse(args[0], out bool value);

                settingsService?.RetrieveSetting<BooleanSetting>("foobool".ToIdentifier())?.Set(value);
            });
    }

    public void Dispose()
    {

    }

    public void OnContentLoaded()
    {

    }
}