using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QQ空间登录
{
    public partial class FrmMain : Form
    {
        QQ空间 qq = new QQ空间();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            qq.init();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
    }
}
