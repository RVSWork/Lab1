using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    class Server
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private Dictionary<String,Article> BackupCopies;
        private Dictionary<String, Article> EditableCopies;

        private class ConnectionInfo
        {
            public Socket Socket;
            public Thread Thread;
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }
        private Thread acceptThread;
        private List<ConnectionInfo> connections;
        public Server() {

            BackupCopies = new Dictionary<String, Article>();
            EditableCopies = new Dictionary<String, Article>();
            //state = new StateObject();
            connections = new List<ConnectionInfo>();
        }



        /*public void addArticle() { }
        public void addBackupCopy() { }
        public void addEditableCopy() { }
        public void deleteArticle() { }
        public void deleteBackupCopy() { }
        public void deleteEditableCopy() { }
        public Article getArticle(String name) { return new Article(name); }
        public static void getMsg(Socket handler, String data) {
             String data 
             byte[] bytes = new byte[1024];
             int bytesRec = handler.Receive(bytes);
             data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
             processingMsg(data);
         }
        public bool keepArticle() { return false; }
        public void memberOfBackup() { }
        public void memberOfEditable() { }*/
        public Article getAticleFromDataBase(String key)
        {
            // Лера, твой ход. Чтение из базы данных и помещение в Article
            return new Article(key);
        }
        public void saveArticleInDataBase(Article article)
        {
            // Лера, твой ход. Сохрани статью в базе данных
        }
        public bool ArticleExistInDataBase(String key)
        {
            // Лера, твой ход. Проверь сущесвует ли запись
            return true;
        }
        public bool MemberOfBackup(String key) { return BackupCopies.ContainsKey(key); }
        
        public Message ReadRequest(String key)
        {
            Article article=null;
            Message msg = new Message();
            if (ArticleExistInDataBase(key))
            {
                if (MemberOfBackup(key))
                {
                    article = new Article(key);
                    article=BackupCopies[key];
                }
                else article = getAticleFromDataBase(key);
               
            }
            else msg.setCodeStatus(404);
            msg.setArticle(article);
            return msg;
        }
        public Message СhangeRequest(String key)
        {
            Article article=null;
            Message msg = new Message();
            if (ArticleExistInDataBase(key))
            {
                if (!MemberOfBackup(key))
                {
                    article = getAticleFromDataBase(key);
                    BackupCopies.Add(key, article);
                    EditableCopies.Add(key, article);
                }
                else msg.setCodeStatus(406);
                
            }
            else msg.setCodeStatus(404);
            msg.setArticle(article);
            return msg;
        }
        public Message CreateRequest(String key)
        {
            Article article = null;
            Message msg = new Message();
            if (!ArticleExistInDataBase(key))
            {
                if (!EditableCopies.ContainsKey(key))
                {
                    article = new Article(key);
                    article.setContent(String.Empty);
                    EditableCopies.Add(key, article);
                }
                else msg.setCodeStatus(406);
            }
            else msg.setCodeStatus(405);
            msg.setArticle(article);
            return msg;
        }
        public Message CompletionRequest(String key)
        {
            Message msg = new Message();
            if (EditableCopies.ContainsKey(key))
            {
                Article article = EditableCopies[key];
                saveArticleInDataBase(article);
                if (BackupCopies.ContainsKey(key))
                    BackupCopies.Remove(key);
                EditableCopies.Remove(key);
            }
            else msg.setCodeStatus(404);
            return msg;
        }
        public Message SaveRequest(Article article)
        {
            Message msg = new Message();
            if (EditableCopies.ContainsKey(article.getKey()))
            {
                EditableCopies[article.getKey()] = article;
            }
            else msg.setCodeStatus(404);
            return msg;
        }

        public Message ProcessingMsg(Message msg) {
            if (msg.getCodeStatus() == 200)
            {
                Message answer;
                switch (msg.getCodeMode())
                {
                    case 0:
                        answer = ReadRequest(msg.getArticle().getKey());
                        break;
                        
                    case 1:
                        answer=СhangeRequest(msg.getArticle().getKey());
                        break;
                    case 2:
                        answer=CreateRequest(msg.getArticle().getKey());
                        break;
                    case 3:
                        answer =CompletionRequest(msg.getArticle().getKey());
                        break;
                    case 4:
                        answer = SaveRequest(msg.getArticle());
                        break;
                    default:
                        break;
                    
                }
            }
            return msg;
        }
        public void saveArticle() { }

        public void StartListening()
        {
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            ConnectionInfo connection = new ConnectionInfo();
            connection.Socket = handler;

            connection.Thread = new Thread(ProcessConnection);
            connection.Thread.IsBackground = true;
            connection.Thread.Start(connection);
            lock(connections) connections.Add(connection);

            // Create the state object.
            //state = new StateObject();
            //state.workSocket = handler;
            /*handler.BeginReceive(connection.buffer, 0, ConnectionInfo.BufferSize, 0,
                new AsyncCallback(ReadCallback), connection);*/
        }

        private void ProcessConnection(object state)
        {
            ConnectionInfo connection = (ConnectionInfo)state;
            //byte[] buffer = new byte[255];
            try
            {
                while (true)
                {
                    int bytesRead = connection.Socket.Receive(
                    connection.buffer);
                    
                    if (bytesRead > 0)
                    {
                        Message msg=Serializer.ByteArrayToMessage(connection.buffer);
                        Message answer=ProcessingMsg(msg);
                        connection.buffer = Serializer.MessageToByteArray(answer);
                        connection.Socket.Send(connection.buffer);
                    }
                    else if (bytesRead == 0) return;
                }
            }
            catch (SocketException exc)
            {
                Console.WriteLine("Socket exception: " +
                    exc.SocketErrorCode);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception: " + exc);
            }
            finally
            {
                connection.Socket.Close();
                lock (connections) connections.Remove(
                    connection);
            }
        }
        

        /* public static void ReadCallback(IAsyncResult ar)
         {
             String content = String.Empty;

             // Retrieve the state object and the handler socket
             // from the asynchronous state object.
             ConnectionInfo connection = (ConnectionInfo)ar.AsyncState;
             Socket handler = state.Socket;

             // Read data from the client socket. 
             int bytesRead = handler.EndReceive(ar);
             if (bytesRead > 0)
             {
                 // There  might be more data, so store the data received so far.
             }
         }
         public static void sendMsg(Socket hander, String data)
         {
             byte[] byteData = Encoding.ASCII.GetBytes(data);
             hander.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), hander);
         }
         private static void SendCallback(IAsyncResult ar)
         {
             try
             {
                 // Retrieve the socket from the state object.
                 Socket handler = (Socket)ar.AsyncState;

                 // Complete sending the data to the remote device.
                 int bytesSent = handler.EndSend(ar);
                 Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                 handler.Shutdown(SocketShutdown.Both);
                 handler.Close();

             }
             catch (Exception e)
             {
                 Console.WriteLine(e.ToString());
             }


         }*/
    }
}
