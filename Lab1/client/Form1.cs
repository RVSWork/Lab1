using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication;

namespace Client
{
    
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        String ip = "localhost";
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
            Communication.Message message = new Communication.Message();
            
            message.setArticle(article1);
            message.setCodeMode(0);
            //конвертируем объект message в массив байтов
            byte[] msg = Serializer.MessageToByteArray(message);
            //обращаемся к сокету клиента
            SocketClient socketClient = new SocketClient();
            //соединяемся с сервером
            SocketClient.ConnectToServer(11000, "localhost");
            //отправили сообщение серверу
            socketClient.SendMsg(msg);
            //получили ответ
            byte[] msgfromserver = socketClient.GetMsg();
            //закрыли соединение
            socketClient.SocketClose();
            Communication.Message messagefromserver = new Communication.Message();
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
                string s = socketClient.ProcessingErorr(messagefromserver);
                Warning thirdForm = new Warning(s);
                thirdForm.ShowInTaskbar = false; //скрываем форму из панели задач
                thirdForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                thirdForm.ShowDialog(this);//указываем владельца для формы
            }
            
            
            
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
            Communication.Message message = new Communication.Message();
            message.setArticle(article2);
            message.setCodeMode(1);
            //конвертируем объект message в массив байтов
            byte[] msg = Serializer.MessageToByteArray(message);
            //обращаемся к сокету клиента
            SocketClient socketClient = new SocketClient();
            //соединяемся с сервером
            SocketClient.ConnectToServer(11000, ip);
            //отправили сообщение серверу
            socketClient.SendMsg(msg);
            //получили ответ
            byte[] msgfromserver = socketClient.GetMsg();
            //закрыли соединение
            socketClient.SocketClose();
            Communication.Message messagefromserver = new Communication.Message();
            //преобразуем ответ сервера в объект Message
            messagefromserver = Serializer.ByteArrayToMessage(msgfromserver);
            //обрабатываем полученное Message
            bool b = socketClient.ProcessingMsg(messagefromserver);
            
            if (b)
            {
                //открываем форму для редактирования
                Editor fouthForm = new Editor(messagefromserver.getArticle().getKey());
                fouthForm.ShowInTaskbar = false; //скрываем форму из панели задач
                fouthForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                fouthForm.ShowDialog(this);//указываем владельца для формы
                while (fouthForm.DialogResult == DialogResult.OK)
                {
                    //добавляем отредактированное содержимое
                    article2.setContent(fouthForm.ReturnData());
                    //изменяем код режима
                    message.setCodeMode(4);
                    //конвертируем объект message в массив байтов
                    byte[] msg2 = Serializer.MessageToByteArray(message);
                    //обращаемся к сокету клиента
                    SocketClient socketClient2 = new SocketClient();
                    //соединяемся с сервером
                    SocketClient.ConnectToServer(11000, ip);
                    //отправили сообщение серверу
                    socketClient2.SendMsg(msg2);
                    //получили ответ
                    byte[] msgfromserver2 = socketClient2.GetMsg();
                    //закрыли соединение
                    socketClient2.SocketClose();
                    Communication.Message messagefromserver2 = new Communication.Message();
                    //преобразуем ответ сервера в объект Message
                    messagefromserver2 = Serializer.ByteArrayToMessage(msgfromserver2);
                    //обрабатываем полученное Message
                    bool bb = socketClient2.ProcessingMsg(messagefromserver2);
                    if (bb)
                    {
                        Good fifthForm = new Good("Сохранено!");
                        fouthForm.ShowInTaskbar = false; //скрываем форму из панели задач
                        fouthForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                        fouthForm.ShowDialog(this);//указываем владельца для формы
                    }
                    else {
                        Warning thirdForm = new Warning("Не удалось");
                        thirdForm.ShowInTaskbar = false; //скрываем форму из панели задач
                        thirdForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                        thirdForm.ShowDialog(this);//указываем владельца для формы
                    }

                }
                //else 
                if (fouthForm.DialogResult == DialogResult.Cancel)
                {
                    //изменяем код режима
                    message.setCodeMode(3);
                    //конвертируем объект message в массив байтов
                    byte[] msg3 = Serializer.MessageToByteArray(message);
                    //обращаемся к сокету клиента
                    SocketClient socketClient3 = new SocketClient();
                    //соединяемся с сервером
                    SocketClient.ConnectToServer(11000, ip);
                    //отправили сообщение серверу
                    socketClient3.SendMsg(msg3);
                    //получили ответ
                    byte[] msgfromserver3 = socketClient3.GetMsg();
                    //закрыли соединение
                    socketClient3.SocketClose();
                    Communication.Message messagefromserver3 = new Communication.Message();
                    //преобразуем ответ сервера в объект Message
                    messagefromserver3 = Serializer.ByteArrayToMessage(msgfromserver3);
                    //обрабатываем полученное Message
                    bool bb = socketClient3.ProcessingMsg(messagefromserver3);
                    if (bb) { fouthForm.Close(); }
                    else {
                        Warning thirdForm = new Warning("Не удалось");
                        thirdForm.ShowInTaskbar = false; //скрываем форму из панели задач
                        thirdForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                        thirdForm.ShowDialog(this);//указываем владельца для формы
                    }
                }
            }
            else
            {
                string s = socketClient.ProcessingErorr(messagefromserver);
                Warning thirdForm = new Warning(s);
                thirdForm.ShowInTaskbar = false; //скрываем форму из панели задач
                thirdForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                thirdForm.ShowDialog(this);//указываем владельца для формы
            }
            
            

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
            Communication.Message message = new Communication.Message();
            message.setArticle(article3);
            message.setCodeMode(2);
           //не дописано!!!
            //конвертируем объект message в массив байтов
            byte[] msg = Serializer.MessageToByteArray(message);
            //обращаемся к сокету клиента
            SocketClient socketClient = new SocketClient();
            //соединяемся с сервером
            SocketClient.ConnectToServer(11000, ip);
            //отправили сообщение серверу
            socketClient.SendMsg(msg);
            //получили ответ
            byte[] msgfromserver = socketClient.GetMsg();
            //закрыли соединение
            socketClient.SocketClose();
            Communication.Message messagefromserver = new Communication.Message();
            //преобразуем ответ сервера в объект Message
            messagefromserver = Serializer.ByteArrayToMessage(msgfromserver);
            //обрабатываем полученное Message
            bool b = socketClient.ProcessingMsg(messagefromserver);
            if (b)
            {
                //открываем форму для создания (оно же редактирование пустой записи)
                Editor fouthForm = new Editor(messagefromserver.getArticle().getKey());
                fouthForm.ShowInTaskbar = false; //скрываем форму из панели задач
                fouthForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                fouthForm.ShowDialog(this);//указываем владельца для формы
                while (fouthForm.DialogResult == DialogResult.OK)
                {
                    //добавляем отредактированное содержимое
                    article3.setContent(fouthForm.ReturnData());
                    //изменяем код режима
                    message.setCodeMode(4);
                    //конвертируем объект message в массив байтов
                    byte[] msg2 = Serializer.MessageToByteArray(message);
                    //обращаемся к сокету клиента
                    SocketClient socketClient2 = new SocketClient();
                    //соединяемся с сервером
                    SocketClient.ConnectToServer(11000, ip);
                    //отправили сообщение серверу
                    socketClient2.SendMsg(msg2);
                    //получили ответ
                    byte[] msgfromserver2 = socketClient2.GetMsg();
                    //закрыли соединение
                    socketClient2.SocketClose();
                    Communication.Message messagefromserver2 = new Communication.Message();
                    //преобразуем ответ сервера в объект Message
                    messagefromserver2 = Serializer.ByteArrayToMessage(msgfromserver2);
                    //обрабатываем полученное Message
                    bool bb = socketClient2.ProcessingMsg(messagefromserver2);
                    if (bb)
                    {
                        Good fifthForm = new Good("Сохранено!");
                        fouthForm.ShowInTaskbar = false; //скрываем форму из панели задач
                        fouthForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                        fouthForm.ShowDialog(this);//указываем владельца для формы
                    }
                    else
                    {
                        Warning thirdForm = new Warning("Не удалось");
                        thirdForm.ShowInTaskbar = false; //скрываем форму из панели задач
                        thirdForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                        thirdForm.ShowDialog(this);//указываем владельца для формы
                    }
                }
            
             if (fouthForm.DialogResult == DialogResult.Cancel)
            {
                //изменяем код режима
                message.setCodeMode(3);
                //конвертируем объект message в массив байтов
                byte[] msg3 = Serializer.MessageToByteArray(message);
                //обращаемся к сокету клиента
                SocketClient socketClient3 = new SocketClient();
                //соединяемся с сервером
                SocketClient.ConnectToServer(11000, ip);
                //отправили сообщение серверу
                socketClient3.SendMsg(msg3);
                //получили ответ
                byte[] msgfromserver3 = socketClient3.GetMsg();
                //закрыли соединение
                socketClient3.SocketClose();
                Communication.Message messagefromserver3 = new Communication.Message();
                //преобразуем ответ сервера в объект Message
                messagefromserver3 = Serializer.ByteArrayToMessage(msgfromserver3);
                //обрабатываем полученное Message
                bool bb = socketClient3.ProcessingMsg(messagefromserver3);
                if (bb) { fouthForm.Close(); }
                else
                {
                    Warning thirdForm = new Warning("Не удалось");
                    thirdForm.ShowInTaskbar = false; //скрываем форму из панели задач
                    thirdForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                    thirdForm.ShowDialog(this);//указываем владельца для формы
                }
            }
            }
            else 
            {
                string s = socketClient.ProcessingErorr(messagefromserver);
                Warning thirdForm = new Warning(s);
                thirdForm.ShowInTaskbar = false; //скрываем форму из панели задач
                thirdForm.StartPosition = FormStartPosition.CenterScreen;//устанавливаем форму по центру экрана
                thirdForm.ShowDialog(this);//указываем владельца для формы
            }
            
        }
    }
}
