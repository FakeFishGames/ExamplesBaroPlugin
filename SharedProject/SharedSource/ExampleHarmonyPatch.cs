using HarmonyLib;
using Barotrauma;
using Microsoft.Xna.Framework;
using Barotrauma.Plugins;

namespace Examples;

[HarmonyPatch]
public static class ExampleHarmonyPatch
{
#if CLIENT
    // Patches the text color getter to always return Color.Red, making all text in game red
    // Note that HarmonyProvider.PatchAll() must be called at startup for these patch annotations to work
    [HarmonyPatch(typeof(GUITextBlock), nameof(GUITextBlock.TextColor), MethodType.Getter), HarmonyPostfix]
    private static void GUITextBlock_GetTextColor_Post(ref Color __result)
    {
        if (Plugin.SettingsService.RetrieveSetting<BooleanSetting>("enableredtext".ToIdentifier())?.Value == true)
        {
            __result = Color.Red;
        }
    }
#endif
}