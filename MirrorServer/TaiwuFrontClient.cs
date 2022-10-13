using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class TaiwuFrontClient
    {
        public readonly IDictionary<long, MirrorMsg> msDict;
        private readonly string _pipeName;
        public readonly MirrorClient mirrorClient;
        public TaiwuFrontClient(string pipeName)
        {
            _pipeName = pipeName;
            mirrorClient = new MirrorClient(_pipeName);
            msDict = new ConcurrentDictionary<long, MirrorMsg>();
        }
        public void Start()
        {
            mirrorClient.ServerMessageReceivedEvent += ServerMessageReceived;
            mirrorClient.ServerClientConnectedEvent += ClientConnectedEvent;
            mirrorClient.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
            mirrorClient.Start();

        }

        //public object Query(string callname, Dictionary<string, object>  args)
        //{
        //    JsonConvert.SerializeObject
        //}
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
                        switch (msg.FunctionCode)
                        {

                            case 1:
                                break;
                            default:
                                break;
                        }
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
