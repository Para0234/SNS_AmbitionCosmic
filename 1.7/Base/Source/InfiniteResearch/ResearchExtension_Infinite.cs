using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace InfiniteResearch
{
    public class ResearchExtension_Infinite : DefModExtension
    {
        private static Dictionary<ResearchProjectDef, int> timesCompleted = new Dictionary<ResearchProjectDef, int>();
        private static bool patched;

        public override IEnumerable<string> ConfigErrors()
        {
            if (!patched) DoPatches();
            return base.ConfigErrors();
        }

        public static void DoPatches()
        {
            patched = true;
            var harm = new Harmony("legodude17.infiniteresearch");
            harm.Patch(AccessTools.Method(typeof(ResearchManager), nameof(ResearchManager.FinishProject)),
                postfix: new HarmonyMethod(typeof(ResearchExtension_Infinite), nameof(Reset)));
            harm.Patch(AccessTools.Method(typeof(ResearchManager), nameof(ResearchManager.ExposeData)),
                postfix: new HarmonyMethod(typeof(ResearchExtension_Infinite), nameof(Save)));
        }

        public static void Reset(ResearchProjectDef proj, Dictionary<ResearchProjectDef, float> ___progress)
        {
            if (proj.HasModExtension<ResearchExtension_Infinite>())
            {
                var completedTimes = timesCompleted.ContainsKey(proj) ? timesCompleted[proj] : 0;
                proj.description = proj.description.Replace("SOS.ResearchPerformedTimes".Translate(completedTimes), "").TrimEnd();
                completedTimes++;
                proj.description += "\n\n" + "SOS.ResearchPerformedTimes".Translate(completedTimes);
                timesCompleted[proj] = completedTimes;
                ___progress[proj] = 0f;
            }
        }

        public static void Save(ResearchManager __instance)
        {
            Scribe_Collections.Look(ref timesCompleted, "timesCompleted", LookMode.Def, LookMode.Value);
        }
    }
}