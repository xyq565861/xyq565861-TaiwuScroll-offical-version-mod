extern alias gd;

using ConchShip.EventConfig.Taiwu;
using gd::GameData.Common;
using gd::GameData.Domains;
using gd::GameData.Domains.Character;
using gd::GameData.Domains.Character.Relation;
using gd::GameData.Domains.TaiwuEvent.EventHelper;
using gd::GameData.Utilities;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taiwuhentai
{
    [HarmonyPatch(typeof(Character))]
    class test
    {
        [HarmonyPatch("ApplyBecomeHusbandOrWife")]
        static void Prefix(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool succeed, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
        {
            Debuglogger.Log("in Character marry event"+ "context " + context+ ", selfChar " + selfChar+ ", targetChar" + targetChar+ ", charBehaviorType " + charBehaviorType+ ", succeed " + succeed+ ", selfIsTaiwuPeople " + selfIsTaiwuPeople+ ", targetIsTaiwuPeople " + targetIsTaiwuPeople);
        }

    }

}
