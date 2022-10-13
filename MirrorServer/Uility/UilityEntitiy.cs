using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MirrorNet
{
    public class ClientConnectedEventArgs : EventArgs
    {
        public string ClientId { get; set; }
    }

    public class ClientDisconnectedEventArgs : EventArgs
    {
        public string ClientId { get; set; }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string ClientId { get; set; }

        public byte[] Data { get; set; }
    }
    public class TaskResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
    public enum PipeState
    {
        init = 0,
        wait = 1,
        connected = 2,
        validated = 3,
        dead = 4
    }
    public enum MsgFunctionCode
    {
        error = 0,
        query = 1,
        ret = 2,
        asyncQuery = 3,
        asyncRet = 4
    }
}
