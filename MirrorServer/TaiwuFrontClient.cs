using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class TaiwuFrontClient
    {
        int dictMaxSize = 50;
        public readonly IDictionary<string, ErrorMsg> errorDict;
        public readonly IDictionary<string, RetMsg> retDict;
        public readonly IDictionary<string, QueryMsg> queryDict;
        private readonly string _pipeName;
        public readonly MirrorClient mirrorClient;
        public TaiwuFrontClient(string pipeName)
        {
            _pipeName = pipeName;
            mirrorClient = new MirrorClient(_pipeName);
            queryDict = new ConcurrentDictionary<string, QueryMsg>();
            retDict = new ConcurrentDictionary<string, RetMsg>();
            errorDict = new ConcurrentDictionary<string, ErrorMsg>();
        }
        public void Start()
        {
            mirrorClient.ServerMessageReceivedEvent += ServerMessageReceived;
            mirrorClient.ServerClientConnectedEvent += ClientConnectedEvent;
            mirrorClient.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
            mirrorClient.Start();

        }
        public void Stop()
        {
            mirrorClient.ServerMessageReceivedEvent -= ServerMessageReceived;
            mirrorClient.ServerClientConnectedEvent -= ClientConnectedEvent;
            mirrorClient.ServerClientDisconnectedEvent -= ClientDisconnectedEvent;
            mirrorClient.Stop();

        }
        public object Query(string Assemblystr, string NamespaceStr, string ClassStr, string MethodStr, List<object> Agrs,int waitTime=5)
        {
            object result=null;
            TaiwuQuery taiwuQuery = new TaiwuQuery();
            taiwuQuery.Initialize(Assemblystr,NamespaceStr, ClassStr, MethodStr, Agrs);

            QueryMsg queryMsg = new QueryMsg();
            queryMsg.Initialize(taiwuQuery.ProtocolDataUnit, taiwuQuery.id);
            byte[] buffer = queryMsg.ProtocolDataUnit;
            if(queryDict.Count> dictMaxSize)
            {
                throw new InvalidOperationException("too  many Query, pipe blocked ");
            }
            if (mirrorClient.Send(buffer).IsSuccess)
            {
                string callId = queryMsg.CallId;
                queryDict[callId] = queryMsg;
                try
                {
                    bool waitingFlag = true;
                    for (int i = 0; i < waitTime*2; i++)
                    {
                        if (retDict.ContainsKey(callId))
                        {
                            string retdata = retDict[callId].Massage;
                            result = JsonConvert.DeserializeObject(retdata);
                            retDict.Remove(callId);
                            waitingFlag = false;

                            break;
                        }
                        if (errorDict.ContainsKey(callId))
                        {
                            string errdata = errorDict[callId].Massage;

                            errorDict.Remove(callId);
                            waitingFlag = false;
                            throw new SystemException(string.Format("Query {0} faill , error:{1}", callId, errdata));

                        }
                        Thread.Sleep(500);
                    }
                    if (waitingFlag)
                    {
                        throw new TimeoutException(string.Format("Query {0} faill , time out", callId));
                    }


                }
                finally
                {
                    queryDict.Remove(callId);
                }
                
                return result;
            }
            else
            {
                throw new InvalidOperationException(string.Format("Communication error, fail to query"));
            }



        }
        private void ServerMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e != null)
            {
                var Client = sender as MirrorClient;
                if (Client != null)
                {
                    MirrorMsg msg = new MirrorMsg();
                    try
                    {
                        msg.TryFormate(e.Data);
                        switch (msg.FunctionCode)
                        {

                            case (byte)MsgFunctionCode.error :
                                ErrorMsg errMsg = new ErrorMsg();

                                errMsg.TryFormate(e.Data);
                                if (errMsg!=null)
                                {

                                    if (queryDict.ContainsKey(errMsg.CallId))
                                    {
                                        if (errorDict.Count > dictMaxSize)
                                        {
                                            throw new InvalidOperationException("too  many Error, pipe blocked ");
                                        }
                                        errorDict[errMsg.CallId] = errMsg;
                                    }
                                }
                                

                                break;
                            case (byte)MsgFunctionCode.query:
                                break;
                            case (byte)MsgFunctionCode.ret:

                                RetMsg retMsg = new RetMsg();
                                retMsg.TryFormate(e.Data);
                                if (retMsg != null)
                                {

                                    if (queryDict.ContainsKey(retMsg.CallId))
                                    {
                                        if (retDict.Count > dictMaxSize)
                                        {
                                            throw new InvalidOperationException("too  many Ret, pipe blocked ");
                                        }
                                        retDict[retMsg.CallId] = retMsg;
                                    }
                                }

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
