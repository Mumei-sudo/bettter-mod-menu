using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace BetterModMenu
{
    // Change vanilla filters
    [HarmonyPatch(typeof(Page_ModsConfig), "ModListsInOrder")]
    internal static class Patch_ModListsInOrder
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int count = 0;
            bool skippingfilteringmods = false;
            foreach (var instruction in instructions)
            {
                if (instruction.OperandIs(AccessTools.Field(typeof(Page_ModsConfig), "filteredActiveModListOrderCached")))
                    count++;
                if (count == 2)
                    skippingfilteringmods = true;
                if (skippingfilteringmods)
                {
                    if (instruction.opcode == OpCodes.Callvirt) // AddRange
                        count++;
                    if (count == 4)
                    {
                        skippingfilteringmods = false;
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Call,
                            AccessTools.Method(typeof(Patch_ModListsInOrder), "FilterModLists"));
                    }
                    continue;
                }
                yield return instruction;
            }
        }
        public static void FilterModLists(Page_ModsConfig modsPage)
        {
            Traverse trav = Traverse.Create(modsPage);
            string filter = trav.Field("filter").GetValue<string>();
            List<ModMetaData> filteredActiveModList = trav.Field("filteredActiveModListOrderCached").GetValue<List<ModMetaData>>();
            List<ModMetaData> filteredInactiveActiveModList = trav.Field("filteredInactiveModListOrderCached").GetValue<List<ModMetaData>>();
            List<ModMetaData> inactiveMods = trav.Field("inactiveModListOrderCached").GetValue<List<ModMetaData>>()
                .Where(m => m.Name.ToLower().Contains(filter.ToLower())).ToList();
            List<ModMetaData> activeMods = trav.Field("activeModListOrderCached").GetValue<List<ModMetaData>>()
                .Where(m => m.Name.ToLower().Contains(BetterModMenu.filter2.ToLower())).ToList();
            filteredInactiveActiveModList.AddRange(inactiveMods);
            filteredActiveModList.AddRange(activeMods);

        }
    }
}