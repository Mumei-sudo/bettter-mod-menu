using HarmonyLib;
using RimWorld;

namespace BetterModMenu
{
    [HarmonyPatch(typeof(Page_ModsConfig), "PreOpen")]
    internal static class Patch_PreOpen
    {
        public static void Postfix(Page_ModsConfig __instance)
        {
            BetterModMenu.modsConfigPage = __instance;
            BetterModMenu.quickSearchWidget.filter.Text = "";
        }
    }
}