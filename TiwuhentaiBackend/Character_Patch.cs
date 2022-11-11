extern alias gd;

using gd::Config;
using gd::GameData.DomainEvents;
using gd::GameData.Domains;
using gd::GameData.Domains.Character;
using gd::GameData.Domains.Character.Ai;
using gd::GameData.Domains.Character.ParallelModifications;
using gd::GameData.Domains.Character.Relation;
using gd::GameData.Domains.Information;
using gd::GameData.Domains.Information.Collection;
using gd::GameData.Domains.LifeRecord;
using gd::GameData.Domains.Taiwu;
using gd::GameData.Domains.World.Notification;
using gd::GameData.Utilities;
using HarmonyLib;
using Redzen.Random;
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
	[HarmonyPatch(typeof(Character), "ApplyBecomeBoyOrGirlFriend")]
	class Character_Patch_ApplyBecomeBoyOrGirlFriend
	{
		public static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{

			//mainChildGender.SetLocalSymInfo("15",15,16);
			Label label1 = il.DefineLabel();
			Label label2 = il.DefineLabel();



			CodeInstruction node1 = new CodeInstruction(OpCodes.Nop);
			CodeInstruction node2 = new CodeInstruction(OpCodes.Nop);

			node1.labels.Add(label1);
			node2.labels.Add(label2);
			//71  00B4 ldarg.s selfIsTaiwuPeople(5)
			//72  00B6 ldarg.s targetIsTaiwuPeople(6)
			//73  00B8 or
			//74  00B9 Callvirt.i4.0
			//75  00BA and
			//76  00BB brtrue  123(0136) nop
			//77  00C0 nop




			instructions = new CodeMatcher(instructions)
						  .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
							  new CodeMatch(OpCodes.Callvirt, typeof(LifeRecordCollection).GetMethod("AddConfessLoveSucceed"))
						  )
				 .Repeat(matcher => // Do the following for each match
							 matcher.Advance(2).InsertAndAdvance(
								  new CodeInstruction(OpCodes.Ldarg_S, 5),
								  new CodeInstruction(OpCodes.Ldarg_S, 6),
								  new CodeInstruction(OpCodes.Or, null),
								  new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Taiwuhentai), nameof(Taiwuhentai.noOverheardConfessionTaiwu))),
								  new CodeInstruction(OpCodes.And, null),
								  new CodeInstruction(OpCodes.Brtrue_S, label1),
								  new CodeInstruction(OpCodes.Nop, null)


							 ).Advance(1)
						 )

				.InstructionEnumeration();
			//122 0134    br.s    129(0141) nop
			//123 0136    ldloc.s monthlyNotificationCollection(6)
			//124 0138    ldloc.0
			//125 0139    ldloc.3
			//126 013A ldloc.1
			//127 013B callvirt    instance void GameData.Domains.World.Notification.MonthlyNotificationCollection::AddMarriage(int32, valuetype GameData.Domains.Map.Location, int32)
			//128 0140    nop

			instructions = new CodeMatcher(instructions)
				 .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
					 new CodeMatch(OpCodes.Callvirt, typeof(InformationDomain).GetMethod("ReceiveSecretInformation"))
						 )
				.Repeat(matcher => // Do the following for each match


							matcher.Advance(2).InsertAndAdvance(
								  new CodeInstruction(OpCodes.Br_S, label2),
								  node1,
								  new CodeInstruction(OpCodes.Ldloc_S, 5),
								  new CodeInstruction(OpCodes.Ldloc_S, 0),
								  new CodeInstruction(OpCodes.Ldloc_S, 2),
								  new CodeInstruction(OpCodes.Ldloc_S, 1),
								  new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(MonthlyNotificationCollection), "AddConfessLoveAndSucceed")),
								  node2
								).Advance(1)
						)

				.InstructionEnumeration();



			return instructions;
		}

	}
	[HarmonyPatch(typeof(Character), "ApplyBecomeHusbandOrWife")]
	class Character_Patch_ApplyBecomeHusbandOrWife
	{
		public static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{

			//mainChildGender.SetLocalSymInfo("15",15,16);
			Label label1 = il.DefineLabel();
			Label label2 = il.DefineLabel();



			CodeInstruction node1 = new CodeInstruction(OpCodes.Nop);
			CodeInstruction node2 = new CodeInstruction(OpCodes.Nop);

			node1.labels.Add(label1);
			node2.labels.Add(label2);
			//71  00B4 ldarg.s selfIsTaiwuPeople(5)
			//72  00B6 ldarg.s targetIsTaiwuPeople(6)
			//73  00B8 or
			//74  00B9 Callvirt.i4.0
			//75  00BA and
			//76  00BB brtrue  123(0136) nop
			//77  00C0 nop




			instructions = new CodeMatcher(instructions)
						  .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
							  new CodeMatch(OpCodes.Callvirt, typeof(LifeRecordCollection).GetMethod("AddProposeMarriageSucceed"))
						  )
				 .Repeat(matcher => // Do the following for each match
							 matcher.Advance(2).InsertAndAdvance(
								  new CodeInstruction(OpCodes.Ldarg_S, 5),
								  new CodeInstruction(OpCodes.Ldarg_S, 6),
								  new CodeInstruction(OpCodes.Or, null),
								  new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Taiwuhentai), nameof(Taiwuhentai.noOverheardProposeTaiwu))),
								  new CodeInstruction(OpCodes.And, null),
								  new CodeInstruction(OpCodes.Brtrue_S, label1),
								  new CodeInstruction(OpCodes.Nop, null)


							 ).Advance(1)
						 )

				.InstructionEnumeration();
			//122 0134    br.s    129(0141) nop
			//123 0136    ldloc.s monthlyNotificationCollection(6)
			//124 0138    ldloc.0
			//125 0139    ldloc.3
			//126 013A ldloc.1
			//127 013B callvirt    instance void GameData.Domains.World.Notification.MonthlyNotificationCollection::AddMarriage(int32, valuetype GameData.Domains.Map.Location, int32)
			//128 0140    nop

			instructions = new CodeMatcher(instructions)
				 .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
					 new CodeMatch(OpCodes.Callvirt, typeof(InformationDomain).GetMethod("ReceiveSecretInformation"))
						 )
				.Repeat(matcher => // Do the following for each match


							matcher.Advance(2).InsertAndAdvance(
								  new CodeInstruction(OpCodes.Br_S, label2),
								  node1,
								  new CodeInstruction(OpCodes.Ldloc_S,6),
								  new CodeInstruction(OpCodes.Ldloc_S,0),
								  new CodeInstruction(OpCodes.Ldloc_S,3),
								  new CodeInstruction(OpCodes.Ldloc_S,1),
								  new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(MonthlyNotificationCollection), "AddMarriage")),
								  node2
								).Advance(1)
						)

				.InstructionEnumeration();



			return instructions;
		}

	}
	[HarmonyPatch(typeof(Character), "OfflineIncreaseAge")]
	public class Character_Patch_OfflineIncreaseAge
    {
		static bool Prefix(Character __instance)
        {
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			if (Taiwuhentai.taiwuAgeCap!=-1&&taiwuId == __instance.GetId())
            {
				if(__instance.GetCurrAge()>= Taiwuhentai.taiwuAgeCap)
                {
					return false;
                }
            }
			if (Taiwuhentai.taiwuSpouseAgeCap != -1 && HentaiUtility.GetTaiwuAliveSpousePool().Contains( __instance.GetId()))
			{
				if (__instance.GetCurrAge() >= Taiwuhentai.taiwuSpouseAgeCap)
				{
					return false;
				}
			}

			return true;
		}
		
	}
	[HarmonyPatch(typeof(Character), "CalcFertility")]
	class Character_Patch_CalcFertility
	{
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
	}
	[HarmonyPatch(typeof(Character), "PeriAdvanceMonth_ExecuteFixedActions")]
	class Character_Patch_PeriAdvanceMonth_ExecuteFixedActions
	{
		public static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{

			//mainChildGender.SetLocalSymInfo("15",15,16);
			Label label1 = il.DefineLabel();
			Label label2 = il.DefineLabel();



			CodeInstruction node1 = new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Taiwuhentai), nameof(Taiwuhentai.fertilityIgnoreAgeTaiwuSpouse)));
			CodeInstruction node2 = new CodeInstruction(OpCodes.Nop);

			node1.labels.Add(label1);
			node2.labels.Add(label2);
			//7   000F    ldloc.1
			//8   0010    ldc.i4.1
			//9   0011    ceq
			//10  0013    brfalse.s   25(0042) ldloc.1
			//11  0015    call    class [System.Collections] System.Collections.Generic.HashSet`1<int32> [TiwuhentaiBackend] Taiwuhentai.HentaiUtility::GetTaiwuAliveAdoredPool()
			//12	001A ldarg.0
			//13	001B ldfld   int32[GameData.dll] GameData.Domains.Character.Character::_id
			//14	0020	callvirt instance bool class [System.Collections] System.Collections.Generic.HashSet`1<int32>::Contains(!0)
			//15	0025	brtrue.s	21 (0039) ldsfld bool[TiwuhentaiBackend] Taiwuhentai.Taiwuhentai::fertilityIgnoreAgeTaiwuSpouse
			//16	0027	call    class [System.Collections] System.Collections.Generic.HashSet`1<int32> [TiwuhentaiBackend] Taiwuhentai.HentaiUtility::GetTaiwuAliveSpousePool()
			//17	002C ldarg.0
			//18	002D	ldfld int32[GameData.dll]GameData.Domains.Character.Character::_id
			//19	0032	callvirt instance bool class [System.Collections] System.Collections.Generic.HashSet`1<int32>::Contains(!0)
			//20	0037	brfalse.s	25 (0042) ldloc.1 
			//21	0039	ldsfld bool[TiwuhentaiBackend] Taiwuhentai.Taiwuhentai::fertilityIgnoreAgeTaiwuSpouse
			//22	003E	brfalse.s   25 (0042) ldloc.1 
			//23	0040	ldc.i4.2
			//24	0041	stloc.1




			instructions = new CodeMatcher(instructions)
						  .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
							  new CodeMatch(OpCodes.Call, typeof(Character).GetMethod("GetAgeGroup"))
						  )
				 .Repeat(matcher => // Do the following for each match
							 matcher.Advance(2).InsertAndAdvance(
								  new CodeInstruction(OpCodes.Ldloc_1, null),
								  new CodeInstruction(OpCodes.Ldc_I4_1, null),
								  new CodeInstruction(OpCodes.Ceq, null),
								  new CodeInstruction(OpCodes.Brfalse_S, label2),
								  new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HentaiUtility), "GetTaiwuAliveSpousePool")),
								  new CodeInstruction(OpCodes.Ldarg_0, null),
								  new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Character), "_id")),
								  new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(HashSet<int>), "Contains")),
								  new CodeInstruction(OpCodes.Brtrue_S, label1),
								  new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HentaiUtility), "GetTaiwuAliveAdoredPool")),
								  new CodeInstruction(OpCodes.Ldarg_0, null),
								  new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Character), "_id")),
								  new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(HashSet<int>), "Contains")),
								  new CodeInstruction(OpCodes.Brfalse_S, label2),
								  node1,
								  new CodeInstruction(OpCodes.Brfalse_S, label2),
								  new CodeInstruction(OpCodes.Ldc_I4_2, null),
								  new CodeInstruction(OpCodes.Stloc_1, null),
								  node2

							 ).Advance(1)
						 )

				.InstructionEnumeration();


			return instructions;
		}

	}
	[HarmonyPatch(typeof(Character), "OfflineExecuteFixedAction_MakeLove_Mutual")]
	class Character_Patch_OfflineExecuteFixedAction_MakeLove_Mutual
	{
		public static bool Prefix(IRandomSource random, Character __instance, int targetCharId, bool allowRape, PeriAdvanceMonthFixedActionModification mod)
		{

			int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();
			bool targetflagTaiwu = targetCharId == charidTaiwu;
			bool charflagTaiwu = __instance.GetId() == charidTaiwu;

			bool targetflagTaiwuSpouse = HentaiUtility.GetTaiwuAliveSpousePool().Contains(targetCharId);
			bool charflagTaiwuSpouse = HentaiUtility.GetTaiwuAliveSpousePool().Contains(__instance.GetId());

			bool targetflagTaiwuAdored = HentaiUtility.GetTaiwuAliveAdoredPool().Contains(targetCharId);
			bool charflagTaiwuAdored = HentaiUtility.GetTaiwuAliveAdoredPool().Contains(__instance.GetId());

			if (Taiwuhentai.preventTaiwuSpouseIllegalLove)
			{

				if (((charflagTaiwuSpouse || charflagTaiwuAdored) && !targetflagTaiwu) || ((targetflagTaiwuAdored || targetflagTaiwuSpouse) && !charflagTaiwu))
				{
					return false;
				}


			}

			Character target = DomainManager.Character.GetElement_Objects(targetCharId);
			bool flag = target.GetAgeGroup() != 0;
			if (flag && (Taiwuhentai.fertilityIgnoreAgeTaiwuSpouse && (targetflagTaiwu || charflagTaiwu)))
			{
				bool flag2 = __instance.GetGender() == 1;
				bool flag3 = __instance.GetGender() == target.GetGender();
				Character father;
				Character mother;

				//bool flag4 = __instance.GetGender() == target.GetGender() && ((DomainManager.Taiwu.GetTaiwuCharId() == targetCharId && Taiwuhentai.lesbianPregnantIO == 2) || (DomainManager.Taiwu.GetTaiwuCharId() == __instance.GetId() && Taiwuhentai.lesbianPregnantIO == 1));

				if (flag2 || (Taiwuhentai.lesbianPregnantTaiwu && flag3 && targetflagTaiwu && Taiwuhentai.lesbianPregnantIO == 2) || (Taiwuhentai.lesbianPregnantTaiwu && flag3 && charflagTaiwu && Taiwuhentai.lesbianPregnantIO == 1))
				{
					father = __instance;
					mother = target;
				}
				else
				{
					father = target;
					mother = __instance;
				}

				Type tChatacter = __instance.GetType();
				BindingFlags bindFlag = BindingFlags.NonPublic | BindingFlags.Instance;
				MethodInfo makeloveMethod = tChatacter.GetMethod("OfflineMakeLove", bindFlag);


				Debuglogger.Log(makeloveMethod.Name);


				if ((charflagTaiwuSpouse && targetflagTaiwu) || (targetflagTaiwuSpouse && charflagTaiwu))
				{
					Debuglogger.Log("OfflineExecuteFixedAction_MakeLove_Mutual TaiwuSpouseaaa" + __instance.GetId() + " " + targetCharId);

					if (random.CheckPercentProb((int)(100 * __instance.GetFertility() * target.GetFertility() / 10000)))
					{
						if (mod.MakeLoveTargetList == null)
						{
							mod.MakeLoveTargetList = new List<ValueTuple<Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool>>();
						}
						Debuglogger.Log("OfflineExecuteFixedAction_MakeLove_Mutual TaiwuSpouseab" + __instance.GetId() + " " + targetCharId);

						bool returnValue = (bool)makeloveMethod.Invoke(__instance, new object[] { random, father, mother, false });

						Debuglogger.Log("OfflineExecuteFixedAction_MakeLove_Mutual TaiwuSpouseac" + __instance.GetId() + " " + targetCharId);

						mod.MakeLoveTargetList.Add(new ValueTuple<Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool>(target, PeriAdvanceMonthFixedActionModification.MakeLoveState.Legal, returnValue));
					}
					Debuglogger.Log("OfflineExecuteFixedAction_MakeLove_Mutual TaiwuSpousebbb" + __instance.GetId() + " " + targetCharId);
					return false;
				}
				if ((charflagTaiwuAdored && targetflagTaiwu) || (targetflagTaiwuAdored && charflagTaiwu))
				{
					Debuglogger.Log("OfflineExecuteFixedAction_MakeLove_Mutual TaiwuAdored aaa" + __instance.GetId() + " " + targetCharId);

					if (random.CheckPercentProb((int)((short)AiHelper.FixedActionConstants.BoyAndGirlFriendMakeLoveBaseChance[(int)__instance.GetBehaviorType()] * __instance.GetFertility() * target.GetFertility() / 10000)))
					{
						if (mod.MakeLoveTargetList == null)
						{
							mod.MakeLoveTargetList = new List<ValueTuple<Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool>>();
						}
						Debuglogger.Log("OfflineExecuteFixedAction_MakeLove_Mutual TaiwuAdoredab" + __instance.GetId() + " " + targetCharId);

						bool returnValue = (bool)makeloveMethod.Invoke(__instance, new object[] { random, father, mother, false });
						Debuglogger.Log("OfflineExecuteFixedAction_MakeLove_Mutual TaiwuAdoredac" + __instance.GetId() + " " + targetCharId);

						mod.MakeLoveTargetList.Add(new ValueTuple<Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool>(target, PeriAdvanceMonthFixedActionModification.MakeLoveState.Illegal, returnValue));
					}
					Debuglogger.Log("OfflineExecuteFixedAction_MakeLove_Mutual TaiwuAdoredbbb" + __instance.GetId() + " " + targetCharId);
					return false;
				}
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(Character), "OfflineMakeLove")]
	class Character_Patch_OfflineMakeLove
	{
		static bool Prefix(IRandomSource random, Character father, Character mother, bool isRape, ref bool __result)
		{
			int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();

			if (father.GetId() == charidTaiwu || mother.GetId() == charidTaiwu)
			{
				bool flag = mother.GetGender() == father.GetGender();
				bool flag2;
				if (flag && !Taiwuhentai.lesbianPregnantTaiwu)
				{
					flag2 = false;
				}
				else
				{
					Type type = typeof(Character);
					BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
					MethodInfo addFeatureMethod = type.GetMethod("OfflineAddFeature", bindingFlags);
					addFeatureMethod.Invoke(mother, new object[] { (short)196, true });
					addFeatureMethod.Invoke(father, new object[] { (short)196, true });

					bool flag3 = !PregnantState.CheckPregnant(random, father, mother, isRape);
					if (flag3)
					{
						flag2 = false;
					}
					else
					{
						addFeatureMethod.Invoke(mother, new object[] { (short)197, true });
						flag2 = true;
					}
				}
				__result = flag2;
				return false;
			}

			return true;

		}
	}
	[HarmonyPatch(typeof(Character), "ComplementPeriAdvanceMonth_ExecuteFixedActions")]
	class Character_Patch_ComplementPeriAdvanceMonth_ExecuteFixedActions
	{
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {

            //mainChildGender.SetLocalSymInfo("15",15,16);
            Label label1 = il.DefineLabel();
            Label label2 = il.DefineLabel();



            CodeInstruction node1 = new CodeInstruction(OpCodes.Nop);
            CodeInstruction node2 = new CodeInstruction(OpCodes.Nop);

            node1.labels.Add(label1);
            node2.labels.Add(label2);
            //113 014C ldsfld  bool[TiwuhentaiBackend] Taiwuhentai.Taiwuhentai::noOverheardIllegalMakeLoveTaiwu
            //114 0151    brfalse.s   125(016C) nop
            //113 014C ldloc.s target(9)
            //114 014E    ldfld int32 GameData.Domains.Character.Character::_id
            //115 0153    ldloc.s taiwuCharId(6)
            //116 0155    ceq
            //117 0157    brtrue.s    137(018B) br 235(02A2)
            //118 0159    ldloc.0
            //119 015A ldfld   int32 GameData.Domains.Character.Character::_id
            //120 015F    ldloc.s taiwuCharId(6)
            //121 0161    ceq
            //122 0163    brtrue.s    137(018B) br 235(02A2)


            instructions = new CodeMatcher(instructions)
                          .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
                              new CodeMatch(OpCodes.Callvirt, typeof(LifeRecordCollection).GetMethod("AddMakeLoveIllegal"))
                          )
                 .Repeat(matcher => // Do the following for each match
                             matcher.Advance(1).InsertAndAdvance(
                                  new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Taiwuhentai), nameof(Taiwuhentai.noOverheardIllegalMakeLoveTaiwu))),
                                  new CodeInstruction(OpCodes.Brfalse_S, label2),

                                  new CodeInstruction(OpCodes.Ldloc_S, 9),
                                  new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Character), "_id")),
                                  new CodeInstruction(OpCodes.Ldloc_S, 6),
                                  new CodeInstruction(OpCodes.Ceq, null),
                                  new CodeInstruction(OpCodes.Brtrue_S, label1),
                                  new CodeInstruction(OpCodes.Ldloc_0, null),
                                  new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Character), "_id")),
                                  new CodeInstruction(OpCodes.Ldloc_S, 6),
                                  new CodeInstruction(OpCodes.Ceq, null),
                                  new CodeInstruction(OpCodes.Brtrue_S, label1),
                                  node2
                             ).SetAndAdvance(OpCodes.Nop, null)
                         )

                .InstructionEnumeration();
            instructions = new CodeMatcher(instructions)
                 .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
                     new CodeMatch(OpCodes.Callvirt, typeof(SecretInformationCollection).GetMethod("AddMakeLoveIllegal"))
                         )
                .Repeat(matcher => // Do the following for each match


                            matcher.Advance(8).InsertAndAdvance(
                                node1
                                ).Advance(1)
                        )

                .InstructionEnumeration();

			//238 02A2 ldloc.0
			//239 02A3 ldfld   int8 GameData.Domains.Character.Character::_gender
			//240 02A8 ldc.i4.1
			//241 02A9 ceq
			//242 02AB brtrue.s    249(02BB) ldc.i4.1
			//243 02AD ldloc.s character(0)
			//244 02AF ldloc.s target(9)
			//245 02B1 call    bool[TiwuhentaiBackend] Taiwuhentai.HentaiUtility::GetLesBianIO(class [TiwuhentaiBackend.dll] GameData.Domains.Character.Character, class [TiwuhentaiBackend.dll] GameData.Domains.Character.Character)
			//246	02B6 brtrue.s    249 (02BB) ldc.i4.1 
			//247	02B8 ldc.i4.0
			//248	02B9 br.s    250 (02BC) stloc.s V_26(26)
			//249	02BB ldc.i4.1
			//250	02BC stloc.s V_26 (26)
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();



			CodeInstruction node3 = new CodeInstruction(OpCodes.Ldc_I4_1,null);
			CodeInstruction node4 = new CodeInstruction(OpCodes.Nop, null);

			node3.labels.Add(label3);
			node4.labels.Add(label4);


			instructions = new CodeMatcher(instructions)
.MatchForward(false, // false = move at the start of the match, true = move at the end of the match
//new CodeMatch(OpCodes.Ldfld, typeof(Character).GetField("_gender"))
new CodeMatch(OpCodes.Call, typeof(Events).GetMethod("RaiseMakeLove"))
 )
.Repeat(matcher => // Do the following for each match


	matcher.Advance(13).InsertAndAdvance(
				new CodeInstruction(OpCodes.Brtrue_S, label3),
				new CodeInstruction(OpCodes.Ldloc_S, 0),
				new CodeInstruction(OpCodes.Ldloc_S, 9),
				new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HentaiUtility), "GetLesBianIO")),
				new CodeInstruction(OpCodes.Brtrue_S, label3),
				new CodeInstruction(OpCodes.Ldc_I4_0, null),
				new CodeInstruction(OpCodes.Br_S, label4),
				node3,
				node4
		).Advance(1)
)
.InstructionEnumeration();

			return instructions;
        }

    }

}
