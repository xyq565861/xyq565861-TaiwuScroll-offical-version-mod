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
	[PluginConfig("Taiwuhentai_Backend", "FD.FLY", "0.0.8")]
	public class Taiwuhentai : TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{
			Debuglogger.Log("star injecting back plugin dll");

			this.HarmonyInstance.PatchAll(typeof(ModDomain_Patch));
			this.HarmonyInstance.PatchAll(typeof(RelationType_Patch));
			this.HarmonyInstance.PatchAll(typeof(Relation_Patch));
			this.HarmonyInstance.PatchAll(typeof(CharacterDomain_Newbron_Patch));
			this.HarmonyInstance.PatchAll(typeof(CharacterDomain_Marry_Patch));
			this.HarmonyInstance.PatchAll(typeof(Character_Patch_PeriAdvanceMonth));
			this.HarmonyInstance.PatchAll(typeof(Character_Patch_ComplementPeriAdvanceMonth));
			this.HarmonyInstance.PatchAll(typeof(PregnantState_Patch));
			this.HarmonyInstance.PatchAll(typeof(EventHelper_BoyOrGirlFriend));
			this.HarmonyInstance.PatchAll(typeof(EventHelper_HusbandOrWifePatch));
			//this.HarmonyInstance.PatchAll(typeof(Test));

			Debuglogger.Log("injected back plugin dll");

		}
		public override void Dispose()
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
		public override void OnModSettingUpdate()
		{
			DomainManager.Mod.GetSetting(base.ModIdStr, "unrestrainedSpouseNum", ref Taiwuhentai.unrestrainedSpouseNum);
			DomainManager.Mod.GetSetting(base.ModIdStr, "unrestrainedSpouseFactions", ref Taiwuhentai.unrestrainedSpouseFactions);
			DomainManager.Mod.GetSetting(base.ModIdStr, "spouseAge", ref Taiwuhentai.spouseAge);
			DomainManager.Mod.GetSetting(base.ModIdStr, "fertilityIgnoreAgeTaiwu", ref Taiwuhentai.fertilityIgnoreAgeTaiwu);
			DomainManager.Mod.GetSetting(base.ModIdStr, "fertilityIgnoreAgeTaiwuSpouse", ref Taiwuhentai.fertilityIgnoreAgeTaiwuSpouse);
			DomainManager.Mod.GetSetting(base.ModIdStr, "responsibleParent", ref Taiwuhentai.responsibleParent);
			DomainManager.Mod.GetSetting(base.ModIdStr, "bloodTies", ref Taiwuhentai.bloodTies);
			DomainManager.Mod.GetSetting(base.ModIdStr, "childGender", ref Taiwuhentai.childGender);
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

			DomainManager.Mod.GetSetting(base.ModIdStr, "debugMode", ref Taiwuhentai.debugMode);
			Debuglogger.Log(string.Format("back plugin setting complete:\n " +
				"unrestrainedSpouse:{0}\n " +
				"unrestrainedSpouseFactions:{1}\n " +
				"spouseAge:{2}\n " +
				"fertilityIgnoreAgeTaiwu:{3}\n" +
				" fertilityIgnoreAgeTaiwuSpouse:{4}\n" +
				" responsibleParent:{5}\n " +
				"bloodTies:{6}\n " +
				"childGender:{7}\n " +
				"rateOfConfessionTaiwu:{8}\n " +
				"rateOfConfession:{9}\n" +
				"preventTaiwuSpouseStray:{10}\n" +
				"preventTaiwuSpouseIllegalLove:{11}\n" +
				"rateOfPregnantTaiwu:{12}\n" +
				"rateOfPregnant:{13}\n" +
				"lesbianPregnantTaiwu:{14}\n" +
				"lesbianPregnantTaiwu:{15}\n" +
				"noOverheardIllegalMakeLoveTaiwu:{16}\n" +

				"debugMode:{17}\n"
				, new object[]
			{
				Taiwuhentai. unrestrainedSpouseNum,
				Taiwuhentai. unrestrainedSpouseFactions,
				Taiwuhentai. spouseAge,
				Taiwuhentai. fertilityIgnoreAgeTaiwu,
				Taiwuhentai. fertilityIgnoreAgeTaiwuSpouse,
				Taiwuhentai. responsibleParent,
				Taiwuhentai. bloodTies,
				Taiwuhentai. childGender,
				Taiwuhentai. rateOfConfessionTaiwu,
				Taiwuhentai. rateOfConfession,
				Taiwuhentai. preventTaiwuSpouseStray,
				Taiwuhentai. preventTaiwuSpouseIllegalLove,
				Taiwuhentai. rateOfPregnantTaiwu,
				Taiwuhentai.rateOfPregnant,
				Taiwuhentai.lesbianPregnantTaiwu,
				Taiwuhentai.lesbianPregnantIO,

				Taiwuhentai.noOverheardIllegalMakeLoveTaiwu,

				Taiwuhentai.debugMode
			}));
		}
		
		public static bool unrestrainedSpouseNum;
		public static bool unrestrainedSpouseFactions;
		public static int spouseAge;
		public static bool fertilityIgnoreAgeTaiwu = false;
		public static bool fertilityIgnoreAgeTaiwuSpouse = false;
		public static bool responsibleParent;
		public static bool bloodTies=false;
		public static int childGender=2;
		public static int rateOfConfessionTaiwu;
		public static int rateOfConfession=1;
		public static bool preventTaiwuSpouseStray;
		public static bool preventTaiwuSpouseIllegalLove;
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

		public static bool noOverheardIllegalMakeLoveTaiwu=false;





		public static bool debugMode= false;

	}
}
