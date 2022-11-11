extern alias gd;
using gd.GameData.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuModdingLib.Core.Plugin;

namespace Taiwuhentai
{
	[PluginConfig("Taiwuhentai_Backend", "FD.FLY", "0.0.11")]
	public class Taiwuhentai : TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{
			Debuglogger.Log("star injecting back plugin dll");
			UpdateSetting();
			HarmontPatch();
			//this.HarmonyInstance.PatchAll(typeof(Test));

			Debuglogger.Log("injected back plugin dll");

		}
		
		public override void Dispose()
		{
            try
            {
				HarmontDisPatch();
			}
            catch
            {

            }

		}
		public void HarmontDisPatch()
		{
			this.HarmonyInstance.UnpatchAll("Taiwuhentai event");
			this.HarmonyInstance.UnpatchSelf();
		}
		//public override void OnEnterNewWorld()
		//{
		//	Debuglogger.Log("star injecting event package dll");
		//	base.OnEnterNewWorld();
		//	this.HarmonyInstance.PatchAll(typeof(TaiwuEvent_bad63f08115a45aa970cfa203dd85e2b_Patch));
		//	Debuglogger.Log("injected event package dll");
		//}
		public void HarmontPatch()
        {

			this.HarmonyInstance.PatchAll(typeof(ModDomain_Patch));

			Debuglogger.Log("patch ModDomain_Patch");
			if (noOverheardConfessionTaiwu)
			{
				this.HarmonyInstance.PatchAll(typeof(Character_Patch_ApplyBecomeBoyOrGirlFriend));
				Debuglogger.Log("patch Character_Patch_ApplyBecomeBoyOrGirlFriend");

			}
			if (noOverheardProposeTaiwu)
			{
				this.HarmonyInstance.PatchAll(typeof(Character_Patch_ApplyBecomeHusbandOrWife));
				Debuglogger.Log("patch Character_Patch_ApplyBecomeHusbandOrWife");

			}
			if (taiwuAgeCap > 0 || taiwuSpouseAgeCap > 0)
            {
				this.HarmonyInstance.PatchAll(typeof(Character_Patch_OfflineIncreaseAge));
				Debuglogger.Log("patch Character_Patch_OfflineIncreaseAge");

			}
			if (fertilityIgnoreAgeTaiwu|| fertilityIgnoreAgeTaiwuSpouse)
			{
				this.HarmonyInstance.PatchAll(typeof(Character_Patch_CalcFertility));
				Debuglogger.Log("patch Character_Patch_CalcFertility");

				this.HarmonyInstance.PatchAll(typeof(Character_Patch_PeriAdvanceMonth_ExecuteFixedActions));
				Debuglogger.Log("patch Character_Patch_PeriAdvanceMonth_ExecuteFixedActions");

			}
			if (lesbianPregnantTaiwu|| fertilityIgnoreAgeTaiwuSpouse|| preventTaiwuSpouseIllegalLove)
            {
				this.HarmonyInstance.PatchAll(typeof(Character_Patch_OfflineExecuteFixedAction_MakeLove_Mutual));
				Debuglogger.Log("patch Character_Patch_OfflineExecuteFixedAction_MakeLove_Mutual");

			}
			if (lesbianPregnantTaiwu)
            {
				this.HarmonyInstance.PatchAll(typeof(Character_Patch_OfflineMakeLove));
				Debuglogger.Log("patch Character_Patch_OfflineMakeLove");

			}
			if (rateOfTaiwuSpouseFavorabilityReduce!=0)
			{
				this.HarmonyInstance.PatchAll(typeof(CharacterDomain_Favorability_Patch_ChangeFavorability));
				Debuglogger.Log("patch CharacterDomain_Favorability_Patch_ChangeFavorability");

			}
			if (lesbianPregnantTaiwu|| noOverheardIllegalMakeLoveTaiwu)
            {
				this.HarmonyInstance.PatchAll(typeof(Character_Patch_ComplementPeriAdvanceMonth_ExecuteFixedActions));
				Debuglogger.Log("patch Character_Patch_ComplementPeriAdvanceMonth_ExecuteFixedActions");

			}
			if (preventCricketPregnant|| rateTaiwuPregnantTime != 0)
            {
				this.HarmonyInstance.PatchAll(typeof(CharacterDomain_Newbron_Patch_CreatePregnantState));
				Debuglogger.Log("patch CharacterDomain_Newbron_Patch_CreatePregnantState");

			}
			if (childGender!=2)
            {
				this.HarmonyInstance.PatchAll(typeof(CharacterDomain_Newbron_Patch_ParallelCreateNewbornChildren));
				Debuglogger.Log("patch CharacterDomain_Newbron_Patch_ParallelCreateNewbornChildren");

			}
			if (unrestrainedSpouseNum|| responsibleParent)
            {
			this.HarmonyInstance.PatchAll(typeof(CharacterDomain_Newbron_Patch_ParallelCreateIntelligentCharacter));
				Debuglogger.Log("patch CharacterDomain_Newbron_Patch_ParallelCreateIntelligentCharacter");


			}
			if (unrestrainedSpouseNum)
            {
				this.HarmonyInstance.PatchAll(typeof(CharacterDomain_Marry_Patch_AddHusbandOrWifeRelations));
				Debuglogger.Log("patch CharacterDomain_Marry_Patch_AddHusbandOrWifeRelations");

			}
			if (bloodTies)
			{
				this.HarmonyInstance.PatchAll(typeof(EventHelper_Patch_AddRelation));
				Debuglogger.Log("patch EventHelper_Patch_EventHelper_Patch_AddRelation");

			}
			if (expelOtherAdored)
            {
				this.HarmonyInstance.PatchAll(typeof(EventHelper_Patch_ApplyRelationBecomeBoyOrGirlFriend));
				Debuglogger.Log("patch EventHelper_Patch_ApplyRelationBecomeBoyOrGirlFriend");

			}
			if (allowTaiwuNtr)
            {
				this.HarmonyInstance.PatchAll(typeof(EventHelper_Patch_ApplyRelationBecomeHusbandOrWife));
				Debuglogger.Log("patch EventHelper_Patch_ApplyRelationBecomeHusbandOrWife");

			}

            if (rateOfPregnant!=1|| rateOfPregnantTaiwu!=2|| taiwuSpouseChildCap!=-1|| taiwuChildCap!=-1|| lesbianPregnantTaiwu)
            {
				this.HarmonyInstance.PatchAll(typeof(PregnantState_Patch_CheckPregnant));
				Debuglogger.Log("patch PregnantState_Patch_CheckPregnant");

			}
			if (rateOfConfessionTaiwu > 0|| rateOfConfession!=1|| preventTaiwuSpouseStray|| adjustFavorabilityWeightInConfession)
			{
				this.HarmonyInstance.PatchAll(typeof(Relation_Patch_GetStartRelationSuccessRate_BoyOrGirlFriend));
				Debuglogger.Log("patch Relation_Patch_GetStartRelationSuccessRate_BoyOrGirlFriend");

			}
            if (unrestrainedSpouseNum||bloodTies)
            {
				this.HarmonyInstance.PatchAll(typeof(RelationType_Patch_AllowAddingHusbandOrWifeRelation));
				Debuglogger.Log("patch RelationType_Patch_AllowAddingHusbandOrWifeRelation");
			}
		}
		public override void OnModSettingUpdate()
		{
			try
			{
				HarmontDisPatch();
			}
			catch
			{

			}
			UpdateSetting();
			foreach (var item in typeof(Taiwuhentai).GetFields())
			{
				Debuglogger.Log(string.Format("  {0}:   {1}", item.Name, item.GetValue(this)));
			}
			try
			{
				HarmontPatch();
			}
			catch
			{

			}
		}
		public void UpdateSetting()
        {
			DomainManager.Mod.GetSetting(base.ModIdStr, "unrestrainedSpouseNum", ref Taiwuhentai.unrestrainedSpouseNum);
			DomainManager.Mod.GetSetting(base.ModIdStr, "unrestrainedSpouseFactions", ref Taiwuhentai.unrestrainedSpouseFactions);
			DomainManager.Mod.GetSetting(base.ModIdStr, "spouseAge", ref Taiwuhentai.spouseAge);
			DomainManager.Mod.GetSetting(base.ModIdStr, "taiwuAgeCap", ref Taiwuhentai.taiwuAgeCap);
			DomainManager.Mod.GetSetting(base.ModIdStr, "taiwuSpouseAgeCap", ref Taiwuhentai.taiwuSpouseAgeCap);
			DomainManager.Mod.GetSetting(base.ModIdStr, "fertilityIgnoreAgeTaiwu", ref Taiwuhentai.fertilityIgnoreAgeTaiwu);
			DomainManager.Mod.GetSetting(base.ModIdStr, "fertilityIgnoreAgeTaiwuSpouse", ref Taiwuhentai.fertilityIgnoreAgeTaiwuSpouse);
			DomainManager.Mod.GetSetting(base.ModIdStr, "responsibleParent", ref Taiwuhentai.responsibleParent);
			DomainManager.Mod.GetSetting(base.ModIdStr, "bloodTies", ref Taiwuhentai.bloodTies);
			DomainManager.Mod.GetSetting(base.ModIdStr, "childGender", ref Taiwuhentai.childGender);
			DomainManager.Mod.GetSetting(base.ModIdStr, "adjustFavorabilityWeightInConfession", ref Taiwuhentai.adjustFavorabilityWeightInConfession);
			DomainManager.Mod.GetSetting(base.ModIdStr, "rateOfConfessionTaiwu", ref Taiwuhentai.rateOfConfessionTaiwu);
			DomainManager.Mod.GetSetting(base.ModIdStr, "rateOfConfession", ref Taiwuhentai.rateOfConfession);
			DomainManager.Mod.GetSetting(base.ModIdStr, "preventTaiwuSpouseStray", ref Taiwuhentai.preventTaiwuSpouseStray);
			DomainManager.Mod.GetSetting(base.ModIdStr, "preventTaiwuSpouseIllegalLove", ref Taiwuhentai.preventTaiwuSpouseIllegalLove);
			DomainManager.Mod.GetSetting(base.ModIdStr, "rateOfPregnantTaiwu", ref Taiwuhentai.rateOfPregnantTaiwu);
			DomainManager.Mod.GetSetting(base.ModIdStr, "rateOfPregnant", ref Taiwuhentai.rateOfPregnant);
			DomainManager.Mod.GetSetting(base.ModIdStr, "lesbianPregnantTaiwu", ref Taiwuhentai.lesbianPregnantTaiwu);


			DomainManager.Mod.GetSetting(base.ModIdStr, "lesbianPregnantIO", ref Taiwuhentai.lesbianPregnantIO);

			DomainManager.Mod.GetSetting(base.ModIdStr, "preventCricketPregnant", ref Taiwuhentai.preventCricketPregnant);
			DomainManager.Mod.GetSetting(base.ModIdStr, "taiwuChildCap", ref Taiwuhentai.taiwuChildCap);
			DomainManager.Mod.GetSetting(base.ModIdStr, "taiwuSpouseChildCap", ref Taiwuhentai.taiwuSpouseChildCap);
			DomainManager.Mod.GetSetting(base.ModIdStr, "rateTaiwuPregnantTime", ref Taiwuhentai.rateTaiwuPregnantTime);
			DomainManager.Mod.GetSetting(base.ModIdStr, "allowTaiwuNtr", ref Taiwuhentai.allowTaiwuNtr);
			DomainManager.Mod.GetSetting(base.ModIdStr, "expelOtherAdored", ref Taiwuhentai.expelOtherAdored);

			DomainManager.Mod.GetSetting(base.ModIdStr, "noOverheardIllegalMakeLoveTaiwu", ref Taiwuhentai.noOverheardIllegalMakeLoveTaiwu);
			DomainManager.Mod.GetSetting(base.ModIdStr, "noOverheardProposeTaiwu", ref Taiwuhentai.noOverheardProposeTaiwu);
			DomainManager.Mod.GetSetting(base.ModIdStr, "noOverheardConfessionTaiwu", ref Taiwuhentai.noOverheardConfessionTaiwu);

			DomainManager.Mod.GetSetting(base.ModIdStr, "rateOfTaiwuSpouseFavorabilityReduce", ref Taiwuhentai.rateOfTaiwuSpouseFavorabilityReduce);

			DomainManager.Mod.GetSetting(base.ModIdStr, "debugMode", ref Taiwuhentai.debugMode);



		}

		
		public static bool unrestrainedSpouseNum=false;
		public static bool unrestrainedSpouseFactions = false;
		public static int spouseAge=16;

