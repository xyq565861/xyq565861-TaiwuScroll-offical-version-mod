using Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData.Domains.Character;
namespace TaiwuhentaiFrontBackComponent
{
	class MirrorBackUtility
	{
		public static short GetAgeCharm(GameData.Domains.Character.Character characters, int showAge)
		{
            if (characters == null)
            {
				Debuglogger.Log("GetAgeCharm error,characters is null");
				return -1;
            }
			short num;
			try
			{


				CharacterItem template = Config.Character.Instance[characters.GetTemplateId()];
				bool isFixed = characters.IsCreatedWithFixedTemplate();
				bool flag2 = isFixed;
				int value;
				if (flag2)
				{
					value = (int)template.BaseAttraction;
				}
				else
				{
					short clothingDisplayId = characters.GetClothingDisplayId();
					value = (int)characters.GetAvatar().GetCharm((short)showAge, clothingDisplayId);
				}
				var getCommonPropertyBonus = typeof(GameData.Domains.Character.Character).GetMethod("GetCommonPropertyBonus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				int num2 = (int)getCommonPropertyBonus.Invoke(characters, new object[] { ECharacterPropertyReferencedType.Attraction, 0 });
				var getPropertyBonusOfCombatSkillEquippingAndBreakout = typeof(GameData.Domains.Character.Character).GetMethod("GetPropertyBonusOfCombatSkillEquippingAndBreakout", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				int num3 = (int)getPropertyBonusOfCombatSkillEquippingAndBreakout.Invoke(characters, new object[] { ECharacterPropertyReferencedType.Attraction, 0 });

				value += num2;
				value += num3;
				bool flag3 = !characters.GetEquipment()[4].IsValid() && !isFixed;
				if (flag3)
				{
					value /= 2;
				}
				num = (short)Math.Clamp(value, 0, 900);
			}
			catch
			{

				Debuglogger.Log("GetAgeCharm error,charid=" + characters.GetId() + "showage=" + showAge);
				num = -1;

			}
			return num;
		}

	}
}