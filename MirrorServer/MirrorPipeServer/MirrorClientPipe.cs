using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MirrorNet
{
    class MirrorClientPipe
    {
        private readonly NamedPipeClientStream _pipe;

        private readonly object _lockingObject = new object();
        private const int BufferSize = 2048;
        private bool _isStopping;
        private List<byte[]> sendList;
        public string Id { get; private set; }
        public PipeState state;
        public MirrorClientPipe(string serverId)
        {
            state = PipeState.init;
            _pipe = new NamedPipeClientStream(".", serverId, PipeDirection.InOut, PipeOptions.Asynchronous );
            sendList = new List<byte[]>();


        }
        public void Start()
        {
            lock (_lockingObject)
            {
                state = PipeState.wait;
                const int tryConnectTimeout = 1000 * 60 *5; // 5min
                _pipe.Connect(tryConnectTimeout);
                _pipe.ReadMode = PipeTransmissionMode.Message;
                state = PipeState.connected;
                OnConnected();
                BeginRead(new Info());
            }

        }
        public event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;
        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        private void Shakehands()
        {
            if (_pipe.IsConnected)
            {

                lock (_lockingObject)
                {
                    var taskCompletionSource = new TaskCompletionSource<TaskResult>();
                    var buffer = Encoding.UTF8.GetBytes(Id + "^*#^valid");
                    _pipe.BeginWrite(buffer, 0, buffer.Length, asyncResult =>
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
            var readBytes = _pipe.EndRead(result);
            if (readBytes > 0)
            {
                var info = (Info)result.AsyncState;


                info.StringBuilder.Append(Encoding.UTF8.GetString(info.Buffer, 0, readBytes));
                info.BufferBuilder.AddRange(info.Buffer);
                if (!_pipe.IsMessageComplete)
                {
                    BeginRead(info);
                }
                else
                {

                    var message = info.StringBuilder.ToString().TrimEnd('\0');
                    if (message.Contains("^*#^"))
                    {
                        string[] strings = message.Split("^*#^");
                        if (strings.Length > 1)
                        {
                            switch (strings[1])
                            {
                                case "connect":
                                    lock (_lockingObject)
                                    {

                                        Id = strings[0];
                                        Shakehands();
                                        state = PipeState.validated;
                                        foreach (var bufferitem in sendList)
                                        {
                                            var taskCompletionSource = new TaskCompletionSource<TaskResult>();

                                            if (_pipe.IsConnected)
                                            {

                                                var tickbuffer = Encoding.UTF8.GetBytes(Id + "^*#^tick");
                                                _pipe.BeginWrite(bufferitem, 0, bufferitem.Length, asyncResult =>
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
                                        sendList.Clear();

                                    }
                                    break;

                                case "dead":
                                    lock (_lockingObject)
                                    {

                                        if (strings[0].Equals(Id))
                                        {
                                            state = PipeState.dead;
                                        }

                                    }
                                    break;
                                case "tick":
                                    lock (_lockingObject)
                                    {

                                        if (strings[0].Equals(Id))
                                        {
                                            foreach(var bufferitem in sendList)
                                            {
                                                var taskCompletionSource = new TaskCompletionSource<TaskResult>();

                                                if (_pipe.IsConnected)
                                                {
                                                   
                                                    var tickbuffer = Encoding.UTF8.GetBytes(Id + "^*#^tick");
                                                    _pipe.BeginWrite(bufferitem, 0, bufferitem.Length, asyncResult =>
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
                                            sendList.Clear();
                                        }
                                            

                                    }
                                    break;
                                default: break;
                            }

                        }
                    }
                    else
                    {

                        OnMessageReceived(info.BufferBuilder.ToArray(), Id);

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

        private void BeginRead(Info info)
        {
            try
            {
                _pipe.BeginRead(info.Buffer, 0, BufferSize, EndReadCallBack, info);
            }
            catch (Exception ex)
            {
                Debuglogger.Log(ex);
                throw;
            }
        }
        public Task<TaskResult> Send(byte[] buffer)
        {
            var taskCompletionSource = new TaskCompletionSource<TaskResult>();

            if (_pipe.IsConnected)
            {
                lock (_lockingObject)
                {
                    sendList.Add(buffer);
                    var tickbuffer = Encoding.UTF8.GetBytes(Id + "^*#^tick");
                    _pipe.BeginWrite(buffer, 0, buffer.Length, asyncResult =>
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

        public void Stop()
        {
            try
            {
                _pipe.WaitForPipeDrain();
            }
            catch(Exception e)
            {
                Debuglogger.Log("Cannot Stop pipe, pipe is closed");
            }
            finally
            {
                _pipe.Close();
                _pipe.Dispose();
            }
        }

        private TaskResult EndWriteCallBack(IAsyncResult asyncResult)
        {
            _pipe.EndWrite(asyncResult);
            _pipe.Flush();

            return new TaskResult { IsSuccess = true };
        }


        private void OnMessageReceived(byte[] data, string clientId)
        {
            if (MessageReceivedEvent != null)
            {
                MessageReceivedEvent(this,
                    new MessageReceivedEventArgs
                    {
                        ClientId = clientId,
                        Data = data,
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
        private class Info
        {
            public readonly byte[] Buffer;
            public readonly StringBuilder StringBuilder;
            public readonly List<byte> BufferBuilder;
            public Info()
            {
                Buffer = new byte[BufferSize];
                StringBuilder = new StringBuilder();
                BufferBuilder = new List<byte>();
            }
        }

    }

}
