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
    [HarmonyPatch(typeof(ModDomain))]
    class ModDomain_Patch
    {
        [HarmonyPatch("LoadAllEventPackages")]
        static void Postfix()
        {
            Harmony harmony = new Harmony("Taiwuhentai event");
            Debuglogger.Log("star injecting event package dll");
            harmony.PatchAll(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_ExpressLove__Patch));
            harmony.PatchAll(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_Proposal__Patch));

            Debuglogger.Log("injected event package dll");
        }
    }
}
