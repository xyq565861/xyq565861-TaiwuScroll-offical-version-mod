extern alias gd;

using gd::GameData.Common;
using gd::GameData.Domains;
using gd::GameData.Domains.Character;
using gd::GameData.Domains.Character.Creation;
using gd::GameData.Domains.Character.ParallelModifications;
using gd::GameData.Domains.Character.Relation;
using gd::GameData.Domains.Taiwu;
using gd::GameData.Utilities;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Taiwuhentai
{
    [HarmonyPatch(typeof(CharacterDomain))]
    class CharacterDomain_Newbron_Patch
    {
        [HarmonyPatch("CreatePregnantState")]
        static bool Prefix(CharacterDomain __instance, DataContext context, Character mother, Character father, bool isRaped)
        {
           
            int currDate = DomainManager.World.GetCurrDate();
            int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
            bool flag = taiwuCharId == mother.GetId() || taiwuCharId == father.GetId();
            if (flag)
            {
                PregnantState state = new PregnantState(mother, father, isRaped);
                state.IsHuman =( !context.Random.CheckPercentProb(DomainManager.Taiwu.GetCricketLuckPoint() / 100))||Taiwuhentai.preventCricketPregnant;
                bool isHuman = state.IsHuman;
                float pregnantTimerate = 1;
                switch (Taiwuhentai.rateOfPregnantTaiwu)
                {
                    case 0:
                        pregnantTimerate = 1;
                        break;
                    case 1:
                        pregnantTimerate = 0.5f;
                        break;
                    case 2:
                        pregnantTimerate = 0.25f;
                        break;
                    case 3:
                        pregnantTimerate = 2f;
                        break;
                    case 4:
                        pregnantTimerate = 4f;
                        break;
                    default:
                        break;
                }
                if (isHuman)
                {
                    state.ExpectedBirthDate =(int)(( currDate + context.Random.Next(6, 10) * pregnantTimerate));
                }
                else
                {
                    state.ExpectedBirthDate = (int)((currDate + 42 * pregnantTimerate));
                }
                Type type = typeof(CharacterDomain);
                BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo addPregnantStatesMethod = type.GetMethod("AddElement_PregnantStates", bindingFlags);
                addPregnantStatesMethod.Invoke(__instance, new object[] { mother.GetId(), state, context });
                return false;
            }
            else
            {
                return true;
            }


        }


        [HarmonyPatch("ParallelCreateNewbornChildren")]
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {

            //mainChildGender.SetLocalSymInfo("15",15,16);
            Label label1 = il.DefineLabel();
            Label label2 = il.DefineLabel();
            Label label3 = il.DefineLabel();


            CodeInstruction node1 = new CodeInstruction(OpCodes.Ldc_I4_1);
            CodeInstruction node2 = new CodeInstruction(OpCodes.Stloc_S, 65);
            CodeInstruction node3 = new CodeInstruction(OpCodes.Nop);
            node1.labels.Add(label1);
            node2.labels.Add(label2);
            node3.labels.Add(label3);
            //Debuglogger.Log((sbyte)11);
            //Debuglogger.Log((int)11);
            //Debuglogger.Log(mainChildGender.LocalType);
            //Debuglogger.Log(mainChildGender.IsPinned);
            //230 01FA ldloc.s motherId(2)
            //231 01FC ldsfld  class [Taiwuhentai.dll]
            //        GameData.Domains.Taiwu.TaiwuDomain[Taiwuhentai.dll] GameData.Domains.DomainManager::Taiwu
            //232	0201	callvirt instance int32[Taiwuhentai.dll] GameData.Domains.Taiwu.TaiwuDomain::GetTaiwuCharId()
            //233	0206	beq.s   239 (0218) ldc.i4.1 
            //234	0208	ldloc.s fatherId(4)
            //235	020A ldsfld  class [Taiwuhentai.dll]
            //        GameData.Domains.Taiwu.TaiwuDomain[Taiwuhentai.dll] GameData.Domains.DomainManager::Taiwu
            //236	020F	callvirt instance int32[Taiwuhentai.dll] GameData.Domains.Taiwu.TaiwuDomain::GetTaiwuCharId()
            //237	0214	ceq
            //238	0216	br.s    240 (0219) stloc.s V_65(65)
            //239	0218	ldc.i4.1
            //240	0219	stloc.s V_65(65)
            //241	021B ldloc.s V_65 (65)
            //242	021D	brfalse.s	248 (0229) ldloc.0 
            //243	021F	nop

            instructions = new CodeMatcher(instructions)
                         .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
                             new CodeMatch(OpCodes.Call, typeof(Gender).GetMethod("GetRandom"))
                         )
                .Repeat(matcher => // Do the following for each match
                            matcher.Advance(2).InsertAndAdvance(
                                 new CodeInstruction(OpCodes.Ldloc_S, 2),
                                 new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(DomainManager), nameof(DomainManager.Taiwu))),
                                 new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(TaiwuDomain), "GetTaiwuCharId")),
                                 new CodeInstruction(OpCodes.Beq_S, label1),
                                 new CodeInstruction(OpCodes.Ldloc_S, 4),
                                 new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(DomainManager), nameof(DomainManager.Taiwu))),
                                 new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(TaiwuDomain), "GetTaiwuCharId")),
                                 new CodeInstruction(OpCodes.Ceq, null),
                                 new CodeInstruction(OpCodes.Br_S, label2),
                                 node1,
                                 node2,
                                 new CodeInstruction(OpCodes.Ldloc_S, 65),
                                 new CodeInstruction(OpCodes.Brfalse_S, label3),
                                 new CodeInstruction(OpCodes.Nop, null),
                                 new CodeInstruction(OpCodes.Ldc_I4_2, null),
                                 new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Taiwuhentai), nameof(Taiwuhentai.childGender))),
                                 new CodeInstruction(OpCodes.Cgt, null),
                                 new CodeInstruction(OpCodes.Brfalse_S, label3),
                                 new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Taiwuhentai), nameof(Taiwuhentai.childGender))),
                                 new CodeInstruction(OpCodes.Conv_I1, null),
                                 new CodeInstruction(OpCodes.Stloc_S, 15),
                                 new CodeInstruction(OpCodes.Ldloca_S, 15),
                                 new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Debuglogger), "Log", new Type[] { typeof(object) })),
                                 node3
                            ).SetAndAdvance(OpCodes.Ldloc_0, null)
                        )

               .InstructionEnumeration();
            instructions = new CodeMatcher(instructions)
                 .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
                     new CodeMatch(OpCodes.Call, typeof(Gender).GetMethod("Flip"))
                         )
                .Repeat(matcher => // Do the following for each match
                            matcher.SetAndAdvance(OpCodes.Nop, null)
                        )

                .InstructionEnumeration();

            return instructions;
        }

        [HarmonyPatch("ParallelCreateIntelligentCharacter")]
        static bool Prefix(ref CreateIntelligentCharacterModification __result, DataContext context, ref IntelligentCharacterCreationInfo info, bool recordModification = true)
        {
          if(info.PregnantState != null)
            {
                if (info.FatherCharId == DomainManager.Taiwu.GetTaiwuCharId() || info.MotherCharId == DomainManager.Taiwu.GetTaiwuCharId())
                {
                    CreateIntelligentCharacterModification createIntelligentCharacterModification = new CreateIntelligentCharacterModification();
                    createIntelligentCharacterModification.FatherCharId = info.FatherCharId;
                    createIntelligentCharacterModification.MotherCharId = info.MotherCharId;
                    PregnantState pregnantState = info.PregnantState;
                    bool flag1 = false;
                    bool flag2 = false;
                    if (!pregnantState.CreateFatherRelation)
                    {
                        if (Taiwuhentai.unrestrainedSpouseNum)
                        {
                            HashSet<int> fatherSpouses = HentaiUtility.GetAllAliveSpouse(info.FatherCharId);
                            foreach (var item in fatherSpouses)
                            {
                                Debuglogger.Log("fatherSpouses" + item);
                                if (item.Equals(info.MotherCharId))
                                {
                                    Debuglogger.Log("Hit fatherSpouses" + item);
                                    flag1 = true;
                                    break;
                                }
                            }
                        }
                        if (Taiwuhentai.responsibleParent && !flag1)
                        {
                            HashSet<int> fatherSpouses = HentaiUtility.GetAllAliveAdored(info.FatherCharId);
                            foreach (var item in fatherSpouses)
                            {
                                if (item.Equals(info.MotherCharId))
                                {
                                    flag1 = true;
                                    break;
                                }
                            }
                        }

                    }
                    if (!pregnantState.CreateMotherRelation)
                    {
                        if (Taiwuhentai.unrestrainedSpouseNum)
                        {
                            HashSet<int> motherSpouses = HentaiUtility.GetAllAliveSpouse(info.MotherCharId);
                            foreach (var item in motherSpouses)
                            {
                                if (item.Equals(info.FatherCharId))
                                {
                                    flag2 = true;
                                    break;
                                }
                            }
                        }
                        if (Taiwuhentai.responsibleParent && !flag2)
                        {
                            HashSet<int> motherSpouses = HentaiUtility.GetAllAliveAdored(info.MotherCharId);
                            foreach (var item in motherSpouses)
                            {
                                if (item.Equals(info.FatherCharId))
                                {
                                    flag2 = true;
                                    break;
                                }
                            }
                        }
                    }



                    createIntelligentCharacterModification.CreateFatherRelation = (info.PregnantState != null && (flag1 || info.PregnantState.CreateFatherRelation || info.FatherCharId != info.PregnantState.FatherId));
                    createIntelligentCharacterModification.CreateMotherRelation = (pregnantState != null && (flag2 || pregnantState.CreateMotherRelation));
                    createIntelligentCharacterModification.ReincarnationCharId = info.ReincarnationCharId;
                    CreateIntelligentCharacterModification mod = createIntelligentCharacterModification;
                    Character character = new Character(info.CharTemplateId);
                    mod.Self = character;
                    character.OfflineCreateIntelligentCharacter(context, mod, ref info);
                    if (recordModification)
                    {
                        ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
                        recorder.RecordType(ParallelModificationType.CreateIntelligentCharacter);
                        recorder.RecordParameterClass<CreateIntelligentCharacterModification>(mod);
                        recorder.RecordParameterUnmanaged<bool>(true);
                    }

                    Debuglogger.Log(
                        " Is Taiwu event" +
                        " FatherCharId:" + createIntelligentCharacterModification.FatherCharId +
                        " MotherCharId:" + createIntelligentCharacterModification.MotherCharId +
                        " CreateFatherRelation:" + createIntelligentCharacterModification.CreateFatherRelation +
                        " InfoPregnantStateCreateFatherRelation:" + pregnantState.CreateFatherRelation +
                        " InfoPregnantStateFatherCharId:" + pregnantState.FatherId +
                        " CreateMotherRelation:" + createIntelligentCharacterModification.CreateMotherRelation +
                        " ReincarnationCharId:" + createIntelligentCharacterModification.ReincarnationCharId +
                        " recordModification:" + recordModification
                        );
                    //PropertyInfo[] propertyInfos = DomainManager.Character.GetType().GetProperties();
                    //foreach (var item in propertyInfos)
                    //{
                    //    Debuglogger.Log(" propertyInfos:" + item.Name + " PropertyType.Name:" + item.PropertyType.Name + " Attributes:" + item.Attributes.ToString());


                    // }
                    //FieldInfo[] fieldAttributes = DomainManager.Character.GetType().GetFields();
                    //foreach (var item in fieldAttributes)
                    //{
                    //    Debuglogger.Log(" fieldAttributes:" + item.Name + " FieldInfo.Name:" + item.FieldType.Name + " Attributes:" + item.Attributes.ToString());


                    //}
                    //IEnumerable<FieldInfo> runTimeFieldAttributes = DomainManager.Character.GetType().GetRuntimeFields();
                    //Dictionary<int, RelatedCharacters> _relatedCharIds=new Dictionary<int, RelatedCharacters>();
                    //foreach (var item in runTimeFieldAttributes)
                    //{
                    //    Debuglogger.Log(" runTimeFieldAttributes:" + item.Name + " FieldInfo.Name:" + item.FieldType.Name + " Attributes:" + item.Attributes.ToString());
                    //    if (item.Name.Equals("_relatedCharIds"))
                    //    {
                    //        _relatedCharIds = (Dictionary<int, RelatedCharacters>)item.GetValue(DomainManager.Character);
                    //        Debuglogger.Log("Hit _relatedCharIds");
                    //    }
                    //}
                    //if (_relatedCharIds != null)
                    //{

                    //    Debuglogger.Log("_relatedCharIds.size:"+ _relatedCharIds.Count);

                    //}
                    __result = mod;
                    return false;

                }
            }
            
            return true;
        }
      
        

    }
    [HarmonyPatch(typeof(CharacterDomain))]
    class CharacterDomain_Marry_Patch
    {
        [HarmonyPatch("AddHusbandOrWifeRelations")]
        static bool Prefix(CharacterDomain __instance, DataContext context, int charId, int spouseCharId, int establishmentDate = -2147483648)
        {
            if (Taiwuhentai.unrestrainedSpouseNum&&(charId == DomainManager.Taiwu.GetTaiwuCharId() || spouseCharId == DomainManager.Taiwu.GetTaiwuCharId()))
            {
                __instance.AddRelation(context, charId, spouseCharId, 1024, establishmentDate);
                return false;
                //List<int> selfBloodChildren = ObjectPool<List<int>>.Instance.Get();
                //selfBloodChildren.Clear();
                //selfBloodChildren.AddRange(__instance.GetRelatedCharIds(charId, 2));
                //List<int> spouseBloodChildren = ObjectPool<List<int>>.Instance.Get();
                //spouseBloodChildren.Clear();
                //spouseBloodChildren.AddRange(__instance.GetRelatedCharIds(spouseCharId, 2));
                //List<int> selfStepChildren = ObjectPool<List<int>>.Instance.Get();
                //selfStepChildren.Clear();
                //selfStepChildren.AddRange(__instance.GetRelatedCharIds(charId, 16));
                //List<int> spouseStepChildren = ObjectPool<List<int>>.Instance.Get();
                //spouseStepChildren.Clear();
                //spouseStepChildren.AddRange(__instance.GetRelatedCharIds(spouseCharId, 16));
                //List<int> selfAdoptiveChildren = ObjectPool<List<int>>.Instance.Get();
                //selfAdoptiveChildren.Clear();
                //selfAdoptiveChildren.AddRange(__instance.GetRelatedCharIds(charId, 128));
                //List<int> spouseAdoptiveChildren = ObjectPool<List<int>>.Instance.Get();
                //spouseAdoptiveChildren.Clear();
                //spouseAdoptiveChildren.AddRange(__instance.GetRelatedCharIds(spouseCharId, 128));
                //int i = 0;
                //int count = selfBloodChildren.Count;
                //while (i < count)
                //{
                //    bool flag = selfBloodChildren[i] != spouseCharId;
                //    if (flag)
                //    {
                //        __instance.AddRelation(context, selfBloodChildren[i], spouseCharId, 8, establishmentDate);
                //    }
                //    i++;
                //}
                //int j = 0;
                //int count2 = spouseBloodChildren.Count;
                //while (j < count2)
                //{
                //    bool flag2 = spouseBloodChildren[j] != charId;
                //    if (flag2)
                //    {
                //        __instance.AddRelation(context, spouseBloodChildren[j], charId, 8, establishmentDate);
                //    }
                //    j++;
                //}
                //int k = 0;
                //int count3 = selfStepChildren.Count;
                //while (k < count3)
                //{
                //    bool flag3 = selfStepChildren[k] != spouseCharId;
                //    if (flag3)
                //    {
                //        __instance.AddRelation(context, selfStepChildren[k], spouseCharId, 8, establishmentDate);
                //    }
                //    k++;
                //}
                //int l = 0;
                //int count4 = spouseStepChildren.Count;
                //while (l < count4)
                //{
                //    bool flag4 = spouseStepChildren[l] != charId;
                //    if (flag4)
                //    {
                //        __instance.AddRelation(context, spouseStepChildren[l], charId, 8, establishmentDate);
                //    }
                //    l++;
                //}
                //int m = 0;
                //int count5 = selfAdoptiveChildren.Count;
                //while (m < count5)
                //{
                //    bool flag5 = selfAdoptiveChildren[m] != spouseCharId;
                //    if (flag5)
                //    {
                //        __instance.AddRelation(context, selfAdoptiveChildren[m], spouseCharId, 64, establishmentDate);
                //    }
                //    m++;
                //}
                //int n = 0;
                //int count6 = spouseAdoptiveChildren.Count;
                //while (n < count6)
                //{
                //    bool flag6 = spouseAdoptiveChildren[n] != charId;
                //    if (flag6)
                //    {
                //        __instance.AddRelation(context, spouseAdoptiveChildren[n], charId, 64, establishmentDate);
                //    }
                //    n++;
                //}
                //int i2 = 0;
                //int selfChildCount = selfBloodChildren.Count;
                //while (i2 < selfChildCount)
                //{
                //    int selfBloodChildId = selfBloodChildren[i2];
                //    int j2 = 0;
                //    int spouseChildCount = spouseBloodChildren.Count;
                //    while (j2 < spouseChildCount)
                //    {
                //        bool flag7 = selfBloodChildId != spouseBloodChildren[j2];
                //        if (flag7)
                //        {
                //            __instance.AddRelation(context, selfBloodChildId, spouseBloodChildren[j2], 32, establishmentDate);
                //        }
                //        j2++;
                //    }
                //    int j3 = 0;
                //    int spouseChildCount2 = spouseStepChildren.Count;
                //    while (j3 < spouseChildCount2)
                //    {
                //        bool flag8 = selfBloodChildId != spouseStepChildren[j3];
                //        if (flag8)
                //        {
                //            __instance.AddRelation(context, selfBloodChildId, spouseStepChildren[j3], 32, establishmentDate);
                //        }
                //        j3++;
                //    }
                //    int j4 = 0;
                //    int spouseChildCount3 = spouseAdoptiveChildren.Count;
                //    while (j4 < spouseChildCount3)
                //    {
                //        bool flag9 = selfBloodChildId != spouseAdoptiveChildren[j4];
                //        if (flag9)
                //        {
                //            __instance.AddRelation(context, selfBloodChildId, spouseAdoptiveChildren[j4], 256, establishmentDate);
                //        }
                //        j4++;
                //    }
                //    i2++;
                //}
                //int i3 = 0;
                //int selfChildCount2 = selfStepChildren.Count;
                //while (i3 < selfChildCount2)
                //{
                //    int selfStepChildId = selfStepChildren[i3];
                //    int j5 = 0;
                //    int spouseChildCount4 = spouseBloodChildren.Count;
                //    while (j5 < spouseChildCount4)
                //    {
                //        bool flag10 = selfStepChildId != spouseBloodChildren[j5];
                //        if (flag10)
                //        {
                //            __instance.AddRelation(context, selfStepChildId, spouseBloodChildren[j5], 32, establishmentDate);
                //        }
                //        j5++;
                //    }
                //    int j6 = 0;
                //    int spouseChildCount5 = spouseStepChildren.Count;
                //    while (j6 < spouseChildCount5)
                //    {
                //        bool flag11 = selfStepChildId != spouseStepChildren[j6];
                //        if (flag11)
                //        {
                //            __instance.AddRelation(context, selfStepChildId, spouseStepChildren[j6], 32, establishmentDate);
                //        }
                //        j6++;
                //    }
                //    int j7 = 0;
                //    int spouseChildCount6 = spouseAdoptiveChildren.Count;
                //    while (j7 < spouseChildCount6)
                //    {
                //        bool flag12 = selfStepChildId != spouseAdoptiveChildren[j7];
                //        if (flag12)
                //        {
                //            __instance.AddRelation(context, selfStepChildId, spouseAdoptiveChildren[j7], 256, establishmentDate);
                //        }
                //        j7++;
                //    }
                //    i3++;
                //}
                //int i4 = 0;
                //int selfChildCount3 = selfAdoptiveChildren.Count;
                //while (i4 < selfChildCount3)
                //{
                //    int selfAdoptiveChildId = selfAdoptiveChildren[i4];
                //    int j8 = 0;
                //    int spouseChildCount7 = spouseBloodChildren.Count;
                //    while (j8 < spouseChildCount7)
                //    {
                //        bool flag13 = selfAdoptiveChildId != spouseBloodChildren[j8];
                //        if (flag13)
                //        {
                //            __instance.AddRelation(context, selfAdoptiveChildId, spouseBloodChildren[j8], 256, establishmentDate);
                //        }
                //        j8++;
                //    }
                //    int j9 = 0;
                //    int spouseChildCount8 = spouseStepChildren.Count;
                //    while (j9 < spouseChildCount8)
                //    {
                //        bool flag14 = selfAdoptiveChildId != spouseStepChildren[j9];
                //        if (flag14)
                //        {
                //            __instance.AddRelation(context, selfAdoptiveChildId, spouseStepChildren[j9], 256, establishmentDate);
                //        }
                //        j9++;
                //    }
                //    int j10 = 0;
                //    int spouseChildCount9 = spouseAdoptiveChildren.Count;
                //    while (j10 < spouseChildCount9)
                //    {
                //        bool flag15 = selfAdoptiveChildId != spouseAdoptiveChildren[j10];
                //        if (flag15)
                //        {
                //            __instance.AddRelation(context, selfAdoptiveChildId, spouseAdoptiveChildren[j10], 256, establishmentDate);
                //        }
                //        j10++;
                //    }
                //    i4++;
                //}
                //ObjectPool<List<int>>.Instance.Return(selfBloodChildren);
                //ObjectPool<List<int>>.Instance.Return(spouseBloodChildren);
                //ObjectPool<List<int>>.Instance.Return(selfStepChildren);
                //ObjectPool<List<int>>.Instance.Return(spouseStepChildren);
                //ObjectPool<List<int>>.Instance.Return(selfAdoptiveChildren);
                //ObjectPool<List<int>>.Instance.Return(spouseAdoptiveChildren);





            }
            

            return true;
        }

    }
    }
