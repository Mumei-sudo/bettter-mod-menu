using HarmonyLib;
using RimWorld;
using Verse;

namespace BetterModMenu
{
    public class BetterModMenu : Mod
    {
        public static BetterModMenu Instance;
        public static BetterModMenuSettings settings;
        public static Page_ModsConfig modsConfigPage;
        public static QuickSearchWidget quickSearchWidget = new QuickSearchWidget();
        public static string filter2 = "";

        public BetterModMenu(ModContentPack content) : base(content)
        {
            Instance = this;
            settings = GetSettings<BetterModMenuSettings>();
            new Harmony("Mumei.BetterModMenu").PatchAll();
        }
    }
}