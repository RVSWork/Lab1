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
    public partial class Editor : Form
    {
        public Article article2;
        public Communication.Message message;
        String ip;
        DialogResult result;
        public Editor(string header, Article art, Communication.Message mess, String ipp)
        {
            InitializeComponent();
            textBox1.Text = header;
            article2 = art;
            message = mess;
            ip = ipp; 
            if (message.getCodeMode() == 1){
                string s = article2.getContent();
                textBox2.Text = s; }
            
        }
            
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //добавляем отредактированное содержимое
            article2.setContent(ReturnData());
            //изменяем код режима
            message.setCodeMode(4);
            //конвертируем объект message в массив байтов
            byte[] msg2 = Serializer.MessageToByteArray(message);
            // обращаемся к pipe клиента
            PipeClient pipeClient2 = new PipeClient();
            //соединяемся с сервером
            byte[] msgfromserver2 = PipeClient.ConnectToServer(11000, msg2);//получили ответ
            Communication.Message messagefromserver2 = new Communication.Message();
            //преобразуем ответ сервера в объект Message
            messagefromserver2 = Serializer.ByteArrayToMessage(msgfromserver2);
            //обрабатываем полученное Message
            bool bb = pipeClient2.ProcessingMsg(messagefromserver2);
            if (bb)
            {
                
                result=MessageBox.Show("Сохранено!", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Information,MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                
                
            }
            else
            {
                MessageBox.Show("Не удалось!", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            

        }

        

        
        public string ReturnData()
        {
            return (textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
            //изменяем код режима
            message.setCodeMode(3);
            //конвертируем объект message в массив байтов
            byte[] msg3 = Serializer.MessageToByteArray(message);
            // обращаемся к pipe клиента
            PipeClient pipeClient3 = new PipeClient();
            //соединяемся с сервером
            byte[] msgfromserver3 = PipeClient.ConnectToServer(11000, msg3);//получили ответ
            Communication.Message messagefromserver3 = new Communication.Message();
            //преобразуем ответ сервера в объект Message
            messagefromserver3 = Serializer.ByteArrayToMessage(msgfromserver3);
            //обрабатываем полученное Message
            bool bb = pipeClient3.ProcessingMsg(messagefromserver3);
            if (bb) { this.Close(); }
            else
            {
                MessageBox.Show("Не удалось!", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
        private void Editor_FormClosed(object sender,FormClosedEventArgs e )
        {
            //изменяем код режима
            
             message.setCodeMode(5);
            //конвертируем объект message в массив байтов
            byte[] msg3 = Serializer.MessageToByteArray(message);
            // обращаемся к pipe клиента
            PipeClient pipeClient3 = new PipeClient();
            //соединяемся с сервером
            byte[] msgfromserver3 = PipeClient.ConnectToServer(11000, msg3);//получили ответ
            Communication.Message messagefromserver3 = new Communication.Message();
            //преобразуем ответ сервера в объект Message
            messagefromserver3 = Serializer.ByteArrayToMessage(msgfromserver3);
            //обрабатываем полученное Message
            bool bb = pipeClient3.ProcessingMsg(messagefromserver3);
            if (bb) { this.Close(); }
            else
            {
                MessageBox.Show("Не удалось!", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
