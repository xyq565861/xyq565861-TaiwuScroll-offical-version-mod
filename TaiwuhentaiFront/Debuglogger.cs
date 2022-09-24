
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaiwuhentaiFront
{
	public static class Debuglogger
    {
		public static void Log(string text)
		{
			
             if (TaiwuhentaiFront.debugMode)
			{
				StreamWriter streamWriter = File.AppendText(Debuglogger.logFile);
				DateTime now = DateTime.Now;
				streamWriter.WriteLine(string.Format("Front plugin:{0}-{1}-{2} {3}:{4}:{5}：{6}.", new object[]
				{
				now.Year,
				now.Month,
				now.Day,
				now.Hour,
				now.Minute,
				now.Second,
				text
				}));
				streamWriter.Close();
			}

		}

		// Token: 0x0400001C RID: 28
		private static string logFile =System.IO.Directory.GetCurrentDirectory() +"//TaiwuHentaiLog.txt";
	}
}
