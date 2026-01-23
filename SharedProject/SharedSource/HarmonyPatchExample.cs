using HarmonyLib;
using Barotrauma;
using Microsoft.Xna.Framework;

namespace Examples;

[HarmonyPatch]
public static class HarmonyPatchExamples
{
#if CLIENT
    [HarmonyPatch(typeof(GUITextBlock), nameof(GUITextBlock.TextColor), MethodType.Getter), HarmonyPostfix]
    private static void GUITextBlock_GetTextColor_Post(ref Color __result)
    {
        __result = Color.Red;
    }
#endif
}