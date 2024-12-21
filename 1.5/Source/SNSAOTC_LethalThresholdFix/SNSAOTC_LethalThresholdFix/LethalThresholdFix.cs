using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using HarmonyLib;
using Verse;
using System.Reflection;

namespace SNSAOTC_LethalThresholdFix
{
    [StaticConstructorOnStartup]
    static class Patch_LethalThreshold
    {
        static Patch_LethalThreshold()
        {
            new Harmony("explodoboy.SNSAmbitionCosmic").PatchAll(); //PatchAll() MUST be included in a patch or else nothing happens.
        }
        [HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.LethalDamageThreshold), MethodType.Getter)] //Due to something about how internal vanilla stuff is set up, you've got to modify the getter. I don't know, I didn't make this.
        class PatchHarmony_LethalThreshold
        {
            [HarmonyPostfix]
            public static void LethalDamageThresholdPostfix(ref float __result) //This method does the actual patch.
            {
                __result = 24000f; //This changes the lethal damage theshold to 24,000 as a float value.
            }
        }
    }
}
