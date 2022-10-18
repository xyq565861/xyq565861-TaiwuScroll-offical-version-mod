using GameData.Domains;
using MirrorNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuModdingLib.Core.Plugin;

namespace MirrorNetTaiwuServer
{
	[PluginConfig("TaiwuMirrorServer", "FD.FLY", "0.0.1")]
	public class TaiwuMirrorServer : TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{

			this.HarmonyInstance.PatchAll(typeof(ModDomain_Patch));

		}

		public override void OnModSettingUpdate()
		{


			DomainManager.Mod.GetSetting(base.ModIdStr, "pipName", ref TaiwuMirrorServer.pipName);
			DomainManager.Mod.GetSetting(base.ModIdStr, "debugMode", ref TaiwuMirrorServer.debugMode);

			Debuglogger.Log(string.Format("front plugin setting complete:\n pipName:{0}\n debugMode:{1}\n ", new object[]
			{

				TaiwuMirrorServer.pipName,
				TaiwuMirrorServer.debugMode,


			}));
		}

		public override void Dispose()
		{
			this.HarmonyInstance.UnpatchSelf();
			this.HarmonyInstance.UnpatchAll("MirrorNetServer");
		}



		public static string pipName;

		public static bool debugMode;
	}

}
