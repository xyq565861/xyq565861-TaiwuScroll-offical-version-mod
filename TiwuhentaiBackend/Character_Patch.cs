extern alias gd;

using gd::Config;
using gd::GameData.Domains;
using gd::GameData.Domains.Character;
using gd::GameData.Domains.Information.Collection;
using gd::GameData.Domains.LifeRecord;
using gd::GameData.Domains.Taiwu;
using HarmonyLib;
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
		static void Postfix(Character __instance, ref bool __result, Character father, Character mother)
		{
			int fatherId = father.GetId();
			int motherId = mother.GetId();
			int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();
			if (charidTaiwu == fatherId || charidTaiwu == motherId)
			{
				Debuglogger.Log("OfflineMakeLove" + __result);
			}
		}
	

		[HarmonyPatch("OfflineExecuteFixedAction_MakeLove_Mutual")]
		static bool Prefix(Character __instance, int targetCharId, bool allowRape)
		{

			int charidTaiwu = DomainManager.Taiwu.GetTaiwuCharId();
			if (Taiwuhentai.preventTaiwuSpouseIllegalLove)
			{

				if ((HentaiUtility.GetTaiwuAliveSpousePool().Contains(__instance.GetId()) && targetCharId!= charidTaiwu)|| (HentaiUtility.GetTaiwuAliveSpousePool().Contains(targetCharId) && __instance.GetId() != charidTaiwu))
				{
					

					return false;
				}
                if ((HentaiUtility.GetTaiwuAliveAdoredPool().Contains(__instance.GetId()) && targetCharId != charidTaiwu) || (HentaiUtility.GetTaiwuAliveAdoredPool().Contains(targetCharId) && __instance.GetId() != charidTaiwu))
                {


                    return false;
                }

            }
			return true;
		}
		[HarmonyPatch("ComplementPeriAdvanceMonth_ExecuteFixedActions")]
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

			return instructions;
		}
	}

}
