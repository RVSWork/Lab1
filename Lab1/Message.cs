using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    class Message
    {
        private
            Article Article;
            int Code_mode;
            int Code_status;
            String Name_addressee;
            String Name_sender;
        public Message(){ }
        public void setArticle(Article article) { Article = article; }
        
        public void setCodeMode(int code) { Code_mode = code; }
        public void setCodeStatus(int code) { Code_status = code; }
        public void setAddressee(String addressee) { Name_addressee = addressee; }
        public void setSender(String sender) { Name_sender = sender; }

        public Article getArticle() { return Article; }
        public String getAdressee() { return Name_addressee; }
        public String getSender() { return Name_sender; }
        public int getCodeMode() { return Code_mode; }
        public int getCodeStatus() { return Code_status; }
    }

    /*
    Code_mode
    ------------------
    0-читать
    1-изменять
    2-создавать
    3-завершить
    4-сохранить
    ------------------
    Code_status
    ------------------
   200-ОК
   404-Запись не сущуствует
   405-Запись уже существует
   406-Запись нельзя редактировать
    */
}
