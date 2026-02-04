using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace BetterModMenu
{
    [HarmonyPatch(typeof(Page_ModsConfig), "DoWindowContents")]
    internal static class Patch_DoWindowContents
    {
        public const float searchBarWidth = 220f;
        public const float modlistY = 70f;

        // Move vanilla UI positions
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool widgetGroupBegan = false;
            bool movedSearchBar = false;

            foreach (var instruction in instructions)
            {
                if (instruction.Calls(AccessTools.Method(typeof(Widgets), "BeginGroup")))
                    widgetGroupBegan = true;
                if (widgetGroupBegan && !movedSearchBar)
                {
                    // Vanilla searchbar y
                    if (instruction.opcode == OpCodes.Ldloc_2)
                    {
                        instruction.opcode = OpCodes.Ldc_R4;
                        instruction.operand = 35f;
                    }
                    // Vanilla searchbar width
                    if (instruction.OperandIs(354f))
                    {
                        instruction.operand = searchBarWidth;
                        movedSearchBar = true;
                    }
                }
                // Vanilla modlist y
                if (instruction.OperandIs(40f))
                    instruction.operand = modlistY;

                // Move dragged mods position to be right of mouse instead middle so can scroll the modlist under
                if (instruction.OperandIs(125f))
                    instruction.operand = -1f;

                yield return instruction;
            }
        }

        public static void Prefix(Page_ModsConfig __instance)
        {
            var trav = Traverse.Create(__instance);

            // Draw enabled mods search bar
            BetterModMenu.quickSearchWidget.OnGUI(new Rect(searchBarWidth + 30, 35f, searchBarWidth, 30f));
            if (BetterModMenu.quickSearchWidget.filter.Text != BetterModMenu.filter2)
                trav.Field("modListsDirty").SetValue(true);
            BetterModMenu.filter2 = BetterModMenu.quickSearchWidget.filter.Text;
            
            // Display DLCs and Mods count
            List<ModMetaData> enabledContents = ModsConfig.ActiveModsInLoadOrder.ToList();
            List<ModMetaData> disabledContents = trav.Field("inactiveModListOrderCached").GetValue<List<ModMetaData>>();
            int enabledDLCs = enabledContents.Where(c => c.Expansion != null).Count() - 1;
            int totalDLCs = ModLister.AllExpansions.Count() - 1;
            int enabledMods = enabledContents.Count() - enabledDLCs - 1;
            int totalMods = enabledContents.Count() + disabledContents.Count() - totalDLCs - 1;
            Widgets.Label(new Rect(0f, 0f, 999f, 30f),
                $"{"DLCs".Translate()}: {enabledDLCs}/{totalDLCs}    {"Mods".Translate()}: {enabledMods}/{totalMods}");
        }
    }
}