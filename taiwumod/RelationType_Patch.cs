extern alias gd;

using gd::GameData.Domains;
using gd::GameData.Domains.Character.Relation;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taiwuhentai
{
	[HarmonyPatch(typeof(RelationType))]
	class RelationType_Patch
	{
		[HarmonyPatch("AllowAddingHusbandOrWifeRelation")]
		static void Postfix(ref bool __result, int charId, int relatedCharId)
		{
			
			int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();
			Debuglogger.Log("in RelationType marry event" + " charId " + charId + " relatedCharId " + relatedCharId + " taiwuid " + charidTaiwu + "charIdtaiwu?" + (charId == charidTaiwu) + "relatedCharIdchartaiwu?" + (relatedCharId == charidTaiwu));

			if(charidTaiwu!= charId&& charidTaiwu!= relatedCharId)
            {
				return;
            }
			bool flag = (DomainManager.Character.GetAliveSpouse(charId) >= 0 && !Taiwuhentai.unrestrainedSpouseNum) || (DomainManager.Character.GetAliveSpouse(relatedCharId) >= 0 && !Taiwuhentai.unrestrainedSpouseNum);

			if (flag)
			{
				__result = false;
				return;
			}
			else
			{
				RelatedCharacter relation;
				bool flag2 = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
				if (flag2)
				{
					relation.RelationType = ushort.MaxValue;
				}
				bool flag3 = (DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation) && !Taiwuhentai.bloodTies);
				if (flag3)
				{
					__result = false;
					return;
				}
				else
				{
					bool flag4 = relation.RelationType == ushort.MaxValue;
					if (flag4)
					{
						__result = true;
						return;
					}
					else
					{
						ushort relationTypes = relation.RelationType;
						bool flag5 = relationTypes == 0;
						if (flag5)
						{
							__result = true;
							return;
						}
						else
						{
							bool flag6 = (relationTypes & 1024) > 0;
							if (flag6)
							{
								__result = false;
								return;
							}
							else
							{
								bool flag7 = (RelationType.ContainBloodExclusionRelations(relationTypes) && !Taiwuhentai.bloodTies);
								__result = !flag7;
								return;
							}
						}
					}
				}
			}
			return ;
		}

	}
}
