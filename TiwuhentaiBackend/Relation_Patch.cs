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
    [HarmonyPatch(typeof(AiHelper.Relation), "GetStartRelationSuccessRate_BoyOrGirlFriend")]
    class Relation_Patch_GetStartRelationSuccessRate_BoyOrGirlFriend
    {
        static void Postfix(ref int __result,Character selfChar, Character targetChar, RelatedCharacter targetToSelf) 
        {
            int selfCharId = selfChar.GetId();
            int targetCharId = targetChar.GetId();
            int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();
            Debuglogger.Log("__result ConfessionSuccessRate " + __result);
            if (charidTaiwu == selfCharId || charidTaiwu == targetCharId)
            {
                if (Taiwuhentai.adjustFavorabilityWeightInConfession)
                {
                    Debuglogger.Log("rateOfConfessionTaiwu a " + __result);

                    double a = 0 -(double)__result / 50;
                    double b = 12-a;
                    if(b<0)
                    {
                        b = 0;
                    }
                    double c = (13 * b / (12 * b + 24)) * 6;
                    double d =Math.Pow(targetToSelf.Favorability / 3614*c,2);

                    __result += (short)d*3;
                    Debuglogger.Log("rateOfConfessionTaiwu b " + __result);
                }
                if (Taiwuhentai.rateOfConfessionTaiwu > 0 && __result < Taiwuhentai.rateOfConfessionTaiwu * 10)
                {
                    Debuglogger.Log("rateOfConfessionTaiwu c " + __result);
                    __result = Taiwuhentai.rateOfConfessionTaiwu * 10;
                    Debuglogger.Log("rateOfConfessionTaiwu d " + __result);
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
