using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    class MirrorMsgImpl
    {

        public MirrorMsgImpl()
        {

        }
        public MirrorMsgImpl(byte FunctionCode,string CallId)
        {
            functionCode = FunctionCode;
            callId = CallId;
        }

        public byte functionCode;
        public long token;
        public ulong length;
        public byte[] data;

        public string callId; 
    }
}
