using Barotrauma;
using Barotrauma.Plugins;
using System.Xml.Linq;
using Barotrauma.Networking;

namespace Examples;

public class ExampleStatusEffectAction
{
    public static void Register()
    {
        Plugin.StatusEffectService.RegisterAction(new StatusEffectActionFactory("Broadcast", 
            (StatusEffect statusEffect, XElement element) => new BroadcastStatusEffectAction(statusEffect, element)));
    }
}

public class BroadcastStatusEffectAction : IStatusEffectAction
{
    private readonly string _message;

    public BroadcastStatusEffectAction(StatusEffect statusEffect, XElement element)
    {
        _message = element.GetAttributeString("message", "");
    }

    public void Apply(StatusEffectParams effectParams)
    {
        if (_message.IsNullOrEmpty()) { return; }

        Character? character = effectParams.Targets.OfType<Character>().FirstOrDefault();

        string msg = _message;
        if (character != null)
        {
            msg = $"{character.Name}: {msg}";
        }

#if SERVER
        GameMain.Server.SendChatMessage(msg, ChatMessageType.ServerMessageBoxInGame);
#elif CLIENT
        if (GameMain.NetworkMember is null)
        {
            GUIMessageBox messageBox = new GUIMessageBox("", msg, Array.Empty<LocalizedString>(), type: GUIMessageBox.Type.InGame);
        }
#endif
    }
}