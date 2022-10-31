using GameData.Domains.TaiwuEvent.EventOption;
using HarmonyLib;
using System;

namespace TeamMateLess
{
	[HarmonyPatch(typeof(OptionConditionMatcher), "TeamMateLess")]
	public class OptionConditionMatcher_Patch
	{

		public static void Postfix(ref bool __result)
		{
            if (TeamMateLess.enable)
            {
				__result = true;
			}
				
		}
	}
}
