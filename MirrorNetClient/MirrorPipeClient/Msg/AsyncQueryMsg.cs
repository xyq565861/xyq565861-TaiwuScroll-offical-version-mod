using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class AsyncQueryMsg : MirrorMsg
    {

        public AsyncQueryMsg()
        {
            this.FunctionCode = (byte)MsgFunctionCode.asyncQuery;
        }
        public override int MinimumFrameSize
        {
            get { return 1; }
        }
    }

}
