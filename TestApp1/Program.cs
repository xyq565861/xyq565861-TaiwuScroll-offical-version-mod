using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MirrorNet;


namespace TestApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            string strBuffer;
            Console.WriteLine("操作说明：\n先打开的进程运行A,等待另外一个进程B的操作。\n然后，再打开一个进程运行B，写入数据到共享内存中。");
            Console.WriteLine("----------------------------------------------\n请输入线程运行的模式[A/B]：");
            strBuffer = Console.ReadLine();


            if (strBuffer.Contains("A") || strBuffer.Contains("a"))
            {
                TaiwuBakenServer taiwuBakenServer = new TaiwuBakenServer("Taiwu/Hentai/Mirrorpipe");
                taiwuBakenServer.Start();
                //MirrorServer mirrorServer = new MirrorServer("Taiwu/Hentai/Mirrorpipe");

                //mirrorServer.ServerMessageReceivedEvent += ServerMessageReceived;
                //mirrorServer.ServerClientConnectedEvent += ClientConnectedEvent;
                //mirrorServer.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
                //mirrorServer.Start();
                while (true)
                {
                    strBuffer = Console.ReadLine();

                }


            }
            //else if (strBuffer.Contains("B") || strBuffer.Contains("b"))
            //{
            //    TaiwuFrontClient taiwuFrontClient = new TaiwuFrontClient("Taiwu/Hentai/Mirrorpipe");
            //    taiwuFrontClient.Start();
            //    //MirrorClient mirrorClient = new MirrorClient("Taiwu/Hentai/Mirrorpipe");
            //    //mirrorClient.ServerMessageReceivedEvent += MessageReceived;
            //    //mirrorClient.ServerClientConnectedEvent += ClientConnectedEvent;
            //    //mirrorClient.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
            //    //mirrorClient.Start();
            //    while (true)
            //    {

            //        strBuffer = Console.ReadLine();
            //        try
            //        {
            //            //TaiwuQuery tm = new TaiwuQuery();
            //            //tm.Initialize("MirrorNet", "MirrorNet", "UilityTools", "pluse", new List<object> { 7, 5 });
            //            //QueryMsg qm = new QueryMsg();

            //            object obj = taiwuFrontClient.Query("MirrorNet", "MirrorNet", "UilityTools", "pluse", new List<object> { 7, 5 });
            //           // qm.Initialize(tm.ProtocolDataUnit(), tm.id);
            //           // int i = (int)QueryCall(qm);
            //            Console.WriteLine(obj);
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //            Console.WriteLine(ex.StackTrace);
            //        }


            //    }
            //}
            else if (strBuffer.Contains("C") || strBuffer.Contains("c"))
            {
                while (true)
                {
                    strBuffer = Console.ReadLine();
                    string str1 = Guid.NewGuid().ToString();
                    long num2 = DateTime.Now.Ticks;

                    List<byte> vs = new List<byte>();
                    byte[] a = BitConverter.GetBytes(num2);
                    byte[] h = Encoding.UTF8.GetBytes(str1);
                    byte[] e = Encoding.UTF8.GetBytes(str1+'\0');
                    Console.WriteLine(string.Format("{0}-{1}-{2}", str1.Length, h.Length, Encoding.UTF8.GetString(h)));
                    Console.WriteLine(string.Format("{0}-{1}-{2}", num2, a.Length, BitConverter.ToUInt64(a)));


                }

            }
        }

        //private static object QueryCall(QueryMsg queryMsg)
        //{
        //    TaiwuQuery taiwuQuery = new TaiwuQuery(queryMsg.CallId);
        //    taiwuQuery.TryFormate(queryMsg.Massage);
        //    System.Reflection.Assembly m_Assembly = System.Reflection.Assembly.Load(taiwuQuery.Assemblystr);
        //    Type t = m_Assembly.GetType(taiwuQuery.NamespaceStr + "." + taiwuQuery.ClassStr);
        //    System.Object obj = Activator.CreateInstance(t);
        //    MethodInfo method = t.GetMethod(taiwuQuery.MethodStr);
        //    BindingFlags flag = BindingFlags.Static | BindingFlags.IgnoreCase;
        //    //ParameterInfo[] paramInfos = method.GetParameters();
        //    //LinkedList<object> ts = new LinkedList<object>();
        //    object[] parameters = taiwuQuery.Args.ToArray();


        //    object returnValue = method.Invoke(obj, flag, Type.DefaultBinder, parameters, null);
        //    return returnValue;
        //}

        static private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {

            if (e != null)
            {
                QueryMsg msg = new QueryMsg();
                try
                {
                    msg.TryFormate(e.Data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                Console.WriteLine(msg.Massage);


            }
        }
        static private void ServerMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e != null)
            {
                var Server = sender as MirrorServer;
                if (Server != null)
                {



                }
            }

        }
        static private void ClientDisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            if (e != null)
            {
                Console.WriteLine("ClientDisconnected" + e.ClientId);
            }
        }
        static private void ClientConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            if (e != null)
            {
                Console.WriteLine("ClientConnected" + e.ClientId);
            }
        }
    }

}
