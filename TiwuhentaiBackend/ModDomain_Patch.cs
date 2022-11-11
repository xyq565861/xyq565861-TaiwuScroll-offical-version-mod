extern alias gd;

using gd::GameData.Domains.Mod;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taiwuhentai
{
    [HarmonyPatch(typeof(ModDomain), "LoadAllEventPackages")]
    class ModDomain_Patch
    {
        static void Postfix()
        {
            Harmony harmony = new Harmony("Taiwuhentai event");
            Debuglogger.Log("star injecting event package dll");
            if (Taiwuhentai.spouseAge!=16||Taiwuhentai.bloodTies)
            {
                harmony.PatchAll(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_ExpressLove_Patch_OnOption9VisibleCheck));
                Debuglogger.Log("patch TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_ExpressLove_Patch_OnOption9VisibleCheck");


            }
            if (Taiwuhentai.allowTaiwuNtr||Taiwuhentai.unrestrainedSpouseNum||Taiwuhentai.unrestrainedSpouseFactions||Taiwuhentai.bloodTies)
            {
                harmony.PatchAll(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_Proposal_Patch_OnOption10VisibleCheck));
                Debuglogger.Log("patch TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_Proposal_Patch_OnOption10VisibleCheck");


            }
            if (Taiwuhentai.unrestrainedSpouseFactions)
            {
                harmony.PatchAll(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_Proposal_Patch_OnOption11VisibleCheck));
                Debuglogger.Log("patch TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_Proposal_Patch_OnOption11VisibleCheck");

            }
            Debuglogger.Log("injected event package dll");
        }
    }
}
