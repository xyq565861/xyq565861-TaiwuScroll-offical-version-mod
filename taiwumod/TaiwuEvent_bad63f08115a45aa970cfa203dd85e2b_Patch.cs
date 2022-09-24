
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
	public class TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_expresslove__Patch
	{
		[HarmonyPatch("OnOption9VisibleCheck")]
		public static bool Prefix(ref bool __result, TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b __instance)
		{

			bool flag = !EventHelper.CheckMainStoryLineProgress(8);
			
			if (flag)
			{
				__result = false;
			}
			else
			{
				Character character = __instance.ArgBox.GetCharacter("RoleTaiwu");
				Character character2 = __instance.ArgBox.GetCharacter("CharacterId");
				bool flag2 = EventHelper.GetRoleAge(character) >= Taiwuhentai.spouseAge && EventHelper.GetRoleAge(character2) >= Taiwuhentai.spouseAge && EventHelper.CheckHasRelationship(character, character2, 8192) && (!EventHelper.CheckHasRelationship(character, character2, 512)||Taiwuhentai.bloodTies);
				if (flag2)
				{
					bool flag3 = EventHelper.CheckHasRelationship(character, character2, 16384) && EventHelper.CheckHasRelationship(character2, character, 16384);
					if (flag3)
					{
						__result = false;
					}
					else
					{
						bool flag4 =( EventHelper.CheckHasRelationship(character, character2, 2048) || EventHelper.CheckHasRelationship(character2, character, 2048))&&!Taiwuhentai.bloodTies;
						if (flag4)
						{
							__result = false;
						}
						else
						{
							bool flag5 = (EventHelper.HasNominalBloodRelation(character.GetId(), character2.GetId())&&!Taiwuhentai.bloodTies) || (EventHelper.HasBloodExclusionRelation(character.GetId(), character2.GetId())&&!Taiwuhentai.bloodTies);
							__result = !flag5;
						}
					}
				}
				else
				{
					__result = false;
				}
			}


			return false;
		}
	}
	[HarmonyPatch(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b))]
	public class TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_proposal__Patch
	{
		[HarmonyPatch("OnOption10VisibleCheck")]
		public static bool Prefix(ref bool __result, TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b __instance)
		{
			Debuglogger.Log("injected marry event option10");


			bool flag = !EventHelper.CheckMainStoryLineProgress(8);
			if (flag)
			{
				__result = false;
			}
			else
			{
				Character character = __instance.ArgBox.GetCharacter("RoleTaiwu");
                Character character2 = __instance.ArgBox.GetCharacter("CharacterId");
				int charId = -1;
				__instance.ArgBox.Get("CharacterId", ref charId);
				bool flag2 = EventHelper.GetRoleAge(character) >= Taiwuhentai.spouseAge && EventHelper.GetRoleAge(character2) >= Taiwuhentai.spouseAge && (EventHelper.CheckHasRelationship(character, character2, 16384)) && EventHelper.CheckHasRelationship(character2, character, 16384);
				if (flag2)
				{
					bool flag3 = EventHelper.RoleHasAliveSpouse(EventArgBox.TaiwuCharacterId) && !Taiwuhentai.unrestrainedSpouseNum;
					bool flag4 = flag3 || EventHelper.RoleHasAliveSpouse(charId);
					if (flag4)
					{
						__result = false;
					}
					bool flag5 = (EventHelper.GetRoleMonkType(character2) != 0 && !Taiwuhentai.unrestrainedSpouseFactions) || !CanStartHusbandOrWife(character.GetId(), character2.GetId(), 1024);
					if (flag5)
					{
						__result = false;
					}
					bool flag6 = character2.OrgAndMonkTypeAllowMarriage() || Taiwuhentai.unrestrainedSpouseFactions;
					if (flag6)
					{
						__result = true;
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
		public static bool AllowAddingHusbandOrWifeRelation(int charId, int relatedCharId)
		{
			bool flag = ( DomainManager.Character.GetAliveSpouse(charId) >= 0 && !Taiwuhentai.unrestrainedSpouseNum) || DomainManager.Character.GetAliveSpouse(relatedCharId) >= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				RelatedCharacter relation;
				bool flag2 = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
				if (flag2)
				{
					relation.RelationType = ushort.MaxValue;
				}
				bool flag3 = DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation)&&!Taiwuhentai.bloodTies;
				if (flag3)
				{
					result = false;
				}
				else
				{
					bool flag4 = relation.RelationType == ushort.MaxValue;
					if (flag4)
					{
						result = true;
					}
					else
					{
						ushort relationTypes = relation.RelationType;
						bool flag5 = relationTypes == 0;
						if (flag5)
						{
							result = true;
						}
						else
						{
							bool flag6 = (relationTypes & 1024) > 0;
							if (flag6)
							{
								result = false;
							}
							else
							{
								bool flag7 = RelationType.ContainBloodExclusionRelations(relationTypes)&&!Taiwuhentai.bloodTies;
								result = !flag7;
							}
						}
					}
				}
			}
			return result;
		}

		public static bool CanStartHusbandOrWife(int charId, int relatedCharId, ushort relationType)
		{
			bool flag = !AllowAddingHusbandOrWifeRelation(charId, relatedCharId);
			if (!flag)
			{
				Character selfChar = DomainManager.Character.GetElement_Objects(charId);
				Character targetChar = DomainManager.Character.GetElement_Objects(relatedCharId);
				RelatedCharacter selfToTarget = DomainManager.Character.GetRelation(charId, relatedCharId);
				RelatedCharacter targetToSelf = DomainManager.Character.GetRelation(relatedCharId, charId);
				selfToTarget.Favorability = 30000;
				return CanStartRelation_HusbandOrWife(charId, selfToTarget, selfChar.GetBehaviorType(), relatedCharId, targetToSelf, targetChar.GetBehaviorType());

			}
			return false;
		}
		public static bool CanStartRelation_HusbandOrWife(int selfCharId, RelatedCharacter selfToTarget, sbyte selfBehaviorType, int targetCharId, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
		{
			bool flag = (((selfToTarget.RelationType & 16384) == 0 || (targetToSelf.RelationType & 16384) == 0 ))||( RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType) && !Taiwuhentai.bloodTies) || (DomainManager.Character.GetAliveSpouse(selfCharId) >= 0&&!Taiwuhentai.unrestrainedSpouseNum) || DomainManager.Character.GetAliveSpouse(targetCharId) >= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
				AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(5);
				short minSelfFavorabilityReq = relationsCfg.MinFavorability[(int)selfBehaviorType];
				result = ((short)favorabilityTypeToTarget >= minSelfFavorabilityReq);
			}
			return result;
		}
	}
}