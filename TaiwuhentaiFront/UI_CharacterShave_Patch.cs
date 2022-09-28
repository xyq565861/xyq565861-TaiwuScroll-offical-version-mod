using CharacterDataMonitor;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.AvatarSystem.AvatarRes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UICommon.Character;
using UICommon.Character.Avatar;

namespace TaiwuhentaiFront
{
	[HarmonyPatch(typeof(UI_CharacterShave))]
	class UI_CharacterShave_Patch
	{
		[HarmonyPatch("OnGetCharacterAvatarData")]
		static bool Prefix(UI_CharacterShave __instance, string ____name, AvatarInfoMonitor ____monitor, AvatarData ____avatarData, AvatarData ____avatarDataOld, BasicInfoMonitor ____basicInfoMonitor)
		{
			
			try
			{
				Debuglogger.Log(" OnGetCharacterAvatarData start");
				Avatar[] avatars = new Avatar[]{__instance.CGet<Avatar>("Avatar")};

				AvatarAdjustController avatarAdjustController = __instance.CGet<AvatarAdjustController>("Content");
				bool flag = ____monitor.AvatarData.GetGrowableElementShowingState(0) && ____monitor.AvatarData.GetGrowableElementShowingAbility(0);
				bool flag2 = flag && ____monitor.AvatarData.FrontHairId != 1;

				AvatarAsset asset = SingletonObject.getInstance<AvatarManager>().GetAsset((int)____monitor.AvatarData.AvatarId, EAvatarElementsType.Hair1, new short[]
				{
					____monitor.AvatarData.FrontHairId
				});

				bool active = flag2 && asset != null && asset.Config != null && !asset.Config.DisableRelativeType;
				bool active2 = ____monitor.AvatarData.GetGrowableElementShowingState(1) && ____monitor.AvatarData.GetGrowableElementShowingAbility(1) && ____monitor.AvatarData.Beard1Id != 1;
				bool active3 = ____monitor.AvatarData.GetGrowableElementShowingState(2) && ____monitor.AvatarData.GetGrowableElementShowingAbility(2) && ____monitor.AvatarData.Beard2Id != 1;
				bool active4 = ____monitor.AvatarData.GetGrowableElementShowingState(6) && ____monitor.AvatarData.GetGrowableElementShowingAbility(6);

				____avatarData = new AvatarData(____monitor.AvatarData);

				____avatarData.Copy(____monitor.AvatarData);
				____avatarDataOld = new AvatarData(____monitor.AvatarData);

				____avatarDataOld.Copy(____monitor.AvatarData);
				____name = __instance.CGet<TextMeshProUGUI>("CharacterName").text;


				avatarAdjustController.AvatarAdjustItemEyes = new AvatarAdjustItemEyes();


				avatarAdjustController.Init(ref ____avatarData, ____basicInfoMonitor.Gender, ____monitor.AvatarAge, avatars, false, ____basicInfoMonitor.Gender != ____avatarData.Gender);


				avatarAdjustController.AvatarAdjustItemFrontHair.gameObject.SetActive(flag2);
				avatarAdjustController.AvatarAdjustItemBackHair.gameObject.SetActive(active);
				avatarAdjustController.AvatarAdjustItemBeard1.gameObject.SetActive(active2);
				avatarAdjustController.AvatarAdjustItemBeard2.gameObject.SetActive(active3);
				avatarAdjustController.AvatarAdjustItemEyeBrows.gameObject.SetActive(active4);
				__instance.Element.ShowAfterRefresh();
			}
			catch(Exception e)
			{
				Debuglogger.Log(" OnGetCharacterAvatarData error" + e.Message+e.StackTrace);
				return true;
			}
			return false;
		}
	}
}
