using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MirrorNet.MirrorClientPipe;

namespace MirrorNet
{
    public class MirrorClient
    {
        private readonly string _pipeName;
        private readonly SynchronizationContext _synchronizationContext;
        private MirrorClientPipe _pipe;

        public string pipeid {
            get { return _pipe.Id; }

        }



        public event EventHandler<ClientConnectedEventArgs> ServerClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ServerClientDisconnectedEvent;
        public event EventHandler<MessageReceivedEventArgs> ServerMessageReceivedEvent;
        public MirrorClient(string serverId)
        {
            _synchronizationContext = AsyncOperationManager.SynchronizationContext;
            _pipeName = serverId;
            _pipe = new MirrorClientPipe(_pipeName);
        }

        public void Start()
        {
            _pipe.ClientConnectedEvent += ClientConnectedHandler;
            _pipe.ClientDisconnectedEvent += ClientDisconnectedHandler;
            _pipe.MessageReceivedEvent += MessageReceivedHandler;
            _pipe.Start();

        }


        public void Stop()
        {
            try
            {

                UnregisterFromServerEvents(_pipe);
                _pipe.Stop();

            }
            catch (Exception e)
            {
                Debuglogger.Log("Fialed to stop pipe");

            }
            finally
            {
                _pipe = new MirrorClientPipe(_pipeName);
            }
        }


        public TaskResult Send(byte[] buffer)
        {

            if (_pipe.state == PipeState.init)
            {
                Start();
                for(var i = 0; i < 50; i++)
                {
                    if (_pipe.state == PipeState.validated)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                
            }
            TaskResult result;

                result = _pipe.Send(buffer).Result;


            return result;

        }


        public TaskResult SendMassage(string massage)
        {
            var buffer = Encoding.UTF8.GetBytes(massage);
            return Send(buffer);
        }

        private void UnregisterFromServerEvents(MirrorClientPipe pipe)
        {
            pipe.ClientConnectedEvent -= ClientConnectedHandler;
            pipe.ClientDisconnectedEvent -= ClientDisconnectedHandler;
            pipe.MessageReceivedEvent -= MessageReceivedHandler;
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


        private void ClientConnectedHandler(object sender, ClientConnectedEventArgs eventArgs)
        {
            OnClientConnected(eventArgs);


        }


        private void ClientDisconnectedHandler(object sender, ClientDisconnectedEventArgs eventArgs)
        {
            OnClientDisconnected(eventArgs);

            Stop();

        }


        private void MessageReceivedHandler(object sender, MessageReceivedEventArgs eventArgs)
        {
            OnMessageReceived(eventArgs);
        }


    }
}