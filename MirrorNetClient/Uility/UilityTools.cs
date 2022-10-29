using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace MirrorNet
{
    public class UilityTools
    {
        public static void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();

        }

        public static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            byte[] array;
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                array = ms.GetBuffer();
            }
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    binaryFormatter.Serialize(memoryStream, obj);
            //    array = memoryStream.ToArray();
            //}
            return array;
        }
        public static object ByteArrayToObject(byte[] buffer)
        {
            object obj;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                IFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Binder = new TaiwuQueryImplBinder();
                memoryStream.Write(buffer, 0, buffer.Length);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                obj = binaryFormatter.Deserialize(memoryStream);
            }
            return obj;
        }
        public class TaiwuQueryImplBinder : SerializationBinder

        {
            public override Type BindToType(string assemblyName, string typeName)
            {

                Assembly ass = Assembly.GetExecutingAssembly();
                return ass.GetType(typeName);

            }

        }
        public static int pluse(int a, int b)
        {
            return (int)a + (int)b + TestEntity.b;
        }
    }
}
