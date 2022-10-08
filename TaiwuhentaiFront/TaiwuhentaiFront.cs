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
			

			harmony = new Harmony("Taiwuhentai front");
			harmony.PatchAll(typeof(UI_CharacterMenuInfo_Patch));
			harmony.PatchAll(typeof(MouseTipCharacter_Patch));

		

		}

		public override void OnModSettingUpdate()
		{

			ModManager.GetSetting(base.ModIdStr, "ageMirror", ref TaiwuhentaiFront.ageMirror);
			ModManager.GetSetting(base.ModIdStr, "showAge", ref TaiwuhentaiFront.showAge);
			ModManager.GetSetting(base.ModIdStr, "debugMode", ref TaiwuhentaiFront.debugMode);

			Debuglogger.Log(string.Format("front plugin setting complete:\n ageMirror:{0}\n showAge:{1}\n debugMode:{2}\n harmony:{3}", new object[]
			{
				TaiwuhentaiFront.ageMirror,
				TaiwuhentaiFront.showAge,
				TaiwuhentaiFront.debugMode,
				(harmony!=null).ToString()
				
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
		public static bool ageMirror;
		public static int showAge;
		public static bool debugMode;
	}
}
