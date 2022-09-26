extern alias gd;

using gd::GameData.Domains.Character;
using gd::GameData.Domains.Character.Ai;
using gd::GameData.Domains.Character.Relation;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taiwuhentai
{
    [HarmonyPatch(typeof(AiHelper.Relation))]
    class Relation_Patch
    {
        [HarmonyPatch("GetStartRelationSuccessRate_BoyOrGirlFriend")]
        static void Postfix(ref int __result) {


            Debuglogger.Log("Origin successRate BoyOrGirlFriend  "+ __result);

            if (Taiwuhentai.rateOfConfession>0&& __result < Taiwuhentai.rateOfConfession*10)
            {
                __result = Taiwuhentai.rateOfConfession * 10;
            }
        }
    }
}
