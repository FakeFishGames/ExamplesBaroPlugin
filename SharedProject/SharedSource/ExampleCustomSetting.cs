using Barotrauma;
using Barotrauma.Networking;
using Barotrauma.Plugins;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Xml.Linq;

namespace Examples;

public class MyCustomSetting(Identifier identifier, string defaultValue) : BaseSetting<string>(identifier, defaultValue)
{
    public override void Load(XElement element)
    {
        Value = element.GetAttributeString("value", DefaultValue);
    }

    public override void Save(XElement element)
    {
        element.SetAttribute("value", Value);
    }

#if CLIENT
    public override void CreateUI()
    {
        MainUI = new GUIFrame(new RectTransform((1.0f, 0.10f), null), style: null);

        var horizontal = new GUILayoutGroup(new RectTransform(Vector2.One, MainUI.RectTransform), isHorizontal: true);
        var vertical = new GUILayoutGroup(new RectTransform((0.5f, 1f), horizontal.RectTransform), isHorizontal: false);

        var label = new GUITextBlock(new RectTransform((1f, 0.5f), vertical.RectTransform), "Test setting", wrap: false, textAlignment: Alignment.Center);

        var textBox = new GUITextBox(new RectTransform((1f, 0.5f), vertical.RectTransform), Value);

        textBox.OnAddedToGUIUpdateList = _ =>
        {
            textBox.Text = Value;
            textBox.Enabled = CanClientEdit;
        };

        textBox.OnTextChanged += (GUITextBox textBox, string text) =>
        {
            Set(text);
            return true;
        };

        var image = new GUIImage(new RectTransform(new Point(256, 256), horizontal.RectTransform), ItemPrefab.Prefabs["screwdriver"].InventoryIcon);

        image.OnAddedToGUIUpdateList = (GUIComponent comp) =>
        {
            if (ItemPrefab.Prefabs.TryGet(Value, out ItemPrefab? result))
            {
                image.Sprite = result!.InventoryIcon;
            }
        };
    }
#endif
}

public class ExampleCustomSetting
{
    public static void CreateSettings()
    {
        ISettingsService settingsService = Plugin.SettingsService;
        IDebugConsole debugConsole = Plugin.DebugConsole;

        settingsService.RegisterSetting(new BooleanSetting($"enableredtext".ToIdentifier(), false, label: $"Enable red text")
        {
            ShowInUI = true,
            ToolTip = "Makes all text in game red",
            SyncMode = SettingSyncMode.ClientPermissiveDesync
        });

        settingsService.RegisterSetting(new FloatSetting($"foobar".ToIdentifier(), 5.0f, label: $"Foobar")
        {
            ShowInUI = true,
            ToolTip = "This is foobar",
            LabelFunc = (x) => $"{MathF.Round(x)} foobar(s)",
            StepValue = 1f,
            Range = (0f, 100f),
            SyncMode = SettingSyncMode.ServerAuthority,
            IsAllowedToSet = (ISetting setting, Client? c) => c?.Permissions.HasFlag(ClientPermissions.ManageRound) ?? true
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
            IsAllowedToSet = (ISetting setting, Client? c) => c?.Permissions.HasFlag(ClientPermissions.ManageRound) ?? true
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

        settingsService.RegisterSetting(new MyCustomSetting($"customsetting".ToIdentifier(), "screwdriver")
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
}