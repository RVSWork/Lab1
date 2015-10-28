using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Message
    {
        private
            Article mArticle;
            int Code_mode;
            int Code_status;
            String Key_article;
            String Name_addressee;
            String Name_sender;
        public Message(){ }
        public void setArticle(Article article) { mArticle = article; }
        public void setKey(String key) { Key_article = key; }
        public void setCodeMode(int code) { Code_mode = code; }
        public void setCodeStatus(int code) { Code_status = code; }
        public void setAddressee(String addressee) { Name_addressee = addressee; }
        public void setSender(String sender) { Name_sender = sender; }

        public Article getArticle() { return mArticle; }
        public String getKey() { return Key_article; }
        public String getAdressee() { return Name_addressee; }
        public String getSender() { return Name_sender; }
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
