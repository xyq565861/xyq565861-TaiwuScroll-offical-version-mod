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
        public static bool getGetBisexual(long charId)
        {
            GameData.Domains.Character.Character selfChar = DomainManager.Character.GetElement_Objects((int)charId);
            return selfChar.GetBisexual();
        }
    }
}