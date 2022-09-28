using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuModdingLib.Core.Plugin;
using UnityEngine;
namespace TaiwuhentaiFront
{
	[PluginConfig("Taiwuhentai_Frontend", "FD.FLY", "0.0.1")]
    public class TaiwuhentaiFront: TaiwuRemakePlugin
	{
		
		public override void Initialize()
		{
			UnityEngine.Debug.LogError("！！！！！！！！！！！！！！！！！！！！！！！！！！！！");

			harmony = new Harmony("Taiwuhentai front");
			harmony.PatchAll(typeof(UI_CharacterShave_Patch));

			UnityEngine.Debug.LogError("********************************");

		}

		public override void OnModSettingUpdate()
		{

			ModManager.GetSetting(base.ModIdStr, "displayGlamour", ref TaiwuhentaiFront.displayGlamour);
			ModManager.GetSetting(base.ModIdStr, "debugMode", ref TaiwuhentaiFront.debugMode);

			Debuglogger.Log(string.Format("front plugin setting complete:\n displayGlamour:{0}\n debugMode:{1}\n harmony:{2}", new object[]
			{
				TaiwuhentaiFront.displayGlamour,
				TaiwuhentaiFront.debugMode,
				(harmony==null).ToString()
				
			}));
		}

		public override void Dispose()
		{
			bool flag = harmony != null;
			if (flag)
			{
				harmony.UnpatchSelf();
			}
		}




		static Harmony harmony;
		public static bool displayGlamour;
		public static bool debugMode;
	}
}
