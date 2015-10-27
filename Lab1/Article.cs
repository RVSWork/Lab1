using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Article
    {
        public Article(String key) { Key = key; }
        private String Key;
        private String Content;
        public String getKey(){ return Key; }
        public String getContent() { return Content; }
        public void setContent(String content) { Content = content; }
    }
}
