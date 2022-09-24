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
	[PluginConfig("Taiwuhentai_Backend", "FD.FLY", "0.0.1")]
	public class Taiwuhentai : TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{
			Debuglogger.Log("star injecting back plugin dll");

			this.HarmonyInstance.PatchAll(typeof(ModDomain_Patch));
			

			Debuglogger.Log("injected back plugin dll");

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
			DomainManager.Mod.GetSetting(base.ModIdStr, "unrestrainedSpouse", ref Taiwuhentai.unrestrainedSpouseNum);
			DomainManager.Mod.GetSetting(base.ModIdStr, "unrestrainedSpouseFactions", ref Taiwuhentai.unrestrainedSpouseFactions);
			DomainManager.Mod.GetSetting(base.ModIdStr, "spouseAge", ref Taiwuhentai.spouseAge);
			DomainManager.Mod.GetSetting(base.ModIdStr, "bloodTies", ref Taiwuhentai.bloodTies);

			DomainManager.Mod.GetSetting(base.ModIdStr, "debugMode", ref Taiwuhentai.debugMode);
			Debuglogger.Log(string.Format("back plugin setting complete:\n unrestrainedSpouse:{0}\n unrestrainedSpouseFactions:{1}\n spouseAge:{2}\n bloodTies:{3}\n debugMode:{4}", new object[]
			{
				Taiwuhentai.unrestrainedSpouseNum,
				Taiwuhentai.unrestrainedSpouseFactions,
				Taiwuhentai.spouseAge,
				Taiwuhentai.bloodTies,
				Taiwuhentai.debugMode
			}));
		}
		public static bool unrestrainedSpouseNum;
		public static bool unrestrainedSpouseFactions;
		public static int spouseAge;
		public static bool bloodTies;

		public static bool debugMode;

	}
}
