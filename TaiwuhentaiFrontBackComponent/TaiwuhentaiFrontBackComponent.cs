using GameData.Domains;
using HarmonyLib;
using System;
using TaiwuModdingLib.Core.Plugin;

namespace TaiwuhentaiFrontBackComponent
{
    public class TaiwuhentaiFrontBackComponent : TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{
		


		}

		public override void OnModSettingUpdate()
		{

			DomainManager.Mod.GetSetting(base.ModIdStr, "ageMirror", ref TaiwuhentaiFrontBackComponent.ageMirror);
			DomainManager.Mod.GetSetting(base.ModIdStr, "showAge", ref TaiwuhentaiFrontBackComponent.showAge);
			DomainManager.Mod.GetSetting(base.ModIdStr, "debugMode", ref TaiwuhentaiFrontBackComponent.debugMode);

			Debuglogger.Log(string.Format("front plugin setting complete:\n ageMirror:{0}\n showAge:{1}\n debugMode:{2}\n harmony:{3}", new object[]
			{
				TaiwuhentaiFrontBackComponent.ageMirror,
				TaiwuhentaiFrontBackComponent.showAge,
				TaiwuhentaiFrontBackComponent.debugMode,
				

			}));
		}

		public override void Dispose()
		{
				this.HarmonyInstance.UnpatchSelf();
		}




		public static bool ageMirror;
		public static int showAge;
		public static bool debugMode;
	}
}
