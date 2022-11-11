extern alias gd;

using gd::GameData.Common;
using gd::GameData.Domains;
using gd::GameData.Domains.Character;
using gd::GameData.Domains.Character.Relation;
using gd::GameData.Domains.Map;
using gd::GameData.Domains.TaiwuEvent;
using gd::GameData.Domains.TaiwuEvent.EventHelper;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taiwuhentai
{
    [HarmonyPatch(typeof(EventHelper), "AddRelation")]
    class EventHelper_Patch_AddRelation
    {
        static bool Prefix(int charId, int relatedCharId, ushort relationType)
        {
            int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
            bool flag =   Taiwuhentai.bloodTies && (taiwuCharId == charId || taiwuCharId == relatedCharId);
            if (flag)
            {
                DomainManager.Character.AddRelation(DomainManager.TaiwuEvent.MainThreadDataContext, charId, relatedCharId, relationType, int.MinValue);
                return false;
            }

            return true;

        }
    }
    [HarmonyPatch(typeof(EventHelper), "ApplyRelationBecomeBoyOrGirlFriend")]
    class EventHelper_Patch_ApplyRelationBecomeBoyOrGirlFriend
    {
        static bool Prefix(Character selfChar, Character targetChar, bool succeed) 
        {
            int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
            bool flag = succeed && Taiwuhentai.expelOtherAdored && (taiwuCharId == selfChar.GetId() || taiwuCharId == targetChar.GetId());
            if (flag)
            {
                if (taiwuCharId == selfChar.GetId())
                {
                    HashSet<int> targetCharAliveAdored = HentaiUtility.GetAllAliveAdored(targetChar.GetId());

                    if (targetCharAliveAdored != null && targetCharAliveAdored.Count > 0)
                    {
                        DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
                        Location location = targetChar.GetLocation();
                        int currDate = DomainManager.World.GetCurrDate();
                        foreach (var item in targetCharAliveAdored)
                        {
                            RelatedCharacter relation = DomainManager.Character.GetRelation(targetChar.GetId(), item);
                            RelatedCharacter relation2 = DomainManager.Character.GetRelation(item, targetChar.GetId());
                            if (RelationType.HasRelation(relation.RelationType, RelationType.Adored))
                            {
                                DomainManager.Character.ChangeRelationType(mainThreadDataContext, targetChar.GetId(), item, RelationType.Adored, 0);
                            }
                            if (RelationType.HasRelation(relation2.RelationType, RelationType.Adored))
                            {
                                DomainManager.Character.ChangeRelationType(mainThreadDataContext, item, targetChar.GetId(), RelationType.Adored, 0);
                            }

                            DomainManager.LifeRecord.GetLifeRecordCollection().AddBreakupMutually(targetChar.GetId(), currDate, item, location);
                        }
                    }

                }
                if (taiwuCharId == targetChar.GetId())
                {
                    HashSet<int> selfCharAliveAdored = HentaiUtility.GetAllAliveAdored(selfChar.GetId());

                    if (selfCharAliveAdored != null && selfCharAliveAdored.Count > 0)
                    {
                        DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
                        Location location = selfChar.GetLocation();
                        int currDate = DomainManager.World.GetCurrDate();
                        foreach (var item in selfCharAliveAdored)
                        {
                            RelatedCharacter relation = DomainManager.Character.GetRelation(selfChar.GetId(), item);
                            RelatedCharacter relation2 = DomainManager.Character.GetRelation(item, selfChar.GetId());
                            if (RelationType.HasRelation(relation.RelationType, RelationType.Adored))
                            {
                                DomainManager.Character.ChangeRelationType(mainThreadDataContext, selfChar.GetId(), item, RelationType.Adored, 0);
                            }
                            if (RelationType.HasRelation(relation2.RelationType, RelationType.Adored))
                            {
                                DomainManager.Character.ChangeRelationType(mainThreadDataContext, item, selfChar.GetId(), RelationType.Adored, 0);
                            }
                            DomainManager.LifeRecord.GetLifeRecordCollection().AddBreakupMutually(selfChar.GetId(), currDate, item, location);
                        }
                    }

                }
            }





            return true;

        }
    }
    [HarmonyPatch(typeof(EventHelper), "ApplyRelationBecomeHusbandOrWife")]
    class EventHelper_Patch_ApplyRelationBecomeHusbandOrWife
    {
        static bool Prefix(Character selfChar, Character targetChar, bool succeed)
        {
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = succeed && Taiwuhentai.allowTaiwuNtr && (taiwuCharId == selfChar.GetId() || taiwuCharId == targetChar.GetId());
			if (flag)
            {
                if (taiwuCharId == selfChar.GetId())
                {
                    HashSet<int> targetCharAliveSpouse =HentaiUtility. GetAllAliveSpouse(targetChar.GetId());

                    if (targetCharAliveSpouse!=null&&targetCharAliveSpouse.Count>0)
                    {
                        DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
                        Location location = targetChar.GetLocation();
                        int currDate = DomainManager.World.GetCurrDate();
                        foreach (var item in targetCharAliveSpouse)
                        {
                            RelatedCharacter relation = DomainManager.Character.GetRelation(targetChar.GetId(), item);
                            RelatedCharacter relation2 = DomainManager.Character.GetRelation(item, targetChar.GetId());
                            if (RelationType.HasRelation(relation.RelationType, RelationType.HusbandOrWife))
                            {
                                DomainManager.Character.ChangeRelationType(mainThreadDataContext, targetChar.GetId(), item, RelationType.HusbandOrWife, 0);
                            }
                            if (RelationType.HasRelation(relation2.RelationType, RelationType.HusbandOrWife))
                            {
                                DomainManager.Character.ChangeRelationType(mainThreadDataContext, item, targetChar.GetId(), RelationType.HusbandOrWife, 0);
                            }
                            DomainManager.LifeRecord.GetLifeRecordCollection().AddBreakupMutually(targetChar.GetId(), currDate, item, location);
                        }
                    }
                   
                }
                if (taiwuCharId == targetChar.GetId())
                {
                    HashSet<int> selfCharAliveSpouse = HentaiUtility.GetAllAliveSpouse(selfChar.GetId());

                    if (selfCharAliveSpouse != null && selfCharAliveSpouse.Count > 0)
                    {
                        DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
                        Location location = selfChar.GetLocation();
                        int currDate = DomainManager.World.GetCurrDate();
                        foreach (var item in selfCharAliveSpouse)
                        {
                            RelatedCharacter relation = DomainManager.Character.GetRelation(selfChar.GetId(), item);
                            RelatedCharacter relation2 = DomainManager.Character.GetRelation(item, selfChar.GetId());
                            if (RelationType.HasRelation(relation.RelationType, RelationType.HusbandOrWife))
                            {
                                DomainManager.Character.ChangeRelationType(mainThreadDataContext, selfChar.GetId(), item, RelationType.HusbandOrWife, 0);
                            }
                            if (RelationType.HasRelation(relation2.RelationType, RelationType.HusbandOrWife))
                            {
                                DomainManager.Character.ChangeRelationType(mainThreadDataContext, item, selfChar.GetId(), RelationType.HusbandOrWife, 0);
                            }
                            DomainManager.LifeRecord.GetLifeRecordCollection().AddBreakupMutually(selfChar.GetId(), currDate, item, location);
                        }
                    }

                }

            }
			return true;
        }

    }
}
