

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class MirrorServer
    {

        

        private readonly string _pipeName;
        private readonly SynchronizationContext _synchronizationContext;
        private readonly IDictionary<string, MirrorServerPipe> _pipes; // ConcurrentDictionary is thread safe
        private readonly int _maxNumberOfServerInstances = 50;

        public MirrorServer(string pipeName)
        {
            _pipeName = pipeName;
            _synchronizationContext = AsyncOperationManager.SynchronizationContext;
            _pipes = new ConcurrentDictionary<string, MirrorServerPipe>();
        }
        public MirrorServer(string pipeName, int maxNumberOfServerInstances)
        {
            _pipeName = pipeName;
            _maxNumberOfServerInstances = maxNumberOfServerInstances;             
            _synchronizationContext = AsyncOperationManager.SynchronizationContext;
            _pipes = new ConcurrentDictionary<string, MirrorServerPipe>();
        }

        public event EventHandler<MessageReceivedEventArgs> ServerMessageReceivedEvent;
        public event EventHandler<ClientConnectedEventArgs> ServerClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ServerClientDisconnectedEvent;


        public string ServerId
        {
            get { return _pipeName; }
        }

        public void Start()
        {
            StartNamedPipeServer();
            Debuglogger.Log("Server start at" + _pipeName);
        }

        public void Stop()
        {
            foreach (var pipeitem in _pipes.Values)
            {
                try
                {
                    UnregisterFromServerEvents(pipeitem);
                    pipeitem.Stop();
                }
                catch (Exception e)
                {
                    Debuglogger.Log("Fialed to Stop pipe" + pipeitem.Id);
                    Debuglogger.Log("cause" + e.Message);
                    Debuglogger.Log("cause" + e.StackTrace);

                }
            }

            _pipes.Clear();
        }

        public IDictionary<string, TaskResult> SendAll( byte[] buffer)
        {
            IDictionary<string, TaskResult> resultDict=new  ConcurrentDictionary<string, TaskResult>();
            foreach (var pipeitem in _pipes.Values)
            {
                try
                {
                    resultDict[pipeitem.Id]= pipeitem.Send(buffer).Result;
                }
                catch (Exception e)
                {
                    Debuglogger.Log("Fialed to Send buffer pipe" + pipeitem.Id);
                    Debuglogger.Log("cause" + e.Message);
                    Debuglogger.Log("cause" + e.StackTrace);


                }
            }
            return resultDict;
        }

        public  TaskResult Send(byte[] buffer ,string clientId)
        {
            TaskResult result;
            if (_pipes.ContainsKey(clientId))
            {
                result = _pipes[clientId].Send(buffer).Result;

            }
            else
            {
                var taskResult = new TaskResult();
                taskResult.ErrorMessage = "unkonw pipe";
                taskResult.IsSuccess = false;
                result = taskResult;
                Debuglogger.Log("unkonw pipe" + clientId);
                
            }
            return result;

        }

        public IDictionary<string, TaskResult> SendAllMassage(string massage)
        {
            var buffer = Encoding.UTF8.GetBytes(massage);
            return SendAll(buffer);
        }
        public TaskResult SendMassage(string massage, string clientId)
        {
            var buffer = Encoding.UTF8.GetBytes(massage);
            return Send(buffer, clientId);
        }
        private void StartNamedPipeServer()
        {
            if (_pipes != null && _pipes.Count > 0)
            {
                foreach (var pipeitem in _pipes.Values)
                {
                    if (pipeitem.state == PipeState.dead)
                    {
                        try
                        {

                            pipeitem.Stop();


                        }
                        catch (Exception e)
                        {
                            Debuglogger.Log("Fialed to Send buffer pipe" + pipeitem.Id);
                            Debuglogger.Log("cause" + e.Message);
                            Debuglogger.Log("cause" + e.StackTrace);
                        }
                        finally
                        {
                            _pipes.Remove(pipeitem.Id);
                        }
                    }
                }

            }
            var pipe = new MirrorServerPipe(_pipeName, _maxNumberOfServerInstances);
            _pipes[pipe.Id] = pipe;

            pipe.ClientConnectedEvent += ClientConnected;
            pipe.ClientDisconnectedEvent += ClientDisconnected;
            pipe.MessageReceivedEvent += MessageReceived;

            pipe.Start();
        }


        private void StopNamedPipeServer(string id)
        {
            UnregisterFromServerEvents(_pipes[id]);
            _pipes[id].Stop();
            _pipes.Remove(id);
        }


        private void UnregisterFromServerEvents(MirrorServerPipe pipe)
        {
            pipe.ClientConnectedEvent -= ClientConnected;
            pipe.ClientDisconnectedEvent -= ClientDisconnected;
            pipe.MessageReceivedEvent -= MessageReceived;
        }

        private void OnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            _synchronizationContext.Post(e => ServerMessageReceivedEvent.Invoke(this, (MessageReceivedEventArgs)e),
                eventArgs);
        }


        private void OnClientConnected(ClientConnectedEventArgs eventArgs)
        {
            _synchronizationContext.Post(e => ServerClientConnectedEvent.Invoke(this, (ClientConnectedEventArgs)e),
                eventArgs);
        }


        private void OnClientDisconnected(ClientDisconnectedEventArgs eventArgs)
        {
            _synchronizationContext.Post(
                e => ServerClientDisconnectedEvent.Invoke(this, (ClientDisconnectedEventArgs)e), eventArgs);
        }


        private void ClientConnected(object sender, ClientConnectedEventArgs eventArgs)
        {
            OnClientConnected(eventArgs);

            StartNamedPipeServer(); // Create another server as a preparation for new connection
        }


        private void ClientDisconnected(object sender, ClientDisconnectedEventArgs eventArgs)
        {
            OnClientDisconnected(eventArgs);

            StopNamedPipeServer(eventArgs.ClientId); 
        }


        private void MessageReceived(object sender, MessageReceivedEventArgs eventArgs)
        {
            OnMessageReceived(eventArgs);
        }

    }
}
