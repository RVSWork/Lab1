using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Communication;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Windows.Forms;




namespace Client
{
    class PipeClient
    {

        private static NamedPipeClientStream pipeClient;


        private class ConnectionInfoPipe //Вложеный класс который содержит информацию о соединении
        {
            public NamedPipeClientStream pipeClient; //Канал соединения
            public const int BufferSize = 1024;//Размер буфера 
            public byte[] buffer = new byte[BufferSize];//Буфер
        }

        
        public static byte[] ConnectToServer(int port, byte[] msg)
        {
            try
            {
                pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut);
                pipeClient.Connect();
                ConnectionInfoPipe connection = new ConnectionInfoPipe();
                int max = connection.buffer.Length;
                SendMessageFromPipe(11000, msg, connection, max);
                int bytesRec = pipeClient.Read(connection.buffer, 0, max);//получили ответ от сервера
                return connection.buffer;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                byte[] a = { 0 };
                return a;
            }
            finally
            {
                pipeClient.Close();
            }
        }

        
        static void SendMessageFromPipe(int port, byte[] msg, ConnectionInfoPipe connection, int max)
        {

            try {
                connection.pipeClient = pipeClient;
                connection.buffer = msg;
                connection.pipeClient.Write(connection.buffer, 0, connection.buffer.Length);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);   
            }
        }

        public bool ProcessingMsg(Communication.Message msg)
        {
            if (msg.getCodeStatus() == 200)
                return true;
            else return false;
        }
        public string ProcessingErorr(Communication.Message msg)
        {
            if (msg.getCodeStatus() == 404)
            { return "Запись не сущуствует"; }
            else if (msg.getCodeStatus() == 405)
            {
                return "Запись уже существует";
            }
            else if (msg.getCodeStatus() == 406)
            { return "В настоящий момент запись нельзя редактировать, попробуйте позднее"; }
            else return "Неизвестная ошибка";
        }

    }
}
