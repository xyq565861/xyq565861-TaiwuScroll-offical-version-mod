using GameData.Domains;
using HarmonyLib;
using System;
using TaiwuModdingLib.Core.Plugin;

namespace TaiwuhentaiFrontBackComponent
{
	[PluginConfig("TaiwuhentaiFrontBackComponent", "FD.FLY", "0.0.1")]
	public class TaiwuhentaiFrontBackComponent : TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{

			//this.HarmonyInstance.PatchAll(typeof(ModDomain_Patch));

		}

		public override void OnModSettingUpdate()
		{


			DomainManager.Mod.GetSetting(base.ModIdStr, "debugMode", ref TaiwuhentaiFrontBackComponent.debugMode);

			Debuglogger.Log(string.Format("front plugin setting complete:\n debugMode:{0}\n ", new object[]
			{

				TaiwuhentaiFrontBackComponent.debugMode,


			}));
		}

		public override void Dispose()
		{
			this.HarmonyInstance.UnpatchSelf();
		}




		public static bool debugMode;
	}
}
