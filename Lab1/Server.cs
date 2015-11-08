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
using Communication;
using System.IO.Pipes;

namespace Server
{ 

    class Server
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false); //Уведомляет один или более ожидающих потоков о том, что произошло событие.
        private Dictionary<String,Article> BackupCopies; //Содержит резевные копии статей
        private Dictionary<String, Article> EditableCopies;//Содержит копии редактируемых статей
        private Dictionary<String, Article> Store;
        private List<ConnectionInfo> connections;//
        

        private class ConnectionInfo //Вложеный класс который содержит информацию о соединении
        {
            public Socket Socket; //Сокет соединения
            public Thread Thread;//Поток соединения
            public const int BufferSize = 1024;//Размер буфера 
            public byte[] buffer = new byte[BufferSize];//Буфер
        }
            

            // private Thread acceptThread;
            

        public Server() {

            BackupCopies = new Dictionary<String, Article>();
            EditableCopies = new Dictionary<String, Article>();
            connections = new List<ConnectionInfo>();
            Store = new Dictionary<String, Article>();
        }



        // Тут новые классы и методы для работы с Pipe

        private List<ConnectionInfoPipe> connectionsPipe;//
        private class ConnectionInfoPipe //Вложеный класс который содержит информацию о соединении
        {
            public NamedPipeServerStream serverPipe; //Канал соединения
            public Thread Thread;//Поток соединения
            public const int BufferSize = 1024;//Размер буфера 
            public byte[] buffer = new byte[BufferSize];//Буфер
        }
        //Функция которая настраивает соединение, по которому будет слушать 
        public void StartListeningPipe()
        {
               
                while (true)
                {
                   
                        NamedPipeServerStream pipeServer =
                        new NamedPipeServerStream("testpipe", PipeDirection.InOut);
                        Console.Write("Waiting for client connection...");
                        pipeServer.WaitForConnection();
                        Console.WriteLine("Client connected.");
                        ConnectionInfoPipe connection = new ConnectionInfoPipe();
                        connection.serverPipe = pipeServer;
                        connection.Thread = new Thread(ProcessConnectionPipe);
                        connection.Thread.IsBackground = true;
                        connection.Thread.Start(connection);
                        lock (connectionsPipe) connectionsPipe.Add(connection);
                }
            
            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
        public void ProcessConnectionPipe(Object status)
        {
            ConnectionInfoPipe connection = (ConnectionInfoPipe)status;
            int max = connection.buffer.Length;
            
            Console.WriteLine("Есть соединение");
            try
            {
                int bytesRead = connection.serverPipe.Read(connection.buffer, 0, max);
                if (bytesRead > 0)
                {
                    Message msg = Serializer.ByteArrayToMessage(connection.buffer);
                    Console.WriteLine(msg.getArticle().getKey());
                    Console.WriteLine(msg.getCodeMode());
                    Message answer = ProcessingMsg(msg);
                    connection.buffer = Serializer.MessageToByteArray(answer);
                    connection.serverPipe.Write(connection.buffer, 0, max);
                }
                else if (bytesRead == 0) return;
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception: " + exc);
            }
            finally
            {
                connection.serverPipe.Close();
                lock (connectionsPipe) connectionsPipe.Remove(
                    connection);
            }
        }


        // Тут закончились Pipe



        public void StartListening()
    {
        //byte[] bytes = new Byte[1024];

        IPHostEntry ipHost = Dns.GetHostEntry("localhost");// Разрешает имя узла или IP - адрес в экземпляр


        IPAddress ipAddr = ipHost.AddressList[0];//Получает первый ip связанный с этим узлом
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000); // Представляет сетевую конечную точка в виде IP-адреса и номера порта

        Socket listener = new Socket(ipAddr.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);//Инициализирует новый экземпляр класса Socket, используя заданные семейство адресов, тип сокета и протокол.
            
                try
                {
                    listener.Bind(ipEndPoint); //Привязывание сокета к прослушиваемому порту
                    listener.Listen(100); //Устанавливает объект Socket в состояние прослушивания. Максимальное кол-во соединений 100
 
                    while (true)
                    {
                        allDone.Reset(); //Задает несигнальное состояние события, вызывая блокирование потоков.

                        // Start an asynchronous socket to listen for connections.
                        Console.WriteLine("Waiting for a connection...");
                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);

                        // Дождитесь соединения, прежде чем продолжить.
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
            lock (connections) connections.Add(connection);

    // Create the state object.
    //state = new StateObject();
    //state.workSocket = handler;
    /*handler.BeginReceive(connection.buffer, 0, ConnectionInfo.BufferSize, 0,
        new AsyncCallback(ReadCallback), connection);*/
}


        private void ProcessConnection(object state)
        {
             ConnectionInfo connection = (ConnectionInfo)state;
            try { 
                  while (true)
                  {
                      int bytesRead = connection.Socket.Receive(
                      connection.buffer);

                      if (bytesRead > 0)
                      {
                          Message msg =Serializer.ByteArrayToMessage(connection.buffer);
                          Console.WriteLine(msg.getArticle().getKey());
                          Console.WriteLine(msg.getCodeMode());
                          Message answer =ProcessingMsg(msg);
                         // Console.WriteLine(answer.getCodeStatus());
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
        public Message ProcessingMsg(Message msg)
        {
            Message answer=null;
            if (msg.getCodeStatus() == 200)
            {
                switch (msg.getCodeMode())
                {
                    case 0:
                        answer = ReadRequest(msg.getArticle().getKey());
                        break;

                    case 1:
                        answer = СhangeRequest(msg.getArticle().getKey(), msg.getSender());
                        break;
                    case 2:
                        answer = CreateRequest(msg.getArticle().getKey());
                        break;
                    case 3:
                        answer = CompletionRequest(msg.getArticle().getKey());
                        break;
                    case 4:
                        answer = SaveRequest(msg.getArticle());
                        break;
                    default:
                        break;

                }
            }
            else
            {
                String key = msg.getArticle().getKey();
                if (EditableCopies.ContainsKey(key))
                { EditableCopies.Remove(key); }
                if (BackupCopies.ContainsKey(key))
                    BackupCopies.Remove(key);
            }
            return answer;
        }
        
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
        public Message СhangeRequest(String key, String autor)
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

        public Article getAticleFromDataBase(String key)
        {
            // Лера, твой ход. Чтение из базы данных и помещение в Article
            //Article article = new Article(key);
            //article.setContent("ura");
            if (Store.ContainsKey(key))
                return Store[key];
            else return null;
        }
        public void saveArticleInDataBase(Article article)
        {
            // Лера, твой ход. Сохрани статью в базе данных
            if (Store.ContainsKey(article.getKey()))
                Store[article.getKey()] = article;
            else 
            Store.Add(article.getKey(), article);
        }
        public bool ArticleExistInDataBase(String key)
        {
            // Лера, твой ход. Проверь сущесвует ли запись
            return Store.ContainsKey(key);
        }

        public bool MemberOfBackup(String key) { return BackupCopies.ContainsKey(key); }


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
