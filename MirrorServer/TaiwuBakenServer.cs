using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class TaiwuBakenServer
    {
        private readonly string _pipeName;
        public readonly MirrorServer mirrorServer;
        public TaiwuBakenServer(string pipeName)
        {
            _pipeName = pipeName;
            mirrorServer = new MirrorServer(_pipeName);
            mirrorServer.ServerMessageReceivedEvent += ServerMessageReceived;
            mirrorServer.ServerClientConnectedEvent += ClientConnectedEvent;
            mirrorServer.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
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
                        switch(msg.FunctionCode){

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
