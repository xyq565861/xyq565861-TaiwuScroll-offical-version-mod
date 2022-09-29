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
	[PluginConfig("Taiwuhentai_Backend", "FD.FLY", "0.0.5")]
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
			this.HarmonyInstance.PatchAll(typeof(Character_Patch));
			this.HarmonyInstance.PatchAll(typeof(PregnantState_Patch));
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
			DomainManager.Mod.GetSetting(base.ModIdStr, "rateOfPregnantTaiwu", ref Taiwuhentai.rateOfPregnantTaiwu);
			DomainManager.Mod.GetSetting(base.ModIdStr, "rateOfPregnant", ref Taiwuhentai.rateOfPregnant);
			DomainManager.Mod.GetSetting(base.ModIdStr, "lesbianPregnantTaiwu", ref Taiwuhentai.lesbianPregnantTaiwu);

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
				"rateOfPregnantTaiwu:{11}\n" +
				"rateOfPregnant:{12}\n" +
				"lesbianPregnantTaiwu:{13}\n" +

				"debugMode:{14}\n"
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
				Taiwuhentai. rateOfPregnantTaiwu,
				Taiwuhentai.rateOfPregnant,
				Taiwuhentai.lesbianPregnantTaiwu,
				Taiwuhentai.debugMode
			}));
		}
		
		public static bool unrestrainedSpouseNum;
		public static bool unrestrainedSpouseFactions;
		public static int spouseAge;
		public static bool fertilityIgnoreAgeTaiwu;
		public static bool fertilityIgnoreAgeTaiwuSpouse;
		public static bool responsibleParent;
		public static bool bloodTies;
		public static int childGender;
		public static int rateOfConfessionTaiwu;
		public static int rateOfConfession;
		public static bool preventTaiwuSpouseStray;
		public static int rateOfPregnantTaiwu;
		public static int rateOfPregnant;
		public static bool lesbianPregnantTaiwu;





		public static bool debugMode;

	}
}
