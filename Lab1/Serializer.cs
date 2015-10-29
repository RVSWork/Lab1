using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    static class Serializer
    {
        public static Message ByteArrayToMessage(byte[] arrBytes)
        {
            Message obj = null;
            try
            {
                Message t = new Message();
                byte[] b = Serializer.MessageToByteArray(t);
                for (int i = 0; i < 90; i++)
                {
                    
                    arrBytes[i] = b[i];
                }
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                obj = (Message)binForm.Deserialize(memStream);
                if(obj==null) Console.WriteLine("Сорри");
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Ошибка сериализации: {0}", _Exception.ToString());
            }

            return obj;
        }
        public static byte[] MessageToByteArray(Message obj)
        {
            MemoryStream ms = null;
            try
            {
                if (obj == null)
                    return null;
                BinaryFormatter bf = new BinaryFormatter();
                ms = new MemoryStream();
                bf.Serialize(ms, obj);

            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Ошибка десериализации", _Exception.ToString());
            }
            return ms.ToArray();

        }
    }

}