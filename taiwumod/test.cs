extern alias gd;

using gd::Config;
using gd::GameData.Domains.Character;
using gd::GameData.Domains.TaiwuEvent.EventHelper;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Character = gd.GameData.Domains.Character.Character;

namespace Taiwuhentai
{

	//[HarmonyPatch(typeof(Character_Patch))]

	//public class Test
	//{
	//	[HarmonyPatch("CalcFertility")]
	//	static void Postfix(Character_Patch __instance, short __result)
	//	{

	//		Dictionary<int, int> _pregnancyLockEndDates = new Dictionary<int, int>();

	//		Debuglogger.Log("gamedate" + _pregnancyLockEndDates);
	//		Debuglogger.Log("result" + __result);
	//			short clampedAge = __instance.GetPhysiologicalAge();
	//			Debuglogger.Log("clampedAge="+ clampedAge+"age="+ __instance.GetActualAge());


			
			
	//	}
	//}

}