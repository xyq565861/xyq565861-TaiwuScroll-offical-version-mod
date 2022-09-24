using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UICommon.Character;

namespace TaiwuhentaiFront
{
    [HarmonyPatch(typeof(CharacterAge))]
    class CharacterAge_Patch
    {
        [HarmonyPatch("FillElement")]
        static void Postfix(ref CharacterAge __instance,ref MouseTipDisplayer ____fiveElementsMouseTip)
        {
            Debuglogger.Log("output PresetParam");
            if (____fiveElementsMouseTip.PresetParam != null)
            {
                Debuglogger.Log(____fiveElementsMouseTip.PresetParam.ToString());
            }
            else
            {
                Debuglogger.Log("null PresetParam");
            }
            
        }
    }
}
