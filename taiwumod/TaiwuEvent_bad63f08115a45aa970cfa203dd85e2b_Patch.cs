
extern alias gd;
using ConchShip.EventConfig.Taiwu;
using gd.Config;
using gd.GameData.Domains;
using gd.GameData.Domains.Character;
using gd.GameData.Domains.Character.Ai;
using gd.GameData.Domains.Character.Relation;
using gd.GameData.Domains.TaiwuEvent;
using gd.GameData.Domains.TaiwuEvent.EventHelper;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character = gd.GameData.Domains.Character.Character;

namespace Taiwuhentai
{
	[HarmonyPatch(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b))]
	public class TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_ExpressLove__Patch
	{
		[HarmonyPatch("OnOption9VisibleCheck")]
		public static bool Prefix(ref bool __result, TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b __instance)
		{

			bool flag = !EventHelper.CheckMainStoryLineProgress(8);

			if (flag)
			{
				__result = false;
				return false;
			}
			else
			{
				Character character = __instance.ArgBox.GetCharacter("RoleTaiwu");
				Character character2 = __instance.ArgBox.GetCharacter("CharacterId");
				bool flag2 = EventHelper.GetRoleAge(character) >= Taiwuhentai.spouseAge && EventHelper.GetRoleAge(character2) >= Taiwuhentai.spouseAge && EventHelper.CheckHasRelationship(character, character2, 8192) && (!EventHelper.CheckHasRelationship(character, character2, 512) || Taiwuhentai.bloodTies);
				if (flag2)
				{
					bool flag3 = EventHelper.CheckHasRelationship(character, character2, 16384) && EventHelper.CheckHasRelationship(character2, character, 16384);
					if (flag3)
					{
						__result = false;
						return false;
					}
					else
					{
						bool flag4 = (EventHelper.CheckHasRelationship(character, character2, 2048) || EventHelper.CheckHasRelationship(character2, character, 2048)) && !Taiwuhentai.bloodTies;
						if (flag4)
						{
							__result = false;
							return false;
						}
						else
						{
							bool flag5 = (EventHelper.HasNominalBloodRelation(character.GetId(), character2.GetId()) && !Taiwuhentai.bloodTies) || (EventHelper.HasBloodExclusionRelation(character.GetId(), character2.GetId()) && !Taiwuhentai.bloodTies);
							__result = !flag5;
						}
					}
				}
				else
				{
					__result = false;
					return false;
				}
			}


			return false;
		}
	}
	[HarmonyPatch(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b))]
	public class TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_Proposal__Patch
	{
		[HarmonyPatch("OnOption10VisibleCheck")]
		public static bool Prefix(ref bool __result, TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b __instance)
		{
			Debuglogger.Log("injected marry event option10");


			bool flag = !EventHelper.CheckMainStoryLineProgress(8);
			if (flag)
			{
				__result = false;
				return false;
			}
			else
			{
				Character character = __instance.ArgBox.GetCharacter("RoleTaiwu");
				Character character2 = __instance.ArgBox.GetCharacter("CharacterId");
				int charId = -1;
				__instance.ArgBox.Get("CharacterId", ref charId);
				bool hasMarried = (EventHelper.CheckHasRelationship(character, character2, 1024) || EventHelper.CheckHasRelationship(character2, character, 1024));				
				if (hasMarried)
				{
					__result = false;
					return false;
				}
				bool flag2 = EventHelper.GetRoleAge(character) >= Taiwuhentai.spouseAge && EventHelper.GetRoleAge(character2) >= Taiwuhentai.spouseAge && (EventHelper.CheckHasRelationship(character, character2, 16384)) && EventHelper.CheckHasRelationship(character2, character, 16384);

				if (flag2)
				{
					Debuglogger.Log("RoleHasAliveSpouse " + EventArgBox.TaiwuCharacterId +"_"+EventHelper.RoleHasAliveSpouse(EventArgBox.TaiwuCharacterId));
					Debuglogger.Log("RoleHasAliveSpouse " + charId + "_"+EventHelper.RoleHasAliveSpouse(charId));
					bool flag3 = EventHelper.RoleHasAliveSpouse(EventArgBox.TaiwuCharacterId) && !Taiwuhentai.unrestrainedSpouseNum;
					bool flag4 = (flag3 || EventHelper.RoleHasAliveSpouse(charId));
					if (flag4)
					{
						Debuglogger.Log("injected marry event option10 flag3,4);" + flag4);

						__result = false;
						return false;
					}
					bool flag5 = (EventHelper.GetRoleMonkType(character2) != 0 && !Taiwuhentai.unrestrainedSpouseFactions) || !HentaiUtility.CanStartHusbandOrWife(character.GetId(), character2.GetId(), 1024);
					if (flag5)
					{
						Debuglogger.Log("injected marry event option10 flag5);"+ flag5);

						__result = false;
						return false;
					}
					bool flag6 = character2.OrgAndMonkTypeAllowMarriage() || Taiwuhentai.unrestrainedSpouseFactions;
					if (flag6)
					{
						Debuglogger.Log("injected marry event option10 flag6);"+ flag6);

						__result = true;
						return false;
					}
				}

			}

			return false;
		}
		[HarmonyPatch("OnOption11VisibleCheck")]
		public static bool Prefix(ref bool __result)
		{
			Debuglogger.Log("injected marry event option11");
			if (Taiwuhentai.unrestrainedSpouseFactions)
			{
				__result = false;
				return false;
			}
			return true;
		}
	}
}