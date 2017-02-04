using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 百度登录;

namespace 百度登录
{
    public partial class FrmMain : Form
    {
        百度登录类 baidu = new 百度登录类();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            txtuser.Leave += Txtuser_Leave;
            baidu.init();
        }

        private void Txtuser_Leave(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
