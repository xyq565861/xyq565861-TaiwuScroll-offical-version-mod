
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Taiwuhentai
{
	public static class Debuglogger
	{
		public static void Log(object text)
		{

			if (Taiwuhentai.debugMode && text != null)
			{



				try
				{
					readerWriterLockSlim.EnterWriteLock();
					using (FileStream fs = new FileStream(Debuglogger.logFile, FileMode.Append, FileAccess.Write, FileShare.Write))
					{
						StreamWriter streamWriter = new StreamWriter(fs);
						string str = Convert.ToString(text);
						DateTime now = DateTime.Now;
						streamWriter.WriteLine(string.Format("Back plugin:{0}-{1}-{2} {3}:{4}:{5}：{6}.", new object[]
						{
						now.Year,
						now.Month,
						now.Day,
						now.Hour,
						now.Minute,
						now.Second,
						str
						}));
						streamWriter.Flush();
						streamWriter.Close();
					}
				}
				catch
				{

				}
				finally
				{
					readerWriterLockSlim.ExitWriteLock();
				}






			}
		}

		static ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();
		private static string logFile = System.IO.Directory.GetCurrentDirectory() + "//TaiwuHentaiLog.txt";
	}
}
