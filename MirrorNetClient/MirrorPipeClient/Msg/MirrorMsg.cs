using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

                    if (str.StartsWith("^") && str.EndsWith("^"))
                    {
                        str = str.Trim('^');

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
                Debuglogger.Log(string.Format("FunctionCode:{0}Token:{1}Length:{2}CallId:{3}", FunctionCode, Token, Length, CallId));
                List<byte> pdu = new List<byte>();

                pdu.Add(FunctionCode);
                pdu.AddRange(BitConverter.GetBytes(Token));
                pdu.AddRange(BitConverter.GetBytes(Length));
                pdu.AddRange(Encoding.UTF8.GetBytes(CallId));
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

        public virtual void Initialize(string massage,string id)
        {
            if (massage == null)
                throw new ArgumentNullException("massage", "Argument massage cannot be null.");
            Token = DateTime.Now.Ticks;
            
            string strframe = '^' + massage + '^';

            byte[] databuff= Encoding.UTF8.GetBytes(strframe);
            Length = (ulong)massage.AsQueryable().LongCount();
            CallId = id;
            _messageImpl.data = Encoding.UTF8.GetBytes(strframe);


        }
        public virtual void Initialize(object obj, string id)
        {
            if (obj == null)
                throw new ArgumentNullException("massage", "Argument obj cannot be null.");
            Token = DateTime.Now.Ticks;
            //string strframe = '^' + id + "#&#" + massage + '^';
            //byte[] databuff = Encoding.UTF8.GetBytes(strframe);
            byte[] databuff;
            //Length = (ulong)massage.AsQueryable().LongCount();
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            databuff = UilityTools.ObjectToByteArray(obj);


            Length = (ulong)databuff.Length;

            CallId = id;
            _messageImpl.data = databuff;
        }
        public void TryFormate(byte[] frame)
        {
            //FunctionCode.size+Token.size+Length.size
            if (frame.Length <1+8+ 8+36+MinimumFrameSize)
                throw new FormatException(String.Format("Message frame must contain at least {0} bytes of data.", MinimumFrameSize));
            FunctionCode = frame[0];
            Token = BitConverter.ToInt64(frame, 1);
            Length= BitConverter.ToUInt64(frame, 9);
            CallId= Encoding.UTF8.GetString(frame,(1+8+8),36).TrimEnd('\0');
           // string str = Encoding.UTF8.GetString((frame.Skip(1 + 8 + 8+36).ToArray())).TrimEnd('\0');

            //if (!str.StartsWith('^')|| !str.EndsWith('^'))
            //{
            //    throw new FormatException(String.Format("Message formate error :{0}", str));
            //}


         
            _messageImpl.data=(frame.Skip(1 + 8 + 8+36).ToArray());

        }

    }
}
