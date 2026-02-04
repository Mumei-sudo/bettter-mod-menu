using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace BetterModMenu
{
    [HarmonyPatch(typeof(Page_ModsConfig), "DoRequirementSection")]
    public static class Patch_DoRequirementSection
    {
        public static FieldInfo visibleReqsField = AccessTools.Field(typeof(Page_ModsConfig), "visibleReqsCached");
        public static void Postfix(Page_ModsConfig __instance, ref float __result)
        {
            List<ModRequirement> visibleReqs = (List<ModRequirement>)visibleReqsField.GetValue(__instance);
            if (visibleReqs.Count == 0)
                __result -= 10;
        }
    }
}
