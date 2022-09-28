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
		static void Postfix(Character __instance, short __result)
		{
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
            if (Taiwuhentai.fertilityIgnoreAgeTaiwu)
            {
				if (taiwuId == __instance.GetId())
				{
					__result = (short)(__result + 100);
					return;
				}
			}
            if (Taiwuhentai.fertilityIgnoreAgeTaiwuSpouse)
            {

				if (HentaiUtility.GetTaiwuAliveSpousePool().Contains(__instance.GetId()))
                {
					__result = (short)(__result + 100);
					return;
				}
				if (HentaiUtility.GetTaiwuAliveAdoredPool().Contains(__instance.GetId()))
				{
					__result = (short)(__result + 100);
					return;
				}

			}
		}
	}
}
