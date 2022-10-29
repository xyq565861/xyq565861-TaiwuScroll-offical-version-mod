using GameData.GameDataBridge.UnityAdapter;
using GameData.Serializer;
using HarmonyLib;
using MirrorNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaiwuhentaiFront
{
    [HarmonyPatch(typeof(UI_CharacterMenu))]
    class UI_CharacterMenu_Patch
    {
        [HarmonyPatch("OnInit")]
        public static void Postfix()
		{

                try
                {
                    if (TaiwuhentaiFront.taiwuFrontClient != null)
                    {
                        TaiwuhentaiFront.taiwuFrontClient.Stop();
                    }
                    TaiwuhentaiFront.taiwuFrontClient = new TaiwuFrontClient(TaiwuhentaiFront.pipName);
                    TaiwuhentaiFront.taiwuFrontClient.Start();
                object obj = TaiwuhentaiFront.taiwuFrontClient.Query("TaiwuhentaiFrontBackComponent", "TaiwuhentaiFrontBackComponent", "UilityTools", "getGetBaseCharm", new List<object> { 5166 });
                Debuglogger.Log(obj);
                Thread.Sleep(4000);

                }
                catch (Exception ex)
                {

                    Debuglogger.Log(ex.Message);
                    Debuglogger.Log(ex.StackTrace);
                }




        }
    }
}
