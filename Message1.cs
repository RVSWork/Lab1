using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Сводное описание для Message
/// </summary>
/// 

    public class Message1
    {
        private
             Article _Article;
        int _Code_mode;
        int _Code_status;
        string Name_addressee;
        string Name_sender;

    //свойства доступные клиенту после сериализации

    public Article Article
    {
        get
        {
            return _Article;
        }
        set
        {
            _Article = value;
        }
    }
    public int Code_mode
    {
        get
        {
            return _Code_mode;
        }
        set
        {
            _Code_mode = value;
        }
    }
    public int Code_status
    {
        get
        {
            return _Code_status;
        }
        set
        {
            _Code_status = value;
        }
    }

    public Message1()
        {
            
            Code_status = 200;
        }
        public Message1(Article article, int code_mode)
        {
            _Article = article;
            _Code_mode = code_mode;
            _Code_status = 200;
        }


        
        public void setAddressee(String addressee) { Name_addressee = addressee; }
        public void setSender(String sender) { Name_sender = sender; }

       
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
  407-пустое сообщение клиента
   */
