using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorNet
{
    public  abstract class MirrorMsg
    {


        private MirrorMsgImpl _messageImpl;
        
        public abstract int MinimumFrameSize { get; }
        public MirrorMsg()
        {
            _messageImpl = new MirrorMsgImpl();
        }
        public MirrorMsg(byte functionCode)
        {
            _messageImpl = new MirrorMsgImpl(functionCode);
        }
        public long Token
        {
            get { return _messageImpl.Token; }
            set { _messageImpl.Token = value; }
        }
        public ulong Length
        {
            get { return _messageImpl.Length; }
            set { _messageImpl.Length = value; }
        }
        public byte FunctionCode
        {
            get { return _messageImpl.FunctionCode; }
            set { _messageImpl.FunctionCode = value; }
        }
        public byte[] Data
        {
            get { return _messageImpl.Data; }

        }
        public string Massage
        {
            get {
                string str = Encoding.UTF8.GetString(Data).TrimEnd('\0');

                if (str.StartsWith('^')&& str.EndsWith('^'))
                {
                    str = str.Trim('^');
                }
                else
                {
                    str = "";
                }




                return str;
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
                pdu.AddRange(_messageImpl.Data);
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

        public void Initialize(string frame)
        {
            if (frame == null)
                throw new ArgumentNullException("frame", "Argument frame cannot be null.");
            Token = DateTime.Now.Ticks;
            
            string strframe = '^' + frame + '^';

            byte[] databuff= Encoding.UTF8.GetBytes(strframe);
            Length = (ulong)frame.AsQueryable().LongCount();
           
            _messageImpl.Data = Encoding.UTF8.GetBytes(strframe);


        }

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
                str = str.Trim('^');
            }
            else
            {
                throw new FormatException(String.Format("Message formate error :{0}", str));
            }

            if (Length!= (ulong)(str.AsQueryable().LongCount() ))
                throw new FormatException(String.Format("Message Length verification error,Length:{0}，DataLength：{1}", Length, (ulong)(str.AsQueryable().LongCount())));

            _messageImpl.Data=(frame.Skip(1 + 8 + 8).ToArray());

        }

    }
}
