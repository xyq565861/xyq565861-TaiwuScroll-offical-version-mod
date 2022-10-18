

using GameData.Domains.Mod;
using HarmonyLib;
using MirrorNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNetTaiwuServer
{
    [HarmonyPatch(typeof(ModDomain))]
    class ModDomain_Patch
    {
        [HarmonyPatch("LoadAllEventPackages")]
        static void Postfix()
        {
            Harmony harmony = new Harmony("MirrorNetServer");
            Debuglogger.Log("star init server ");
            TaiwuBakenServer taiwuBakenServer = new TaiwuBakenServer(TaiwuMirrorServer.pipName);
            taiwuBakenServer.Start();
            Debuglogger.Log(" server  start");
        }
    }
}
