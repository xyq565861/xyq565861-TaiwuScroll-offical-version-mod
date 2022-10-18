using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    class TaiwuQueryImpl
    {
        public string assemblyStr;
        public string namespaceStr;
        public string classStr;
        public string methodStr;
        public LinkedList<object> args;

        public TaiwuQueryImpl()
        {
            args = new LinkedList<object>();
        }
    }
}
