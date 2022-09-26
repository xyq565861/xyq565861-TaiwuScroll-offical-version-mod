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
            harmony.PatchAll(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_expresslove__Patch));
            harmony.PatchAll(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_proposal__Patch));
            //harmony.PatchAll(typeof(RelationType_Patch));
            //harmony.PatchAll(typeof(Test));
            Debuglogger.Log("injected event package dll");
        }
    }
}
