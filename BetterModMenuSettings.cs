using System.Collections.Generic;
using UnityEngine;
using Verse;

public class BetterModMenuSettings : ModSettings
{
    public Dictionary<string, Color> modColors = new Dictionary<string, Color>();
    public override void ExposeData()
    {
        Scribe_Collections.Look(ref modColors, "modColors");
    }
}