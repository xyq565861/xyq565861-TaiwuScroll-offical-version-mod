using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class AsyncRetMsg : MirrorMsg
    {

        public AsyncRetMsg()
        {
            this.FunctionCode = (byte)MsgFunctionCode.asyncRet;
        }
        public override int MinimumFrameSize
        {
            get { return 1; }
        }
    }
}
