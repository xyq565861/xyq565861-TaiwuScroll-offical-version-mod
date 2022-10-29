using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class ErrorMsg : MirrorMsg
    {

        public ErrorMsg()
        {
            this.FunctionCode = (byte)MsgFunctionCode.error;
        }
        public override int MinimumFrameSize
        {
            get { return 1; }
        }
    }

}
