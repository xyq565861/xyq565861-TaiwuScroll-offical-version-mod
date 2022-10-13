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
        public MirrorMsgImpl(byte functionCode)
        {
            FunctionCode = functionCode;
        }
        public byte FunctionCode { get; set; }
        public long Token { get; set; }
        public ulong Length { get; set; }
        public byte[] Data { get; set; }
        

    }
}
