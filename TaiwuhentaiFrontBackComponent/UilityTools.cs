using GameData.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiwuhentaiFrontBackComponent
{
    public class UilityTools
    {
        public static bool getGetBisexual(int charId)
        {
            Debuglogger.Log("getGetBisexual"+charId);
            GameData.Domains.Character.Character selfChar = DomainManager.Character.GetElement_Objects((int)charId);
            return selfChar.GetBisexual();
        }
        public static int getGetBaseCharm(int charId)
        {
            Debuglogger.Log("getGetBisexual" + charId);
            GameData.Domains.Character.Character selfChar = DomainManager.Character.GetElement_Objects((int)charId);
            return selfChar.GetAvatar().BaseCharm;
        }
    }
}