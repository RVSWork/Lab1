using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox1.Text.Trim());
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void button5_Click(object sender, EventArgs e) //читать статью
        {
            Form2 secondForm = new Form2();
            secondForm.ShowInTaskbar = false; //скрываем форму из панели задач
            secondForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
            secondForm.ShowDialog(this);//указываем владельца для формы
            //создаем статью
            Article article1 = new Article(textBox2.Text);

            //формируем сообщение
            Message message = new Message();
            message.setArticle(article1);
            message.setCodeMode(0);
            //конвертируем объект message в массив байтов
            byte[] msg = Serializer.MessageToByteArray(message);
            //обращаемся к сокету клиента
            SocketClient socketClient = new SocketClient();
            //соединяемся с сервером
            SocketClient.ConnectToServer(11000, "172.18.30.55");
            //отправили сообщение серверу
            socketClient.SendMsg(msg);
            //получили ответ
            byte[] msgfromserver = socketClient.GetMsg();
            //закрыли соединение
            socketClient.SocketClose();
            Message messagefromserver = new Message();
            //преобразуем ответ сервера в объект Message
            messagefromserver = Serializer.ByteArrayToMessage(msgfromserver);
            //обрабатываем полученное Message
            bool b = socketClient.ProcessingMsg(messagefromserver);
            if (b)
            {
                //для теста ответ пока выводим в textBox3
                textBox3.Text = messagefromserver.getArticle().getContent();
            }
            else
            {
 
            }
            string s =socketClient.ProcessingErorr(messagefromserver);
            Warning thirdForm = new Warning( s );
            thirdForm.ShowInTaskbar = false; //скрываем форму из панели задач
            thirdForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
            thirdForm.ShowDialog(this);//указываем владельца для формы
            
            
        }

        private void button6_Click(object sender, EventArgs e) //редактировать статью
        {
            Form2 secondForm = new Form2();
            secondForm.ShowInTaskbar = false; //скрываем форму из панели задач
            secondForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
            secondForm.ShowDialog(this);//указываем владельца для формы
            //создаем статью
            Article article2 = new Article(textBox2.Text);
            //формируем сообщение
            Message message = new Message();
            message.setArticle(article2);
            message.setCodeMode(1);
            //конвертируем объект message в массив байтов
            byte[] msg = Serializer.MessageToByteArray(message);
            //обращаемся к сокету клиента
            SocketClient socketClient = new SocketClient();
            //соединяемся с сервером
            SocketClient.ConnectToServer(11000, "172.18.30.55");
            //отправили сообщение серверу
            socketClient.SendMsg(msg);
            //получили ответ
            byte[] msgfromserver = socketClient.GetMsg();
            //закрыли соединение
            socketClient.SocketClose();
            Message messagefromserver = new Message();
            //преобразуем ответ сервера в объект Message
            messagefromserver = Serializer.ByteArrayToMessage(msgfromserver);
            //обрабатываем полученное Message
            bool b = socketClient.ProcessingMsg(messagefromserver);
            //Не дописано!!!
            

        }

        private void button7_Click(object sender, EventArgs e) //создать новую статью
        {
            Form2 secondForm = new Form2();
            secondForm.ShowInTaskbar = false; //скрываем форму из панели задач
            secondForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
            secondForm.ShowDialog(this);//указываем владельца для формы
            //создаем статью
            Article article3 = new Article(textBox2.Text);
            //формируем сообщение
            Message message = new Message();
            message.setArticle(article3);
            message.setCodeMode(2);
           //не дописано!!!
            
        }
    }
}
