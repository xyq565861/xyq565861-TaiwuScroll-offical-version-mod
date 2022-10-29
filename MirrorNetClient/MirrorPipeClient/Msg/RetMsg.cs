using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class RetMsg : MirrorMsg
    {

        public RetMsg()
        {
            this.FunctionCode = (byte)MsgFunctionCode.ret;
        }
        public override int MinimumFrameSize
        {
            get { return 1; }
        }
    }

}
