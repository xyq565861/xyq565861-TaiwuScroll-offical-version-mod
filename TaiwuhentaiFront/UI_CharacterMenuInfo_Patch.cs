using FrameWork;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UICommon.Character;
using UnityEngine;

namespace TaiwuhentaiFront
{
	[HarmonyPatch(typeof(UI_CharacterMenuInfo))]
	class UI_CharacterMenuInfo_Patch
	{
		
		[HarmonyPatch("UpdateAge")]
		static void Postfix(UI_CharacterMenuInfo __instance)
		{

			Debuglogger.Log("injected CharacterAge_Patch FillElement");
            if (TaiwuhentaiFront.ageMirror)
            {
				TextMeshProUGUI ____ageLabel = __instance.CGet<TextMeshProUGUI>("Age");
				if (____ageLabel != null)
				{
					Debuglogger.Log("Age found");
					GameObject obj = ____ageLabel.transform.parent.parent.gameObject;
					if (obj != null)
					{

						MouseTipDisplayer component = obj.GetComponent<MouseTipDisplayer>();
						CImage componentBackground = obj.GetComponent<CImage>();
						bool flag = component == null;

						if (flag)
						{

							Debuglogger.Log("No MouseTipDisplayer was found");
							obj.AddComponent<MouseTipDisplayer>();
							if (componentBackground == null)
							{
								Debuglogger.Log("No componentBackground was found");
								obj.AddComponent<CImage>();
								componentBackground = obj.GetComponent<CImage>();
								if (componentBackground != null)
								{
									componentBackground.color = new Color(0, 0, 0, 0);

								}

							}
							component = obj.GetComponent<MouseTipDisplayer>();



						}
						component.name = "HentaiCharacterMouseTips";

						component.Type = TipType.Character;

						if (component.RuntimeParam == null)
						{
							component.RuntimeParam = EasyPool.Get<ArgumentBox>();
							component.RuntimeParam.Clear();
						}
						Debuglogger.Log(__instance.CharacterMenu.CurCharacterId);
						component.RuntimeParam.Set("charId", __instance.CharacterMenu.CurCharacterId);
						component.RuntimeParam.Set("mirrorCharId", __instance.CharacterMenu.CurCharacterId);
						int showAge = 0;
						switch (TaiwuhentaiFront.showAge)
						{
							case 1:
								showAge = 1;
								break;
							case 2:
								showAge = 10;
								break;
							case 3:
								showAge = 25;
								break;
							case 4:
								showAge = 40;
								break;
							case 5:
								showAge = 60;
								break;
							case 6:
								showAge = 90;
								break;
							default:
								showAge = 18;
								break;
						}
						component.RuntimeParam.Set("hentaiShowAge", showAge);
						component.enabled = true;


					}

				}


			}

		}




	}
}

