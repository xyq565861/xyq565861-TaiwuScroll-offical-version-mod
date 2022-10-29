
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class TaiwuFrontClient
    {
        bool starting;
        int dictMaxSize = 50;
        public readonly IDictionary<string, ErrorMsg> errorDict;
        public readonly IDictionary<string, RetMsg> retDict;
        public readonly IDictionary<string, QueryMsg> queryDict;
        public readonly IDictionary<string, QueryMsg> queryDictTemp;
        private readonly string _pipeName;
        public readonly MirrorClient mirrorClient;
        private SynchronizationContext _synchronizationContext;
        Thread clientThread;
        public TaiwuFrontClient(string pipeName)
        {
            _synchronizationContext = AsyncOperationManager.SynchronizationContext;
            starting = false;
            _pipeName = pipeName;
            mirrorClient = new MirrorClient(_pipeName);
            queryDict = new ConcurrentDictionary<string, QueryMsg>();
            retDict = new ConcurrentDictionary<string, RetMsg>();
            errorDict = new ConcurrentDictionary<string, ErrorMsg>();
        }
        public event EventHandler<ClientSendEventArgs> ClientSend;
        public event EventHandler<EventArgs> ClientStop;
        AutoResetEvent threadOne = new AutoResetEvent(false);
        //AutoResetEvent threadTwo = new AutoResetEvent(false);
        public void Start()
        {
            ThreadStart threadStart = new ThreadStart(() =>
            {
               
                Debuglogger.Log("mirrorClient Starting");
                starting = true;
                mirrorClient.ServerMessageReceivedEvent += ServerMessageReceived;
                mirrorClient.ServerClientConnectedEvent += ClientConnectedEvent;
                mirrorClient.ServerClientDisconnectedEvent += ClientDisconnectedEvent;
                mirrorClient.Start();
                ClientSend += ClientSendEvent;
                ClientStop += StopEvent;
                Debuglogger.Log("mirrorClient Start");
                while (true)
                {
                    threadOne.WaitOne();
                    if (queryDictTemp.Count > 0)
                    {
                        foreach (var item in queryDictTemp)
                        {
                            try
                            {
                                Send(item.Value);
                            }
                            catch (Exception ex)
                            {

                                Debuglogger.Log("send " + item.Key + " faile");
                                Debuglogger.Log(ex.Message);
                            }
                            finally
                            {
                                queryDictTemp.Remove(item.Key);
                            }

                        }
                    }
                }
            });
            clientThread = new Thread(threadStart);
            clientThread.Start();
           
          
        }

        public void Stop()
        {
            if (_synchronizationContext != null)
            {


                _synchronizationContext.Post(e => ClientStop.Invoke(this, (EventArgs)e), new EventArgs());
            }
            if (clientThread!=null)
            {
                try
                {
                    clientThread.Abort();
                }
                catch
                {

                }
            }
        }

        public void ToStop()
        {
            ClientSend -= ClientSendEvent;
            mirrorClient.ServerMessageReceivedEvent -= ServerMessageReceived;
            mirrorClient.ServerClientConnectedEvent -= ClientConnectedEvent;
            mirrorClient.ServerClientDisconnectedEvent -= ClientDisconnectedEvent;
            mirrorClient.Stop();
            ClientStop -= StopEvent;
        }
        public void Send(QueryMsg queryMsg)
        {


          
            byte[] buffer = queryMsg.ProtocolDataUnit;
            if (queryDict.Count > dictMaxSize)
            {
                throw new InvalidOperationException("too  many Query, pipe blocked ");
            }

            if (mirrorClient.Send(buffer).IsSuccess)
            {
                string callId = queryMsg.CallId;
                queryDict[callId] = queryMsg;

            }


        }
        public object Query(string Assemblystr, string NamespaceStr, string ClassStr, string MethodStr, List<object> Agrs, int waitTime = 5)
        {

            object result = null;
            TaiwuQuery taiwuQuery = new TaiwuQuery();
            taiwuQuery.Initialize(Assemblystr, NamespaceStr, ClassStr, MethodStr, Agrs);
            Debuglogger.Log("A");

            QueryMsg queryMsg = new QueryMsg();

            queryMsg.Initialize(taiwuQuery.ProtocolDataUnit(), taiwuQuery.id);
            Debuglogger.Log("B");
            byte[] buffer = queryMsg.ProtocolDataUnit;
            if (queryDictTemp.Count > dictMaxSize)
            {
                throw new InvalidOperationException("too  many TempQuery, pipe blocked ");
            }
            string callId = queryMsg.CallId;

            queryDictTemp[callId] = queryMsg;
            threadOne.Set();
            // if (_synchronizationContext != null)
            //_synchronizationContext.Post(e => ClientSend.Invoke(this, (ClientSendEventArgs)e),
            //new ClientSendEventArgs() { DataBuffer = buffer });
            //ClientSend.Invoke(this, new ClientSendEventArgs() { DataBuffer = buffer });
            try
                {
                    bool waitingFlag = true;
                    for (int i = 0; i < waitTime * 2; i++)
                    {
                        if (retDict.ContainsKey(callId))
                        {
                            result = UilityTools.ByteArrayToObject(retDict[callId].Data);
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

                            case (byte)MsgFunctionCode.error:
                                ErrorMsg errMsg = new ErrorMsg();

                                errMsg.TryFormate(e.Data);
                                if (errMsg != null)
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
            Stop();
            Thread.Sleep(1000);
            Start();
        }
        private void ClientConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            if (e != null)
            {
                Debuglogger.Log("ClientConnected" + e.ClientId);
            }
        }
        private void ClientSendEvent(object sender, ClientSendEventArgs eventArgs)
        {
            //Send(eventArgs.Assemblystr, eventArgs.NamespaceStr, eventArgs.ClassStr, eventArgs.MethodStr, eventArgs.Agrs, eventArgs.waitTime);
            mirrorClient.Send(eventArgs.DataBuffer);
        }
        private void StopEvent(object sender, EventArgs eventArgs)
        {
            ToStop();

        }
        public class ClientQueryMsg
        {
            public string Assemblystr;
            public string NamespaceStr;
            public string ClassStr;
            public string MethodStr;
            public List<object> Agrs;
            public int waitTime = 5;
        }
    }
}
