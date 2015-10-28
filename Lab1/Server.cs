using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Net.Sockets;


namespace Server
{
    class Server
    {
        private Hashtable List_backup_copies;
        private Hashtable List_editable_copies;
        public Server() {
            List_backup_copies = new Hashtable();
            List_editable_copies = new Hashtable();
        }

        
        public void addArticle() { }
        public void addBackupCopy() { }
        public void addEditableCopy() { }
        public void deleteArticle() { }
        public void deleteBackupCopy() { }
        public void deleteEditableCopy() { }
        public Article getArticle(String name) { return new Article(name); }
        public void getMsg(Socket handler) {
            String data = null;
            byte[] bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            processingMsg(data);
        }
        public bool keepArticle() { return false; }
        public void memberOfBackup() { }
        public void memberOfEditable() { }
        public void processingMsg(String data) {
            switch (data.Length)
            {
                case
            }
        }
        public void saveArticle() { }
        public void sendMsg() { }



    }
}
