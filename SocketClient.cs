using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Lab1
{
    class SocketClient
    {
       //private int port;
        private static Socket senderclient;
        public void SendMsg(byte[] msg)
        {
            SendMessageFromSocket(11000, msg);
        }
        public byte[] GetMsg()
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];
            // Получаем ответ от сервера
            int bytesRec = senderclient.Receive(bytes);
            return bytes;
        }
        public static void ConnectToServer(int port, string IPServer)
        {
            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry(IPAddress.Parse(IPServer));//IPAddress.Parse("localhost"));//"172.18.30.55"));//DEKSTOP-G2J1541");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            senderclient = sender;
            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
        }
        static void SendMessageFromSocket(int port, byte[] msg)
        {

            // Отправляем данные через сокет
            int bytesSent = senderclient.Send(msg);

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()
            /*if (message.IndexOf("<TheEnd>") == -1)
                SendMessageFromSocket(port, message);*/

            // Освобождаем сокет
            senderclient.Shutdown(SocketShutdown.Both);
            senderclient.Close();
        }
        public string ProcessingMsgToRead( Message msg)
        {
            if (msg.getCodeStatus() == 200)
            { return msg.getArticle().getContent(); }
            else if (msg.getCodeStatus() == 404)
            { return "Запись не сущуствует"; }
            else return "Неизвестная ошибка";
        }
    }
}
