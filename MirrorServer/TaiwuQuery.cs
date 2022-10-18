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
        public string Assemblystr { get { return _taiwuQueryImpl.assemblyStr; } private set { _taiwuQueryImpl.assemblyStr = value; } }
        public string NamespaceStr { get { return _taiwuQueryImpl.namespaceStr; } private set { _taiwuQueryImpl.namespaceStr = value; } }
        public string ClassStr { get { return _taiwuQueryImpl.classStr; } private set { _taiwuQueryImpl.classStr = value; } }
        public string MethodStr { get { return _taiwuQueryImpl.methodStr; } private set { _taiwuQueryImpl.methodStr = value; } }
        public string Callname { get { return string.Format("{0}.{1}.{2}", NamespaceStr, ClassStr, MethodStr); } }
        public LinkedList<object> Args { get { return _taiwuQueryImpl.args; } private set { _taiwuQueryImpl.args = value; } }
        public string id;
        public TaiwuQuery()
        {
            _taiwuQueryImpl = new TaiwuQueryImpl();
            id = Guid.NewGuid().ToString();
        }
        public TaiwuQuery(string callId)
        {
            _taiwuQueryImpl = new TaiwuQueryImpl();
            id = callId;
        }
        public void Initialize(string Assemblystr,string NamespaceStr, string ClassStr, string MethodStr, List<object> agrs)
        {


            foreach (var item in agrs)
            {
                if (item != null && item.GetType().IsSerializable&& (item as object)!=null)
                {
                    _taiwuQueryImpl.args.AddLast(item as object);
                    continue;
                }
                throw new ArgumentNullException("agrs", String.Format("type of {0} can not be Serializable", item));

            }
            _taiwuQueryImpl.assemblyStr = Assemblystr;
            _taiwuQueryImpl.namespaceStr = NamespaceStr;
            _taiwuQueryImpl.classStr = ClassStr;
            _taiwuQueryImpl.methodStr = MethodStr;
        }

        public string ProtocolDataUnit
        {
            get
            {
                string str = "";
                str += Assemblystr;
                str += "_+_";
                str += NamespaceStr;
                str += "_+_";
                str += ClassStr;
                str += "_+_";
                str += MethodStr;
                str += "_+_";
                str += id;
                str += "_+_";
                str += JsonConvert.SerializeObject(Args);
                str = '!' + str + '!';
                return str;
            }




        }

        public void TryFormate(string dataStr)
        {
            if (!dataStr.StartsWith('!') || !dataStr.EndsWith('!'))
            {
                throw new FormatException(String.Format("DataStr formate error :{0}", dataStr));
            }
            string str = dataStr.Trim('!');
            string[] strs = str.Split("_+_");

            if (strs.Length < 6)
            {
                throw new FormatException(String.Format("DataStr deserialize error :{0}", dataStr));
            }
            else
            {
                LinkedList<object> ts = JsonConvert.DeserializeObject< LinkedList<object>> (strs[5]) ;

                if (ts == null)
                {
                    throw new FormatException(String.Format("DataStr args deserialize error :{0}", dataStr));

                }
                else
                {
                    Assemblystr = strs[0];
                    NamespaceStr = strs[1];
                    ClassStr = strs[2];
                    MethodStr = strs[3];
                    id = strs[4];
                    Args = ts;

                }
                
            }

        }
    }
}
