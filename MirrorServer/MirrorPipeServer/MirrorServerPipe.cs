using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace MirrorNet
{

    class MirrorServerPipe
    {
        System.Timers.Timer updataTimer;
        private readonly NamedPipeServerStream _pipeServer;
        private bool _isStopping;
        private readonly object _lockingObject = new object();
        private const int BufferSize = 2048;
        public readonly string Id;

        public PipeState state;
        public MirrorServerPipe(string pipeName, int maxNumberOfServerInstances)
        {
            state = PipeState.init;
            _pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxNumberOfServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            Id = Guid.NewGuid().ToString();
        }

        private class Info
        {
            public readonly byte[] Buffer;
            public readonly StringBuilder StringBuilder;
            public readonly List<byte> BufferBuilder;
            public Info()
            {
                Buffer = new byte[BufferSize];
                StringBuilder = new StringBuilder();
                BufferBuilder = new List<byte> ();
            }
        }






        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;
        public event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;


        public string ServerId
        {
            get { return Id; }
        }

        public void Start()
        {
            try
            {
                state = PipeState.wait;
                _pipeServer.BeginWaitForConnection(WaitForConnectionCallBack, null);
            }
            catch (Exception ex)
            {
                Debuglogger.Log(ex);
                throw;
            }
        }

        public void Stop()
        {
            _isStopping = true;

            try
            {
                if (_pipeServer.IsConnected)
                {
                    _pipeServer.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Debuglogger.Log(ex);
                throw;
            }
            finally
            {
                _pipeServer.Close();
                _pipeServer.Dispose();
            }
        }

        public Task<TaskResult> Send(byte[] buffer)
        {
            var taskCompletionSource = new TaskCompletionSource<TaskResult>();

            if (_pipeServer.IsConnected)
            {
                lock (_lockingObject)
                {
                    _pipeServer.BeginWrite(buffer, 0, buffer.Length, asyncResult =>
                    {
                        try
                        {
                            taskCompletionSource.SetResult(EndWriteCallBack(asyncResult));
                        }
                        catch (Exception ex)
                        {
                            taskCompletionSource.SetException(ex);
                        }

                    }, null);
                }


            }
            else
            {
                Debuglogger.Log("Cannot send message, pipe is not connected");
                throw new IOException("pipe is not connected");
            }

            return taskCompletionSource.Task;
        }

        private void BeginRead(Info info)
        {
            try
            {
                _pipeServer.BeginRead(info.Buffer, 0, BufferSize, EndReadCallBack, info);
            }
            catch (Exception ex)
            {
                Debuglogger.Log(ex);
                throw;
            }
        }

        private void WaitForConnectionCallBack(IAsyncResult result)
        {
            if (!_isStopping)
            {
                lock (_lockingObject)
                {
                    if (!_isStopping)
                    {

                        _pipeServer.EndWaitForConnection(result);
                        state = PipeState.connected;
                        Shakehands();
                        OnConnected();
                        updataTimer = new System.Timers.Timer(60 * 1000 * 5);
                        updataTimer.AutoReset = true;
                        updataTimer.Elapsed += UpdataTimer_Elapsed;
                        updataTimer.Start();
                        BeginRead(new Info());
                    }
                }
            }
        }
        private void Shakehands()
        {
            lock (_lockingObject)
            {
                if (_pipeServer.IsConnected)
                {
                    var taskCompletionSource = new TaskCompletionSource<TaskResult>();
                    var buffer = Encoding.UTF8.GetBytes(Id + "^*#^connect");
                    _pipeServer.BeginWrite(buffer, 0, buffer.Length, asyncResult =>
                    {
                        try
                        {
                            taskCompletionSource.SetResult(EndWriteCallBack(asyncResult));
                        }
                        catch (Exception ex)
                        {
                            taskCompletionSource.SetException(ex);
                        }

                    }, null);
                }
            }
        }
        private void EndReadCallBack(IAsyncResult result)
        {
            var readBytes = _pipeServer.EndRead(result);
            if (readBytes > 0)
            {
                var info = (Info)result.AsyncState;


                info.StringBuilder.Append(Encoding.UTF8.GetString(info.Buffer, 0, readBytes));
                info.BufferBuilder.AddRange(info.Buffer);
                if (!_pipeServer.IsMessageComplete)
                {
                    BeginRead(info);
                }
                else
                {

                    var message = info.StringBuilder.ToString().TrimEnd('\0');
                    if (message.Equals(Id+ "^*#^valid"))
                    {
                        if (state == PipeState.connected || state == PipeState.dead)
                        {
                            lock (_lockingObject)
                            {
                                if (state == PipeState.connected || state == PipeState.dead)
                                {
                                    if (updataTimer != null)
                                    {
                                        UilityTools.ResetTimer(updataTimer);

                                    }
                                    state =PipeState.validated;
                                }

                            }
                        }
                    }
                    else
                    {

                        if (state == PipeState.dead)
                        {


                            Send(Encoding.UTF8.GetBytes(Id + "^*#^dead"));//todo:
                            Shakehands();



                        }
                        else if (state == PipeState.validated)
                        {

                            if (updataTimer != null)
                            {
                                UilityTools.ResetTimer(updataTimer);
                                
                            }
                            if (message.Equals(Id + "^*#^tick"))
                            {
                                Send(Encoding.UTF8.GetBytes(Id + "^*#^tick"));//todo:
                            }
                            else
                            {
                                OnMessageReceived(info.BufferBuilder.ToArray(), Id);
                            }
                        }
                      


                      
                    }
                    BeginRead(new Info());
                }
            }
            else
            {
                if (!_isStopping)
                {
                    lock (_lockingObject)
                    {
                        if (!_isStopping)
                        {
                            OnDisconnected();
                            Stop();
                        }
                    }
                }
            }
        }
        private void UpdataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (state == PipeState.connected || state == PipeState.validated)
            {
                lock (_lockingObject)
                {
                    if (state ==PipeState.connected || state == PipeState.validated)
                    {
                        state =PipeState.dead;
                    }

                }
            }
        }
        
        private TaskResult EndWriteCallBack(IAsyncResult asyncResult)
        {

            _pipeServer.EndWrite(asyncResult);
            _pipeServer.Flush();

            return new TaskResult { IsSuccess = true };

        }

        private void OnMessageReceived(byte[] data,string clientId)
        {
            if (MessageReceivedEvent != null)
            {
                MessageReceivedEvent(this,
                    new MessageReceivedEventArgs
                    {   ClientId=clientId,
                        Data = data
                    });
            }
        }


        private void OnConnected()
        {
            if (ClientConnectedEvent != null)
            {
                ClientConnectedEvent(this, new ClientConnectedEventArgs { ClientId = Id });
            }
        }

        private void OnDisconnected()
        {
            if (ClientDisconnectedEvent != null)
            {
                ClientDisconnectedEvent(this, new ClientDisconnectedEventArgs { ClientId = Id });
            }
        }

    }

}
