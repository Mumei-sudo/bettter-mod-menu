using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace BetterModMenu
{
    [HarmonyPatch(typeof(Page_ModsConfig), "DoModInfo")]
    internal static class Patch_DoModInfo
    {
        public static List<Color> colors = new List<Color>
    {
        new Color(0f, 0f, 0f, 0f),
        Color.white,
        Color.gray,
        Color.black,
        new Color(0.75f, 0.75f, 0.75f),
        new Color(0.4f, 0.4f, 0.4f),

        Color.red,
        Color.green,
        Color.blue,

        Color.yellow,
        Color.cyan,
        Color.magenta,

        new Color(0.72f, 0.52f, 0.35f),
        new Color(0.55f, 0.65f, 0.85f),
        new Color(0.45f, 0.7f, 0.45f),
        new Color(0.7f, 0.45f, 0.45f),

        new Color(0.9f, 0.6f, 0.1f),
        new Color(0.6f, 0.3f, 0.7f),
        // Dark Reds
        new Color(0.40f, 0.15f, 0.15f),
        new Color(0.45f, 0.20f, 0.20f),
        new Color(0.35f, 0.10f, 0.10f),
        // Dark Greens
        new Color(0.15f, 0.35f, 0.25f),
        new Color(0.10f, 0.30f, 0.20f),
        new Color(0.20f, 0.40f, 0.30f),
        // Dark Blues
        new Color(0.15f, 0.22f, 0.35f),
        new Color(0.10f, 0.30f, 0.45f),
        new Color(0.12f, 0.25f, 0.40f),
        // Dark Purples
        new Color(0.30f, 0.18f, 0.40f),
        new Color(0.25f, 0.15f, 0.35f),
        new Color(0.20f, 0.12f, 0.30f),
    };
        public static FieldInfo descriptionCachedField = AccessTools.Field(typeof(ModMetaData), "descriptionCached");
               
        public static void Prefix(ref ModMetaData mod)
        {
            if (descriptionCachedField.GetValue(mod) == null)
            {
                string text = mod.Description;
                text = Regex.Replace(text, @"\[(h[1-3])\](.*?)\[/\1\]", "<size=18px><color=#5aa9d6>$2</color></size>");
                text = text.Replace("[hr][/hr]", "________________________________________________\n");
                text = Regex.Replace(text, $@"\[(\/?)(\w+)(=[^\]]+)?\]", "");
                text = Regex.Replace(text, @"\[\*\]\s*", "- ");
                text = text.Trim();
                var lines = text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                    lines[i] = lines[i].Trim();
                text = string.Join("\n", lines);
                text = Regex.Replace(text, @"\n\s*\n+", "\n\n");
                descriptionCachedField.SetValue(mod, text);
            }
        }

        public static void Postfix(Rect r, ModMetaData mod)
        {
            // Fix bug on closing window
            if (mod.PreviewImage == null) return;

            // Calc preview image height
            var width = Mathf.Min(mod.PreviewImage.width, r.width);
            var height = mod.PreviewImage.height * (width / mod.PreviewImage.width);
            height = Mathf.Min(height, Mathf.Ceil(r.height * 0.35f));

            // Draw color button + selector
            Rect rect = new Rect(r.xMax - 30f, height + 10f, 30f, 30f);
            var color = BetterModMenu.settings.modColors.GetValueOrDefault(mod.PackageId, new Color(0f, 0f, 0f, 0f));
            Widgets.DrawBoxSolidWithOutline(rect, color, Color.white);
            if (Widgets.ButtonInvisible(rect))
            {
                Find.WindowStack.Add(new Dialog_ChooseColor("", color, colors, (c =>
                {
                    BetterModMenu.settings.modColors[mod.PackageId] = c;
                    BetterModMenu.Instance.WriteSettings();
                })));
            }
        }
    }
}