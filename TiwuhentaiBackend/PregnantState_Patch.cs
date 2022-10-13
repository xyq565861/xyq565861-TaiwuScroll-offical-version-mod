extern alias gd;

using gd::GameData.Domains;
using gd::GameData.Domains.Character;
using gd::GameData.Utilities;
using HarmonyLib;
using Redzen.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taiwuhentai
{

    [HarmonyPatch(typeof(PregnantState))]

    class PregnantState_Patch
    {
        [HarmonyPatch("CheckPregnant")]
        public static void Postfix(ref bool __result, IRandomSource random, Character father, Character mother)
        {
            int num;
            int fatherId = father.GetId();
            int motherId = mother.GetId();
            int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();
            bool flagGender = father.GetGender() != mother.GetGender()||Taiwuhentai.lesbianPregnantTaiwu;
            bool flagMotherStatus = !mother.GetFeatureIds().Contains(197);
            int randomElement = (int)Math.Round((double)(DomainManager.World.GetProbAdjustOfCreatingCharacter() * 25f * (float)father.GetFertility() * (float)mother.GetFertility() / 10000f));
            bool Pregnancylock= !DomainManager.Character.TryGetElement_PregnancyLockEndDates(mother.GetId(), out num);


            if (charidTaiwu == fatherId || charidTaiwu == motherId)
            {
                if (Taiwuhentai.rateOfPregnantTaiwu != 2)
                {
                    switch (Taiwuhentai.rateOfPregnantTaiwu)
                    {
                        case 0:
                            __result = false;
                            break;
                        case 1:
                            __result = flagGender && flagMotherStatus&& Pregnancylock && random.CheckPercentProb((int)0.5*randomElement);
                            break;
                        case 3:
                            __result = flagGender && flagMotherStatus&& Pregnancylock && random.CheckPercentProb((int)2 * randomElement);
                            break;
                        case 4:


                            RemoveElement_PregnancyLockEndDates
                            DomainManager.RemoveElement_PregnancyLockEndDates();
                            __result = flagGender && flagMotherStatus ;
                            Debuglogger.Log("CheckPregnant Taiwu 4 " + __result);
                            break;
                    }                   

                }
                return;
            }
            if (Taiwuhentai.rateOfPregnant != 1)
            {
                switch (Taiwuhentai.rateOfPregnantTaiwu)
                {
                    case 0:
                        __result = flagGender && flagMotherStatus && random.CheckPercentProb((int)0.5 * randomElement);
                        break;
                    case 2:
                        __result = flagGender && flagMotherStatus && random.CheckPercentProb((int)2 * randomElement);
                        break;

                }
                return;
            }

        }

    }
}
