extern alias gd;

using gd::Config;
using gd::GameData.Domains;
using gd::GameData.Domains.Character.Ai;
using gd::GameData.Domains.Character.Relation;
using gd::GameData.Domains.TaiwuEvent.EventHelper;
using System;
using System.Collections.Generic;
using System.Reflection;
using Character = gd::GameData.Domains.Character.Character;

namespace Taiwuhentai
{
	class HentaiUtility
	{

		public static bool GetLesBianIO(Character character,Character target)
        {
			int taiwuid = DomainManager.Taiwu.GetTaiwuCharId();
			Debuglogger.Log(string.Format("taiwuid{0} character{1} target{2}", taiwuid, character.GetId(), target.GetId()));
			if (Taiwuhentai.lesbianPregnantTaiwu&&character.GetGender()==  target.GetGender()&& (taiwuid == character.GetId() && Taiwuhentai.lesbianPregnantIO == 1 || taiwuid==target.GetId()&&Taiwuhentai.lesbianPregnantIO == 2))
			{
				Debuglogger.Log("true");
				return true;
            }
			Debuglogger.Log("false");

			return false;
        }
		public static bool AllowAddingHusbandOrWifeRelation(int charId, int relatedCharId)
		{
			bool flag = (DomainManager.Character.GetAliveSpouse(charId) >= 0 && !Taiwuhentai.unrestrainedSpouseNum) || (DomainManager.Character.GetAliveSpouse(relatedCharId) >= 0 && !Taiwuhentai.allowTaiwuNtr);
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
				bool flag3 = DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation) && !Taiwuhentai.bloodTies;
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
								bool flag7 = RelationType.ContainBloodExclusionRelations(relationTypes) && !Taiwuhentai.bloodTies;
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
			Debuglogger.Log("CanStartHusbandOrWife" + flag);
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
			bool flag = (((selfToTarget.RelationType & 16384) == 0 || (targetToSelf.RelationType & 16384) == 0)) || (RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType) && !Taiwuhentai.bloodTies) || (DomainManager.Character.GetAliveSpouse(selfCharId) >= 0 && !Taiwuhentai.unrestrainedSpouseNum) || (DomainManager.Character.GetAliveSpouse(targetCharId) >= 0 && !Taiwuhentai.allowTaiwuNtr);
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
		public static HashSet<int> GetAllAliveSpouse(int charId)
		{
			HashSet<int> spouseCharIds = new HashSet<int>();
			RelatedCharacters relatedChars;
			Dictionary<int, RelatedCharacters> _relatedCharIds = new Dictionary<int, RelatedCharacters>();
			if (DomainManager.Character != null)
			{
				IEnumerable<FieldInfo> runTimeFieldAttributes = DomainManager.Character.GetType().GetRuntimeFields();
				foreach (var item in runTimeFieldAttributes)
				{

					if (item.Name.Equals("_relatedCharIds"))
					{
						_relatedCharIds = (Dictionary<int, RelatedCharacters>)item.GetValue(DomainManager.Character);
						Debuglogger.Log("Hit _relatedCharIds");
						break;
					}

				}
			}
			if (_relatedCharIds.Count > 1)
			{
				bool flag = _relatedCharIds.TryGetValue(charId, out relatedChars);

				if (flag)
				{
					HashSet<int> spouseCharIdsSub = relatedChars.HusbandsAndWives.GetCollection();
					Debuglogger.Log("spouseCollection " + spouseCharIdsSub.Count);
					foreach (int spouseCharId in spouseCharIdsSub)
					{
						Debuglogger.Log("spouseCharId " + spouseCharId);
						bool flag2 = DomainManager.Character.IsCharacterAlive(spouseCharId);
						if (flag2)
						{
							Debuglogger.Log("spouseCharId Alive " + spouseCharId);
							spouseCharIds.Add(spouseCharId);
						}
					}

				}

			}
			return spouseCharIds;
		}

		public static HashSet<int> GetAllAliveAdored(int charId)
		{
			HashSet<int> spouseCharIds = new HashSet<int>();
			RelatedCharacters relatedChars;
			Dictionary<int, RelatedCharacters> _relatedCharIds = new Dictionary<int, RelatedCharacters>();
			if (DomainManager.Character != null)
			{
				IEnumerable<FieldInfo> runTimeFieldAttributes = DomainManager.Character.GetType().GetRuntimeFields();
				foreach (var item in runTimeFieldAttributes)
				{

					if (item.Name.Equals("_relatedCharIds"))
					{
						_relatedCharIds = (Dictionary<int, RelatedCharacters>)item.GetValue(DomainManager.Character);
						Debuglogger.Log("Hit _relatedCharIds" + item.Name);
						break;
					}

				}
			}
			if (_relatedCharIds.Count > 1)
			{
				bool flag = _relatedCharIds.TryGetValue(charId, out relatedChars);

				if (flag)
				{
					HashSet<int> spouseCharIdsSub = relatedChars.Adored.GetCollection();
					foreach (int spouseCharId in spouseCharIdsSub)
					{
						Debuglogger.Log("spouseCharId " + spouseCharId);
						bool flag2 = DomainManager.Character.IsCharacterAlive(spouseCharId);
						if (flag2)
						{
							Debuglogger.Log("spouseCharId Alive " + spouseCharId);
							spouseCharIds.Add(spouseCharId);
						}
					}

				}

			}
			return spouseCharIds;
		}


		public static HashSet<int> GetTaiwuAliveSpousePool()
		{

			int gameDate = EventHelper.GetGameDate();
			if (taiwuAliveSpousePool.poolDate != gameDate)
			{
				try
				{
					int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
					taiwuAliveSpousePool.characterIdPool = GetAllAliveSpouse(taiwuId);
					taiwuAliveSpousePool.poolDate = gameDate;
				}
				catch (Exception ex)
				{
					Debuglogger.Log(ex.Message + '\n' + ex.StackTrace);
				}
			}

			return taiwuAliveSpousePool.characterIdPool;



		}
		public static HashSet<int> GetTaiwuAliveAdoredPool()
		{
			int gameDate = EventHelper.GetGameDate();
			if (taiwuAliveAdoredool.poolDate != gameDate)
			{
				try
				{
					int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
					taiwuAliveAdoredool.characterIdPool = GetAllAliveAdored(taiwuId);
					taiwuAliveAdoredool.poolDate = gameDate;
				}
				catch (Exception ex)
				{
					Debuglogger.Log(ex.Message + '\n' + ex.StackTrace);
				}
			}

			return taiwuAliveSpousePool.characterIdPool;
		}
		static HentaiCharacterIdPool taiwuAliveSpousePool;
		static HentaiCharacterIdPool taiwuAliveAdoredool;
		struct HentaiCharacterIdPool
		{
			public int poolDate;
			public HashSet<int> characterIdPool;
			HentaiCharacterIdPool(int gameDate, HashSet<int> anyIdPool)
			{
				poolDate = gameDate;
				characterIdPool = anyIdPool;
			}

		}



	}
}
