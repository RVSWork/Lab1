using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Threading;


/// <summary>
/// Сводное описание для WebServer
/// </summary>
[WebService(Namespace = "urn:MOE", Description = "A Simple Web SOAPServer",Name="SOAPServer")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
// [System.Web.Script.Services.ScriptService]
public class WebServer : System.Web.Services.WebService
{

    public WebServer()

    {

        //Раскомментируйте следующую строку в случае использования сконструированных компонентов 
        //InitializeComponent(); 
        
    }

    

    private static class ArticleStore
    {
        public static Dictionary<String, Article> BackupCopies = new Dictionary<String, Article>(); //Содержит резервные копии статей
        public static Dictionary<String, Article> EditableCopies=new Dictionary<String, Article>();//Содержит копии редактируемых статей
        public static Dictionary<String, Article> Store = new Dictionary<String, Article>(); //вместо БД

       
    }
    static Mutex mutConPipe = new Mutex();//Мьютекс для соедин connectionsPipe
    static Mutex mutCon = new Mutex();//Мьютекс для соедин connections
    static Mutex mutEdit = new Mutex();//Мьютекс для копии редактир статей
    static Mutex mutBackup = new Mutex();//Мьютекс для редактир статей


    [WebMethod]
    public Message1 MsgToClient(string key, int code_mode, string content)
    {
        Message1 answer = new Message1();
        Message1 msgFromClient = new Message1();
        msgFromClient.Article = new Article(key);
        msgFromClient.Code_mode = code_mode;
        msgFromClient.Article.Content = content;
            if (msgFromClient != null)
            {
                
                answer = ProcessingMsg(msgFromClient);
                return answer;
            }
            else
        {
            answer.Code_status = 407;
            return answer;
        }     

    }

   

    public Message1 ProcessingMsg(Message1 msg)
    {
        
        Message1 answer = null;
        if (msg.Code_status == 200)
        {
           
            switch (msg.Code_mode)
            {
                case 0:
                    answer= ReadRequest(msg.Article.Key);
                    break;
                case 1:
                    answer = СhangeRequest(msg.Article.Key, msg.getSender());
                    break;
                case 2:
                    answer = CreateRequest(msg.Article.Key);
                    break;
                case 3:
                    answer = CompletionRequest(msg.Article.Key);
                    break;
                case 4:
                    answer = SaveRequest(msg.Article);
                    break;
                case 5:
                    mutBackup.WaitOne();
                    ArticleStore.BackupCopies.Remove(msg.Article.Key);//??????????????????????????????????
                    mutBackup.ReleaseMutex();
                    mutEdit.WaitOne();
                    ArticleStore.EditableCopies.Remove(msg.Article.Key);//??????????????????????????????????
                    mutEdit.ReleaseMutex();
                    answer = new Message1();
                    break;
                default:
                    break;
            }
        }
        return answer;
    }

    public Message1 ReadRequest(string key)
    {
        Article article = null;
        Message1 msg = new Message1();
        if (ArticleExistInDataBase(key))
        {
            if (MemberOfBackup(key))
            {
                article = new Article(key);
                article = ArticleStore.BackupCopies[key];
            }
            else article = getAticleFromDataBase(key);

        }
        else msg.Code_status = 404;
        msg.Article = article;
        return msg;
    }

    public Message1 СhangeRequest(string key, string autor)
    {
        Article article = null;
        Message1 msg = new Message1();
        if (ArticleExistInDataBase(key))
        {
            if (!MemberOfBackup(key))
            {
                article = getAticleFromDataBase(key);
                mutBackup.WaitOne();
                ArticleStore.BackupCopies.Add(key, article);
                mutBackup.ReleaseMutex();
                mutEdit.WaitOne();
                ArticleStore.EditableCopies.Add(key, article);
                mutEdit.ReleaseMutex();
            }
            else msg.Code_status = 406;

        }
        else msg.Code_status = 404;
        msg.Article = article;
        return msg;
    }

    public Message1 CreateRequest(string key)
    {
        Article article = null;
        Message1 msg = new Message1();
        if (!ArticleExistInDataBase(key))
        {
            if (!MemberOfEdit(key))//!ArticleStore.EditableCopies.ContainsKey(key))
            {
                article = new Article(key);
                article.Content = string.Empty;
                mutEdit.WaitOne();
                ArticleStore.EditableCopies.Add(key, article);
                mutEdit.ReleaseMutex();
               
            }
            else msg.Code_status = 406;
        }
        else msg.Code_status = 405;
        msg.Article = article;
        return msg;
    }

    public Message1 CompletionRequest(string key)
    {
        Message1 msg = new Message1();
        if (ArticleStore.EditableCopies.ContainsKey(key))
        {
            Article article = ArticleStore.EditableCopies[key];
            saveArticleInDataBase(article);
            if (ArticleStore.BackupCopies.ContainsKey(key))
            {
                mutBackup.WaitOne();
                ArticleStore.BackupCopies.Remove(key);
                mutBackup.ReleaseMutex();
             }
        mutEdit.WaitOne();
        ArticleStore.EditableCopies.Remove(key);
        mutEdit.ReleaseMutex();
        
        }
        else msg.Code_status = 404;
        return msg;
    }

    public Message1 SaveRequest(Article article)
    {
        Message1 msg = new Message1();
        if (ArticleStore.EditableCopies.ContainsKey(article.Key))
        {
            
            mutEdit.WaitOne();
            ArticleStore.EditableCopies[article.Key] = article;
            mutEdit.ReleaseMutex();
        }
        else msg.Code_status = 404;
        return msg;
    }

    public Article getAticleFromDataBase(string key)
    {
        // Лера, твой ход. Чтение из базы данных и помещение в Article
        //Article article = new Article(key);
        //article.setContent("ura");
        if (ArticleStore.Store.ContainsKey(key))
            return ArticleStore.Store[key];
        else return null;
    }

    public bool ArticleExistInDataBase(string key)
    {
        // Лера, твой ход. Проверь сущесвует ли запись
        return ArticleStore.Store.ContainsKey(key);
    }

    public void saveArticleInDataBase(Article article)
    {
        // Лера, твой ход. Сохрани статью в базе данных
        if (ArticleStore.Store.ContainsKey(article.Key))
            ArticleStore.Store[article.Key] = article;
        else
            ArticleStore.Store.Add(article.Key, article);
    }

    public bool MemberOfBackup(string key) { return ArticleStore.BackupCopies.ContainsKey(key); }
    public bool MemberOfEdit(string key) { return ArticleStore.EditableCopies.ContainsKey(key); }
}
