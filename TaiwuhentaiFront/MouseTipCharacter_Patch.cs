using CharacterDataMonitor;
using Config;
using FrameWork;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;
using HarmonyLib;
using MirrorNet;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UICommon.Character.Avatar;
using UnityEngine.Assertions;
namespace TaiwuhentaiFront
{
    [HarmonyPatch(typeof(MouseTipCharacter))]
	class MouseTipCharacter_Patch
	{

		[HarmonyPatch("Init")]
		static bool Prefix(ArgumentBox argsBox, ref MouseTipCharacter __instance, ref bool ___NeedWaitData, ref int ____charId)
		{

			int hentaiShowAge;
			int mirrorCharId;
			bool flag = argsBox.Get("hentaiShowAge", out hentaiShowAge);
			bool flag2 = argsBox.Get("mirrorCharId", out mirrorCharId);
			//Debug.LogError(mirrorCharId);
			if (flag && flag2)
			{
				showAge = (short)hentaiShowAge;
				tipInstance = __instance;
				tipCharId = mirrorCharId;
				____charId = mirrorCharId;
				___NeedWaitData = true;
				__instance.AsynchMethodCall<int[]>(DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GetCharacterDisplayDataList, new int[]
				{
				tipCharId,
				}, new Action<int, RawDataPool>(OnHentaiCharDisplayData));

				return false;
			}

			return true;
		}

