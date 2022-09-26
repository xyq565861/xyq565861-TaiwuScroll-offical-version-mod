extern alias gd;

using ConchShip.EventConfig.Taiwu;
using gd::GameData.Common;
using gd::GameData.Domains;
using gd::GameData.Domains.Character;
using gd::GameData.Domains.Character.Relation;
using gd::GameData.Domains.Taiwu;
using gd::GameData.Domains.TaiwuEvent.EventHelper;
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
	public class Test
	{
        [HarmonyPatch("ParallelCreateNewbornChildren")]
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            LocalBuilder fatherId = il.DeclareLocal(typeof(int));
            LocalBuilder mainChildGender = il.DeclareLocal(typeof(sbyte));
            //mainChildGender.SetLocalSymInfo("15",15,16);
            Label label1 = il.DefineLabel();
            Label label2 = il.DefineLabel();
            Label label3 = il.DefineLabel();
            
            
            CodeInstruction node1 = new CodeInstruction(OpCodes.Ldc_I4_1);
            CodeInstruction node2= new CodeInstruction(OpCodes.Stloc_S, 65);
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
                                 new CodeInstruction(OpCodes.Beq_S,label1),
                                 new CodeInstruction(OpCodes.Ldloc_S, 4),
                                 new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(DomainManager), nameof(DomainManager.Taiwu))),
                                 new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(TaiwuDomain), "GetTaiwuCharId")),
                                 new CodeInstruction(OpCodes.Ceq,null),
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
                                 new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Debuglogger), "Log", new Type[] { typeof(object)})),
                                 node3
                            ).SetAndAdvance(OpCodes.Ldloc_0,null)
                        )

               .InstructionEnumeration();
            return instructions;
        }
        //static void Postfix()
        //{
        //    sbyte mainChildGender;
        //    int fatherId = 11;
        //    int motherId = 12;
        //    string mainChildGenderstr;
        //    if (fatherId == DomainManager.Taiwu.GetTaiwuCharId() || motherId == DomainManager.Taiwu.GetTaiwuCharId())
        //    {
        //        mainChildGender = (sbyte)Taiwuhentai.childGender;

        //    }
        //    fatherId = 1;
        //    mainChildGender = (sbyte)fatherId;
        //    mainChildGenderstr = mainChildGender.ToString();
        //    Debuglogger.Log(mainChildGenderstr);
        //    motherId = mainChildGender;

        //}
    }

}