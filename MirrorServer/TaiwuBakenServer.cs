
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class TaiwuBakenServer
    {
        int dictMaxSize = 100;
        private readonly string _pipeName;
        public readonly MirrorServer mirrorServer;
        public readonly IDictionary<string, ErrorMsg> errorDict;
        public readonly IDictionary<string, RetMsg> retDict;
        public readonly IDictionary<string, QueryMsg> queryDict;
        public TaiwuBakenServer(string pipeName)
        {
            _pipeName = pipeName;
            mirrorServer = new MirrorServer(_pipeName);
            queryDict = new ConcurrentDictionary<string, QueryMsg>();
            retDict = new ConcurrentDictionary<string, RetMsg>();
            errorDict = new ConcurrentDictionary<string, ErrorMsg>();
        }

        public void Start()
        {
            mirrorServer.ServerMessageReceivedEvent += ServerMessageReceived;
            mirrorServer.ServerClientConnectedEvent += ClientConnectedEvent;
            mirrorServer.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
            mirrorServer.Start();
            Debuglogger.Log("mirrorServer Start");
        }
        public void Stop()
        {
            mirrorServer.ServerMessageReceivedEvent -= ServerMessageReceived;
            mirrorServer.ServerClientConnectedEvent -= ClientConnectedEvent;
            mirrorServer.ServerClientDisconnectedEvent -= ClientDisconnectedEvent;
            mirrorServer.Stop();

        }
        private void ServerMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e != null)
            {
                var Server= sender as MirrorServer;
                if (Server != null)
                {
                    MirrorMsg msg = new MirrorMsg();
                    try
                    {
                        msg.TryFormate(e.Data);
                        switch (msg.FunctionCode)
                        {

                            case (byte)MsgFunctionCode.error:
                                break;
                            case (byte)MsgFunctionCode.query:
                                QueryMsg queryMsg=new QueryMsg();
                                try
                                {
                                    queryMsg.TryFormate(e.Data);
                                    if (queryMsg != null)
                                    {
                                        object result=QueryCall(queryMsg);

                                        RetMsg retMsg = new RetMsg();
                                        retMsg.Initialize(result, queryMsg.CallId);
                                        Server.Send(retMsg.ProtocolDataUnit, e.ClientId);
                                    }
                                    else
                                    {
                                        throw new SystemException("Massage have queryMsg functionCode but not match the queryMsg formate");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrorMsg errorMsg = new ErrorMsg();
                                    string str = ex.Message + "\n" + ex.StackTrace;
                                    errorMsg.Initialize(str, msg.CallId);
                                    Server.Send(errorMsg.ProtocolDataUnit, e.ClientId);

                                }


                                break;
                            case (byte)MsgFunctionCode.ret:
                                break;
                            case (byte)MsgFunctionCode.asyncQuery:
                                break;
                            case (byte)MsgFunctionCode.asyncRet:
                                break;
                            default:
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debuglogger.Log(ex.Message);

                    }
                    Debuglogger.Log(msg.Massage);


                }
            }

        }

        private static object QueryCall(QueryMsg queryMsg)
        {

            TaiwuQuery taiwuQuery = new TaiwuQuery(queryMsg.CallId);
            //taiwuQuery.TryFormate(queryMsg.Massage);
            taiwuQuery.TryFormate(queryMsg.Data);
            Debuglogger.Log(string.Format("m_Assembly{0}--NamespaceStr{1}--ClassStr{2}--MethodStr{3}--parameters{4}", taiwuQuery.Assemblystr, taiwuQuery.NamespaceStr, taiwuQuery.ClassStr, taiwuQuery.MethodStr, taiwuQuery.Args.ToString()));
            Assembly m_Assembly=null;


            if (taiwuQuery.Assemblystr.Equals("GameData"))
            {
                m_Assembly = Assembly.Load(taiwuQuery.Assemblystr);
            }
            else
            {
                foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
                {
                    
                    if (item.GetName().Name.Equals(taiwuQuery.Assemblystr))
                    {
                        m_Assembly = item;
                    }

                }
            }

            Assembly a_Assembly = Assembly.GetExecutingAssembly();
            Debuglogger.Log(a_Assembly.GetName());
            if (m_Assembly == null)
                throw new Exception("Backend can't find the target Assembly");
            Type t = m_Assembly.GetType(taiwuQuery.NamespaceStr + "." + taiwuQuery.ClassStr);
            if (t == null)
                throw new Exception("Backend can't find the target class");
            System.Object obj = Activator.CreateInstance(t);
            MethodInfo method = t.GetMethod(taiwuQuery.MethodStr);
            if (method == null)
                throw new Exception("Backend can't find the target method");
            BindingFlags flag = BindingFlags.Static | BindingFlags.IgnoreCase;
            //ParameterInfo[] paramInfos = method.GetParameters();
            //LinkedList<object> ts = new LinkedList<object>();
            object[] parameters = taiwuQuery.Args.ToArray();
            object returnValue = method.Invoke(obj, flag, Type.DefaultBinder, parameters, null);
            return returnValue;
        }

        private void ClientDisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            if (e != null)
            {
                Debuglogger.Log("ClientDisconnected" + e.ClientId);
            }
        }
         private void ClientConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            if (e != null)
            {
                Debuglogger.Log("ClientConnected" + e.ClientId);
            }
        }
    }
}
