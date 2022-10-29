using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class QueryMsg : MirrorMsg
    {

        public QueryMsg()
        {
            this.FunctionCode = (byte)MsgFunctionCode.query;
        }
        public override int MinimumFrameSize
        {
            get { return 1; }
        }
    }
}
