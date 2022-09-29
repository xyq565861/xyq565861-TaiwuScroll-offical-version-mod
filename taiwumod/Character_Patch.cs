extern alias gd;

using gd::Config;
using gd::GameData.Domains;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character = gd.GameData.Domains.Character.Character;

namespace Taiwuhentai
{
	[HarmonyPatch(typeof(Character))]
	class Character_Patch
	{
		[HarmonyPatch("CalcFertility")]
		static void Postfix(Character __instance, ref short __result)
		{
			
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			if (Taiwuhentai.fertilityIgnoreAgeTaiwu)
			{
				if (taiwuId == __instance.GetId())
				{
					__result = (short)(__result + 100);
					Debuglogger.Log("taiwuId" + __result);
					return;
				}
			}
			if (Taiwuhentai.fertilityIgnoreAgeTaiwuSpouse)
			{

				if (HentaiUtility.GetTaiwuAliveSpousePool().Contains(__instance.GetId()))
				{
					__result = (short)(__result + 100);
					Debuglogger.Log("GetTaiwuAliveSpousePool" + __result);

					return;
				}
				if (HentaiUtility.GetTaiwuAliveAdoredPool().Contains(__instance.GetId()))
				{
					__result = (short)(__result + 100);
					Debuglogger.Log("GetTaiwuAliveAdoredPool" + __result);

					return;
				}

			}
		}

		[HarmonyPatch("OfflineMakeLove")]
		static void Postfix(Character __instance,  bool __result, Character father, Character mother)
		{
			int fatherId = father.GetId();
			int motherId = mother.GetId();
			int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();
			if (charidTaiwu == fatherId || charidTaiwu == motherId)
			{ 
				Debuglogger.Log("OfflineMakeLove" + __result);
			}
		}
	}

}