		public static int taiwuAgeCap = -1;
		public static int taiwuSpouseAgeCap = -1;
		public static bool fertilityIgnoreAgeTaiwu = false;
		public static bool fertilityIgnoreAgeTaiwuSpouse = false;
		public static bool responsibleParent = false;
		public static bool bloodTies=false;
		public static int childGender=2;
		public static bool adjustFavorabilityWeightInConfession= false;
		public static int rateOfConfessionTaiwu=0;
		public static int rateOfConfession=1;
		public static bool preventTaiwuSpouseStray = false;
		public static bool preventTaiwuSpouseIllegalLove = false;
		public static int rateOfPregnantTaiwu = 2;
		public static int rateOfPregnant=1;
		public static bool lesbianPregnantTaiwu = false;
		public static int lesbianPregnantIO=0;

		public static bool preventCricketPregnant=false;
		public static int taiwuChildCap = -1;
		public static int taiwuSpouseChildCap = -1;
		public static int rateTaiwuPregnantTime=0;
		public static bool allowTaiwuNtr = false;
		public static bool expelOtherAdored = false;

		public static int rateOfTaiwuSpouseFavorabilityReduce = 0;

		public static bool noOverheardIllegalMakeLoveTaiwu=false;
		public static bool noOverheardProposeTaiwu=false;
		public static bool noOverheardConfessionTaiwu=false;





		public static bool debugMode= false;

	}
}
