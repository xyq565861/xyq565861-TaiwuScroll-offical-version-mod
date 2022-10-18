using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public   class MirrorMsg
    {


        private MirrorMsgImpl _messageImpl;
       
        public virtual int MinimumFrameSize { get; }
        public MirrorMsg()
        {
            _messageImpl = new MirrorMsgImpl();
        }
        public MirrorMsg(byte functionCode,string callId)
        {
            _messageImpl = new MirrorMsgImpl(functionCode, callId);
        }
        public string CallId
        {
            get {return _messageImpl.callId; } 
            set { _messageImpl.callId = value; }
        }
        public long Token
        {
            get { return _messageImpl.token; }
            set { _messageImpl.token = value; }
        }
        public ulong Length
        {
            get { return _messageImpl.length; }
            set { _messageImpl.length = value; }
        }
        public byte FunctionCode
        {
            get { return _messageImpl.functionCode; }
            set { _messageImpl.functionCode = value; }
        }
        public byte[] Data
        {
            get { return _messageImpl.data; }

        }
        public string Massage
        {
            get {
                try
                {
                    string str = Encoding.UTF8.GetString(Data).TrimEnd('\0');

                    if (str.StartsWith('^') && str.EndsWith('^'))
                    {
                        str = str.Trim('^');
                        string[] strs = str.Split("#&#");
                        if (strs.Length > 1)
                        {
                            str = strs[1];
                        }
                    }
                    else
                    {
                        str = "";
                    }
                    return str;
                }
                catch(Exception ex)
                {
                    return "";
                }





               
            }
            
        }

        public byte[] ProtocolDataUnit
        {
            get
            {
                List<byte> pdu = new List<byte>();

                pdu.Add(FunctionCode);
                pdu.AddRange(BitConverter.GetBytes(Token));
                pdu.AddRange(BitConverter.GetBytes(Length));
                pdu.AddRange(_messageImpl.data);
                return pdu.ToArray();
            }
        }
        public string Header
        {
            get
            {

                return string.Format("{0}-{1}-{2}", FunctionCode, Token,Length);
            }
        }

        public void Initialize(string massage,string id)
        {
            if (massage == null)
                throw new ArgumentNullException("massage", "Argument massage cannot be null.");
            Token = DateTime.Now.Ticks;
            
            string strframe = '^'  + id+"#&#"+ massage + '^';

            byte[] databuff= Encoding.UTF8.GetBytes(strframe);
            Length = (ulong)massage.AsQueryable().LongCount();
            CallId = id;
            _messageImpl.data = Encoding.UTF8.GetBytes(strframe);


        }
        //public void Initialize(object obj, string id)
        //{
        //    if (obj == null)
        //        throw new ArgumentNullException("massage", "Argument obj cannot be null.");
        //    Token = DateTime.Now.Ticks;

        //    //string strframe = '^' + id + "#&#" + massage + '^';

        //    //byte[] databuff = Encoding.UTF8.GetBytes(strframe);
        //    byte[] databuff;
        //    //Length = (ulong)massage.AsQueryable().LongCount();
        //    using var memoryStream = new MemoryStream();
        //    DataContractSerializer ser = new DataContractSerializer(typeof(object));
        //    ser.WriteObject(memoryStream, obj);
        //    databuff = memoryStream.ToArray();
        //    //return data;

        //    CallId = id;
        //    _messageImpl.data = databuff;


        //}
        public void TryFormate(byte[] frame)
        {
            //FunctionCode.size+Token.size+Length.size
            if (frame.Length <1+8+ 8+MinimumFrameSize)
                throw new FormatException(String.Format("Message frame must contain at least {0} bytes of data.", MinimumFrameSize));
            FunctionCode = frame[0];
            Token = BitConverter.ToInt64(frame, 1);
            Length= BitConverter.ToUInt64(frame, 9);
            string str = Encoding.UTF8.GetString((frame.Skip(1 + 8 + 8).ToArray())).TrimEnd('\0');

            if (str.StartsWith('^') && str.EndsWith('^'))
            {
                string str1 = str.Trim('^');
                string[] strs = str1.Split("#&#");
                if (strs.Length > 1)
                {
                    
                    if (Length != (ulong)(strs[1].AsQueryable().LongCount()))
                        throw new FormatException(String.Format("Message Length verification error,Length:{0}，DataLength：{1}", Length, (ulong)(str.AsQueryable().LongCount())));
                    CallId = strs[0];
                }
                else
                {
                    throw new FormatException(String.Format("Message call id formate error :{0}", str));

                }
            }
            else
            {
                throw new FormatException(String.Format("Message formate error :{0}", str));
            }

         
            _messageImpl.data=(frame.Skip(1 + 8 + 8).ToArray());

        }

    }
}
