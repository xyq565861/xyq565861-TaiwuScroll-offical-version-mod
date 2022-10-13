using System;
using System.Collections.Generic;
using MirrorNet;
using Newtonsoft.Json;

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
                MirrorServer mirrorServer = new MirrorServer("Taiwu/Hentai/Mirrorpipe");

                mirrorServer.ServerMessageReceivedEvent += ServerMessageReceived;
                mirrorServer.ServerClientConnectedEvent += ClientConnectedEvent;
                mirrorServer.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
                mirrorServer.Start();
                while (true)
                {
                    strBuffer = Console.ReadLine();
                    QueryMsg msg = new QueryMsg();
                    msg.Initialize(strBuffer);
                    mirrorServer.SendAll(msg.ProtocolDataUnit);
                }


            }
            else if (strBuffer.Contains("B") || strBuffer.Contains("b"))
            {
                MirrorClient mirrorClient = new MirrorClient("Taiwu/Hentai/Mirrorpipe");
                mirrorClient.ServerMessageReceivedEvent += MessageReceived;
                mirrorClient.ServerClientConnectedEvent += ClientConnectedEvent;
                mirrorClient.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
                mirrorClient.Start();
                while (true)
                {

                    strBuffer = Console.ReadLine();
                    QueryMsg msg = new QueryMsg();


                    Dictionary<object, string> dict = new Dictionary<object, string>();
                    List<QueryMsg> ls = new List<QueryMsg>();
                    QueryMsg a = new QueryMsg();
                    a.Initialize("qweqweas");

                    QueryMsg b = new QueryMsg();
                    b.Initialize("12312312");
                    ls.Add(a);
                    ls.Add(b);
                    dict[1] = "int";
                    dict["asdasdasd"] = "string";
                    dict[ls] =ls.GetType().FullName;


                    strBuffer = Query("MirroNet","Charnars","ASDQWE",dict);
                    msg.Initialize(strBuffer );
                    byte[] buffer = msg.ProtocolDataUnit;

                    mirrorClient.Send(buffer);


                }
            }
            else
            {
                ;//exit
            }
        }

        public static string Query(string namespaceStr, string classStr,string mechodStr, Dictionary<object, string> args)
        {
            string str= namespaceStr;
            str += "_+_";           
             str += classStr;
            str += "_+_";            
             str += mechodStr;
            str += "_+_";
            str += JsonConvert.SerializeObject(args);
            return str;
        }
        static private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {

            if (e != null)
            {
                QueryMsg msg = new QueryMsg();
                try
                {
                    msg.TryFormate(e.Data);
                }
                catch(Exception ex)
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
                    QueryMsg msg = new QueryMsg();
                    try
                    {
                        msg.TryFormate(e.Data);

                        QueryMsg repMsg = new QueryMsg();
                        repMsg.Initialize("Server get massage" + msg.Massage + e.ClientId);
                        Server.Send(repMsg.ProtocolDataUnit, e.ClientId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                    }
                    Console.WriteLine(msg.Massage);


                }
            }

        }
        static private void ClientDisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            if (e != null)
            {
                Console.WriteLine("ClientDisconnected"+e.ClientId);
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
