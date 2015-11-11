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
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MFE;Integrated Security=True";//Устанавливаем связь с БД
        public static ManualResetEvent allDone = new ManualResetEvent(false); //Уведомляет один или более ожидающих потоков о том, что произошло событие.
        private Dictionary<String, Article> BackupCopies; //Содержит резевные копии статей
        private Dictionary<String, Article> EditableCopies;//Содержит копии редактируемых статей
        private Dictionary<String, Article> Store;
        private List<ConnectionInfo> connections;//

        static Mutex mutConPipe = new Mutex();//Мьютекс для соедин connectionsPipe
        static Mutex mutCon = new Mutex();//Мьютекс для соедин connections
        static Mutex mutEdit = new Mutex();//Мьютекс для копии редактир статей
        static Mutex mutBackup = new Mutex();//Мьютекс для редактир статей


        private class ConnectionInfo //Вложеный класс который содержит информацию о соединении
        {
            public Socket Socket; //Сокет соединения
            public Thread Thread;//Поток соединения
            public const int BufferSize = 1024;//Размер буфера 
            public byte[] buffer = new byte[BufferSize];//Буфер
        }


        // private Thread acceptThread;


        public Server()
        {

            BackupCopies = new Dictionary<String, Article>();
            EditableCopies = new Dictionary<String, Article>();
            connections = new List<ConnectionInfo>();
            Store = new Dictionary<String, Article>();
            connectionsPipe = new List<ConnectionInfoPipe>();
        }



        // Тут новые классы и методы для работы с Pipe

        private List<ConnectionInfoPipe> connectionsPipe;//
        private class ConnectionInfoPipe //Вложеный класс который содержит информацию о соединении
        {
            public NamedPipeServerStream serverPipe; //Канал соединения
            //public NamedPipeServerStream serverPipeOut; //Канал соединения
            public Thread Thread;//Поток соединения
            public const int BufferSize = 1024;//Размер буфера 
            public byte[] buffer = new byte[BufferSize];//Буфер
        }
        //Функция которая настраивает соединение, по которому будет слушать 
        public void StartListeningPipe()
        {
            // NamedPipeServerStream pipeServerIn;
            //NamedPipeServerStream pipeServerOut;
            while (true)
            {

                NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, 10);
                //NamedPipeServerStream pipeServerOut = new NamedPipeServerStream("testpipe1", PipeDirection.InOut,10);
                Console.Write("Waiting for client connection...");
                pipeServer.WaitForConnection();
                // pipeServerOut.WaitForConnection();
                Console.WriteLine("Client connected.");

                ConnectionInfoPipe connection = new ConnectionInfoPipe();
                connection.serverPipe = pipeServer;
                //connection.serverPipeOut = pipeServerOut;
                connection.Thread = new Thread(ProcessConnectionPipe);
                connection.Thread.IsBackground = true;
                connection.Thread.Start(connection);
                mutConPipe.WaitOne();
                connectionsPipe.Add(connection);//????????????????????????????7 Соединение поочередно
                mutConPipe.ReleaseMutex();
                //lock (connectionsPipe) connectionsPipe.Add(connection);
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
                    // connection.serverPipe.Write(connection.buffer, 0, max);
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
                mutConPipe.WaitOne();
                connectionsPipe.Remove(//??????????????????????????
                     connection);
                mutConPipe.ReleaseMutex();
                // lock (connectionsPipe) connectionsPipe.Remove(connection);
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
            /* mutCon.WaitOne();
             connections.Add(connection);//???????????????????
             mutCon.ReleaseMutex();*/
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
            try
            {
                while (true)
                {
                    int bytesRead = connection.Socket.Receive(
                    connection.buffer);

                    if (bytesRead > 0)
                    {
                        Message msg = Serializer.ByteArrayToMessage(connection.buffer);
                        Console.WriteLine(msg.getArticle().getKey());
                        Console.WriteLine(msg.getCodeMode());
                        Message answer = ProcessingMsg(msg);
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
            Message answer = null;
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
                    case 5:
                        mutBackup.WaitOne();
                        BackupCopies.Remove(msg.getArticle().getKey());//??????????????????????????????????
                        mutBackup.ReleaseMutex();
                        mutEdit.WaitOne();
                        EditableCopies.Remove(msg.getArticle().getKey());//??????????????????????????????????
                        mutEdit.ReleaseMutex();
                        answer = new Message();
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
            Article article = null;
            Message msg = new Message();
            if (ArticleExistInDataBase(key))
            {
                if (MemberOfBackup(key))
                {
                    article = new Article(key);
                    article = BackupCopies[key];
                }
                else article = getAticleFromDataBase(key);

            }
            else msg.setCodeStatus(404);
            msg.setArticle(article);
            return msg;
        }
        public Message СhangeRequest(String key, String autor)
        {
            Article article = null;
            Message msg = new Message();
            if (ArticleExistInDataBase(key))
            {
                if (!MemberOfBackup(key))
                {
                    article = getAticleFromDataBase(key);
                    mutBackup.WaitOne();
                    BackupCopies.Add(key, article);//??????????????????????????????????
                    mutBackup.ReleaseMutex();
                    mutEdit.WaitOne();
                    EditableCopies.Add(key, article);//??????????????????????????????????
                    mutEdit.ReleaseMutex();
                }
                else msg.setCodeStatus(406);
                // if (!EditableCopies.ContainsKey(key))
                // {

                // }
                /* BackupCopies.Add(key, article);

                 EditableCopies.Add(key, article);*/



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
                    mutEdit.WaitOne();
                    EditableCopies.Add(key, article);
                    mutEdit.ReleaseMutex();
                    // EditableCopies.Add(key, article);
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
                {
                    mutBackup.WaitOne();
                    BackupCopies.Remove(key);
                    mutBackup.ReleaseMutex();
                }
                mutEdit.WaitOne();
                EditableCopies.Remove(key);
                mutEdit.ReleaseMutex();

                //BackupCopies.Remove(key);
                //EditableCopies.Remove(key);
            }
            else msg.setCodeStatus(404);
            return msg;
        }
        public Message SaveRequest(Article article)
        {
            Message msg = new Message();
            if (EditableCopies.ContainsKey(article.getKey()))
            {
                mutEdit.WaitOne();
                EditableCopies[article.getKey()] = article;
                mutEdit.ReleaseMutex();
                //EditableCopies[article.getKey()] = article;
            }
            else msg.setCodeStatus(404);
            return msg;
        }

        public Article getAticleFromDataBase(String key)
        {

            string sqlExpression = "Return_Def"; //Объявляю sql функцию, которая написана в БД
            //Она добавляет новые значения в БД

            if (ArticleExistInDataBase(key))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))//Связываюсь с БД
                {
                    connection.Open();//БД открыта
                    Console.WriteLine("Connection with database ");
                    SqlCommand command = new SqlCommand(sqlExpression, connection);//Говоорю, что применяю sql функцию в указанной БД
                    //устанавливается, что это выражение система будет рассматривать как хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter param = new SqlParameter();//Определяю новые пораметра, котрые получает функция
                    param.ParameterName = "@header_id";
                    param.Direction = ParameterDirection.Input;// Говорю ,что входной параметр 
                    param.SqlDbType = SqlDbType.Char;
                    //ID , который ищем
                    param.Value = key;
                    command.Parameters.Add(param);//Добавляю в функцию

                    param = new SqlParameter();
                    param.ParameterName = "@definition";
                    param.Direction = ParameterDirection.Output;//Выходной параметр
                    param.SqlDbType = SqlDbType.Char;
                    param.Size = 7900;
                    command.Parameters.Add(param);
                    //выполняем хранимую процедуру
                    command.ExecuteNonQuery();//Выполняю функцию
                    //object Indic;
                    string temp;
                    temp = Convert.ToString(command.Parameters["@definition"].Value);//Пребразую полученное значение в строку

                    // temp = (string)Indic;
                    temp = temp.Trim();
                    Article article = new Article(key);
                    article.setContent(temp);
                    return article;
                }
            }

            else return null;

        }


        public void saveArticleInDataBase(Article article)
        {


            string sqlExpression = "Insert_Def";
            if (ArticleExistInDataBase(article.getKey()))
                updateArticleInDataBase(article);
            else
            {


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connection with database ");
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@header_id";
                    param.Direction = ParameterDirection.Input;
                    param.SqlDbType = SqlDbType.Char;

                    param.Value = article.getKey();
                    command.Parameters.Add(param);
                    param = new SqlParameter();
                    param.ParameterName = "@definition";
                    param.Direction = ParameterDirection.Input;
                    param.SqlDbType = SqlDbType.Char;
                    param.Value = article.getContent();
                    command.Parameters.Add(param);
                    command.ExecuteNonQuery();
                }
            }
        }


        public bool ArticleExistInDataBase(String key)
        {
            string sqlExpression = "Return_Def";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection with database ");
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // если имя определено, добавляем параметр для Id

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@header_id";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;
                //ID , который ищем
                param.Value = key;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@definition";
                param.Direction = ParameterDirection.Output;
                param.SqlDbType = SqlDbType.Char;
                param.Size = 7900;
                command.Parameters.Add(param);
                //выполняем хранимую процедуру
                command.ExecuteNonQuery();
                string temp;
                //object Indic;
                temp = Convert.ToString(command.Parameters["@definition"].Value);

                // temp = (string)Indic;
                temp = temp.Trim();

                if (temp.Length == 0)//проверяю размер строки, если ноль,то заданного опред. нет в БД 
                {
                    return false;
                }
                else
                {

                    return true;

                }
            }

        }



        // Обновление записей в БД
        public void updateArticleInDataBase(Article article)
        {


            string sqlExpression = "Update_Def";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection with database ");
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@header_id";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;


                param.Value = article.getKey();
                command.Parameters.Add(param);
                param = new SqlParameter();
                param.ParameterName = "@definition";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;
                param.Value = article.getContent();
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
        }



        public bool MemberOfBackup(String key) { return BackupCopies.ContainsKey(key); }
    }
}
