using GameData.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaiwuhentaiFront
{
	public class UilityTools
	{

		public static object Qurey(string Assemblystr, string NamespaceStr, string ClassStr, string MethodStr, List<object> Agrs, int waitTime = 5)
		{
			Debuglogger.Log("Qurey");
			object returnValue=TaiwuhentaiFront.taiwuFrontClient.Query(Assemblystr, NamespaceStr, ClassStr, MethodStr, Agrs, waitTime);

			Debuglogger.Log("D");

			return returnValue;

		}

		public static int getGetBaseCharm(int charId)
		{
			Debuglogger.Log("getGetBaseCharm");
			int charm = 0;
			object obj = Qurey("TaiwuhentaiFrontBackComponent", "TaiwuhentaiFrontBackComponent", "UilityTools", "getGetBaseCharm", new List<object> { charId });
			charm = (int)obj;
			return charm;
		}
		public static bool getGetBisexual(int charId)
		{
			Debuglogger.Log("getGetBisexual");
			bool flagBex = false;
			object obj = Qurey("TaiwuhentaiFrontBackComponent", "TaiwuhentaiFrontBackComponent", "UilityTools", "getGetBisexual", new List<object> { charId });
			flagBex = (bool)obj;
			return flagBex;
		}
	}
}