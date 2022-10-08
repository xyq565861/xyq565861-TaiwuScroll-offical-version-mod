extern alias gd;

using gd::GameData.Domains;
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
        static void Postfix(ref int __result,Character selfChar, Character targetChar) 
        {
            int selfCharId = selfChar.GetId();
            int targetCharId = targetChar.GetId();
            int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();

            if (charidTaiwu == selfCharId || charidTaiwu == targetCharId)
            {
                if (Taiwuhentai.rateOfConfessionTaiwu > 0 && __result < Taiwuhentai.rateOfConfessionTaiwu * 10)
                {
                    Debuglogger.Log("rateOfConfessionTaiwu a " + __result);
                    __result = Taiwuhentai.rateOfConfessionTaiwu * 10;
                    Debuglogger.Log("rateOfConfessionTaiwu b " + __result);
                }
                return;
            }


            if (Taiwuhentai.preventTaiwuSpouseStray)
            {
                HashSet<int> taiwuSpouse = HentaiUtility.GetTaiwuAliveSpousePool();
                if(taiwuSpouse.Contains(selfCharId)|| taiwuSpouse.Contains(targetCharId))
                {
                    __result = -500;
                    return;
                }
                HashSet<int> taiwuAdored = HentaiUtility.GetTaiwuAliveAdoredPool();
                if (taiwuAdored.Contains(selfCharId) || taiwuAdored.Contains(targetCharId))
                {
                    __result = -500;
                    return;
                }
            }

            if (Taiwuhentai.rateOfConfession != 1 && __result > 0)
            {
                switch (Taiwuhentai.rateOfConfession)
                {
                    case 0:
                        __result = (int)(__result  * 0.5);
                        break;
                    case 2:
                        __result = __result * 2;
                        break;

                }
               

            }
            return;

        }

    }


   

}
