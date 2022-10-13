using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public class TaiwuQuery
    {
        private TaiwuQueryImpl _taiwuQueryImpl;
        public string NamespaceStr { get { return _taiwuQueryImpl.namespaceStr; } private set { _taiwuQueryImpl.namespaceStr = value; } }
        public string ClassStr { get { return _taiwuQueryImpl.classStr; } private set { _taiwuQueryImpl.classStr = value; } }
        public string MethodStr { get { return _taiwuQueryImpl.methodStr; } private set { _taiwuQueryImpl.methodStr = value; } }
        public string Callname { get { return string.Format("{0}.{1}.{2}", NamespaceStr, ClassStr, MethodStr); } }
        public LinkedList<object> Args { get { return _taiwuQueryImpl.args; } private set { _taiwuQueryImpl.args = value; } }

        public TaiwuQuery()
        {
            _taiwuQueryImpl = new TaiwuQueryImpl();
        }

        public void Initialize(string NamespaceStr, string ClassStr, string MethodStr, List<object> agrs)
        {
            _taiwuQueryImpl.namespaceStr = NamespaceStr;
            _taiwuQueryImpl.classStr = ClassStr;
            _taiwuQueryImpl.methodStr = MethodStr;

            foreach (var item in agrs)
            {
                if (item != null && item.GetType().IsSerializable)
                {
                    _taiwuQueryImpl.args.Append(item);
                    continue;
                }
                throw new FormatException(String.Format("type of {0} can not be Serializable", item));

            }
        }

        public string ProtocolDataUnit()
        {

            string str = NamespaceStr;
            str += "_+_";
            str += ClassStr;
            str += "_+_";
            str += MethodStr;
            str += "_+_";
            str += JsonConvert.SerializeObject(Args);
            str = '!' + str + '!';
            return str;


        }

        public void TryFormate(string dataStr)
        {
            if (!dataStr.StartsWith('!') || !dataStr.EndsWith('!'))
            {
                throw new FormatException(String.Format("DataStr formate error :{0}", dataStr));
            }
            string str = dataStr.Trim('!');
            string[] strs = str.Split("_+_");

            if (strs.Length < 4)
            {
                throw new FormatException(String.Format("DataStr deserialize error :{0}", dataStr));
            }
            else
            {
                LinkedList<object> ts = JsonConvert.DeserializeObject(strs[3]) as LinkedList<object>;

                if (ts == null)
                {
                    throw new FormatException(String.Format("DataStr args deserialize error :{0}", dataStr));

                }
                else
                {
                    NamespaceStr = strs[0];
                    ClassStr = strs[1];
                    MethodStr = strs[2];
                    Args = ts;

                }
                
            }

        }
    }
}
