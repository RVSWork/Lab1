using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceReference1;

public partial class WebPageSeparated : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private class Info //Вложеный класс, который содержит информацию для соединения
    {
        public string key;
        public string content;
        public int code_mode;
        public SOAPServerSoapClient client;
        public Info(string k, int code, string con)
        {
            key = k;
            code_mode = code;
            content = con;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)//читать
    {
        Info info = new Info(TextBox1.Text,0,TextBox2.Text);
        info.client = new SOAPServerSoapClient();
        
        Message1 answer = new Message1();
        answer.Code_status = 200;
        while(true)
        {
            if (info.key != null)
            { answer = info.client.MsgToClient(info.key, info.code_mode, info.content);
                break;
            }
            else { Label1.Text = "Укажите название статьи!!"; }
        }
        
        if (answer.Code_status == 404)
        {
            TextBox2.Text = "";
            Label1.Text = "Запись не существует!"; }
        else
        {
            TextBox2.Text = "";
            Label1.Text = info.key;
            TextBox2.Text = answer.Article.Content;
        }
    }

    protected void Button2_Click(object sender, EventArgs e) //редактировать
    {
        Info info = new Info(TextBox1.Text, 1, TextBox2.Text);
        info.client = new SOAPServerSoapClient();
        Message1 answer = new Message1();
        answer.Code_status = 200;
        while (true)
        {
            if (info.key != null)
            {
                answer = info.client.MsgToClient(info.key, info.code_mode, info.content);
                break;
            }
            else { Label1.Text = "Укажите название статьи!!"; }
        }
        if (answer.Code_status == 406 || answer.Code_status == 404)
        {
            TextBox2.Text = "";
            Label1.Text = "Запись нельзя редактировать!"; }
        else
        {
            //редактирвать можно
            Button4.Visible = true;
            Button5.Visible = true;
            Button1.Visible = false;
            Button2.Visible = false;
            Button3.Visible = false;
            TextBox2.ReadOnly = false;
            Label1.Text = info.key;
            TextBox2.Text = answer.Article.Content;
        }
    }

    protected void Button4_Click(object sender, EventArgs e)//сохранить
    {
        Info info = new Info(TextBox1.Text, 4, TextBox2.Text);
        info.client = new SOAPServerSoapClient();
        Message1 answer = new Message1();
        answer.Code_status = 200;
        answer = info.client.MsgToClient(info.key, info.code_mode, info.content);
        if (answer.Code_status == 200)
        {
            Label1.Text = "Сохранено!";
        }
        else { Label1.Text = "Не удалось!"; }
    }

    protected void Button5_Click(object sender, EventArgs e) //завершить редактирование
    {
        Info info = new Info(TextBox1.Text, 3, TextBox2.Text);
        info.client = new SOAPServerSoapClient();
        Message1 answer = new Message1();
        answer.Code_status = 200;
        answer = info.client.MsgToClient(info.key, info.code_mode, info.content);
        if (answer.Code_status == 200)
        {
            Button4.Visible = false; Button5.Visible = false;
            Button3.Visible = true;  Button2.Visible = true;
            Button1.Visible = true;  TextBox2.ReadOnly = true;
            TextBox1.Text = "";  TextBox2.Text = ""; Label1.Text = "";
        }
    }

    protected void Button3_Click(object sender, EventArgs e)//создать новую
    {
        Info info = new Info(TextBox1.Text, 2, TextBox2.Text);
        info.client = new SOAPServerSoapClient();
        Message1 answer = new Message1();
        answer.Code_status = 200;
        while (true)
        {
            if (info.key != null)
            {
                answer = info.client.MsgToClient(info.key, info.code_mode, info.content);
                break;
            }
            else { Label1.Text = "Укажите название статьи!!"; }
        }
        if (answer.Code_status != 200)
        {
            if(answer.Code_status == 405) Label1.Text = "Запись уже сущетвует!!";
            if (answer.Code_status == 406) Label1.Text = "Создать невозможно!!";
        }
        else
        {//можно создавать
            Button4.Visible = true;
            Button5.Visible = true;
            Button1.Visible = false;
            Button2.Visible = false;
            Button3.Visible = false;
            TextBox2.Text = ""; Label1.Text = info.key;
            TextBox2.ReadOnly = false;
        }

    }
}