		private static void OnHentaiCharDisplayData(int offset, RawDataPool dataPool)
		{

			Debuglogger.Log("OnHentaiCharDisplayData"+ offset+"_"+ tipCharId);
			List<CharacterDisplayData> list = EasyPool.Get<List<CharacterDisplayData>>();
			list.Clear();

			Serializer.Deserialize(dataPool, offset, ref list);
			Assert.AreEqual(list.Count, 1);
			_displayData = list[0];
			int num = _displayData.BirthDate % 12;
			bool flag = num < 0;

			if (flag)
			{
				num += 12;
			}
			FeatureMonitor featureMonitor=SingletonObject.getInstance<CharacterMonitorModel>().GetMonitorItem<FeatureMonitor>(tipCharId, 5, false);
			int isChaste = 0;
            if (featureMonitor.FeatureIds.Contains(195))
            {
				isChaste = 1;
			}
			if (featureMonitor.FeatureIds.Contains(196))
            {
				isChaste = 2;
			}
			if (featureMonitor.FeatureIds.Contains(197))
			{
				isChaste = 3;
			}
			Debuglogger.Log("OnHentaiCharDisplayData" + offset + "bb" + tipCharId);
			featureMonitor.FeatureIds.Contains(218);
			_displayData.AvatarRelatedData.DisplayAge = showAge;
			string charMonasticTitleOrNameByDisplayData = NameCenter.GetCharMonasticTitleOrNameByDisplayData(_displayData, tipCharId == SingletonObject.getInstance<BasicGameData>().TaiwuCharId, false);
			MonthItem monthItem = Month.Instance[num];
			WorldMapModel instance = SingletonObject.getInstance<WorldMapModel>();
			string str;
			switch (isChaste)
            { 
				case 1:
					str = "<color=#00ffff>纯洁之身</color>";
					break;
				case 2:
					str = "<color=#dfff00>已经人事</color>";
					break;
				case 3:
					str = "<color=#e60505>身怀六甲</color>";
					break;


				default:
					str = "<color=#000000>薛定谔</color>";
					break;
			}

            tipInstance.CGet<TextMeshProUGUI>("Title").text = charMonasticTitleOrNameByDisplayData+"目前状态："+str+ ",预测该人物<color=#00ff00>" + showAge+ "</color>岁样貌为：";
			tipInstance.CGet<TextMeshProUGUI>("Name").text = charMonasticTitleOrNameByDisplayData;

			tipInstance.CGet<TextMeshProUGUI>("Age").text =  LocalStringManager.GetFormat(52, new object[]
			{
			showAge
			}) ;

			tipInstance.CGet<CImage>("InnateFiveElementsType").SetSprite(string.Format("mousetip_shuxing_{0}", monthItem.FiveElementsType), false, null);
			tipInstance.CGet<Avatar>("Avatar").Refresh(_displayData);
			SetRefersValues(tipInstance,"CharacterGender", CommonUtils.GetGenderString(_displayData.Gender), CommonUtils.GetGenderIcon(_displayData.Gender));
			SetRefersValues(tipInstance,"CharacterBehavior", CommonUtils.GetBehaviorString(_displayData.BehaviorType), Config.BehaviorType.Instance.GetItem((short)_displayData.BehaviorType).Icon);
			SetRefersValues(tipInstance,"CharacterIdentity", CommonUtils.GetCharacterGradeString(_displayData.OrgInfo, _displayData.Gender, showAge), CommonUtils.GetIdentityIcon(_displayData.OrgInfo.Grade));
			Debuglogger.Log("OnHentaiCharDisplayData" + offset + "aa" + tipCharId);

			tipInstance.AsynchMethodCall<List<int>>(DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GetGroupCharDisplayDataList, new List<int>
			{
				tipCharId
			}, new Action<int, RawDataPool>(HentaiOnGetGroupCharDisplayData));
			EasyPool.Free(list);

		}
		private static void SetRefersValues(MouseTipCharacter __instance, string refersName, string valueText,  string icon = null)
		{
			Refers refers = __instance.CGet<Refers>(refersName);
			bool flag = !string.IsNullOrEmpty(icon);
			if (flag)
			{
				refers.CGet<CImage>("Icon").SetSprite(icon, false, null);
			}
			refers.CGet<TextMeshProUGUI>("InfoValue").SetText(valueText, true);
		}
		private static void HentaiOnGetGroupCharDisplayData(int offset, RawDataPool dataPool)
		{
			Assembly assem = Assembly.GetExecutingAssembly();
			Debuglogger.Log($"程序集全名:{assem.FullName}");
			Debuglogger.Log($"程序集的版本：{assem.GetName().Version}");
			Debuglogger.Log($"程序集位置：{assem.Location}");
			Debuglogger.Log($"程序集入口：{assem.EntryPoint}");
			Debuglogger.Log($"获取用于加载程序集的主机上下文：{assem.HostContext}");
			Debuglogger.Log($"CLR 版本的文件夹名：{assem.ImageRuntimeVersion}");
			Debuglogger.Log($"当前程序集是否在当前进程中动态生成的：{assem.IsDynamic}");
			Debuglogger.Log($"当前程序集是否以完全信任方式加载：{assem.IsFullyTrusted}");
			Debuglogger.Log($"当前程序集清单的模块：{assem.ManifestModule}");
			Debuglogger.Log($"获取包含此程序集中模块的集合：{assem.Modules}");
			Debuglogger.Log($"程序集被加载到只反射上下文而不是执行上下文中：{assem.ReflectionOnly}");
			Debuglogger.Log($"CLR 对此程序集强制执行的安全规则集:{assem.SecurityRuleSet}");
			Debuglogger.Log("taiwuFrontClient init");
			TaiwuFrontClient taiwuFrontClient = new TaiwuFrontClient(TaiwuhentaiFront.pipName);
			Debuglogger.Log("taiwuFrontClient starting");
			taiwuFrontClient.Start();
			Debuglogger.Log("taiwuFrontClient start");

			bool flagBex = false;
			try
			{
				object obj = taiwuFrontClient.Query("TaiwuhentaiFrontBackComponent", "TaiwuhentaiFrontBackComponent", "UilityTools", "getGetBisexual", new List<object> { tipCharId });
				flagBex = (bool)obj;


			}
			catch (Exception ex)
			{
				Debuglogger.Log(ex.Message + "\n" + ex.StackTrace);
			}
			List<GroupCharDisplayData> list = null;
			Serializer.Deserialize(dataPool, offset, ref list);
			GroupCharDisplayData groupCharDisplayData = list[0];
			tipInstance.CGet<TextMeshProUGUI>("Health").text = CommonUtils.GetCharacterHealthInfo(groupCharDisplayData.Health, groupCharDisplayData.MaxLeftHealth).Item1;
			CharacterItem item = Character.Instance.GetItem(_displayData.TemplateId);
			bool isFixedCharacter = CreatingType.IsFixedPresetType(item.CreatingType);
			SetRefersValues(tipInstance,"CharacterCharm", groupCharDisplayData.Charm.ToString());
			
			SetRefersValues(tipInstance, "CharacterHappiness", flagBex?"双性":"单性");
			SetRefersValues(tipInstance, "CharacterFavorability", CommonUtils.GetFavorString(groupCharDisplayData.FavorabilityToTaiwu), CommonUtils.GetFavorIcon(groupCharDisplayData.FavorabilityToTaiwu));
			sbyte fameType = FameType.GetFameType(groupCharDisplayData.Fame);
			SetRefersValues(tipInstance, "CharacterFame", CommonUtils.GetFameString(fameType), CommonUtils.GetFameIcon(fameType));
			SetRefersValues(tipInstance, "CharacterSamsara", groupCharDisplayData.PreexistenceCharCount.ToString(), null);
			UIElement element = tipInstance.Element;
			if (element != null)
			{
				element.ShowAfterRefresh();
			}
			EasyPool.Free(list);
		}
		static MouseTipCharacter tipInstance;
		static short showAge;
		static int tipCharId;
		static CharacterDisplayData _displayData;
	}
}
