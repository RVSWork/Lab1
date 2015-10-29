using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Warning : Form
    {
        public Warning( string erorr)
        {
            InitializeComponent();
            label1.Text = erorr;
        }

        private void Warning_Load(object sender, EventArgs e)
        {

        }
    }
}
