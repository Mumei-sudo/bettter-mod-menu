using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BetterModMenu
{
    [HarmonyPatch(typeof(Page_ModsConfig), "DoModRow")]
    internal static class Patch_DoModRow
    {
        public static void Prefix(Page_ModsConfig __instance, ref Rect r, ModMetaData mod)
        {
            if (BetterModMenu.settings.modColors.ContainsKey(mod.PackageId))
                Widgets.DrawBoxSolid(r, BetterModMenu.settings.modColors[mod.PackageId]);

            // Show mod ID if checked
            //    if (!BetterModMenu.showModsID) return;
            //    Traverse trav = Traverse.Create(__instance);
            //    List<ModMetaData> activeMods = trav.Field("activeModListOrderCached").GetValue<List<ModMetaData>>();
            //    List<ModMetaData> inactiveMods = trav.Field("inactiveModListOrderCached").GetValue<List<ModMetaData>>();
            //    int index = activeMods.IndexOf(mod);
            //    if (index < 0)
            //        index = inactiveMods.IndexOf(mod);
            //    Text.Anchor = TextAnchor.MiddleLeft;
            //    Widgets.Label(r, index.ToString());
            //    Text.Anchor = TextAnchor.UpperLeft;
            //    r.x += 28;
        }
    }
}