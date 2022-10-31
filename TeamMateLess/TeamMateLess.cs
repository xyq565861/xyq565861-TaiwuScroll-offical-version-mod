

using GameData.Domains;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuModdingLib.Core.Plugin;

namespace TeamMateLess
{
	[PluginConfig("TeamMateLess", "FD.FLY", "0.0.1")]
	public class TeamMateLess : TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{


			this.HarmonyInstance.PatchAll(typeof(OptionConditionMatcher_Patch));


		}
		public override void Dispose()
		{
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
			DomainManager.Mod.GetSetting(base.ModIdStr, "enable", ref TeamMateLess.enable);

		}
		


		public static bool enable = false;

	}
}
