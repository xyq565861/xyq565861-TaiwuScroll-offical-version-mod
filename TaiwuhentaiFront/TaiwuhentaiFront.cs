using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuModdingLib.Core.Plugin;

namespace TaiwuhentaiFront
{
    [PluginConfig("Taiwuhentai_Frontend", "FD.FLY", "0.0.1")]
    public class TaiwuhentaiFront: TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{
			Debuglogger.Log("star injecting front plugin dll");
			this.HarmonyInstance.PatchAll(typeof(CharacterAge_Patch));
			Debuglogger.Log("injected front plugin dll");

		}
		public override void OnModSettingUpdate()
		{
			ModManager.GetSetting(base.ModIdStr, "displayGlamour", ref TaiwuhentaiFront.displayGlamour);
			ModManager.GetSetting(base.ModIdStr, "debugMode", ref TaiwuhentaiFront.debugMode);

			Debuglogger.Log(string.Format("back plugin setting complete:\n displayGlamour:{0}\n debugMode:{1}", new object[]
			{
				TaiwuhentaiFront.displayGlamour,
				TaiwuhentaiFront.debugMode

			}));
		}
		public static bool displayGlamour;
		public static bool debugMode;
	}
}
